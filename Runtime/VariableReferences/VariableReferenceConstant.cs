using UnityEngine;
using System;

[System.Serializable]
public class VariableReferenceConstant<T> : ISerializationCallbackReceiver, IVariableAccess<T>
{
    [SerializeField] private T constantValue;
    private event Action<T> ChangeEvent;
    private event Action ChangeEventNullParam;

    public VariableReferenceConstant(T value = default(T))
    {
        constantValue = value;
    }
    public T Value { get { return GetValue(); } set { SetValue(value); } }
    public T GetValue()
    {
        return constantValue;
    }
    public void SetValue(T value)
    {
        constantValue = value;
        InvokeChangeEvent(value);
    }
    public void Subscribe(Action function)
    {
        ChangeEventNullParam += function;
        function();
    }
    public void Unsubscribe(Action function)
    {
        ChangeEventNullParam -= function;
    }
    public void Subscribe(Action<T> function)
    {
        ChangeEvent += function;
        function(GetValue());
    }
    public void Unsubscribe(Action<T> function)
    {
        ChangeEvent -= function;
    }
    void InvokeChangeEvent(T v)
    {
        ChangeEvent?.Invoke(v);
        ChangeEventNullParam?.Invoke();
    }
    void OnValidate()
    {
        InvokeChangeEvent(Value);
    }
    void ISerializationCallbackReceiver.OnBeforeSerialize() => this.OnValidate();
    void ISerializationCallbackReceiver.OnAfterDeserialize() { }
}
