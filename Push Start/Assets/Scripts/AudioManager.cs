using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource coinsAudioSrc;
    public AudioSource buildingsAudioSource;
    [Header("Audio Clips")]
    public AudioClip parkClip;
    public AudioClip housesClip;
    public AudioClip farmClip;
    public AudioClip mallClip;
    public AudioClip factoryClip;

    public void PlayBuildingSound(BuildingType type)
    {
        switch (type)
        {
            case BuildingType.FARM:
                buildingsAudioSource.clip = farmClip;
                break;
            case BuildingType.HOUSES:
                buildingsAudioSource.clip = housesClip;
                break;
            case BuildingType.MALL:
                buildingsAudioSource.clip = mallClip;
                break;
            case BuildingType.PARK:
                buildingsAudioSource.clip = parkClip;
                break;
            case BuildingType.FACTORY:
                buildingsAudioSource.clip = factoryClip;
                break;
            default:
                break;
        }
        buildingsAudioSource.Play();
    }

    public void PlayCoinSound()
    {
        coinsAudioSrc.PlayOneShot(coinsAudioSrc.clip, 1f);
    }
}
