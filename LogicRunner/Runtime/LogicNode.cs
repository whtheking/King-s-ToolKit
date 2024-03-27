using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LogicNode 
{
    [SerializeField]
    private ENodeState m_state = ENodeState.Idle;
    [SerializeField]
    private string m_name;
    private string m_guid;
    private LogicNode m_root;
    private LogicNode m_parent;
    private List<LogicNode> m_children = new();

    public virtual ENodeState State => m_state;

    public virtual LogicNode Root => m_root;

    public virtual LogicNode Parent => m_parent;

    public virtual List<LogicNode> Children => m_children;

    public virtual string Name { get => m_name; set { m_name = value; } }

    public virtual string GUID { get => m_guid; set { m_guid = value; } }

    public virtual void Enter()
    {
        Debug.Log("NodeEnter");
    }

    public virtual void Exit()
    {
        Debug.Log("NodeExit");
    }

    public virtual void Init()
    {
        Debug.Log("NodeInit");
    }

    public virtual ENodeState Update()
    {
        return m_state;
    }

    public virtual void AddChild(LogicNode child)
    {
        Children.Add(child);
    }

    public virtual void RemoveChild(LogicNode child)
    {
        Children.Remove(child);
    }
}