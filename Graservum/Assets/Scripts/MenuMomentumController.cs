using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MenuMomentumController : MonoBehaviour {

#pragma warning disable
    [SerializeField]
    private GameObject asteroidDeathParticles;
#pragma warning restore

    private MassSizeController massSizeController;
    private Rigidbody _rigidbody;

    void Start() {
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        massSizeController = gameObject.GetComponent<MassSizeController>();
    }

    void OnCollisionEnter(Collision other) {
        Rigidbody otherRigidbody = other.rigidbody;

        if (otherRigidbody != null) {
            // Check if the other collider is a gravity object.
            if (other.gameObject.layer == LayerMask.NameToLayer("GravityObjects")) {
                // If other rigidbody has smaller mass and is not the player, or if this rigidbody is the player.
                if (otherRigidbody.mass <= _rigidbody.mass) {
                    // Play a random rock collision sound effect.
                    HelperFunctions.PlayRandomRockSound();

                    GameObject particles = Instantiate(asteroidDeathParticles, other.transform.position, Quaternion.identity);
                    particles.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
                }
            }
        }
    }
}
