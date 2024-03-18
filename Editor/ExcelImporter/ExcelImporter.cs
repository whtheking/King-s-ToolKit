using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Reflection;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class ExcelImporter : AssetPostprocessor
{
    class ExcelAssetInfo
    {
        public Type AssetType { get; set; }
        public ExcelAssetAttribute Attribute { get; set; }
        public string ExcelName
        {
            get
            {
                return string.IsNullOrEmpty(Attribute.ExcelName) ? AssetType.Name : Attribute.ExcelName;
            }
        }
    }

    static List<ExcelAssetInfo> cachedInfos = null; // Clear on compile.

    //static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    //{
    //    bool imported = false;
    //    foreach (string path in importedAssets)
    //    {
    //        if (Path.GetExtension(path) == ".xls" || Path.GetExtension(path) == ".xlsx")
    //        {

    //            if (cachedInfos == null) cachedInfos = FindExcelAssetInfos();

    //            var excelName = Path.GetFileNameWithoutExtension(path);
    //            if (excelName.StartsWith("~$")) continue;

    //            ExcelAssetInfo info = cachedInfos.Find(i => i.ExcelName == excelName);

    //            if (info == null) continue;

    //            ImportExcel(path, info);
    //            imported = true;
    //        }
    //    }

    //    if (imported)
    //    {
    //        AssetDatabase.SaveAssets();
    //        AssetDatabase.Refresh();
    //    }
    //}

    [MenuItem("Datas/XlsxLoad")]
    static void LoadAllXlsx()
    {
        bool imported = false;
        foreach (var path in System.IO.Directory.GetFiles(Application.dataPath + "/XLSX"))
        {
            if (Path.GetExtension(path) == ".xls" || Path.GetExtension(path) == ".xlsx")
            {
                if (cachedInfos == null) cachedInfos = FindExcelAssetInfos();

                var excelName = Path.GetFileNameWithoutExtension(path);
                if (excelName.StartsWith("~$")) continue;

                ExcelAssetInfo info = cachedInfos.Find(i => i.ExcelName == excelName);

                if (info == null) continue;

                ImportExcel(path, info);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    static List<ExcelAssetInfo> FindExcelAssetInfos()
    {
        var list = new List<ExcelAssetInfo>();
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (var type in assembly.GetTypes())
            {
                var attributes = type.GetCustomAttributes(typeof(ExcelAssetAttribute), false);
                if (attributes.Length == 0) continue;
                var attribute = (ExcelAssetAttribute)attributes[0];
                var info = new ExcelAssetInfo()
                {
                    AssetType = type,
                    Attribute = attribute
                };
                list.Add(info);
            }
        }
        return list;
    }

    static UnityEngine.Object LoadOrCreateAsset(string assetPath, Type assetType)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(assetPath));

        var asset = AssetDatabase.LoadAssetAtPath(assetPath, assetType);
        if (asset == null)
        {
            asset = ScriptableObject.CreateInstance(assetType.Name);
            AssetDatabase.CreateAsset(asset, assetPath);
            //asset.hideFlags = HideFlags.NotEditable;
        }

        return asset;
    }

    static IWorkbook LoadBook(string excelPath)
    {
        using (FileStream stream = File.Open(excelPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            if (Path.GetExtension(excelPath) == ".xls") return new HSSFWorkbook(stream);
            else return new XSSFWorkbook(stream);
        }
    }

    static List<string> GetFieldNamesFromSheetHeader(ISheet sheet)
    {
        IRow headerRow = sheet.GetRow(0);

        var fieldNames = new List<string>();
        for (int i = 0; i < headerRow.LastCellNum; i++)
        {
            var cell = headerRow.GetCell(i);
            if (cell == null || cell.CellType == CellType.Blank) break;
            fieldNames.Add(cell.StringCellValue);
        }
        return fieldNames;
    }

    static object CellToFieldObject(ICell cell, FieldInfo fieldInfo, object entity, bool isFormulaEvalute = false)
    {
        bool isArray = fieldInfo.FieldType.IsGenericType && fieldInfo.FieldType.GetGenericTypeDefinition() == typeof(List<>);
        object value = null;
        if (cell != null)
        {
            var type = isFormulaEvalute ? cell.CachedFormulaResultType : cell.CellType;
            switch (type)
            {
                case CellType.String:
                    if (fieldInfo.FieldType.IsEnum) value = Enum.Parse(fieldInfo.FieldType, cell.StringCellValue);
                    else value = cell.StringCellValue;
                    break;
                case CellType.Boolean:
                    value = cell.BooleanCellValue;
                    break;
                case CellType.Numeric:
                    var argumentType = fieldInfo.FieldType;
                    if (fieldInfo.FieldType.IsGenericType)
                    {
                        argumentType = argumentType.GetGenericArguments()[0];
                    }
                    value = Convert.ChangeType(cell.NumericCellValue, argumentType);
                    break;
                case CellType.Formula:
                    if (isFormulaEvalute) value = null;
                    value = CellToFieldObject(cell, fieldInfo, true);
                    break;
                default:
                    if (fieldInfo.FieldType.IsValueType)
                    {
                        value = Activator.CreateInstance(fieldInfo.FieldType);
                    }
                    value = null;
                    break;
            }
        }
        if (isArray)
        {
            IList list = (IList)fieldInfo.GetValue(entity);
            if (cell == null)
            {
                var argumentType = fieldInfo.FieldType.GetGenericArguments()[0];
                switch (Type.GetTypeCode(argumentType))
                {
                    case TypeCode.Boolean:
                        value = false;
                        break;
                    case TypeCode.Byte:
                    case TypeCode.SByte:
                    case TypeCode.Char:
                    case TypeCode.Decimal:
                    case TypeCode.Double:
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                    case TypeCode.Int64:
                    case TypeCode.Single:
                    case TypeCode.UInt16:
                    case TypeCode.UInt32:
                    case TypeCode.UInt64:
                        value = Activator.CreateInstance(argumentType);
                        break;

                    case TypeCode.String:
                        value = string.Empty;
                        break;
                    default:
                        value = null;
                        break;
                }
            }
            list.Add(value);
            value = fieldInfo.GetValue(entity);
        }
        return value;
    }

    static object CreateEntityFromRow(IRow row, List<string> columnNames, Type entityType, string sheetName)
    {
        var entity = Activator.CreateInstance(entityType);

        for (int i = 0; i < columnNames.Count; i++)
        {
            var fieldName = columnNames[i].Split("[")[0];
            FieldInfo entityField = entityType.GetField(
                fieldName,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
            );
            if (entityField == null) continue;
            if (!entityField.IsPublic && entityField.GetCustomAttributes(typeof(SerializeField), false).Length == 0) continue;

            ICell cell = row.GetCell(i);

            try
            {
                object fieldValue = CellToFieldObject(cell, entityField, entity);
                if (fieldValue != null)
                {
                    entityField.SetValue(entity, fieldValue);
                }
            }
            catch
            {
                throw new Exception(string.Format("Invalid excel cell type at row {0}, column {1}, {2} sheet.", row.RowNum, cell.ColumnIndex, sheetName));
            }
        }
        return entity;
    }

    static object GetEntityListFromSheet(ISheet sheet, Type entityType)
    {
        List<string> excelColumnNames = GetFieldNamesFromSheetHeader(sheet);

        Type listType = typeof(List<>).MakeGenericType(entityType);
        MethodInfo listAddMethod = listType.GetMethod("Add", new Type[] { entityType });
        object list = Activator.CreateInstance(listType);

        // row of index 0 is header
        for (int i = 1; i <= sheet.LastRowNum; i++)
        {
            IRow row = sheet.GetRow(i);
            if (row == null) break;

            ICell entryCell = row.GetCell(0);
            if (entryCell == null || entryCell.CellType == CellType.Blank) break;

            // skip comment row
            if (entryCell.CellType == CellType.String && entryCell.StringCellValue.StartsWith("#")) continue;

            var entity = CreateEntityFromRow(row, excelColumnNames, entityType, sheet.SheetName);
            listAddMethod.Invoke(list, new object[] { entity });
        }
        return list;
    }

    static void ImportExcel(string excelPath, ExcelAssetInfo info)
    {
        string assetPath = "";
        string assetName = info.AssetType.Name + ".asset";

        if (string.IsNullOrEmpty(info.Attribute.AssetPath))
        {
            string basePath = Path.GetDirectoryName(excelPath);
            assetPath = Path.Combine(basePath, assetName);
        }
        else
        {
            var path = Path.Combine("Assets", info.Attribute.AssetPath);
            assetPath = Path.Combine(path, assetName);
        }
        UnityEngine.Object asset = LoadOrCreateAsset(assetPath, info.AssetType);

        IWorkbook book = LoadBook(excelPath);

        var assetFields = info.AssetType.GetFields();
        int sheetCount = 0;

        foreach (var assetField in assetFields)
        {
            ISheet sheet = book.GetSheetAt(0);
            if (sheet == null) continue;

            Type fieldType = assetField.FieldType;
            if (!fieldType.IsGenericType || (fieldType.GetGenericTypeDefinition() != typeof(List<>))) continue;

            Type[] types = fieldType.GetGenericArguments();
            Type entityType = types[0];

            object entities = GetEntityListFromSheet(sheet, entityType);
            assetField.SetValue(asset, entities);
            sheetCount++;
        }

        if (info.Attribute.LogOnImport)
        {
            Debug.Log(string.Format("Imported {0} sheets form {1}.", sheetCount, excelPath));
        }

        EditorUtility.SetDirty(asset);
    }
}
