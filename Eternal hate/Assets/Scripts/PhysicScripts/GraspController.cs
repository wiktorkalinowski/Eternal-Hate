using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraspController : MonoBehaviour {

    public float graspDistance = 3;
    public float holdDistance = 2;
    public bool doThrow = false;
    public float throwForce = 100;
    public float correctionForce = 300;
    public bool freezeRotation = true;
    public GameObject graspIcon;
    public GameObject graspedIcon;

    private Camera cam;
    private bool grasp = false;
    private bool _doThrow = false;
    private Rigidbody graspedBody;

    [HideInInspector]
    public GameObject graspedObject
    {
        get {
            if (graspedBody != null)
            {
                return graspedBody.gameObject;
            }
            else
            {
                return null;
            }
        }
    }

	// Use this for initialization
	void Start () {
        cam = Camera.main;
	}
	
    void Update()
    {
        if (TextOnObject.readObjects > 0) return;


        GameObject gameObject = CastRay();
    
        if (Input.GetButtonDown("Action"))
        {
            grasp = !grasp;
        }
        _doThrow = Input.GetMouseButtonDown(0);
        
        if (grasp)
        {
            if (graspedBody == null)
            {
                if (gameObject != null)
                {
                    graspedBody = gameObject.GetComponent<Rigidbody>();
                }
                else
                {
                    grasp = false;
                }
            }
            else
            {
                if (_doThrow && doThrow)
                {
                    Throw(graspedBody);
                }
            }
        }
        else
        {
            Drop();
        }

        if (graspIcon != null) graspIcon.SetActive(gameObject != null && !grasp);
        if (graspedIcon != null) graspedIcon.SetActive(grasp);
    }

    void FixedUpdate()
    {
        if(graspedBody != null)
        {
            GraspRigidbody(graspedBody);
        }
    }
    
    //returns GameObject you want to grasp.
    private GameObject CastRay()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit, graspDistance))
        {
            if (hit.collider.gameObject.GetComponent<Rigidbody>() != null 
                && hit.collider.gameObject.GetComponent<Rigidbody>().isKinematic == false
                && hit.collider.gameObject.tag == "Grasp")
            {
                return hit.collider.gameObject;
            }
            else return null;
        }
        else return null;
    }

    private void GraspRigidbody(Rigidbody rigidbody)
    {
        if(freezeRotation && rigidbody.constraints != RigidbodyConstraints.FreezeRotation)
        {
            rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }
        if (rigidbody.useGravity != false) rigidbody.useGravity = false;

        Vector3 targetPoint = cam.transform.position + cam.transform.forward * holdDistance;

        Vector3 force = targetPoint - rigidbody.transform.position;
        rigidbody.velocity = force.normalized * rigidbody.velocity.magnitude;
        rigidbody.AddForce(force * correctionForce);
        rigidbody.velocity *= Mathf.Min(1, force.magnitude / 2);
    }

    public void Drop()
    {
        if(graspedBody != null){
            if (graspedBody.constraints == RigidbodyConstraints.FreezeRotation)
            {
                graspedBody.constraints = RigidbodyConstraints.None;
            }
            if (graspedBody.useGravity == false) graspedBody.useGravity = true;
            graspedBody = null;
            grasp = false;
        }
    }

    private void Throw(Rigidbody rigidbody)
    {
        Drop();
        Vector3 force = transform.forward * throwForce;
        rigidbody.AddForce(force, ForceMode.Impulse);
        rigidbody.AddTorque(new Vector3(Random.Range(0, 10), Random.Range(0, 10), Random.Range(0, 10)));
    }
}
