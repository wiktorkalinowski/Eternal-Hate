using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSound : MonoBehaviour
{

    private AudioManager audioManager;
    private bool started = false;
    public string soundName;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null) Debug.LogError("No AudioManager found");
    }

    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (audioManager != null)
        {
            if (other.tag == "Player")
            {
                if (!started)
                {
                    audioManager.Play(soundName);
                    started = true;
                }
            }
        }
    }
}
