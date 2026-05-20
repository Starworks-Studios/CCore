using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BoolLock : Modifier<bool, bool>
{
    public bool locked = true;
    public void SetLocked(bool locked)
    {
        this.locked = locked;
        JustChanged();
    }
    public override bool Modify(ref bool value, in bool mode)
    {
        if (locked)
        {
            return true;
        }
        else
        {
            return value;
        }
    }
    public void UnlockAndRemove()
    {
        RemoveFromAllModifiables();
    }
}
public class BoolLockVariable : ModVariable<bool, BoolLock>
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
    public void AddLock(BoolLock newLock)
    {
        AddModifier(newLock);
    }
    public void AlsoLockWhen(BoolLockVariable otherLockVar)
    {
        subLockVars.Add(otherLockVar);
    }
}
