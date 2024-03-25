using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "StageRunner")]
public class GraphDataSaver : ScriptableObject
{
    public class NodeData
    {
        public string GUID;
        public INode LogicNode;
        public GraphNodeData graphNode;

        public NodeData(string gUID, INode logicNode, GraphNodeData graphNode)
        {
            GUID = gUID;
            LogicNode = logicNode;
            this.graphNode = graphNode;
        }
    }

    private List<NodeData> m_nodes = new();

    public List<NodeData> Nodes { get => m_nodes; }

    public void SaveNode(INode logicNode, GraphNodeData graphNodeData)
    {
        m_nodes.Add(new NodeData(logicNode.GUID, logicNode, graphNodeData));
    }

    public void RemoveNode(GraphNode node)
    {
        m_nodes.RemoveAll(x => x.GUID == node.graphNodeData.nodeGUID);
        AssetDatabase.SaveAssets();
    }
}

