using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityManager : MonoBehaviour {

    public float gravityModifier = 1;

    const double GRAVITY_CONSTANT = 0.0000000000667408;
    GameObject[] gravityObjects;

    void Start() {
        updateList();
        InvokeRepeating("updateList", 1.0f, 1.0f);
    }

    void FixedUpdate() {
        exertGravity();
    }

    void updateList() {
        gravityObjects = HelperFunctions.FindGameObjectsOnLayer("GravityObjects");
    }

    void exertGravity() {
        for (int i = 0; i < gravityObjects.Length; i++) {
            for (int j = i + 1; j < gravityObjects.Length; j++) {
                if (gravityObjects[i] != null && gravityObjects[j] != null && i != j) {
                    GameObject object1 = gravityObjects[i];
                    GameObject object2 = gravityObjects[j];
                    Vector3 position1 = object1.transform.position;
                    Vector3 position2 = object2.transform.position;

                    float magnitude = calculateGravityNewton(object1.GetComponent<Rigidbody>().mass, object2.GetComponent<Rigidbody>().mass, Vector3.Distance(position1, position2)) * gravityModifier * Time.fixedDeltaTime;

                    Vector3 direction = (position2 - position1).normalized; // From position1 to position2.
                    Debug.Log("Direction from object1 to object 2: " + direction);

                    object1.GetComponent<Rigidbody>().AddForce(magnitude * direction); // Apply gravitational force on object1 towards object2.
                    Debug.Log("Force on object1: " + magnitude * direction);

                    direction = -direction; // Invert direction of force.
                    object1.GetComponent<Rigidbody>().AddForce(magnitude * direction); // Apply gravitational force on object2 towards object1.
                    Debug.Log("Force on object2: " + magnitude * direction);
                }
            }
        }
    }

    float calculateGravityNewton(float m1, float m2, float r) {
        return (float) GRAVITY_CONSTANT * (m1 * m2) / (r * r);
    }
}
