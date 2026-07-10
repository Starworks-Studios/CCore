using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Variable/Float")]
public class FloatVariableSO : VariableSO<float>
{
    public void PrintValue()
    {
        Debug.Log(name + " value: " + Value);
    }
    public void Increment(float change)
    {
        Value += change;
    }

}