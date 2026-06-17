using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class OnKeyDown : MonoBehaviour
{
    [SerializeField] KeyCode key;
    [SerializeField] InputActionReference inputReference;
    [SerializeField] UnityEvent keyDownEvent;

    private void OnEnable()
    {
        if(inputReference) inputReference.action.performed += FireEvent;
    }
    private void OnDisable()
    {
        if(inputReference) inputReference.action.performed -= FireEvent;
    }
    private void Update()
    {
        if (Input.GetKeyDown(key))
        {
            FireEvent();
        }
    }
    void FireEvent(InputAction.CallbackContext context)
    {
        FireEvent();
    }
    void FireEvent()
    {
        keyDownEvent?.Invoke();
    }
}
