using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Event : EventBroadcaster
{
    public event Action myEvent;

    public void Trigger()
    {
        myEvent?.Invoke();
    }

    public void Subscribe(Action function)
    {
        myEvent += function;
    }

    public void Unsubscribe(Action function)
    {
        myEvent -= function;
    }
    public bool HasAnySubscribers => myEvent != null;
}

public class Event<T> : EventBroadcaster<T>, EventBroadcaster
{
    public event Action<T> myEvent;
    private event Action voidEvent;

    public void Trigger(T t)
    {
        myEvent?.Invoke(t);
        voidEvent?.Invoke();
    }

    public void Subscribe(Action<T> function)
    {
        myEvent += function;
    }

    public void Unsubscribe(Action<T> function)
    {
        myEvent -= function;
    }
    public void Subscribe(Action func)
    {
        voidEvent += func;
    }
    public void Unsubscribe(Action func)
    {
        voidEvent -= func;
    }
}
public class Event<T, U> : ISubscribable<Action<T,U>>
{
    public event Action<T,U> myEvent;

    public void Trigger(T t, U u)
    {
        myEvent?.Invoke(t, u);
    }

    public void Subscribe(Action<T, U> function)
    {
        myEvent += function;
    }

    public void Unsubscribe(Action<T, U> function)
    {
        myEvent -= function;
    }
}
