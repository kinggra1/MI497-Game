using UnityEngine;
using System.Collections;

public class PressurePlate : MonoBehaviour {

	public bool reversible = true;
	private bool active = false;

	public Activatable[] activatedObjects = null;

	public void toggle() {
		if (!active) {
			activate();
		} else if (reversible) {
			deactivate();
		}
	}

	public void activate() {
		foreach (Activatable obj in activatedObjects) {
			obj.activate();
		}
		active = true;
	}

	public void deactivate() {
		foreach (Activatable obj in activatedObjects) {
			obj.deactivate();
		}
		active = false;
	}

	void OnTriggerEnter(Collider other) {
		activate();
	}

	void OnTriggerExit(Collider other) {
		if (reversible) {
			deactivate();
		}
	}
}
