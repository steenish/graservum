using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidManager : MonoBehaviour {

	public Bounds asteroidBounds { get; private set; }

#pragma warning disable
	[SerializeField]
    private float meanMass;
    [SerializeField]
    private float massStandardDeviation;
    [SerializeField]
    private int maxNumAsteroids;
    [SerializeField]
    private float maxVelocity;
    [SerializeField]
    private float spawnDistanceOutsideBounds;
	[SerializeField]
	private float boundsForceMagnitude = 1.0f;
    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private Transform asteroidParent;
    [SerializeField]
    private GameObject asteroidPrefab;
#pragma warning restore

    private Bounds cameraBounds;
	private float zValue;
    private int currentNumAsteroids;

    void Start() {
        cameraBounds = Camera.main.GetComponent<CameraController>().cameraBounds;
        asteroidBounds = new Bounds(cameraBounds.center, cameraBounds.size + new Vector3(spawnDistanceOutsideBounds, spawnDistanceOutsideBounds, 0.0f));

        zValue = GameObject.Find("Player").transform.position.z;
    }

    void Update() {
        // Spawn one asteroid per frame if the maximum number of asteroids has not been reached.
        if (currentNumAsteroids < maxNumAsteroids) {
            SpawnAsteroid();
        }
    }

	public void AsteroidCreated() {
		currentNumAsteroids++;
	}

	public void AsteroidDestroyed() {
        currentNumAsteroids--;
    }

    void FixedUpdate() {
        foreach (GameObject asteroid in HelperFunctions.FindGameObjectsOnLayer("GravityObjects")) {
			Rigidbody asteroidRigidbody = asteroid.GetComponent<Rigidbody>();

			// Do asteroid bounds checking to reflect the velocity.
			if (!asteroidBounds.Contains(asteroid.transform.position)) {
				// Get the required vectors for calculating reflection vector.
				Vector3 closestPoint = asteroidBounds.ClosestPoint(asteroidRigidbody.position);
				Vector3 reflectionNormal = Vector3.Normalize(closestPoint - asteroidRigidbody.position);
				Vector3 direction = Vector3.Normalize(asteroidRigidbody.velocity);

				// Set the position to the closestpoint on the bounds and the velocity to the dampened reflection.
				asteroidRigidbody.position = closestPoint;
				Vector3 reflection = 2 * Vector3.Dot(reflectionNormal, -direction) * reflectionNormal + direction;
				asteroidRigidbody.velocity = reflection * asteroidRigidbody.velocity.magnitude;
			}

			// Do player bounds checking and apply constant force.
			if (!cameraBounds.Contains(asteroid.transform.position)) {
				Vector3 force = Time.fixedDeltaTime * asteroidRigidbody.mass * boundsForceMagnitude * Vector3.Normalize(cameraBounds.ClosestPoint(asteroidRigidbody.position) - asteroidRigidbody.position);
				asteroidRigidbody.AddForce(force);
			}
		}
	}

	private void SpawnAsteroid() {
        currentNumAsteroids++;

        Vector3 position = HelperFunctions.RandomPointInBoundsOutsideBounds(cameraBounds, asteroidBounds);
        position.z = zValue;
        GameObject newAsteroid = Instantiate(asteroidPrefab, position, Quaternion.identity, asteroidParent);
        Rigidbody rigidbody = newAsteroid.GetComponent<Rigidbody>();
        rigidbody.mass = HelperFunctions.GenerateGaussian(meanMass, massStandardDeviation);
        rigidbody.velocity = Vector3.Normalize(cameraBounds.ClosestPoint(position) - position) * Random.Range(1.0f, maxVelocity);
        rigidbody.angularVelocity = new Vector3(Random.Range(0.0f, Mathf.PI), Random.Range(0.0f, Mathf.PI), Random.Range(0.0f, Mathf.PI));

		newAsteroid.GetComponentInChildren<WarningSpriteController>().asteroidManager = this;
	}

	public void DisableWarningSprites() {
		foreach (GameObject asteroid in HelperFunctions.FindGameObjectsOnLayer("GravityObjects")) {
			asteroid.transform.GetChild(0).gameObject.SetActive(false);
		}
	}
}
