using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public List<AudioClip> VoiceoverClips;
    private int _numberOfClips;
    private AudioSource _audioSource;
    private int _index;

    public void Initialize()
    {
        _audioSource = GetComponent<AudioSource>();
        _numberOfClips = VoiceoverClips.Count;
        _index = 0;
    }

    private IEnumerator PlayVoiceover()
    {
        if (_audioSource.isPlaying)
        {
            yield return new WaitForSeconds(_audioSource.clip.length);
            _audioSource.clip = VoiceoverClips[_index];
            _audioSource.Play();
            ++_index;
        }
        else
        {
            _audioSource.clip = VoiceoverClips[_index];
            _audioSource.Play();
            ++_index;
        }
    }
    private IEnumerator PlayVoiceover(float delay)
    {
        if (_audioSource.isPlaying)
        {
            yield return new WaitForSeconds(_audioSource.clip.length + delay);
            _audioSource.clip = VoiceoverClips[_index];
            _audioSource.Play();
            ++_index;
        }
        else
        {
            yield return new WaitForSeconds(delay);
            _audioSource.clip = VoiceoverClips[_index];
            _audioSource.Play();
            ++_index;
        }
    }
    private IEnumerator PlayVoiceover(int i)
    {
        if (_audioSource.isPlaying)
        {
            yield return new WaitForSeconds(_audioSource.clip.length);
            _audioSource.clip = VoiceoverClips[i];
            _audioSource.Play();
        }
        else
        {
            _audioSource.clip = VoiceoverClips[i];
            _audioSource.Play();
        }
    }

    public void PlayNextVoiceover()
    {
        if (_index >= _numberOfClips) return;
        StartCoroutine(PlayVoiceover());

    }

    public void PlayNextVoiceover(float delay)
    {
        if (_index >= _numberOfClips) return;
        StartCoroutine(PlayVoiceover(delay));
    }

    public void PlaySpecificVoiceover(int i)
    {
        if (_index >= _numberOfClips) return;
        StartCoroutine(PlayVoiceover(i));
    }

    public void SetIndex(int i)
    {
        _index = i;
    }

    public void RepeatLastVoiceover()
    {
        if (_audioSource.isPlaying) return;
        if (_index < 1) return;
        StartCoroutine(PlayVoiceover(_index - 1));
    }

}