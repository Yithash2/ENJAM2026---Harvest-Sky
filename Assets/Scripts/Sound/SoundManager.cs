using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource _source;

    public static SoundManager Instance;

    private void Awake()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
    }
    public void PlaySFX(AudioClip clip)
    {
        _source.PlayOneShot(clip);
    }
}
