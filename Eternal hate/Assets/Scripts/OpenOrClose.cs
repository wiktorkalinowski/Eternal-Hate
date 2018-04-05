using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class OpenOrClose : MonoBehaviour {

    public float openDistance = 4;
    private Animator animator;
    private Camera cam;
    private AudioSource[] audioSources;

    private bool opened = false;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        cam = Camera.main;
        audioSources = GetComponents<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        bool closeEnough = CastRay();

        if (Input.GetButtonDown("Action") && closeEnough)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("opened") ||
                animator.GetCurrentAnimatorStateInfo(0).IsName("closed"))
            {
                animator.SetTrigger("openOrClose");
                opened = !opened;

                if (opened && audioSources.Length >= 1) audioSources[0].Play();
                if (!opened && audioSources.Length >= 2) audioSources[1].Play();
                
            }
        }
	}

    private bool CastRay()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, openDistance))
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (hit.collider.gameObject == transform.GetChild(i).gameObject) return true;
            }
            return false;
        }
        else return false;
    }
}
