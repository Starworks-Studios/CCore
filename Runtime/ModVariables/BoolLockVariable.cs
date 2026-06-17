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
        JustChanged();
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
public class BoolLockVariable : ModVariable<bool, ABoolLock>
{
    List<BoolLockVariable> subLockVars = new();
    public bool IsLocked()
    {
        if (subLockVars.Any(lockVar => lockVar.IsLocked())) return true;
        return GetValue();
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
