using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

[RequireComponent(typeof(FirstPersonController))]
public class ExamineObject : MonoBehaviour {

    public float examineDistance = 1;
    public float activeDistance = 3;
    public float rotationSpeedMultiplier = 1;

    private Camera cam;
    private FirstPersonController controller;
    private GameObject examinedObject;

    private Vector3 startingPosition;
    private Quaternion startingRotation;

    private bool _examining = false;
    public bool examining
    {
        get { return _examining; }
    }
	// Use this for initialization
	void Start () {
        cam = Camera.main;
        controller = GetComponent<FirstPersonController>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Action"))
        {
            if (!_examining)
            {
                examinedObject = CastRay();
                if (examinedObject != null)
                {
                    StartExamining();
                }
            }
            else
            {
                StopExamining();
            }
        }

        if(examinedObject != null)
        {
            RotateToMouse(examinedObject.transform);
        }
	}

    private void StartExamining()
    {
        _examining = true;
        controller.enabled = false;

        startingPosition = examinedObject.transform.position;
        startingRotation = examinedObject.transform.rotation;

        Vector3 targetPosition = cam.transform.position + cam.transform.forward.normalized * examineDistance;
        examinedObject.transform.position = targetPosition;
        examinedObject.GetComponent<Rigidbody>().isKinematic = true;
    }

    private void StopExamining()
    {
        _examining = false;
        controller.enabled = true;

        examinedObject.GetComponent<Rigidbody>().isKinematic = false;
        examinedObject.transform.position = startingPosition;
        examinedObject.transform.rotation = startingRotation;

        examinedObject = null;
    }

    private void RotateToMouse(Transform transform)
    {
        Vector2 mouseVelocity = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        mouseVelocity *= rotationSpeedMultiplier;

        Vector3 targetPosition = cam.transform.position + cam.transform.forward.normalized * examineDistance;
        transform.RotateAround(targetPosition, Vector3.up, mouseVelocity.x);
        transform.RotateAround(targetPosition, Vector3.left, mouseVelocity.y);
    }

    private GameObject CastRay()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, activeDistance))
        {
            if (hit.collider.gameObject.GetComponent<Rigidbody>() != null
                && hit.collider.gameObject.GetComponent<Rigidbody>().isKinematic == false
                && hit.collider.gameObject.tag == "Examine")
            {
                return hit.collider.gameObject;
            }
            else return null;
        }
        else return null;
    }
}
