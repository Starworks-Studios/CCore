using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
///Abstract class outlining a SO that holds a variable and fires events when its value is changed.
/// </summary>

public abstract class IVariableSO : ScriptableObject, ISubscribable<Action>
{
    [TextArea(3, 5)]
    [SerializeField] string description;

    protected event Action VoidChangeEvent;

    protected virtual void OnEnable()
    {
        hideFlags = HideFlags.DontUnloadUnusedAsset; //Colin: may stop data getting reset between scene loads
    }

    public void Subscribe(Action function)
    {
        VoidChangeEvent += function;
    }
    public void Unsubscribe(Action function)
    {
        VoidChangeEvent -= function;
    }
    protected void InvokeVoidEvent()
    {
        VoidChangeEvent?.Invoke();
    }
}

// This SO has a value that can be get or set. That variable does not necessarily live in this SO, though.
public abstract class IVariableSO<T> : IVariableSO, ISubscribable<Action<T>>
{
    public event Action<T> ChangeEvent;
    public T Value
    {
        get
        {
            return GetValue();
        }
        set
        {
            SetValue(value);
            InvokeChangeEvent(value);
        }
    }

    // Get the value at the heart of this Variable. We may have to traverse through several IVariableSOs to get there, so there is a maximum depth.
    public abstract T GetValue(int depth=0, int maxDepth=10);
    public static implicit operator T(IVariableSO<T> vr) => vr.Value; //implicitly cast
    protected abstract void SetValue(T newValue);

    protected virtual void OnValidate()
    {
        InvokeChangeEvent(Value);
    }

    public void InvokeChangeEvent()
    {
        InvokeChangeEvent(Value);
    }

    protected virtual void InvokeChangeEvent(T v)
    {
        ChangeEvent?.Invoke(v);
        InvokeVoidEvent();
    }

    public void Subscribe(Action<T> function)
    {
        ChangeEvent += function;
    }

    public void Unsubscribe(Action<T> function)
    {
        ChangeEvent -= function;
    }

    public override string ToString()
    {
        return "" + Value;
    }
}
