using System.Collections.Generic;

public enum ENodeState
{
    None = 0,
    Idle = 1,
    Running = 2,
    Success = 3,
    Fail = 4
}

public interface INode
{
    public ENodeState State { get; }

    public string Name { get; set; }

    public string GUID { get; set; }

    public INode Root { get; }

    public INode Parent { get; }

    public List<INode> Children { get; }


    public void Init() { }

    public void Enter() { }

    public ENodeState Update()
    {
        return State;
    }

    public void Exit() { }

    public void AddChild(INode child)
    {
        Children.Add(child);
    }

    public void RemoveChild(INode child)
    {
        Children.Remove(child);
    }
}