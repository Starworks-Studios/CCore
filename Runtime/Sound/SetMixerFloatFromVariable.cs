using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SetMixerFloatFromVariable : MonoBehaviour
{
    [SerializeField] string variableName = "music volume";
    [SerializeField] FloatReference floatRef;
    [SerializeField] AudioMixer mixer;

    [SerializeField] float decibelOffset = 0f;

    private void OnEnable()
    {
        floatRef.Subscribe(OnFloatRefChange);
    }
    private void OnDisable()
    {
        floatRef.Unsubscribe(OnFloatRefChange);
    }
    private void Start()
    {
        OnFloatRefChange(floatRef.Value);
    }

    void OnFloatRefChange(float newValue)
    {
        newValue = Mathf.Clamp(newValue, 0f, 1f);
        mixer.SetFloat(variableName, PercentToVolume(newValue) + decibelOffset);
        //print("set mixer to " + newValue);
    }

    public static float PercentToVolume(float percentage)
    {
        return Mathf.Log10(Mathf.Max(0.0001f, percentage * 2f)) * 25f;
    }
}
