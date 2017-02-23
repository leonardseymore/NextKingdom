using UnityEngine;
using UnityStandardAssets.ImageEffects;

public partial class Game : MonoBehaviour {
    public void PlayAudioSourceRandom(AudioSource audio)
    {
        if (Random.Range(0, 10) < 3)
        {
            audio.Play();
        }
    }
}
