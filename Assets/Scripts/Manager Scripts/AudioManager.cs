using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public List<AudioClip> SoundClips = new List<AudioClip>();

    void Start()
    {

    }

    public AudioSource PlayAudio(AudioClip clip, Transform location)
    {
        return PlayAudio(clip, location, 1f, 1f);
    }

    public AudioSource PlayAudio(AudioClip clip, Transform location, float volume)
    {
        return PlayAudio(clip, location, volume, 1f);
    }

    /// <summary>
    /// Plays sound on empty object, destroys it after finished.
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="location"></param>
    /// <param name="volume"></param>
    /// <param name="pitch"></param>
    /// <returns></returns>
    public AudioSource PlayAudio(AudioClip clip, Transform location, float volume, float pitch)
    {
        //make the empty game object
        GameObject createdAudio = new GameObject("Audio: " + clip.name);
        createdAudio.transform.position = location.position;
        createdAudio.transform.parent = location;

        //Create the source
        AudioSource source = createdAudio.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.Play();
        Destroy(createdAudio, clip.length);
        return source;
    }

    public AudioSource PlayAudio(AudioClip clip, Transform location, float volume, float pitch, float time)
    {
        //make the empty game object
        GameObject createdAudio = new GameObject("Audio: " + clip.name);
        createdAudio.transform.position = location.position;
        createdAudio.transform.parent = location;

        //Create the source
        AudioSource source = createdAudio.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.Play();
        Destroy(createdAudio, time);
        return source;
    }

    public AudioSource PlayAudio(AudioClip clip, Vector3 point)
    {
        return PlayAudio(clip, point, 1f, 1f);
    }

    public AudioSource PlayAudio(AudioClip clip, Vector3 point, float volume)
    {
        return PlayAudio(clip, point, volume, 1f);
    }

    /// <summary>
    /// Plays a sound at a point - then destroys it after.
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="point"></param>
    /// <param name="volume"></param>
    /// <param name="pitch"></param>
    /// <returns></returns>
    public AudioSource PlayAudio(AudioClip clip, Vector3 point, float volume, float pitch)
    {
        //Create an empty game object
        GameObject createdAudio = new GameObject("Audio: " + clip.name);
        createdAudio.transform.position = point;

        //Create the source
        AudioSource source = createdAudio.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.Play();
        Destroy(createdAudio, clip.length);
        return source;
    }
}