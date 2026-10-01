using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
/*!Class with only static fields/methods that plays audio. All Audio SOs call its scripts.*/
public class SoundManager : MonoBehaviour
{
    //this pair is called whenever playing a directionless oneshot
    private static GameObject oneShotOb;
    private static AudioSource oneShotAudS;

    public static SoundManager _instance;
    /*!Property for the SoundManager script instance. Creates a GameObject and adds a SoundManager if it doesn't exist*/
    public static SoundManager sm
    {
        get
        {
            if (_instance == null)
            {
                var s = new GameObject("SoundManager");
                _instance = s.AddComponent<SoundManager>();
            }
            return _instance;
        }
    }

    public static void PlayOneShot(SoundSO sound, float volume = 1f)
    {
        if (oneShotOb == null)
        {
            MakeSoundObject(out oneShotOb, out oneShotAudS);
        }
        oneShotAudS.PlayOneShot(sound.Clip, volume * sound.Volume);
    }


    /*!Calls the position Play() implementation with no position.*/
    public static AudioSource Play(SoundSO sound) {
        return Play(sound, Vector3.zero);
    }
    /*!Creates the proper sound source object, plays its sound at the correct position, then destroys the object after the sound effect plays*/
    public static AudioSource Play(SoundSO sound, Vector3 position) {
        if (!sound) return null;
        var source = GenerateSource(sound);
        if (source)
        {
            source.transform.position = position;
            source.Play();
            DJ.Instance.StartCoroutine(destroyIn(source, source.clip.length/source.pitch + 0.2f));
        }
        return source;
    }
    /*!Creates the necessary AudioSource object from the SO's information, parents it to the SoundManager, and returns.*/
    public static AudioSource GenerateSource(SoundSO sound)
    {
        if (!sound) return null;
        GameObject soundOb;
        AudioSource source;
        MakeSoundObject(out soundOb, out source);

        if (sound.dontDestroyOnLoad)
        {
            DontDestroyOnLoad(soundOb);
            //Debug.Log($"Dont destroy {sound} ");
        }
        else
        {
            soundOb.transform.SetParent(sm.transform);//Sets parent to the SoundManager
        }
        source.clip = sound.Clip;//Gets random clip from SO
        source.volume = sound.Volume;//Gets volume from SO
        source.pitch = Random.Range(sound.pitchRange.x, sound.pitchRange.y);//Gets random pitch from pitchrange
        source.spatialBlend = sound.spatialBlend;//gets spatialBlend from SO
        if (sound.group) {
            source.outputAudioMixerGroup = sound.group;//gets mixer group if SO has one
        }
        
        return source;
    }
    /*!Loads then instantiates the prefab DefaultAudioSource, gets its AudioSource, then renames it to "Sound"*/
    static void MakeSoundObject(out GameObject ob, out AudioSource source) {
        ob = Instantiate((GameObject)Resources.Load("DefaultAudioSource"));
        source = ob.GetComponent<AudioSource>();
        ob.name = "Sound";
    }
    private static IEnumerator destroyIn(AudioSource source, float time)
    {
        yield return new WaitForSecondsRealtimeCustom(time);
        if (source != null)
        {
            source.Stop();
            Object.Destroy(source.gameObject);
        }
        yield return null;
    }
}
