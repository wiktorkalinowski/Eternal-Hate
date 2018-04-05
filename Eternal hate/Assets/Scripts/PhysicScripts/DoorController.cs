using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(HingeJoint))]
public class DoorController : MonoBehaviour {

    public enum State
    {
        opened, closed, opening, closing
    }

    public float openForce = 5;
    public float openDistance = 2;
    public float openAngle = -70;
    public float closeAngle = 0;

    private Camera cam;
    private Rigidbody rigidbody;
    private HingeJoint joint;
    private State state = State.closed;

    // Use this for initialization
    void Start () {
        cam = Camera.main;
        rigidbody = GetComponent<Rigidbody>();
        joint = GetComponent<HingeJoint>();
        if(state == State.closed) rigidbody.isKinematic = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Action"))
        {
            if (CastRay())
            {
                if (state == State.closed) state = State.opening;
                if (state == State.opened) state = State.closing;
            }
        }
	}

    private void FixedUpdate()
    {
        if (state == State.opening) Open();
        if (state == State.closing) Close();
        Debug.Log(state);
    }

    void Open()
    {
        rigidbody.isKinematic = false;
        rigidbody.AddForce(transform.forward * openForce, ForceMode.Force);
        if(joint.angle <= openAngle)
        {
            state = State.opened;
        }

        Debug.Log("OPEN FUNCTION");
    }

    void Close()
    {
        rigidbody.AddForce(transform.forward * -openForce, ForceMode.Force);
        if (joint.angle >= closeAngle)
        {
            state = State.closed;
            rigidbody.isKinematic = true;
        }
        Debug.Log("CLOSE FUNCTION");
    }

    private bool CastRay()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, openDistance))
        {
            if (hit.collider.gameObject == gameObject)
            {
                return true;
            }
            else return false;
        }
        else return false;
    }


}
