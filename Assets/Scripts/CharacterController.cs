using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {

	public enum State { Standing, Running, Slowing, Jumping, Falling, Climbing }


	public float gravity = 9.8f;
	public float hMaxVelocity = 4f;
	public float vMaxVelocity;

	public float hAcceleration = 2f;
	public float hDeceleration = 6f;

	private float hInput = 0f;
	private float hVelocity = 0f;
	private float vVelocity = 0f;
	private Vector3 newPos;

	private bool onGround = true;
	private bool onLadder;
	private bool onWall;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		// Check if we are grounded
		RaycastHit hitInfo;
		bool hit = Physics.Raycast(new Ray(transform.position, -transform.up), out hitInfo, 1f); 
		Debug.DrawRay(transform.position, -transform.up*1f,Color.cyan);
		if (hit) {
			onGround = true;
			vVelocity = 0f;
			transform.position = hitInfo.point + transform.up * 1f;
		} else {
			onGround = false;
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

		// force upwards when jumping
		if (Input.GetButtonDown("Jump") && onGround) {
			vVelocity = 5f;
		}

		// apply gravity
		if (!onGround) {
			vVelocity -= gravity * Time.deltaTime;
		}

		Debug.Log(vVelocity);

		newPos = transform.position;
		newPos += transform.right * hVelocity * Time.deltaTime;
		newPos += transform.up * vVelocity * Time.deltaTime;
		transform.position = newPos;
	}
}
