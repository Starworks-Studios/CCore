using UnityEngine;

public class SetVariableReferenceOnStart<T> : MonoBehaviour
{
    [UnityEngine.Serialization.FormerlySerializedAs("boolRef")]
    [SerializeField] VariableReference<T> varRef;
    [SerializeField] T targetValue;

    private void Start()
    {
        varRef.Value = targetValue;
    }
}
