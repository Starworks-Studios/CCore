using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public abstract class ABoolLock : Modifier<bool, bool>
{
    public abstract bool IsLocked { get; }
    public void UnlockAndRemove() { RemoveFromAllModifiables(); }
    public override bool Modify(ref bool value, in bool mode)
    {
        if (IsLocked)
        {
            return true;
        }
        else
        {
            return value;
        }
    }
}
public class BoolLock : ABoolLock
{
    bool locked = true;
    public override bool IsLocked => locked;
    public void SetLocked(bool locked)
    {
        this.locked = locked;
        MarkChanged();
    }
}
public class BoolLockFunc : ABoolLock
{
    public override bool IsLocked => func();
    Func<bool> func;
    public BoolLockFunc(Func<bool> func)
    {
        this.func = func;
    }
    //public static implicit operator BoolLockFunc(Func<bool> func)
    //{
    //    return new BoolLockFunc(func);
    //}
}
public class BoolLockReference : ABoolLock
{
    BoolReference boolRef;
    public override bool IsLocked => boolRef ? boolRef.Value : false;
    public BoolLockReference(BoolReference boolRef)
    {
        this.boolRef = boolRef;
        boolRef?.SubscribeAndCall(OnBoolValueChanged);
    }
    ~BoolLockReference()
    {
        boolRef?.Unsubscribe(OnBoolValueChanged);
    }
    void OnBoolValueChanged(bool newValue)
    {
        MarkChanged();
    }

}
public class BoolLockVariable : ModVariable<bool, ABoolLock>
{
    List<BoolLockVariable> subLockVars = new();
    public bool IsLocked()
    {
        return GetValue();
    }
    protected override bool CalculateValue(bool mode)
    {
        if (subLockVars.Any(lockVar => lockVar.IsLocked())) return true;
        return base.CalculateValue(mode);
    }
    protected override bool BaseValue()
    {
        return false;
    }
    public BoolLock Lock()
    {
        var newLock = new BoolLock();
        AddModifier(newLock);
        return newLock;
    }
    public void AddLock(ABoolLock newLock)
    {
        AddModifier(newLock);
    }
    public void AlsoLockWhen(BoolLockVariable otherLockVar)
    {
        subLockVars.Add(otherLockVar);
    }
}
