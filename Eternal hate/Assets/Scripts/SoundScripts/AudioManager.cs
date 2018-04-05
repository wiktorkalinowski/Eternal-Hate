using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour {

    public static AudioManager instance;
    public Sound[] sounds;

    private void Awake()
    {
        //makes sure that there is only one AudioManager in all scenes
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        foreach(Sound s in sounds)
        {
            s.audioSource = gameObject.AddComponent<AudioSource>();
            s.audioSource.clip = s.audioClip;

            s.audioSource.volume = s.volume;
            s.audioSource.pitch = s.pitch;
            s.audioSource.loop = s.loop;
        }
    }

    // Use this for initialization
    void Start () {

	}

    public void Play(string name)
    {
        Sound s = Array.Find<Sound>(sounds, sound => sound.name == name);
        if (s != null) s.audioSource.Play();
        else Debug.LogError("Sound '" + name + "' not found");
    }

    public void Play(string name, float volume)
    {
        volume = Mathf.Clamp(volume, 0, 1);
        Sound s = Array.Find<Sound>(sounds, sound => sound.name == name);
        if (s != null)
        {
            s.audioSource.volume = volume;
            s.audioSource.Play();
        }
        else Debug.LogError("Sound '" + name + "' not found");
    }
}
