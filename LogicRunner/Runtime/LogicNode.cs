using System.Collections.Generic;
using UnityEngine;

public class LogicNode : MonoBehaviour, INode
{

    private ENodeState m_state = ENodeState.Idle;
    private INode m_root;
    private INode m_parent;
    private List<INode> m_children = new();
    private string m_name;
    private string m_guid;

    public ENodeState State => m_state;

    public INode Root => m_root;

    public INode Parent => m_parent;

    public List<INode> Children => m_children;

    public string Name { get => m_name; set { m_name = value; } }

    public string GUID { get => m_guid; set { m_guid = value; } }

    public void Enter()
    {
        Debug.Log("NodeEnter");
    }

    public void Exit()
    {
        Debug.Log("NodeExit");
    }

    public void Init()
    {
        Debug.Log("NodeInit");
    }

    public ENodeState Update()
    {
        return m_state;
    }
}