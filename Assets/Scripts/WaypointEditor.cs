using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Waypoint))]
public class WaypointEditor : Editor {
    public override void OnInspectorGUI() {
        Waypoint waypoint = (Waypoint)target;

        waypoint.playerForwardRotation = EditorGUILayout.FloatField("Player Forward Rotation", waypoint.playerForwardRotation);
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("---Area Left of Waypoint---");
        EditorGUILayout.LabelField("---Player:");
        waypoint.leftDestinationType = (Waypoint.PlayerDestination)EditorGUILayout.EnumPopup("Player Destination", waypoint.leftDestinationType);
        switch(waypoint.leftDestinationType) {
            case (Waypoint.PlayerDestination.Waypoint):
                waypoint.leftDestination = (Waypoint)EditorGUILayout.ObjectField("Waypoint", waypoint.leftDestination, typeof(Waypoint), true);
                break;
            case (Waypoint.PlayerDestination.None):
                break;
        }
        EditorGUILayout.LabelField("---Camera:");
        waypoint.leftCameraType = (CameraController.CameraMovement)EditorGUILayout.EnumPopup("Camera Movement", waypoint.leftCameraType);
        switch (waypoint.leftCameraType) {
            case (CameraController.CameraMovement.SmoothFollow):
                waypoint.leftCameraOffset = EditorGUILayout.Vector3Field("Offset from player: ", waypoint.leftCameraOffset);
                break;
            case (CameraController.CameraMovement.Static):
            case (CameraController.CameraMovement.StaticRotate):
                waypoint.leftCameraPosition = EditorGUILayout.Vector3Field("Absolute Camera Position: ", waypoint.leftCameraPosition);
                break;
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("---Area Right of Waypoint---");
        EditorGUILayout.LabelField("Player:");
        waypoint.rightDestinationType = (Waypoint.PlayerDestination)EditorGUILayout.EnumPopup("Player Destination", waypoint.rightDestinationType);
        switch (waypoint.rightDestinationType) {
            case (Waypoint.PlayerDestination.Waypoint):
                waypoint.rightDestination = (Waypoint)EditorGUILayout.ObjectField("Waypoint: ", waypoint.rightDestination, typeof(Waypoint), true);
                break;
            case (Waypoint.PlayerDestination.None):
                break;
        }
        EditorGUILayout.LabelField("Camera:");
        waypoint.rightCameraType = (CameraController.CameraMovement)EditorGUILayout.EnumPopup("Camera Movement", waypoint.rightCameraType);
        switch (waypoint.rightCameraType) {
            case (CameraController.CameraMovement.SmoothFollow):
                waypoint.rightCameraOffset = EditorGUILayout.Vector3Field("Offset from player: ", waypoint.rightCameraOffset);
                break;
            case (CameraController.CameraMovement.Static):
            case (CameraController.CameraMovement.StaticRotate):
                waypoint.rightCameraPosition = EditorGUILayout.Vector3Field("Absolute camera position: ", waypoint.rightCameraPosition);
                break;
        }
    }
}
