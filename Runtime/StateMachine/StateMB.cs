using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMB<SM, S> : MonoBehaviour, IState<SM, S>
    where SM : IStateMachine<SM, S>
    where S : StateMB<SM, S>
{
    protected SM stateMachine;

    public SM StateMachine { 
        get => stateMachine; 
        set => stateMachine = value; 
    }

    protected virtual void Awake()
    {
        StateMachine = GetComponentInParent<SM>();
    }

    protected virtual void OnDisable() { }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual bool Equals(S other) {
        return this == other;
    }
    public void SetState(S state)
    {
        ((IState<SM, S>)this).SetStateInternal(state);
    }
    public virtual void FixedUpdateState() { }
    public virtual void UpdateState() { }
}
