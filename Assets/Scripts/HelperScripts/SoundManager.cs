using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{

    private static AudioSource shootAudioSource;

    private static AudioSource audioSource;

    public static void PlaySound(AudioClip clip)
    {
        if (audioSource == null)
        {
            GameObject sound = new GameObject("Sound", typeof(AudioSource));
            audioSource = sound.GetComponent<AudioSource>();
        }

        audioSource.PlayOneShot(clip);
    }

    public static void PlayShootSound(AudioClip shootSound)
    {
        if(shootAudioSource == null)
        {
            GameObject sound = new GameObject("ShootSound", typeof(AudioSource));
            shootAudioSource = sound.GetComponent<AudioSource>();
        }

        shootAudioSource.PlayOneShot(shootSound);
    }
}
