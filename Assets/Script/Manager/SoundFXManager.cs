
using UnityEngine;

using System.Collections;
using System;

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
        int rand = UnityEngine.Random.Range(0, audoClip.Length);

        AudioSource audioSource = Instantiate(_soundFXObject, transform.position, Quaternion.identity);

        audioSource.clip = audoClip[rand];

        audioSource.volume = volumn;

        audioSource.Play();

        float clipLength = audioSource.clip.length;

        StartCoroutine(StartCountDown(clipLength, () => { Destroy(audioSource.gameObject); }));
    }

    public void PlaySoundFXClipAt(AudioClip[] audoClip, Transform transform, float volumn, int index)
    {
       
        AudioSource audioSource = Instantiate(_soundFXObject, transform.position, Quaternion.identity);

        audioSource.clip = audoClip[index];

        audioSource.volume = volumn;

        audioSource.Play();

        float clipLength = audioSource.clip.length;

        StartCoroutine(StartCountDown(clipLength, () => { Destroy(audioSource.gameObject); }));
    }

    public void PlaySoundFXClipWithLength(AudioClip[] audoClip, Transform transform, float volumn, float duration, int index)
    {

        AudioSource audioSource = Instantiate(_soundFXObject, transform.position, Quaternion.identity);

        audioSource.clip = audoClip[index];

        audioSource.volume = volumn;

        audioSource.loop = true;

        audioSource.Play();



        //float clipLength = audioSource.clip.length;

        //DestroyAfterRealtime(audioSource.gameObject, duration);
        StartCoroutine(StartCountDown(duration, () => { Destroy(audioSource.gameObject); }));
    }


    public void PlaySoundFXInSequence(
    AudioClip[] audioClips,
    Transform transform,
    float volume,
    float duration,
    float bufferTime,
    float startTimeForFinalSFX // when to start final SFX (relative to sequence start)
)
    {
        // 0. First SFX
        PlaySoundFXClipAt(audioClips, transform, volume, 0);

        // 1. Second SFX: main loop
        PlaySoundFXClipWithLength(audioClips, transform, volume, duration + bufferTime, 1);

        // 2. Final SFX: manual timing
        float safeTime = Mathf.Max(0f, startTimeForFinalSFX); // prevent negative wait
        StartCoroutine(StartCountDown(safeTime, () =>
        {
            PlaySoundFXClipAt(audioClips, transform, volume, 2);
        }));
    }


    public IEnumerator StartCountDown(float duration, Action onFinished)
    {
        yield return new WaitForSecondsRealtime(duration);
        onFinished?.Invoke();
    }
   
}
