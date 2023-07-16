using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    AudioSource audioSource;

    void Awake()
    {
        audioSource= GetComponent<AudioSource>();// находим ссылку на AudioSource
    }

    public void PlaySound(AudioClip sound)
    {
        audioSource.PlayOneShot(sound); // вкл конкретную музыку  1 раз
                                        //не то который подставлен в AudioSource
                                        // а то который мы ему передаем
    }
}
