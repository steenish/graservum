using System.Collections;
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
                // If other rigidbody has smaller mass and is not the player, or if this rigidbody is the player.
                if (otherRigidbody.mass <= _rigidbody.mass && otherRigidbody.tag != "Player" || _rigidbody.tag == "Player") {
                    // Set new velocity to product of direction and magnitude
                    _rigidbody.velocity = (otherRigidbody.mass * otherRigidbody.velocity + _rigidbody.mass * _rigidbody.velocity) / (otherRigidbody.mass + _rigidbody.mass);

					// Set new angular velocity.
					_rigidbody.angularVelocity = (otherRigidbody.mass * otherRigidbody.angularVelocity + _rigidbody.mass * _rigidbody.angularVelocity) / (otherRigidbody.mass + _rigidbody.mass);

                    // Add other object's mass to this object's mass.
                    _rigidbody.mass += otherRigidbody.mass;

                    // Play a random rock collision sound effect.
                    int soundNumber = Random.Range(1, 32);
                    string soundName = "Rock" + ((soundNumber < 10) ? "0" : "") + soundNumber;
                    AudioManager.instance.Play(soundName);

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
