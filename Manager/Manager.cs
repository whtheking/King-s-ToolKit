using UnityEngine;

public abstract class Manager<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T s_instance;

    public static T Instance { get { return s_instance; } }

    public abstract void Init();
    public abstract void Tick(float dt);
    public abstract void Uninit();


    private void Awake()
    {
        s_instance = this as T;
    }

    private void OnDestroy()
    {
        s_instance = null;
    }

}