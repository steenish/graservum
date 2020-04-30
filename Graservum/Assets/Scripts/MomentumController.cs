using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MomentumController : MonoBehaviour {

    [SerializeField]
#pragma warning disable
    private AsteroidManager asteroidManager;
#pragma warning restore

    private Rigidbody _rigidbody;

    void Start() {
        _rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider other) {
        Rigidbody otherRigidbody = other.gameObject.GetComponent<Rigidbody>();

        if (otherRigidbody != null) {
            // Check if the other collider is a gravity object.
            if (other.gameObject.layer == LayerMask.NameToLayer("GravityObjects")) {
				
                // Set new velocity to product of direction and magnitude
                _rigidbody.velocity = (otherRigidbody.mass * otherRigidbody.velocity + _rigidbody.mass * _rigidbody.velocity) / (otherRigidbody.mass + _rigidbody.mass);

                // Add other object's mass to this object's mass.
                _rigidbody.mass += otherRigidbody.mass;

                // Remove the asteroid from the asteroid manager.
                asteroidManager.RemoveAsteroid(otherRigidbody.mass);

                // Destroy the other object.
                Destroy(other.gameObject);
            }
        }
    }
}
