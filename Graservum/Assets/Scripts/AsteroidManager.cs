using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidManager : MonoBehaviour {

#pragma warning disable
    [SerializeField]
    private float meanMass;
    [SerializeField]
    private float massStandardDeviation;
    [SerializeField]
    private float minMass;
    [SerializeField]
    private float massLossRatio = 1.0f;
    [SerializeField]
    private int maxNumAsteroids;
    [SerializeField]
    private float maxVelocity;
    [SerializeField]
    private float spawnDistanceOutsideBounds;
    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private Transform asteroidParent;
    [SerializeField]
    private GameObject asteroidPrefab;
#pragma warning restore

    private Bounds asteroidBounds { get; set; }
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

        // Decrease the mass of each asteroid by the given ratio, and destroy if it goes below the threshold.
        foreach (GameObject asteroid in HelperFunctions.FindGameObjectsOnLayer("GravityObjects")) {
            Rigidbody rb = asteroid.GetComponent<Rigidbody>();
            rb.mass *= massLossRatio;

            if (rb.mass < minMass) {
                Destroy(asteroid);
                AsteroidDestroyed();
            }
        }
    }

    public void AsteroidDestroyed() {
        currentNumAsteroids--;
    }

    void FixedUpdate() {
        // Do bounds checking and apply spring forces.
        foreach (GameObject asteroid in HelperFunctions.FindGameObjectsOnLayer("GravityObjects")) {
            if (!asteroidBounds.Contains(asteroid.transform.position)) {
				Rigidbody asteroidRigidbody = asteroid.GetComponent<Rigidbody>();
				
				// Get the required vectors for calculating reflection vector.
				Vector3 closestPoint = asteroidBounds.ClosestPoint(asteroidRigidbody.position);
				Vector3 reflectionNormal = Vector3.Normalize(closestPoint - asteroidRigidbody.position);
				Vector3 direction = Vector3.Normalize(asteroidRigidbody.velocity);

				// Set the position to the closestpoint on the bounds and the velocity to the dampened reflection.
				asteroidRigidbody.position = closestPoint;
				Vector3 reflection = 2 * Vector3.Dot(reflectionNormal, -direction) * reflectionNormal + direction;
				asteroidRigidbody.velocity = reflection * asteroidRigidbody.velocity.magnitude;
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
    }
}
