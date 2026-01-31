using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    public void SetMasterVolume(float level)
    {
        
        audioMixer.SetFloat("MasterVol", Mathf.Log10(level) * 20.0f);
    }

    public void SetMusicVolume(float level)
    {
        audioMixer.SetFloat("MusicVol", Mathf.Log10(level) * 20.0f);
    }

    public void SetSFXVolume(float level)
    {
        audioMixer.SetFloat("SFXVol", Mathf.Log10(level) * 20.0f);
    }
}
