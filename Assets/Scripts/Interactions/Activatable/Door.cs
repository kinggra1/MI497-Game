using UnityEngine;
using System.Collections;
using System;

public class Door : Activatable {

    public bool open = false;
    public float transitionSpeed = 1f;

    private bool moving = false;

    private Vector3 closedPos;
    private Vector3 openPos;
    private Vector3 destination;

    // Use this for initialization
    void Start () {
        closedPos = transform.position;
        openPos = transform.position + transform.up * 3f;

        if (open) {
            transform.position = openPos;
        }
	}
	
	// Update is called once per frame
	void Update () {
	    if (moving) {
            transform.position = Vector3.MoveTowards(transform.position, destination, transitionSpeed * Time.deltaTime);

            if (transform.position == destination) {
                moving = false;
            }
        }
	}

    public override void activate() {
        base.activate();
        moving = true;
        open = true;
        destination = openPos;
    }

    public override void deactivate() {
        base.deactivate();
        moving = true;
        open = false;
        destination = closedPos;
    }

    public override void horizontalInput(float input) {
        throw new NotImplementedException();
    }

    public override void verticalInput(float input) {
        throw new NotImplementedException();
    }
}
