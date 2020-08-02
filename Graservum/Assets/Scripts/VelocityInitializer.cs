using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityInitializer : MonoBehaviour {

#pragma warning disable
    [SerializeField]
    private GameObject[] objects;
    [SerializeField]
    private Velocity[] velocities;
    [SerializeField]
    private bool randomizeAll = false;
    [SerializeField]
    private float randomVelocityLowerBound;
    [SerializeField]
    private float randomVelocityUpperBound;
    [SerializeField]
    private float randomAngularLowerBound;
    [SerializeField]
    private float randomAngularUpperBound;
#pragma warning restore

    void Start() {
        for (int i = 0; i < objects.Length; ++i) {
            Rigidbody rigidbody = objects[i].GetComponent<Rigidbody>();
            Velocity randomVelocity = !randomizeAll ? null : Velocity.RandomVelocity(randomVelocityLowerBound, randomVelocityUpperBound, randomAngularLowerBound, randomAngularUpperBound);
            rigidbody.velocity = !randomizeAll ? velocities[i].velocity : randomVelocity.velocity;
            rigidbody.angularVelocity = !randomizeAll ? velocities[i].angularVelocity : randomVelocity.angularVelocity;
        }
    }
}
