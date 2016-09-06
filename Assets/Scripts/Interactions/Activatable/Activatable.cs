using UnityEngine;
using System.Collections;

public abstract class Activatable : MonoBehaviour {

    public bool active = false;

    public bool isActive() {
        return active;
    }

    public virtual void activate() {
        active = true;
    }

    public virtual void deactivate() {
        active = false;
    }

    public abstract void horizontalInput(float input);
    public abstract void verticalInput(float input);
}
