using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public bool OpenSound;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip ClickSound;
    [SerializeField] private AudioClip WeaponImpact;
    [SerializeField] private AudioClip attackAudio;
    [SerializeField] private AudioClip loseAudio;

    public void PlayClickSound()
    {
        if (OpenSound)
        {
            audioSource.PlayOneShot(ClickSound);
        }
    }

    public void PlayWeaponImpackSound()
    {
        if (OpenSound) audioSource.PlayOneShot(WeaponImpact);
    }

    public void PlayAttackAudio()
    {
        if (OpenSound) audioSource.PlayOneShot(attackAudio);
    }

    public void PlayLoseAudio()
    {
        if (OpenSound) audioSource.PlayOneShot(loseAudio);
    }
}