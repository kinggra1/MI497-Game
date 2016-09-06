using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public enum CameraMovement { SmoothFollow, Static, StaticRotate }
    private CameraMovement type = CameraMovement.SmoothFollow;

    public static Vector3 defaultOffset = new Vector3(0f, 3f, -10f);
    public static Quaternion defaultRotation = Quaternion.Euler(10f, 0f, 0f);

    public PlayerController playerController;
    private Transform playerTransform;

    public Vector3 cameraOffset { get; set; }
    public Vector3 targetPos { get; set; }
    public Quaternion targetRot { get; set; }

    public void SetMovementType(CameraMovement newType) {
        type = newType;
    }

	// Use this for initialization
	void Start () {
        playerTransform = playerController.transform;
	}

    // Update is called once per frame
    void Update() {

        switch (type) {
            case CameraMovement.SmoothFollow:
                targetPos = playerTransform.position + (playerTransform.up * 3f) + (-playerTransform.forward * 10f);
                targetRot = Quaternion.Euler(10f, 0f, 0f);
                break;

            // we do not update our target position, only go to the specified target position (set externally)
            case CameraMovement.Static:
                break;

            case CameraMovement.StaticRotate:
                targetRot = Quaternion.LookRotation(playerTransform.position - transform.position);
                break;
        }

        // move 95% of the way to our target every second for some easy smoothing
        transform.position = Vector3.Lerp(transform.position, targetPos, 0.95f * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, 0.95f * Time.deltaTime);
	}


}
