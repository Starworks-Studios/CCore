using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AddToRuntimeSets<T> : MonoBehaviour
{
    public RuntimeSet<T>[] runtimeSets;
    private T thing;

    protected virtual T GetThing()
    {
        return GetComponent<T>();
    }

    private void OnEnable()
    {
        thing = GetThing();
        foreach (var set in runtimeSets)
        {
            set.Add(thing);
        }
    }

    private void OnDisable()
    {
        foreach (var set in runtimeSets)
        {
            set.Remove(thing);
        }
    }
}