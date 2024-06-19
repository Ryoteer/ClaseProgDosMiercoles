using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicRequester : MonoBehaviour
{
    [Header("<color=yellow>Audio</color>")]
    [SerializeField] private AudioClip _clipToPlay;

    private void Start()
    {
        MusicManager.Instance.PlayMusicClip(_clipToPlay);
    }
}
