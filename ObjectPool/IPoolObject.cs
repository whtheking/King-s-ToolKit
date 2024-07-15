using System;

public interface IPoolObject
{
    Action ActionOnRelease { get; set; }
    abstract void OnObjectCreate();
    void OnObjectRelease()
    {
        if (ActionOnRelease != null)
            ActionOnRelease();
    }
    abstract void OnObjectDestroy();

}