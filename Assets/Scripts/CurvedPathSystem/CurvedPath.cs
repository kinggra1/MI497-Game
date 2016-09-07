using UnityEngine;
using System.Collections;

public class CurvedPath : MonoBehaviour {

	private BezierCurve path;

	private Waypoint leftWaypoint;
	private Waypoint rightWaypoint;

	void Start() {
		leftWaypoint.transform.position = path.GetPoint(0);
		rightWaypoint.transform.position = path.GetPoint(1);
	}

	public void GetPoint(float t) {
		path.GetPoint(t);
	}
}
