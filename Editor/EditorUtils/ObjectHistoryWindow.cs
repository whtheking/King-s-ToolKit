using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 选中gameObject历史
/// </summary>
public class ObjectHistoryWindow : EditorWindow
{
    public List<GameObject> selectedObjects = new();
    public Vector2 scrollPos = Vector2.zero;
    [MenuItem("Tools/ObjectHistory")]
    public static void OpenWindow()
    {
        var window = GetWindow<ObjectHistoryWindow>("GameObjectSelectionHistory");
    }

    private void Update()
    {
        if (selectedObjects.Count > 20)
        {
            selectedObjects.RemoveAt(selectedObjects.Count - 1);
        }
        if (selectedObjects.Count == 0)
        {
            selectedObjects.Add(Selection.gameObjects[0]);
        }
        else if (selectedObjects[0] != Selection.gameObjects[0])
        {
            selectedObjects.Insert(0, Selection.gameObjects[0]);
        }
    }

    private void OnGUI()
    {
        bool del = false;
        scrollPos = GUILayout.BeginScrollView(scrollPos);
        {
            foreach (var obj in selectedObjects)
            {
                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button(obj.name))
                    {
                        del = true;
                        Selection.activeGameObject = obj;
                    }
                }
                GUILayout.EndHorizontal();
            }
        }
        GUILayout.EndScrollView();
        if (del)
        {
            selectedObjects.RemoveRange(0, selectedObjects.IndexOf(Selection.activeGameObject));
        }
    }
}