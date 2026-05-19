using UnityEngine;

public class TimescaleMultComponent : MonoBehaviour
{
    [SerializeField] private FloatReference mult = new FloatReference(true, 1f); 
    private FloatMultiplier floatMult;
    private void Awake()
    {
        floatMult = new FloatMultiplier(() => {return mult; });
    }
    private void OnEnable()
    {
        TimescaleKeeper.timeScale.AddModifier(floatMult);
    }
    private void OnDisable()
    {
        TimescaleKeeper.timeScale.RemoveModifier(floatMult);
    }
}
