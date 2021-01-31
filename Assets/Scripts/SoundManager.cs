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

    static AudioSource audioSource;

    void Start()
    {
        llamaSound = Resources.Load<AudioClip>("llama");
        pewSound = Resources.Load<AudioClip>("pew");
        goalAchievedSound = Resources.Load<AudioClip>("");
        enemyDeathSound = Resources.Load<AudioClip>("blahDeath");
        powSound = Resources.Load<AudioClip>("pow");
        hergSound = Resources.Load<AudioClip>("herg");

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
            default:
                audioSource.PlayOneShot(hergSound);
                break;
        }
    }
}
