using UnityEngine;
using System.Collections;

public class Waypoint : MonoBehaviour {

    public enum PlayerDestination { Waypoint, None }
    public PlayerDestination leftDestinationType = PlayerDestination.None;
    public PlayerDestination rightDestinationType = PlayerDestination.None;
    public Waypoint leftDestination;
    public Waypoint rightDestination;
    public float playerForwardRotation;

    public CameraController.CameraMovement leftCameraType;
    public Vector3 leftCameraPosition;
    public Vector3 leftCameraOffset = CameraController.defaultOffset;
    public CameraController.CameraMovement rightCameraType;
    public Vector3 rightCameraPosition;
    public Vector3 rightCameraOffset = CameraController.defaultOffset;

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

                    cam.SetMovementType(leftCameraType);

                    switch (leftCameraType) {
                        case CameraController.CameraMovement.SmoothFollow:
                            cam.cameraOffset = leftCameraOffset;
                            break;
                        case CameraController.CameraMovement.Static:
                        case CameraController.CameraMovement.StaticRotate:
                            cam.targetPos = leftCameraPosition;
                            break;
                    }

                    switch(leftDestinationType) {
                        case PlayerDestination.None:
                            break;
                        case PlayerDestination.Waypoint:
                            player.setWaypointInfo(leftDestination.transform.position, player.transform.position);
                            break;
                    }

                    break;

                case PlayerController.Direction.Right:

                    cam.SetMovementType(rightCameraType);

                    switch (rightCameraType) {
                        case CameraController.CameraMovement.SmoothFollow:
                            cam.cameraOffset = rightCameraOffset;
                            break;
                        case CameraController.CameraMovement.Static:
                        case CameraController.CameraMovement.StaticRotate:
                            cam.targetPos = rightCameraPosition;
                            break;
                    }

                    switch (rightDestinationType) {
                        case PlayerDestination.None:
                            break;
                        case PlayerDestination.Waypoint:
                            player.setWaypointInfo(player.transform.position, rightDestination.transform.position);
                            break;
                    }
                    break;
            }
        }
    }
}
