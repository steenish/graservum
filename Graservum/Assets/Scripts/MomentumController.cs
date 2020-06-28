﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MomentumController : MonoBehaviour {

    private MassSizeController massSizeController;
    private Rigidbody _rigidbody;

    void Start() {
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        massSizeController = gameObject.GetComponent<MassSizeController>();
    }

    void OnTriggerEnter(Collider other) {
        Rigidbody otherRigidbody = other.attachedRigidbody;

        if (otherRigidbody != null) {
            // Check if the other collider is a gravity object.
            if (other.gameObject.layer == LayerMask.NameToLayer("GravityObjects")) {
                // Check if other asteroid has larger mass.
                if (otherRigidbody.mass <= _rigidbody.mass) {
                    // Set new velocity to product of direction and magnitude
                    _rigidbody.velocity = (otherRigidbody.mass * otherRigidbody.velocity + _rigidbody.mass * _rigidbody.velocity) / (otherRigidbody.mass + _rigidbody.mass);

                    // Add other object's mass to this object's mass.
                    _rigidbody.mass += otherRigidbody.mass;

                    // Destroy the other object.
                    GameObject.Find("AsteroidManager").GetComponent<AsteroidManager>().AsteroidDestroyed();
                    Destroy(other.gameObject);
                }
            }
        } else {
            // TODO: Handle player case?
        }
    }
}
