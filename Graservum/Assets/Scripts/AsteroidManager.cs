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

    private float zValue;
    private int currentNumAsteroids;
    private List<GameObject> asteroids;

    void Start() {
        zValue = GameObject.Find("Player").transform.position.z;
    }

    void Update() {
        // Spawn one asteroid per frame if the maximum number of asteroids has not been reached.
        if (currentNumAsteroids < maxNumAsteroids) {
            SpawnAsteroid();
        }
    }

    private void SpawnAsteroid() {
        currentNumAsteroids++;
        Bounds cameraBounds = Camera.main.GetComponent<CameraController>().cameraBounds;

        Vector3 position = HelperFunctions.RandomPointOutsideBounds2d(cameraBounds, spawnDistanceOutsideBounds);
        position.z = zValue;
        GameObject newAsteroid = Instantiate(asteroidPrefab, position, Quaternion.identity, asteroidParent);
        Rigidbody rigidbody = newAsteroid.GetComponent<Rigidbody>();
        rigidbody.mass = HelperFunctions.GenerateGaussian(meanMass, massStandardDeviation);
        rigidbody.velocity = Vector3.Normalize(cameraBounds.ClosestPoint(position) - position) * Random.Range(1.0f, maxVelocity);
        rigidbody.angularVelocity = new Vector3(Random.Range(0.0f, Mathf.PI), Random.Range(0.0f, Mathf.PI), Random.Range(0.0f, Mathf.PI));

        asteroids.Add(newAsteroid);
    }

    // TODO bounds checking
}
