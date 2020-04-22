using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityManager : MonoBehaviour {

	[SerializeField]
	private bool modifiedGravity = false;
	[SerializeField]
    [Range(0.0f, 100.0f)]
    private float gravityModifier = 1;

    private const double GRAVITY_CONSTANT = 0.0000000000667408;
    private GameObject[] _gravityObjects;
	private float gravityCoefficient = 1;

	void Start() {
        updateList();
        InvokeRepeating("updateList", 1.0f, 1.0f);
        if (modifiedGravity) {
            gravityCoefficient = gravityModifier * 100000000000.0f;
        }
    }

    void FixedUpdate() {
        exertGravity();
    }

    void updateList() {
        _gravityObjects = HelperFunctions.FindGameObjectsOnLayer("GravityObjects");
    }

    void exertGravity() {
        for (int i = 0; i < _gravityObjects.Length; i++) {
            for (int j = i + 1; j < _gravityObjects.Length; j++) {
                if (_gravityObjects[i] != null && _gravityObjects[j] != null && i != j) {
                    GameObject object1 = _gravityObjects[i];
                    GameObject object2 = _gravityObjects[j];
                    Vector3 position1 = object1.transform.position;
                    Vector3 position2 = object2.transform.position;

                    float magnitude = calculateGravityNewton(object1.GetComponent<Rigidbody>().mass, object2.GetComponent<Rigidbody>().mass, Vector3.Distance(position1, position2)) * gravityCoefficient * Time.fixedDeltaTime;

                    Vector3 direction = (position2 - position1).normalized; // From position1 to position2.

                    object1.GetComponent<Rigidbody>().AddForce(magnitude * direction); // Apply gravitational force on object1 towards object2.

                    direction = -direction; // Invert direction of force.

                    object2.GetComponent<Rigidbody>().AddForce(magnitude * direction); // Apply gravitational force on object2 towards object1.
                }
            }
        }
    }

    float calculateGravityNewton(float m1, float m2, float r) {
        return (float) GRAVITY_CONSTANT * (m1 * m2) / (r * r);
    }
}
