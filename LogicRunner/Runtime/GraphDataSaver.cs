using NPOI.POIFS.Properties;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class NodeData : ScriptableObject
{
    public string GUID;
    public LogicNode LogicNode;
    public GraphNodeData GraphNode;
}


[CreateAssetMenu(menuName = "StageRunner")]
public class GraphDataSaver : ScriptableObject
{
    [SerializeField]
    private List<NodeData> m_nodes = new();

    public List<NodeData> Nodes { get => m_nodes; }

    public void SaveNode(LogicNode logicNode, GraphNodeData graphNodeData)
    {
        var nodeData = NodeData.CreateInstance<NodeData>();
        nodeData.GUID = logicNode.GUID;
        nodeData.LogicNode = logicNode;
        nodeData.GraphNode = graphNodeData;
        m_nodes.Add(nodeData);
        AssetDatabase.AddObjectToAsset(nodeData, this);
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    public void RemoveNode(GraphNode node)
    {
        m_nodes.RemoveAll(x => x.GUID == node.graphNodeData.nodeGUID);
        AssetDatabase.SaveAssets();
    }
}

