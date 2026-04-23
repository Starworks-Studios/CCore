using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Modifier
{
    public Event ChangedEvent = new Event();
    protected void JustChanged()
    {
        ChangedEvent.Trigger();
    }
    private bool removeMe = false;
    public virtual bool RemoveMe => removeMe; // when true, this modifier will be removed from mod variables
    public void RemoveFromAllModifiables() { removeMe = true; }
    public bool skip { get; private set; } = false;
    public void SetSkip(bool shouldSkip) { skip = shouldSkip; }
}
public abstract class Modifier<Data, Mode> : Modifier
{
    public abstract Data Modify(ref Data data, in Mode mode);
}

public abstract class Modifier<Data> : Modifier<Data, bool> { }