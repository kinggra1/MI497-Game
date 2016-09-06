using UnityEngine;
using System.Collections;

public class Waypoint : MonoBehaviour {

    public enum PlayerDestination { Waypoint, None }
    public PlayerDestination leftDestination;
    public PlayerDestination rightDestination;

    public CameraController.CameraMovement leftCameraType;
    public Vector3 leftCameraPosition;
    public Vector3 leftCameraOffset;
    public CameraController.CameraMovement rightCameraType;
    public Vector3 rightCameraPosition;
    public Vector3 rightCameraOffset;

    private PlayerController player;
    private CameraController cam;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject == player.gameObject) {
            switch(player.getDirection()) {
                case PlayerController.Direction.Left:

                    break;

                case PlayerController.Direction.Right:

                    break;
            }
        }
    }
}
