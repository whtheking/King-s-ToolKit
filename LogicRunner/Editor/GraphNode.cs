using System;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GraphNode : Node
{
    //逻辑数据
    private INode m_node;

    //编辑器数据
    public GraphNodeData graphNodeData;

    public LogicRunnerGraphView logicRunnerGraphView;

    public Port inputPort;
    public Port outputPort;
    public Action<GraphNode> onSelected = null;

    public INode Node { get => m_node; }

    public GraphNode(INode node, GraphNodeData data, LogicRunnerGraphView runnerView)
    {
        graphNodeData= data;
        m_node = node;
        title = m_node.Name;
        style.left = graphNodeData.viewPosition.x;
        style.top = graphNodeData.viewPosition.y;
        viewDataKey = node.GUID;
        logicRunnerGraphView = runnerView;
        CreatePorts();
    }

    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);
        graphNodeData.viewPosition = newPos.position;
    }

    private void CreatePorts()
    {
        inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(INode));
        inputPort.portName = "In";
        inputContainer.Add(inputPort);
        outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(INode));
        outputPort.portName = "Out";
        outputContainer.Add(outputPort);
        RefreshExpandedState();
        RefreshPorts();
    }

    public override void OnSelected()
    {
        base.OnSelected();
        onSelected(this);
    }
}