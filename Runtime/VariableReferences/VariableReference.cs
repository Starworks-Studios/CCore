using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
/*!A generic class that can schedule events when its value is changed. 
 * COLINHELP - need an explanation on how this class is being used
 */
public class VariableReference<T> : ISerializationCallbackReceiver, IVariableAccess<T>
{
    [SerializeField]
    public bool useConstant = false;
    public T constantValue;

    [SerializeField] private IVariableSO<T> variable;
    private IVariableSO<T> prevVariable;
    T lastSerializedValue;

    private event Action<T> ChangeEvent;
    private event Action ChangeEventNullParam;
    /*!implicitly cast C# generic class as custom VariableReference value.
     * This allows for declaring the generic type of a VariableReference variable like any generic variable.
     * COLINHELP - is that right lol
     */
    public static implicit operator T(VariableReference<T> vr) => vr.Value;
    [SerializeField] bool DEBUG;

    public VariableReference(bool useConstant = false)
    {
        this.useConstant = useConstant;
    }

    public T Value
    {
        get
        {
            return GetValue();
        }
        set
        {
            SetValue(value);
        }
    }
    public T GetValue()
    {
        return GetDeepValue();
    }
    public T GetDeepValue(int depth = 0, int maxDepth = 10)
    {
        return (useConstant || !Variable) ? constantValue : Variable.Value;
    }
    public void SetValue(T value)
    {
        if (useConstant)
        {
            constantValue = value;
            InvokeChangeEvent(value);
        }
        else
        {
            Variable.Value = value;
        }
    }

    public IVariableSO<T> Variable
    {
        get => variable;
        set
        {
            prevVariable?.Unsubscribe(InvokeChangeEvent);
            variable = value;
            prevVariable = variable;
            if (variable)
            {
                variable.Subscribe(InvokeChangeEvent);
                InvokeChangeEvent(variable.Value);
            }
        }
    }

    public bool Equals(T x, T y)
    {
        // 1. Safe Unity Object Null & Lifecycle Check
        if (x is UnityEngine.Object unityX && y is UnityEngine.Object unityY)
        {
            return unityX == unityY; // Correctly uses Unity's custom == operator
        }

        // 2. Fallback for standard C# types (ints, strings, structs, etc.)
        return EqualityComparer<T>.Default.Equals(x, y);
    }

    void OnValidate()
    {
        if (prevVariable != variable)
        {
            Variable = variable;
        }
        if (useConstant)
        {
            if(!Equals(Value, lastSerializedValue))
            {
                InvokeChangeEvent(Value);
                lastSerializedValue = Value;
            }
        }
    }

    void InvokeChangeEvent(T v)
    {
        if (DEBUG) Debug.Log("invoke change");
        ChangeEvent?.Invoke(v);
        ChangeEventNullParam?.Invoke();
    }

    public void Subscribe(Action<T> function)
    {
        SubscribeWithoutNotify(function);
        Variable = variable;
    }
    public void SubscribeAndCall(Action<T> function)
    {
        Subscribe(function);
        function(Value);
    }
    public void SubscribeWithoutNotify(Action<T> function)
    {
        ChangeEvent += function;
    }

    public void Unsubscribe(Action<T> function)
    {
        ChangeEvent -= function;
    }
    public void Subscribe(Action function)
    {
        ChangeEventNullParam += function;
        Variable = variable;
    }

    public void Unsubscribe(Action function)
    {
        ChangeEventNullParam -= function;
    }

    //Allows generic classes to receive OnValidate() callbacks
    void ISerializationCallbackReceiver.OnBeforeSerialize() => this.OnValidate();
    void ISerializationCallbackReceiver.OnAfterDeserialize() { }
    public void PrintValue()
    {
        Debug.Log("reference value: "+Value);
    }
}

//Only concrete (non-generic) classes may be serialized.
//Make a new concrete class below for VariableReferences of different types

[System.Serializable]
public class FloatReference : VariableReference<float>
{
    public FloatReference(float initialValue) : base(true)
    {
        Value = initialValue;
    }
    public FloatReference(bool useConstant = true, float initialValue = 0f) : base(useConstant)
    {
        Value = initialValue;
    }
    public static implicit operator float(FloatReference f) => f.Value;
    public void Clamp(float min, float max)
    {
        Value = Mathf.Clamp(Value, min, max);
    }
}
[System.Serializable]
public class DoubleReference : VariableReference<double>
{

}

[System.Serializable]
public class IntReference : VariableReference<int>
{
    public IntReference()
    {

    }
    public IntReference(int startingValue) : this()
    {
        useConstant = true;
        Value = startingValue;
    }
    // Operations mutate
    //public static IntReference operator +(IntReference a, int b)
    //{
    //    a.Value += b;
    //    return a;
    //}
    //public static IntReference operator -(IntReference a, int b)
    //{
    //    a.Value -= b;
    //    return a;
    //}
}
[System.Serializable]
public class BoolReference : VariableReference<bool>
{
    public BoolReference(bool _useConstant = false) : base(_useConstant)
    {
        
    }
    public void Toggle()
    {
        Value = !Value;
    }
}
[System.Serializable]
public class GameObjectReference : VariableReference<GameObject>
{
    public GameObjectReference(bool useConstant = false) : base(useConstant){}
}
[System.Serializable]
public class AnimationCurveReference : VariableReference<AnimationCurve> { }
[System.Serializable]
public class DoubleCurveReference : VariableReference<ParticleSystem.MinMaxCurve> { }

