using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Velocity {

    [SerializeField]
    private Vector3 _velocity;
    public Vector3 velocity { get => _velocity; private set => _velocity = value; }

    [SerializeField]
    private Vector3 _angularVelocity;
    public Vector3 angularVelocity { get => _angularVelocity; private set => _angularVelocity = value; }

    public Velocity() {
        velocity = Vector3.zero;
        angularVelocity = Vector3.zero;
    }

    public Velocity(Vector3 velocity, Vector3 angularVelocity) {
        this.velocity = velocity;
        this.angularVelocity = angularVelocity;
    }

    public static Velocity RandomVelocity(float velocityLowerBound, float velocityUpperBound, float angularLowerBound, float angularUpperBound) {
        Vector3 velocity = new Vector3(Random.Range(velocityLowerBound, velocityUpperBound),
                                       Random.Range(velocityLowerBound, velocityUpperBound),
                                       Random.Range(velocityLowerBound, velocityUpperBound));
        Vector3 angularVelocity = new Vector3(Random.Range(angularLowerBound, angularUpperBound),
                                              Random.Range(angularLowerBound, angularUpperBound),
                                              Random.Range(angularLowerBound, angularUpperBound));
        return new Velocity(velocity, angularVelocity);
    }
}
