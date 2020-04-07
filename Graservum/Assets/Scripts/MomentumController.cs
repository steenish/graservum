﻿using System.Collections;
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
                // Set new velocity to product of direction and magnitude
                _rigidbody.velocity = (otherRigidbody.mass * otherRigidbody.velocity + _rigidbody.mass * _rigidbody.velocity) / (otherRigidbody.mass + _rigidbody.mass);

                // Add other object's mass to this object's mass.
                _rigidbody.mass += otherRigidbody.mass;

                // Destroy the other object.
                Destroy(other.gameObject);
            }
        }
        Debug.Log("Collision.");
    }
}
