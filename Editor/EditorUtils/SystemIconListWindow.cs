using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

/// <summary>
/// unity内置Icon集合
/// </summary>
public class SystemIconListWindow : EditorWindow
{
    string iconLabel = "";
    public List<Texture2D> buildInIcons = new List<Texture2D>();

    [MenuItem("Tools/System Icon")]
    public static void ShowWindow()
    {
        var window = EditorWindow.GetWindow<SystemIconListWindow>("SystemIcons");
        window.buildInIcons = Resources.FindObjectsOfTypeAll<Texture2D>().ToList();
        window.buildInIcons = window.buildInIcons.Where(x => x.name != "").ToList();
        window.buildInIcons.Sort((Texture2D a, Texture2D b) => { return a.name.CompareTo(b.name); });
    }
    public Vector2 scrollPosition;
    void OnGUI()
    {
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        int colCount = (int)position.width / 50 - 1;
        for (int i = 0; i < buildInIcons.Count; i += colCount)
        {
            GUILayout.BeginHorizontal();
            for (int j = 0; j < colCount; j++)
            {
                int index = i + j;
                if (index < buildInIcons.Count)
                {
                    if (GUILayout.Button(buildInIcons[index], GUILayout.Width(50), GUILayout.Height(30)))
                    {
                        iconLabel = buildInIcons[index].name;
                    }
                    GUILayout.Height(30);
                }
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();
        GUILayout.TextArea(iconLabel, GUILayout.Width(position.y), GUILayout.Height(30));
    }
}

