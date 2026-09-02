using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.ComponentModel;
using System.Linq;

public abstract class RuntimeSet : ScriptableObject
{
    [TextArea(4, 6)]
    [SerializeField] string note;

    [Tooltip("Just for debugging")]
    [SerializeField] protected int currentLength;

    public abstract void TryAdd(object o);
    public abstract void TryRemove(object o);
}
/// <summary>
/// A collection of references that can be popuplated and managed at runtime.
/// </summary>
public abstract class RuntimeSet<T> : RuntimeSet, IVariableAccess<T>
{
    [SerializeField, ReadOnly(true)] private List<T> list = new List<T>();
    public IReadOnlyCollection<T> collection => list;

    public event Action RemoveEvent;
    public event Action EmptyEvent;
    public event Action ChangeEvent;

    void OnValidate()
    {
        currentLength = Count;
    }

    public void Add(T item)
    {
        list.Add(item);
#if UNITY_EDITOR
        currentLength = Count;
#endif
        OnChange();
    }


    public void Remove(T item)
    {
        list.Remove(item);
        RemoveEvent?.Invoke();
        if (list.Count <= 0)
        {
            EmptyEvent?.Invoke();
        }
#if UNITY_EDITOR
        currentLength = Count;
#endif
        OnChange();
    }

    public T this[int index]
    {
        get => list[index];
    }

    public T GetAndRemove(int index) {
        T temp = list[index];
        Remove(temp);
        return temp;
    }

    public int Count
    {
        get
        {
            return list.Count;
        }
    }
    void OnChange()
    {
        ChangeEvent?.Invoke();
    }
    protected T GetThingFromObject(object o)
    {
        if (o is T t)
        {
            return t;
        }
        else if (o is GameObject go)
        {
            var c = go.GetComponent<T>();
            return c;
        }
        return default(T);
    }
    public override void TryAdd(object o)
    {
        var thing = GetThingFromObject(o);
        if (thing != null) Add(thing);
    }
    public override void TryRemove(object o)
    {
        var thing = GetThingFromObject(o);
        if (thing != null) Remove(thing);
    }

    /// <summary>
    /// Value default to the first element in the collection
    /// </summary>
    /// <returns></returns>
    public T GetValue()
    {
        return collection.FirstOrDefault();
    }
}