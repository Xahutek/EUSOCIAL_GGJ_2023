using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEffectManager : MonoBehaviour
{
    public static AudioEffectManager main;
    public float volume;
    public AudioSource soundtrackSource,effectSource;
    [System.Serializable]public class AudioEffect
    {
        public string name;
        public AudioClip clip;
        public float volume;
        public bool silence;
    }

    public AudioEffect[] effects;

    private void Awake()
    {
        main = this;
    }
    private void Start()
    {
        PlaySoundtrackEffect(SoundtrackEffects[0]);
    }

    float lastVolume = 0;
    public void SilentSoundtrack(float duration)
    {
        lastVolume= soundtrackSource.volume;
        FadeSoundtrack(0f,0.3f);
        Invoke("ActivateSoundtrack", duration);
    }
    public void ActivateSoundtrack() => FadeSoundtrack(lastVolume,0.5f);
    public void FadeSoundtrack(float targetVolume, float duration)
    {
        StartCoroutine(FadeAudio(soundtrackSource, targetVolume, duration));
    }

    public void PlaySoundtrackEffect(AudioEffect effect)
    {
        soundtrackSource.clip = effect.clip;
        soundtrackSource.volume = effect.volume*volume;
        soundtrackSource.Play();
    }
    public void PlaySoundEffect(AudioEffect effect)
    {
        effectSource.PlayOneShot(effect.clip, effect.volume*volume);
        if (effect.silence) SilentSoundtrack(4);
    }
    public void PlaySoundEffect(string name)
    {
        foreach (AudioEffect e in effects)
        {
            if (e.name==name)
            {
                PlaySoundEffect(e);
                return;
            }
        }
    }
    public void ButtonClick() => PlaySoundEffect("Click");
    public void Termitnator() => PlaySoundEffect("Termitnator");
    public void PlaceTile() => PlaySoundEffect("Place");
    public void PlaceOmen() => PlaySoundEffect("Omen");
    public void TriggerOmen() => PlaySoundEffect("Destroy");
    public void UnlockCard() => PlaySoundEffect("Unlock");

    private IEnumerator FadeAudio(AudioSource audioSource, float targetVolume, float duration)
    {
        float currentTime = 0;
        float start = audioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }

        audioSource.volume = targetVolume;
    }

    public bool replaceNew = false;
    public int soundtrackIndex = 0;
    public AudioEffect[] SoundtrackEffects;
    private void FixedUpdate()
    {
        float timeLerp = soundtrackSource.time / soundtrackSource.clip.length;
        if (replaceNew && timeLerp < 0.5f)
        {
            soundtrackIndex= (soundtrackIndex+1)%SoundtrackEffects.Length;
            PlaySoundtrackEffect(SoundtrackEffects[soundtrackIndex]);
            replaceNew = false;
        }
        if (timeLerp > 0.5f)
        {
            replaceNew = true;
        }
    }
}