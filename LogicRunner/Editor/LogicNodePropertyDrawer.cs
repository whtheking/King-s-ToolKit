using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(LogicNode))]
public class LogicNodePropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var state = property.FindPropertyRelative("m_state");
        string stateString = "";
        var richTextStyle = new GUIStyle(GUI.skin.label); richTextStyle.richText = true;
        switch (state.enumValueIndex)
        {
            case 1:
                stateString = "<color=white>Idle</color>";
                break;
            case 2:
                stateString = "<color=yellow>Running</color>";
                break;
            case 3:
                stateString = "<color=green>Success</color>";
                break;
            case 4:
                stateString = "<color=red>Failed</color>";
                break;
        }
        EditorGUILayout.LabelField("State: ",stateString, richTextStyle);
        var name = property.FindPropertyRelative("m_name");
        name.stringValue = EditorGUILayout.TextField("Name: ", name.stringValue);
    }


}