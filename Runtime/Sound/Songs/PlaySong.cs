using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySong : MonoBehaviour
{
    [SerializeField]
    SongSO song;

    private void Start()
    {
        PlayThisSong();
    }
    public void PlayThisSong()
    {
        DJ.PlaySong(song);
    }
    public void Play(SongSO song)
    {
        DJ.PlaySong(song);
    }
    public void StopSong()
    {
        DJ.PlaySong(null);
    }

}
