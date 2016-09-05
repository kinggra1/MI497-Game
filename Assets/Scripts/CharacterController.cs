using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {

	public enum State { Standing, Running, Slowing, Jumping, Falling, Climbing }
    private State state = State.Standing;

	public float gravity = 9.8f;
	public float hMaxVelocity = 4f;
	public float vMaxVelocity;

	public float hAcceleration = 6f;
	public float hDeceleration = 12f;

    public float climbSpeed = 2f;

    public int sideRayCount = 3;

	private float hInput = 0f;
    private float vInput = 0f;
	private float hVelocity = 0f;
	private float vVelocity = 0f;
	private Vector3 newPos;

	private bool onGround = false;
	private bool onRightLadder = false;
    private bool onLeftLadder = false;

    private Vector3 sideRayMinHeight;
    private Vector3 sideRayMaxHeight;

	// Use this for initialization
	void Start () {

    }

    public void SetState(State newState) {
        if (state == newState) {
            return;
        }

        state = newState;

        switch(state) {
            case State.Climbing: // as soon as we touch a climbable, hault formly tracked velocities
                vVelocity = 0f;
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {

        // reset checks
        onGround = false;
        onRightLadder = false;
        onLeftLadder = false;

        RaycastHit hitInfo;
        bool hit;

        // right ladder check
        hit = Physics.Raycast(transform.position, transform.right, out hitInfo, 0.7f);
        Debug.DrawRay(transform.position, transform.right * 0.7f, Color.red);
        if (hit) {
            if (hitInfo.collider.tag == "Climbable") {
                onRightLadder = true;
                SetState(State.Climbing);
            }
        }

        // left ladder check
        hit = Physics.Raycast(transform.position, -transform.right, out hitInfo, 0.7f);
        Debug.DrawRay(transform.position, -transform.right * 0.7f, Color.red);
        if (hit) {
            if (hitInfo.collider.tag == "Climbable") {
                onLeftLadder = true;
                SetState(State.Climbing);
            }
        }




        hInput = Input.GetAxisRaw("Horizontal");
		// Trying to run to the right or left
		if (hInput != 0f) {

			// We have to decelerate before we accelerate
			if (Mathf.Sign(hVelocity) != Mathf.Sign(hInput)) {
                hVelocity -= (hVelocity > 0 ? 1f : -1f) * hDeceleration * Time.deltaTime;
			} else { // accelerate normally
				hVelocity += hInput * hAcceleration * Time.deltaTime;
			}

			// Cap at the maximum horizontal velocity
			if (Mathf.Abs(hVelocity) > hMaxVelocity) {
				hVelocity = (hVelocity > 0 ? 1f : -1f) * hMaxVelocity;
			}

		} else if (hVelocity != 0f) { // slow down if not actively running
			if (Mathf.Abs(hVelocity) < hDeceleration * Time.deltaTime) {
				hVelocity = 0f;
			} else {
				hVelocity -= (hVelocity > 0 ? 1f : -1f) * hDeceleration * Time.deltaTime;
			}
		}



        vInput = Input.GetAxisRaw("Vertical");
        if (vInput != 0) {
            if (onRightLadder || onLeftLadder) {
                transform.position += transform.up * vInput * climbSpeed * Time.deltaTime;
}
        }


        // collision corrections
        // floor correction
        hit = Physics.Raycast(transform.position, -transform.up, out hitInfo, 1f);
        Debug.DrawRay(transform.position, -transform.up * 1f, Color.cyan);
        if (hit) {
            vVelocity = 0f;
            transform.position = hitInfo.point + transform.up * 1f;
            onGround = true;
        }
        else {
            onGround = false;
        }

        // ceiling correction
        hit = Physics.Raycast(transform.position, transform.up, out hitInfo, 1f);
        Debug.DrawRay(transform.position, transform.up * 1f, Color.cyan);
        if (hit && vVelocity > 0) {
            vVelocity = 0f;
            transform.position = hitInfo.point + -transform.up * 1f;
        }

        // side corrections
        sideRayMinHeight = transform.position - transform.up * 0.8f;
        sideRayMaxHeight = transform.position + transform.up * 0.8f;
        // right side
        for (int i = 0; i < sideRayCount; i++) {
            Vector3 origin = Vector3.Lerp(sideRayMinHeight, sideRayMaxHeight, i / (sideRayCount - 1));
            hit = Physics.Raycast(origin, transform.right, out hitInfo, 0.5f);
            Debug.DrawRay(origin, transform.right * 0.5f, Color.cyan);
            if (hit) {
                hVelocity = 0f;
                transform.position = hitInfo.point - transform.right * 0.5f - (origin - transform.position); //(origin - transform.position) for height adjustment
            }
        }
        // left side
        for (int i = 0; i < 3; i++) {
            Vector3 origin = Vector3.Lerp(sideRayMinHeight, sideRayMaxHeight, i / (sideRayCount - 1));
            hit = Physics.Raycast(origin, -transform.right, out hitInfo, 0.5f);
            Debug.DrawRay(origin, -transform.right * 0.5f, Color.cyan);
            if (hit) {
                hVelocity = 0f;
                transform.position = hitInfo.point + transform.right * 0.5f - (origin - transform.position); // same as above
            }
        }




        // force upwards when jumping
        if (Input.GetButtonDown("Jump") && (onGround || onRightLadder || onLeftLadder)) {
			vVelocity = 7f;
		}

		// apply gravity
		if (!onGround && !onLeftLadder && !onRightLadder) {
			vVelocity -= gravity * Time.deltaTime;
		}

		Debug.Log(vVelocity);

		newPos = transform.position;
		newPos += transform.right * hVelocity * Time.deltaTime;
		newPos += transform.up * vVelocity * Time.deltaTime;
		transform.position = newPos;
	}
}
