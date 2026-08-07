using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public abstract class StateMachineMB<SM, S> : MonoBehaviour, IStateMachine<SM, S>
    where SM : IStateMachine<SM, S>
    where S : IState<SM, S>
{
    [SerializeField] public S state;
    S IStateMachine<SM, S>.State { 
        get => state; 
        set => state = value; 
    }
    public void SetState(S state)
    {
        //(IStateMachine<SM, S>).base.SetState(state);
        ((IStateMachine<SM, S>)this).SetStateInternal(state);
        //((SM)this).SetState(state);
    }
    protected virtual void Update()
    {
        state?.UpdateState();
    }
    protected virtual void FixedUpdate()
    {
        state?.FixedUpdateState();
    }

}