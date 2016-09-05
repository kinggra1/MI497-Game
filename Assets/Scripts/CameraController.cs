using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public CharacterController playerController;
    private Transform playerTransform;

    private Vector3 targetPos { get; set; }
    private Quaternion targetRot;

	// Use this for initialization
	void Start () {
        playerTransform = playerController.transform;
	}
	
	// Update is called once per frame
	void Update () {
        targetPos = playerTransform.position + (playerTransform.up * 3f) + (-playerTransform.forward * 10f);
        targetRot = Quaternion.Euler(10f, 0f, 0f);

        // move 90% of the way to our target every second for some smoothing
        transform.position = Vector3.Lerp(transform.position, targetPos, 0.95f * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, 0.95f * Time.deltaTime);
	}


}
