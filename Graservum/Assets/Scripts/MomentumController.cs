using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MomentumController : MonoBehaviour {

    Rigidbody _rigidbody;

    void Start()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider other) {
        Rigidbody otherRigidbody = other.gameObject.GetComponent<Rigidbody>();

        // Check if the other collider is a gravity object.
        if (other.gameObject.layer == LayerMask.NameToLayer("GravityObjects")) {

            // Check if the other object has a greater mass.
            if (otherRigidbody.mass < _rigidbody.mass) {

                // Add together masses and velocities and destroy the other object.
                _rigidbody.mass += otherRigidbody.mass;
                _rigidbody.velocity += otherRigidbody.velocity;
                Destroy(other.gameObject);
            } else if (otherRigidbody.mass == _rigidbody.mass) { // Corner case when the masses are the same.
                // Do something.
            }
        }
    }
}
