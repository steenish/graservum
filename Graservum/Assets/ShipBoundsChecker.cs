using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBoundsChecker : MonoBehaviour {

	[SerializeField]
	private SphereCollider asteroidCollider;
	[SerializeField]
	private SphereCollider shipCollider;

	private Bounds shipColliderBounds;
	private Vector3 shipColliderBoundsSize;

	void Start() {
		shipColliderBounds = shipCollider.bounds;
		shipColliderBoundsSize = shipColliderBounds.size;
    }

    void Update() {
		// Get the current bounds of the player asteroid collider's bounds.
		Bounds asteroidColliderBounds = asteroidCollider.bounds;

		// Grow the ship collider's bounds to encapsulate the asteroid collider's bounds.
		shipColliderBounds.Encapsulate(asteroidColliderBounds);

		// Get the size after encapsulation.
		Vector3 potentialSize = shipColliderBounds.size;

		// If the new size is larger than the ship collider bound's size, destroy the player.
		if (potentialSize.x > shipColliderBoundsSize.x ||
			potentialSize.y > shipColliderBoundsSize.y ||
			potentialSize.z > shipColliderBoundsSize.z) {
			Destroy(transform.parent.gameObject);
		}
    }
}
