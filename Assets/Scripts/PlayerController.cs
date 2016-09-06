using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public enum State { Standing, Running, Slowing, Jumping, Falling, Climbing, UsingControl }
    public enum Direction { Left, Right }
    private State state = State.Standing;
    private Direction direction = Direction.Right;

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
    private bool ladderLeaning = false;

    private Vector3 sideRayMinHeight;
    private Vector3 sideRayMaxHeight;

    private Vector3 waypointLeft;
    private Vector3 waypointRight;
    private Vector3 waypointArcCenter;
    private float waypointProgress;

	// Use this for initialization
	void Start () {

    }

    public void setWaypointInfo(Vector3 left, Vector3 right, Vector3 center, float progress) {
        waypointLeft = left;
        waypointRight = right;
        waypointArcCenter = center;
        waypointProgress = progress;
    }

    // ... not really using this right now
    public void SetState(State newState) {
        if (state == newState) {
            return;
        }

        state = newState;

        switch(state) {
            case State.Climbing: // as soon as we touch a climbable, hault formerly tracked velocities
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
        ladderLeaning = false;

        RaycastHit hitInfo;
        bool hit;

        // floor check
        hit = Physics.Raycast(transform.position, -transform.up, out hitInfo, 1.01f);
        Debug.DrawRay(transform.position, -transform.up * 1.01f, Color.red);
        if (hit) {
            onGround = true;
        } else {
            onGround = false;
        }

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
		// if we are up in the air on a ladder and moving in the opposite direction of the ladder, lean
		if (((onRightLadder && hInput < 0) || (onLeftLadder && hInput > 0)) && !onGround) {
            ladderLeaning = true;

        } else if (hInput != 0f) { // Trying to run to the right or left

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

        if (hVelocity < 0) {
            direction = Direction.Left;
        } else if (hVelocity > 0) {
            direction = Direction.Right;
        }
        // if no velocity, then we maintain our previous direction state


        vInput = Input.GetAxisRaw("Vertical");
        if (vInput != 0) {
            if ((onRightLadder || onLeftLadder) && !ladderLeaning) {
                vVelocity = 0f;
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
        if (Input.GetButtonDown("Jump")) {
            if (ladderLeaning) {
                hVelocity = (onRightLadder ? -1 : 1) * 5f;
                vVelocity = 3f;
            }
            else if (onGround) {
                vVelocity = 7f;
            }
		}

		// apply gravity
		if (!onGround && !onLeftLadder && !onRightLadder) {
			vVelocity -= gravity * Time.deltaTime;
		}

		newPos = transform.position;
		newPos += transform.right * hVelocity * Time.deltaTime;
		newPos += transform.up * vVelocity * Time.deltaTime;
		transform.position = newPos;
	}

    public Direction getDirection() {
        return direction;
    }
}
