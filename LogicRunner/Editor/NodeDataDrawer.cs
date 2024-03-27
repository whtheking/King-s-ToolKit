using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NodeData))]
public class NodeDataDrawer : Editor
{
    public GraphNode graphNode;
    bool foldOut = false;
    public override void OnInspectorGUI()
    {
        var target = serializedObject.targetObject as NodeData;

        var logicNode = serializedObject.FindProperty("LogicNode");
        var guid = serializedObject.FindProperty("GUID");
        EditorGUILayout.PropertyField(logicNode);

        var graphNodeData = serializedObject.FindProperty("GraphNode");
        var pos = graphNodeData.FindPropertyRelative("viewPosition");
        var grayStyle = new GUIStyle();
        grayStyle.normal.textColor = Color.gray;
        foldOut = EditorGUILayout.Foldout(foldOut, "Static Info");
        if (foldOut)
        {
            EditorGUILayout.LabelField($"GUID: {guid.stringValue} ", grayStyle);
            EditorGUILayout.LabelField($"X: {pos.vector2Value.x}  Y: {pos.vector2Value.y}", grayStyle);
        }
        serializedObject.ApplyModifiedProperties();

        if (graphNode != null)
        {
            graphNode.title = logicNode.FindPropertyRelative("m_name").stringValue;
        }

    }
}