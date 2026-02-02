using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager Instance;

    public AudioSource soundFxObject;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        // Spawn in gameobject
        AudioSource audioSource = Instantiate(soundFxObject, spawnTransform.position, Quaternion.identity);

        // Assign the audioclip
        audioSource.clip = audioClip;

        audioSource.volume = volume;

        audioSource.Play();

        float clipDuration = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipDuration);
        
    }
    public void PlayMusicClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        // Spawn in gameobject
        AudioSource audioSource = Instantiate(soundFxObject, spawnTransform.position, Quaternion.identity);

        // Assign the audioclip
        audioSource.clip = audioClip;

        audioSource.volume = volume;

        audioSource.Play();

        float clipDuration = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipDuration);
        
    }

    public void PlayRandomFXClip(AudioClip[] audioClips, Transform spawnTransform, float volume)
    {
        AudioClip audioClip = audioClips[Random.Range(0, audioClips.Length)];

        // Spawn in gameobject
        AudioSource audioSource = Instantiate(soundFxObject, spawnTransform.position, Quaternion.identity);

        // Assign the audioclip
        audioSource.clip = audioClip;

        audioSource.volume = volume;

        audioSource.Play();

        float clipDuration = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipDuration);

    }
}
