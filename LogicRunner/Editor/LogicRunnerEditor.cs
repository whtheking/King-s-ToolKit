using System.Net.Sockets;
using Unity.CodeEditor;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class LogicRunnerEditor : EditorWindow
{
    private VisualElement m_inspectorView;
    private ObjectField m_objectField;
    private LogicRunnerGraphView m_logicRunnerView;
    private GraphDataSaver m_dataSaver;
    private string m_uxmlPath = "Assets/King¡®s ToolKit/LogicRunner/UI/StageRunnerEditor.uxml";
    private string m_ussPath = "Assets/King¡®s ToolKit/LogicRunner/UI/StageRunnerEditor.uss";

    [MenuItem("Tools/logicRunner")]
    public static void ShowWindow()
    {
        LogicRunnerEditor wnd = GetWindow<LogicRunnerEditor>();
        wnd.titleContent = new GUIContent("StageRunner");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(m_uxmlPath);
        visualTree.CloneTree(root);
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(m_ussPath);
        root.styleSheets.Add(styleSheet);


        m_logicRunnerView = root.Q<LogicRunnerGraphView>();
        m_logicRunnerView.SetStyle(m_ussPath);
        m_logicRunnerView.editorWindow = this;
        m_inspectorView = root.Q<VisualElement>("Inspector");
        m_objectField = root.Q<ObjectField>();
        m_objectField.RegisterCallback<ChangeEvent<Object>>(OnObjectFieldChange);
    }

    private void OnObjectFieldChange(ChangeEvent<Object> evt)
    {
        if (evt.newValue is GraphDataSaver)
        {
            m_dataSaver = evt.newValue as GraphDataSaver;
            m_logicRunnerView.PopulateView(m_dataSaver);
        }
        else
        {
            EditorUtility.DisplayDialog("Wanning", "Wrong Type of Asset", "OK");
        }
    }

    public void UpdateInspector(GraphNode node)
    {
        if (node == null)
        {
            return;
        }
        m_inspectorView.Clear();
        var nodeData = m_dataSaver.Nodes.Find(x => x.GUID == node.Node.GUID);
        if (nodeData == null)
        {
            m_inspectorView.Clear();
            return;
        }

        Editor inspectorEditor = Editor.CreateEditor(nodeData);
        (inspectorEditor as NodeDataDrawer).graphNode = node;
        IMGUIContainer container = new IMGUIContainer(() =>
        {
            EditorGUILayout.Space();
            inspectorEditor.OnInspectorGUI();
        });
        m_inspectorView.Add(container);
    }

}
