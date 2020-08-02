using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAsteroidManager : MonoBehaviour {

#pragma warning disable
	[SerializeField]
	private GameObject[] asteroids;
#pragma warning restore

	private Bounds cameraBounds;

    void Start() {
        cameraBounds = Camera.main.GetComponent<CameraController>().cameraBounds;
    }

    void FixedUpdate() {
        foreach (GameObject asteroid in asteroids) {
			Rigidbody asteroidRigidbody = asteroid.GetComponent<Rigidbody>();

			// Do asteroid bounds checking to reflect the velocity.
			if (!cameraBounds.Contains(asteroid.transform.position)) {
				// Get the required vectors for calculating reflection vector.
				Vector3 closestPoint = cameraBounds.ClosestPoint(asteroidRigidbody.position);
				Vector3 reflectionNormal = Vector3.Normalize(closestPoint - asteroidRigidbody.position);
				Vector3 direction = Vector3.Normalize(asteroidRigidbody.velocity);

				// Set the position to the closestpoint on the bounds and the velocity to the dampened reflection.
				asteroidRigidbody.position = closestPoint;
				Vector3 reflection = 2 * Vector3.Dot(reflectionNormal, -direction) * reflectionNormal + direction;
				asteroidRigidbody.velocity = reflection * asteroidRigidbody.velocity.magnitude;
			}
		}
	}
}
