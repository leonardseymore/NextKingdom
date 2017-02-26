using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public partial class Game : MonoBehaviour {

    [Serializable]
    public class SoundFxs
    {
        public AudioClip[] Sword;
        public AudioClip[] Shield;
        public AudioClip[] Explosion;
        public AudioClip[] EvilLaugh;
        public AudioClip[] Monster;
        public AudioClip[] Totem;
        public AudioClip[] Queen;
        public AudioClip[] ButtonClick;

        public AudioClip RandomSword
        {
            get
            {
                return Sword[Random.Range(0, Sword.Length)];
            }
        }

        public AudioClip RandomShield
        {
            get
            {
                return Shield[Random.Range(0, Shield.Length)];
            }
        }

        public AudioClip RandomExplosion
        {
            get
            {
                return Explosion[Random.Range(0, Explosion.Length)];
            }
        }

        public AudioClip RandomEvilLaugh
        {
            get
            {
                return EvilLaugh[Random.Range(0, EvilLaugh.Length)];
            }
        }

        public AudioClip RandomMonster
        {
            get
            {
                return Monster[Random.Range(0, Monster.Length)];
            }
        }


        public AudioClip RandomTotem
        {
            get
            {
                return Totem[Random.Range(0, Totem.Length)];
            }
        }


        public AudioClip RandomQueen
        {
            get
            {
                return Queen[Random.Range(0, Queen.Length)];
            }
        }

        public AudioClip RandomButtonClick
        {
            get
            {
                return ButtonClick[Random.Range(0, ButtonClick.Length)];
            }
        }
    }

    public SoundFxs soundFxs;
    public AudioMixerGroup audioMixerGroup;
    public int NumAudioSources = 8;
    List<AudioSource> audioSources = new List<AudioSource>();
    int nextAudioSource = 0;

    void AwakeAudio()
    {
        for (int i = 0; i < NumAudioSources; i++)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.outputAudioMixerGroup = audioMixerGroup;
            audioSource.playOnAwake = false;
            audioSource.loop = false;
            audioSources.Add(audioSource);
        }
    }

    public void PlayAudioSourceRandom(AudioSource audio)
    {
        if (Random.Range(0, 10) < 3)
        {
            audio.Play();
        }
    }


    private void AudioPlay(AudioClip clip)
    {
        AudioSource audioSource = audioSources[nextAudioSource];
        audioSource.clip = clip;
        audioSource.Play();
        nextAudioSource++;
        if (nextAudioSource > NumAudioSources - 1)
        {
            nextAudioSource = 0;
        }
    }

    public void AudioPlaySword()
    {
        AudioPlay(soundFxs.RandomSword);
    }

    public void AudioPlayShield()
    {
        AudioPlay(soundFxs.RandomShield);
    }

    public void AudioPlayExplosion()
    {
        AudioPlay(soundFxs.RandomExplosion);
    }

    public void AudioPlayEvilLaugh()
    {
        AudioPlay(soundFxs.RandomEvilLaugh);
    }

    public void AudioPlayMonster()
    {
        AudioPlay(soundFxs.RandomMonster);
    }

    public void AudioPlayTotem()
    {
        AudioPlay(soundFxs.RandomTotem);
    }

    public void AudioPlayQueen()
    {
        AudioPlay(soundFxs.RandomQueen);
    }

    public void AudioPlayButtonClick()
    {
        AudioPlay(soundFxs.RandomButtonClick);
    }
}
