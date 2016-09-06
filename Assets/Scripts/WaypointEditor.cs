using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Waypoint))]
public class WaypointEditor : Editor {
    public override void OnInspectorGUI() {
        Waypoint waypoint = (Waypoint)target;

        EditorGUILayout.LabelField("Area Left of Waypoint");
        waypoint.leftCameraType = (CameraController.CameraMovement)EditorGUILayout.EnumPopup("Camera Movement", waypoint.leftCameraType);
        switch (waypoint.leftCameraType) {
            case (CameraController.CameraMovement.SmoothFollow):
                waypoint.leftCameraOffset = EditorGUILayout.Vector3Field("Offset from player: ", waypoint.leftCameraOffset);
                break;
            case (CameraController.CameraMovement.Static):
                waypoint.leftCameraPosition = EditorGUILayout.Vector3Field("Absolute Camera Position: ", waypoint.leftCameraPosition);
                break;
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Area Right of Waypoint");
        waypoint.rightCameraType = (CameraController.CameraMovement)EditorGUILayout.EnumPopup("Camera Movement", waypoint.rightCameraType);
        switch (waypoint.rightCameraType) {
            case (CameraController.CameraMovement.SmoothFollow):
                waypoint.rightCameraOffset = EditorGUILayout.Vector3Field("Offset from player: ", waypoint.rightCameraOffset);
                break;
            case (CameraController.CameraMovement.Static):
                waypoint.rightCameraPosition = EditorGUILayout.Vector3Field("Absolute camera position: ", waypoint.rightCameraPosition);
                break;
        }
    }
}
