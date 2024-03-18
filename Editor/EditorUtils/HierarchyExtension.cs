using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public static partial class HierarchyExtension
{
    [DidReloadScripts]
    public static void Init()
    {
        EditorApplication.hierarchyWindowItemOnGUI += OnhierarchtWindowDraw;
        EditorApplication.RepaintHierarchyWindow();
    }

    private static void OnhierarchtWindowDraw(int instanceID, Rect selectionRect)
    {
        var instantce = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        if (instantce != null)
        {
            GUIContent icon;
            if (!instantce.activeSelf)
            {
                icon = EditorGUIUtility.IconContent("d_redLight");
            }
            else if (!instantce.activeInHierarchy)
            {
                icon = EditorGUIUtility.IconContent("d_orangeLight");
            }
            else
            {
                icon = EditorGUIUtility.IconContent("d_greenLight");
            }
            Rect rect = new Rect(selectionRect);
            rect.x += selectionRect.width;
            rect.width = 14f;
            rect.height = 14f;
            if (GUI.Button(rect, icon, new GUIStyle()))
            {
                if (Selection.gameObjects.Length > 1)
                {
                    foreach (GameObject go in Selection.gameObjects)
                    {
                        go.SetActive(!go.activeSelf);
                    }
                }
                else
                {
                    instantce.SetActive(!instantce.activeSelf);
                }
            }
        }
    }
}

