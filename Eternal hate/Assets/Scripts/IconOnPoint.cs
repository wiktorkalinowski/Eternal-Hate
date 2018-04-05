using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconOnPoint : MonoBehaviour {

    public float pointDistance = 2;
    public GameObject icon;
    private Camera cam;
    private ExamineObject examineObjectScript;

	// Use this for initialization
	void Start () {
        cam = Camera.main;
        examineObjectScript = GameObject.FindGameObjectWithTag("Player").GetComponent<ExamineObject>();
    }
	
	// Update is called once per frame
	void Update () {
        if (examineObjectScript.examining)
        {
            icon.SetActive(false);
            return;
        }

        bool closeEnough = CastRay();
        if (icon != null) icon.SetActive(closeEnough);
    }

    private bool CastRay()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, pointDistance))
        {
            if (hit.collider.gameObject == transform.gameObject) return true;
            for (int i = 0; i < transform.childCount; i++)
            {
                if (hit.collider.gameObject == transform.GetChild(i).gameObject) return true;
            }
            return false;
        }
        else return false;
    }
}
