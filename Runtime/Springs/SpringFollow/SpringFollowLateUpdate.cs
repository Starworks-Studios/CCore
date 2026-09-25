using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringFollowLateUpdate : SpringFollow
{
    [SerializeField] bool useScaledTime = true;
    private void LateUpdate()
    {
        Follow(useScaledTime ? Time.deltaTime : TimescaleKeeper.unscaledDeltaTime);
    }
}
