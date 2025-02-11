using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    private float musicVolum = 0.5f;
    private float soundVolum = 0.5f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetMusicVolumne(float volumn)
    {
        musicVolum = volumn;
    }

    public float GetMusicVolumne()
    {
        return musicVolum;
    }


    public void SetSoundVolumne(float volumn)
    {
        soundVolum = volumn;
    }

    public float GetSoundVolumne()
    {
        return soundVolum;
    }
}
