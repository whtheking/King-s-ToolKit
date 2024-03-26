using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class NodeData : ScriptableObject
{
    public string GUID;
    public LogicNode LogicNode;
    public GraphNodeData GraphNode;

    public NodeData(string gUID, LogicNode logicNode, GraphNodeData graphNode)
    {
        GUID = gUID;
        LogicNode = logicNode;
        GraphNode = graphNode;
    }
}


[CreateAssetMenu(menuName = "StageRunner")]
public class GraphDataSaver : ScriptableObject
{

    private List<NodeData> m_nodes = new();

    public List<NodeData> Nodes { get => m_nodes; }

    public void SaveNode(LogicNode logicNode, GraphNodeData graphNodeData)
    {
        m_nodes.Add(new NodeData(logicNode.GUID, logicNode, graphNodeData));
    }

    public void RemoveNode(GraphNode node)
    {
        m_nodes.RemoveAll(x => x.GUID == node.graphNodeData.nodeGUID);
        AssetDatabase.SaveAssets();
    }
}

