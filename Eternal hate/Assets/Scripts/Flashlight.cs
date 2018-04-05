using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour {

    public GameObject flashlight;
    private bool light = false;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("Flashlight"))
        {
            light = !light;
            flashlight.SetActive(light);
        }
    }
}
