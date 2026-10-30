using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip[] _placeBuildingSounds;
    [SerializeField] private AudioClip _loreReadSound;
    [SerializeField] private AudioClip _buildButton;

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

    public void PlaceBuildingSound()
    {
        PlaySFX(_placeBuildingSounds[UnityEngine.Random.Range(0, 3)]);
    }
    public void LoreSound()
    {
        PlaySFX(_loreReadSound);
    }
    public void BuildButtonSound()
    {
        PlaySFX(_buildButton);
    }
}
