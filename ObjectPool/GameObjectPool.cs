using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class GameObjectPool<T> where T : IPoolObject
{
    private Queue<GameObject> m_pool = new();
    private GameObject m_template;
    private Transform m_container;
    private int m_count;
    public void InitPool(GameObject template, Transform Container)
    {
        m_template = template;
        m_container = Container;
    }

    public void ReleasePool()
    {
        foreach (var poolObject in m_pool)
        {
            var poolObjectComp = poolObject.GetComponent<IPoolObject>();
            poolObjectComp.OnObjectDestroy();
        }
        m_pool.Clear();
    }

    public GameObject CreateObject()
    {
        if (m_template == null)
            return null;
        GameObject poolObject = null;
        if (m_pool.Count > 0)
        {
            poolObject = m_pool.Dequeue();
            poolObject.GetComponent<IPoolObject>().OnObjectCreate();
        }
        else
        {
            poolObject = GameObject.Instantiate(m_template, m_container);
            poolObject.GetComponent<IPoolObject>().ActionOnRelease += () =>
            {
                m_pool.Enqueue(poolObject);
            };
        }
        return poolObject;
    }
}