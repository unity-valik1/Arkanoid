using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    AudioSource audioSource;

    void Awake()
    {
        audioSource= GetComponent<AudioSource>();// ������� ������ �� AudioSource
    }

    public void PlaySound(AudioClip sound)
    {
        audioSource.PlayOneShot(sound); // ��� ���������� ������  1 ���
                                        //�� �� ������� ���������� � AudioSource
                                        // � �� ������� �� ��� ��������
    }
}
