
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UIElements;

public class LogicRunnerGraphView : UnityEditor.Experimental.GraphView.GraphView
{
    public GraphDataSaver graphDataSaver;
    public GraphNode selectedNode = null;
    public LogicRunnerEditor editorWindow;

    public new class UxmlFactory : UxmlFactory<LogicRunnerGraphView, UxmlTraits> { }

    public LogicRunnerGraphView()
    {


        Insert(0, new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
    }

    public void SetStyle(string ussPath)
    {
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(ussPath);
        styleSheets.Add(styleSheet);
    }

    public void PopulateView(GraphDataSaver dataSaver)
    {
        graphDataSaver = dataSaver;
        graphViewChanged -= OnGraphViewChange;
        DeleteElements(graphElements);
        graphViewChanged += OnGraphViewChange;

        foreach (var nodeData in graphDataSaver.Nodes)
        {
            CreateGraphNode(nodeData.LogicNode, nodeData.GraphNode);
        }
        foreach (var nodeData in graphDataSaver.Nodes)
        {
            var parentView = GetNodeView(nodeData.GUID);
            foreach (var child in nodeData.LogicNode.Children)
            {
                var childView = GetNodeView(child.GUID);
                var edge = parentView.outputPort.ConnectTo(childView.inputPort);
                AddElement(edge);
            }
        }
    }

    public GraphNode GetNodeView(string guid)
    {
        return GetNodeByGuid(guid) as GraphNode;
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        base.BuildContextualMenu(evt);
        var mousePos = evt.localMousePosition;
        {
            List<Type> types = new()
            {
                typeof(LogicNode)
            };
            types.AddRange(TypeCache.GetTypesDerivedFrom(typeof(LogicNode)));
            foreach (var type in types)
            {
                evt.menu.AppendAction($"{type.Name}", (_) =>
                {
                    CreateNode(type, mousePos);
                });
            }
        }
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.ToList().Where(port => port.direction != startPort.direction && port.node != startPort.node).ToList();
    }

    public void CreateNode(Type type, Vector2 mousePosition)
    {
        var node = Activator.CreateInstance(type) as LogicNode;
        node.Name = type.Name;
        node.GUID = GUID.Generate().ToString();
        var graphNodeData = new GraphNodeData();
        graphNodeData.viewPosition = mousePosition;
        graphNodeData.name = type.Name;
        graphNodeData.nodeGUID = node.GUID;
        CreateGraphNode(node, graphNodeData);
        graphDataSaver.SaveNode(node, graphNodeData);
    }

    public void CreateGraphNode(LogicNode node, GraphNodeData graphNodeData)
    {
        GraphNode graphNode = new GraphNode(node, graphNodeData, this);
        AddElement(graphNode);
        graphNode.onSelected = OnNodeSelected;
    }


    private void OnNodeSelected(GraphNode node)
    {
        editorWindow.UpdateInspector(node);
    }

    private GraphViewChange OnGraphViewChange(GraphViewChange graphViewChange)
    {
        if (graphViewChange.elementsToRemove != null)
        {
            foreach (var element in graphViewChange.elementsToRemove)
            {
                var edge = element as Edge;
                if (edge != null)
                {
                    GraphNode parent = edge.output.node as GraphNode;
                    GraphNode child = edge.input.node as GraphNode;
                    parent.Node.RemoveChild(child.Node);
                }

                var node = element as GraphNode;
                if (node != null)
                {
                    graphDataSaver.RemoveNode(node);
                }

            }
        }
        if (graphViewChange.edgesToCreate != null)
        {
            foreach (var edge in graphViewChange.edgesToCreate)
            {
                GraphNode parent = edge.output.node as GraphNode;
                GraphNode child = edge.input.node as GraphNode;
                parent.Node.AddChild(child.Node);
            };
        }
        {

        }
        return graphViewChange;
    }


}