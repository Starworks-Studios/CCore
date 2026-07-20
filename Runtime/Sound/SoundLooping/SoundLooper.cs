using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SoundLooper : MonoBehaviour
{
    public SoundSO sound;
    public SoundLoopSO soundLoop;
    public bool startEnabled = false;
    [Tooltip("Create and play silent audiosources even while the loop is not enabled")]
    public bool playAlways = false;

    [Range(0f,1f)]
    public float volume = 1f;
    private float fadeVolume = 1f;

    [Range(0f, 3f)]
    [SerializeField] public float pitch = 1f;

    [SerializeField] float fadeDuration = 0.5f;
    [SerializeField] Ease fadeEase;
    private Tween fadeTween;

    public bool loopEnabled { get; private set; } = false;
    private bool loopCoroutineRunning = false;
    private Coroutine loopingRoutine;

    private float TotalVolume
    {
        get
        {
            return volume * fadeVolume;
        }
    }

    private void Awake()
    {
        fadeVolume = startEnabled ? 1f : 0f;
        if (startEnabled || playAlways) Enable(true);
    }

    private void OnEnable()
    {
        Enable(loopEnabled);
    }
    private void OnDisable()
    {
        loopCoroutineRunning = false;
    }
    public void TrySetLoopEnable(bool enable)
    {
        if (loopEnabled == enable) return;
        Enable(enable);
    }
    public void SetVolume(float volume)
    {
        this.volume = volume;
    }
    private void Enable(bool enable)
    {
        loopEnabled = enable;
        //print("enable loop " + enable);
        if (loopEnabled || playAlways)
        {
            if (!loopCoroutineRunning)
            {
                loopCoroutineRunning = true;
                loopingRoutine = StartCoroutine(Looping());
            }
        }
        else if (loopingRoutine != null)
        {
            StopCoroutine(loopingRoutine);
            loopCoroutineRunning = false;
        }

        if(fadeTween != null)
        {
            fadeTween.Kill();
        }
        fadeTween = DOTween.To(() => fadeVolume, f => fadeVolume = f, enable ? 1f : 0f, fadeDuration).SetEase(fadeEase).SetUpdate(true);
    }

    private IEnumerator Looping()
    {
        //Debug.Log("start looping");
        while (true)
        {
            var source = SoundManager.Play(sound, transform.position);
            source.transform.SetParent(transform);

            //This part stops the crackle when a new clip starts
            source.Stop();
            source.volume = 0f;
            source.Play();
            //source.time = 0.05f;

            StartCoroutine(LoopingSource(source));
            // here I set it to "affected by the time scale"
            float clipLength = source.clip.length;
            yield return new WaitForSecondsRealtime(sound.minPlayDelay);
            float t = 0f;
            float pitch = 1f;
            float a = 1f / (clipLength * soundLoop.startStagger);
            while (true)
            {
                if (source)
                {
                    pitch = source.pitch;
                }
                t += Time.unscaledDeltaTime*pitch*a;
                if (t >= 1f) break;
                yield return null;
            }
            //yield return new WaitForSecondsRealtime(source.clip.length*soundLoop.startStagger / source.pitch + sound.minPlayDelay);
        }
    }

    private IEnumerator LoopingSource(AudioSource source)
    {
        float duration = source.clip.length;
        float progress = 0;
        float pitchOffset = source.pitch - 1f;
        while (progress < 1f && source != null)
        {
            //progress = Mathf.Min(1f, progress + Time.unscaledDeltaTime / duration);
            progress = Mathf.Min(1f, progress + Time.unscaledDeltaTime / (duration * pitch));
            source.volume = sound.Volume * TotalVolume * soundLoop.volumeCurve.Evaluate(progress);
            source.pitch = pitch + pitchOffset;
            yield return null;
        }
        if (source != null) source.Stop();
        yield return null;
    }
}
