using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MomentumController : MonoBehaviour {

    Rigidbody _rigidbody;

    void Start() {
        _rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    void Update() {
        Debug.Log(this + " velocity: " + _rigidbody.velocity);
    }

    void OnTriggerEnter(Collider other) {
        Rigidbody otherRigidbody = other.gameObject.GetComponent<Rigidbody>();

        // Check if the other collider is a gravity object.
        if (other.gameObject.layer == LayerMask.NameToLayer("GravityObjects")) {

            // Check if the other object has a greater mass.
            if (otherRigidbody.mass <= _rigidbody.mass) {
                // Calculate momentums
                Vector3 otherMomentum = otherRigidbody.mass * otherRigidbody.velocity;
                Vector3 momentum = _rigidbody.mass * _rigidbody.velocity;
                
                float totalMass = otherRigidbody.mass + _rigidbody.mass;
                //Vector3 direction = (otherRigidbody.velocity + _rigidbody.velocity).normalized;

                // Add together masses.
                _rigidbody.mass += otherRigidbody.mass;

                // Set new velocity to product of direction and magnitude
                _rigidbody.velocity = (otherMomentum + momentum) / totalMass;

                // Destroy the other object.
                Destroy(other.gameObject);
            }
        }
        Debug.Log("Collision.");
    }
}
