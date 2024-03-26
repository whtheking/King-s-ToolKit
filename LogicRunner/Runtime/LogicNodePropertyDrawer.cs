using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(LogicNode))]
public class LogicNodePropertyDrawer : Editor
{
    public override void OnInspectorGUI()
    {
        
        base.OnInspectorGUI();
        var state = serializedObject.FindProperty("m_state");
        if (state != null)
        {
            EditorGUILayout.EnumPopup((ENodeState)state.enumValueIndex);
        }
        var name = serializedObject.FindProperty("m_name");
        if (name != null)
        {
            name.stringValue = EditorGUILayout.TextField(name.stringValue);
        }
    }

}