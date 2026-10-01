using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForSecondsRealtimeCustom : CustomYieldInstruction
{
    private float secondsRemaining;
    public override bool keepWaiting
    {
        get
        {
            if (CanCountDown) secondsRemaining -= TimescaleKeeper.unscaledDeltaTime;

            return secondsRemaining > 0f;
        }
    }

    protected virtual bool CanCountDown => true;

    public WaitForSecondsRealtimeCustom(float seconds)
    {
        secondsRemaining = seconds;
    }
}

public class WaitForSecondsRealtimeFreezable : WaitForSecondsRealtimeCustom
{
    protected override bool CanCountDown => Time.timeScale > 0f;

    public WaitForSecondsRealtimeFreezable(float seconds) : base(seconds) { }
}
public class WaitForSecondsRealtimePausible : WaitForSecondsRealtimeCustom
{
    protected override bool CanCountDown => !TimescaleKeeper.IsPaused;
    public WaitForSecondsRealtimePausible(float seconds) : base(seconds) { }
}