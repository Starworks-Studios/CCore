using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForSecondsRealtimeFreezable : CustomYieldInstruction
{
    private float secondsRemaining;
    public override bool keepWaiting
    {
        get
        {
            if (Time.timeScale > 0f) secondsRemaining -= TimescaleKeeper.unscaledDeltaTime;

            return secondsRemaining > 0f;
        }
    }

    public WaitForSecondsRealtimeFreezable(float seconds)
    {
        secondsRemaining = seconds;
    }
}
public class WaitForSecondsRealtimePausible : CustomYieldInstruction
{
    private float secondsRemaining;
    public override bool keepWaiting
    {
        get
        {
            if (!TimescaleKeeper.IsPaused) secondsRemaining -= TimescaleKeeper.unscaledDeltaTime;

            return secondsRemaining > 0f;
        }
    }

    public WaitForSecondsRealtimePausible(float seconds)
    {
        secondsRemaining = seconds;
    }
}