using System;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GraphNode : Node
{
    //�߼�����
    private LogicNode m_node;

    //�༭������
    public GraphNodeData graphNodeData;

    public LogicRunnerGraphView logicRunnerGraphView;

    public Port inputPort;
    public Port outputPort;
    public Action<GraphNode> onSelected = null;

    public LogicNode Node { get => m_node; }

    public GraphNode(LogicNode node, GraphNodeData data, LogicRunnerGraphView runnerView)
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
        inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(LogicNode));
        inputPort.portName = "In";
        inputContainer.Add(inputPort);
        outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(LogicNode));
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