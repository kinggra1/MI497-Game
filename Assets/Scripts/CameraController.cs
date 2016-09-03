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
        targetPos = playerTransform.position + new Vector3(0f, 3f, -10f);
        targetRot = Quaternion.Euler(10f, 0f, 0f);

        transform.position = targetPos;
        transform.rotation = targetRot;
	}


}
