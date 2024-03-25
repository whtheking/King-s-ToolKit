using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LogicNode))]
public class LogicNodePropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property, label);

    }
}