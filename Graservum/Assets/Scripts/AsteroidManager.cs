using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AsteroidManager : MonoBehaviour {

#pragma warning disable
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
    private float[] asteroidMasses = { 10000, 1000, 100, 60, 30, 10 };
    private int[] numAsteroidsPerMass;
    private int[] numMaxAsteroidsPerMass;
    private List<GameObject> asteroids;
    private int numCurrentAsteroids = 0;
    private int numMaxAsteroids = 0;

    void Start() {
        zValue = playerTransform.position.z;
        asteroids = new List<GameObject>();
        numAsteroidsPerMass = new int[asteroidMasses.Length];

        // Adjust maximum number of asteroids for Zipf.
        numMaxAsteroidsPerMass = new int[asteroidMasses.Length];
        for (int i = 0; i < asteroidMasses.Length; ++i) {
            numMaxAsteroidsPerMass[i] = (int) Mathf.Pow(2, i);
            numMaxAsteroids += numMaxAsteroidsPerMass[i];
        }

        SpawnAsteroids(numMaxAsteroids - numCurrentAsteroids);
    }

    void Update() {
        int numNewAsteroids = numMaxAsteroids - numCurrentAsteroids;

        if (numNewAsteroids > 0) {
            SpawnAsteroids(numNewAsteroids);
        }
    }

    private void SpawnAsteroids(int numNewAsteroids) {
        numCurrentAsteroids += numNewAsteroids;
        Bounds cameraBounds = Camera.main.GetComponent<CameraController>().cameraBounds;

        for (int i = 0; i < numNewAsteroids; ++i) {
            Vector3 position = HelperFunctions.RandomPointOutsideBounds2d(cameraBounds, spawnDistanceOutsideBounds);
            position.z = zValue;
            GameObject newAsteroid = Instantiate(asteroidPrefab, position, Quaternion.identity, asteroidParent);
            Rigidbody rigidbody = newAsteroid.GetComponent<Rigidbody>();
            rigidbody.mass = PickMass();
            rigidbody.velocity = Vector3.Normalize(cameraBounds.ClosestPoint(position) - position) * UnityEngine.Random.Range(1.0f, maxVelocity);
            rigidbody.angularVelocity = new Vector3(UnityEngine.Random.Range(0.0f, Mathf.PI), UnityEngine.Random.Range(0.0f, Mathf.PI), UnityEngine.Random.Range(0.0f, Mathf.PI));
            asteroids.Add(newAsteroid);
        }
    }

    private float PickMass() {
        int randomIndex = UnityEngine.Random.Range(0, numAsteroidsPerMass.Length);
        float resultingMass = asteroidMasses[asteroidMasses.Length - 1];
        
        // Go through asteroid masses linearly starting at the random index.
        for (int i = 0; i < numAsteroidsPerMass.Length; ++i) {
            int index = (randomIndex + i) % numAsteroidsPerMass.Length; // Calculate index into numAsteroidsPerMass.
            int numAsteroids = numAsteroidsPerMass[index];
            
            // If there are fewer asteroids of the mass at the index than the maximum number of asteroids of that mass.
            if (numAsteroids < numMaxAsteroidsPerMass[index]) {
                numAsteroidsPerMass[index] += 1;
                resultingMass = asteroidMasses[index];
                break;
            }
        }
        return resultingMass;
    }

    public void RemoveAsteroid(float mass) {
        int index = FindIndexOfMass(mass);
        Debug.Assert(index != -1, "index of mass not found");
        if (index != -1) {
            numCurrentAsteroids--;
            numAsteroidsPerMass[index]--;
        }
    }

    private int FindIndexOfMass(float mass) {
        for (int i = 0; i < asteroidMasses.Length; ++i) {
            if (mass < asteroidMasses[i] + HelperFunctions.EPSILON && mass > asteroidMasses[i] - HelperFunctions.EPSILON)
                return i;
        }
        return -1;
    }
}
