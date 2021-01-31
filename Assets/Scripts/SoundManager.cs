using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static AudioClip powSound;
    public static AudioClip enemyDeathSound;
    public static AudioClip goalAchievedSound;
    public static AudioClip llamaSound;
    public static AudioClip pewSound;
    public static AudioClip hergSound;
    public static AudioClip regularGunshotSound;
    public static AudioClip footstepsSound;

    static AudioSource audioSource;

    void Start()
    {
        llamaSound = Resources.Load<AudioClip>("llama");
        pewSound = Resources.Load<AudioClip>("pew");
        enemyDeathSound = Resources.Load<AudioClip>("blahDeath");
        powSound = Resources.Load<AudioClip>("pow");
        hergSound = Resources.Load<AudioClip>("herg");
        regularGunshotSound = Resources.Load<AudioClip>("gunshot");
        footstepsSound = Resources.Load<AudioClip>("footsteps");

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {

    }

    public static void PlaySound(string sound)
    {
        switch (sound)
        {
            case "llama":
                audioSource.PlayOneShot(llamaSound);
                break;
            case "pow":
                audioSource.PlayOneShot(powSound);
                break;
            case "pew":
                audioSource.PlayOneShot(pewSound);
                break;
            case "death":
                audioSource.PlayOneShot(enemyDeathSound);
                break;
            case "gunshot":
                audioSource.PlayOneShot(regularGunshotSound);
                break;
            case "footsteps":
                audioSource.PlayOneShot(footstepsSound);
                break;
            default:
                audioSource.PlayOneShot(hergSound);
                break;
        }
    }

    public static void StopSound()
    {
        audioSource.Stop();
    }
}
