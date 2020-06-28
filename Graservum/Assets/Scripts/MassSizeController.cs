using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MassSizeController : MonoBehaviour {
    // This entire class assumes that all gravity objects are entirely spherical.

    [SerializeField]
    private float Density = 3.34f; // Density measured in mass units per distance units cubed.
    // Density of the moon: 3.34 kg/m3 (or mass units per distance units cubed).

    Rigidbody _rigidbody;

    void Start() {
        _rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    void Update() {
        // Calculate the new scale from the radius depending on mass and density.
        float scale = GetRadius() * 2;

        // Set the new scale.
        gameObject.transform.localScale = new Vector3(scale, scale, scale);
    }

    // Returns the radius of the sphere with the given mass (density is given globally).
    float GetRadius() {
        return Mathf.Pow(3 * _rigidbody.mass / (4 * Mathf.PI * Density), 1f / 3f);
    }
}
