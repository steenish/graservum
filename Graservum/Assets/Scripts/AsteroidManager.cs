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
    [SerializeField]
    private float springStiffness = 1.0f;

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
    }

    public void AsteroidDestroyed() {
        currentNumAsteroids--;
    }

    void FixedUpdate() {
        // Do bounds checking and apply spring forces.
        foreach (GameObject asteroid in HelperFunctions.FindGameObjectsOnLayer("GravityObjects")) {
            if (!asteroidBounds.Contains(asteroid.transform.position)) {
                asteroid.GetComponent<Rigidbody>().AddForce(springStiffness * Vector3.Normalize(asteroidBounds.ClosestPoint(transform.position) - transform.position));
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
