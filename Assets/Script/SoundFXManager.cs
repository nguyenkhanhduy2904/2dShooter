using System.Security.Cryptography;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager Instance;
    [SerializeField] private AudioSource _soundFXObject;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }


    public void PlaySoundFXClip(AudioClip[] audoClip, Transform transform, float volumn)
    {
        int rand = Random.Range(0, audoClip.Length);

        AudioSource audioSource = Instantiate(_soundFXObject, transform.position, Quaternion.identity);

        audioSource.clip = audoClip[rand];

        audioSource.volume = volumn;

        audioSource.Play();

        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength);
    }

    public void PlaySoundFXClipAt(AudioClip[] audoClip, Transform transform, float volumn, int index)
    {
       
        AudioSource audioSource = Instantiate(_soundFXObject, transform.position, Quaternion.identity);

        audioSource.clip = audoClip[index];

        audioSource.volume = volumn;

        audioSource.Play();

        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength);
    }
}
