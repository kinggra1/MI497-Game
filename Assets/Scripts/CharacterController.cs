using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {

	public float hMaxVelocity = 0.4f;
	public float vMaxVelocity;

	public float hAcceleration = 0.2f;
	public float vAcceleration;

	public float hDeceleration = 0.2f;

	private float hInput = 0f;
	private float hVelocity = 0f;
	private float vVelocity = 0f;
	private Vector3 newPos;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		hInput = Input.GetAxisRaw("Horizontal");
		// Trying to run to the right or left
		if (hInput != 0f) {

			// We have to decelerate before we accelerate
			if (Mathf.Sign(hVelocity) != Mathf.Sign(hInput)) {
				hVelocity -= (hVelocity > 0 ? 1f : -1f) * hDeceleration * Time.deltaTime;
			} else { // accelerate normally
				hVelocity += hInput * hAcceleration * Time.deltaTime;
			}

			if (Mathf.Abs(hVelocity) > hMaxVelocity) {
				hVelocity = (hVelocity > 0 ? 1f : -1f) * hMaxVelocity;
			}
		} else if (hVelocity != 0f) { // slow down if not actively running
			if (Mathf.Abs(hVelocity) < hDeceleration) {
				hVelocity = 0f;
			} else {
				hVelocity -= (hVelocity > 0 ? 1f : -1f) * hDeceleration * Time.deltaTime;
			}
		}


		newPos = transform.position;
		newPos += transform.right * hVelocity;
		transform.position = newPos;
	}
}
