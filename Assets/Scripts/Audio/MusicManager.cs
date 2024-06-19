using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    #region Singleton
    public static MusicManager Instance;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    [Header("<color=yellow>Audio</color>")]
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private AudioSource _source;

    private float _actualMasterVol = 0f, _actualMusicVol = 0f, _actualSFXVol = 0f;
    public float GetMasterVolume { get { return _actualMasterVol; } }
    public float GetMusicVolume { get { return _actualMusicVol; } }
    public float GetSFXVolume { get { return _actualSFXVol; } }

    public void PlayMusicClip(AudioClip clip)
    {
        if (_source.isPlaying)
        {
            _source.Stop();
        }

        _source.clip = clip;

        _source.Play();
    }

    public void SetMasterVolume(float value)
    {
        if (value <= 0) value = 0.0001f;

        _actualMasterVol = value;

        _mixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
    }

    public void SetMusicVolume(float value)
    {
        if (value <= 0) value = 0.0001f;

        _actualMusicVol = value;

        _mixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
    }

    public void SetSFXVolume(float value)
    {
        if (value <= 0) value = 0.0001f;

        _actualSFXVol = value;

        _mixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
    }
}
