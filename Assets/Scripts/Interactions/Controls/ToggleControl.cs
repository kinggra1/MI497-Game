using UnityEngine;
using System.Collections;

public class ToggleControl : MonoBehaviour {

    public bool reversible = true;
    public Activatable[] activatedObjects = null;

    private PlayerController player;
    private bool canToggle = false;
    private bool toggled = false;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void toggle() {
        if (!toggled) {
            foreach (Activatable obj in activatedObjects) {
                obj.activate();
            }
            toggled = true;
        } else if (reversible) {
            foreach (Activatable obj in activatedObjects) {
                obj.deactivate();
            }
            toggled = false;
        }
    }

    void Update() {
        if (canToggle && Input.GetButtonDown("Fire1")) {
            toggle();
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject == player.gameObject) {
            canToggle = true;
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject == player.gameObject) {
            canToggle = false;
        }
    }
}
