using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using TMPro;

public class TextOnObject : MonoBehaviour {

    public float readDistance = 2;
    public GameObject scrollView;
    public TextAsset textAsset;

    private TextMeshProUGUI content;
    private bool reading = false;
    private Camera cam;
    private FirstPersonController controller;

    public static int readObjects = 0;

    private void Start()
    {
        cam = Camera.main;
        controller = GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>();
        if (textAsset == null) Debug.Log("TextAsset is equal to null.");
        if (scrollView == null) Debug.Log("ScrollView is equal to null.");
        else
        {
            content = scrollView.GetComponentInChildren<TextMeshProUGUI>();
            if (content == null) Debug.Log("ScrollView has no TextMeshProUGUI component");
        }
    }

    private void Update()
    {
        if(scrollView == null || textAsset == null || content == null)
        {
            return;
        }
        if (!reading)
        {
            if (Input.GetButtonDown("Action") && CastRay())
            {
                reading = true;
                content.text = textAsset.text;
                scrollView.SetActive(true);
                controller.enabled = false;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                readObjects++;
            }
        }
        else if (reading)
        {
            if (Input.GetButtonDown("Cancel"))
            {
                reading = false;
                scrollView.SetActive(false);
                controller.enabled = true;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                readObjects--;
            }
        }
    }

    private bool CastRay()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, readDistance))
        {
            if (hit.collider.gameObject == this.gameObject)
            {
                return true;
            }
            else return false;
        }
        else return false;
    }
}
