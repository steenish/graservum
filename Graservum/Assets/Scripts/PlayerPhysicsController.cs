using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysicsController : MonoBehaviour
{

	// --- Inspector-exposd public properties ---

	[SerializeField]
	private float _accelerationRate;
	public float accelerationRate {
		get { return _accelerationRate; }
		private set { _accelerationRate = value; }
	}

	[SerializeField]
	private float _cooldownRate;
	public float cooldownRate {
		get { return _cooldownRate; }
		private set { _cooldownRate = value; }
	}

	// --- Inspector-exposed private fields ---

#pragma warning disable
	[SerializeField]
	private PlayerInput playerInput;
	[SerializeField]
	private float emissionSpeed = 100.0f;
	[SerializeField]
	[Range(0.01f, 1.0f)]
	private float maxEmittedMassPerSecondFraction = 0.2f;
	[SerializeField]
	private float velocityDamping = 1.0f;
#pragma warning restore

	// --- Public properties ---

	public bool currentlyAccelerating { get; set; } = false;
	public Bounds playerBounds { get; private set; }

	// --- Private fields ---
	private float inverseMass;
	private Rigidbody _rigidbody;

	void Start() {
		playerBounds = Camera.main.GetComponent<CameraController>().cameraBounds;
		_rigidbody = GetComponent<Rigidbody>();
		inverseMass = 1 / _rigidbody.mass;
	}

	void FixedUpdate() {
		// If player input has said to accelerate on last Update() of PlayerInput, accelerate.
		if (currentlyAccelerating) {
			Accelerate();
		}
		CheckBounds();
	}

	private void Accelerate() {
		// Calculate mass.
		float differentialEmittedMass = playerInput.burnSlider.sliderProgress * maxEmittedMassPerSecondFraction * _rigidbody.mass * Time.fixedDeltaTime;
		float newMass = _rigidbody.mass - differentialEmittedMass;

		// Add the emitted mass to the score.
		playerInput.score += differentialEmittedMass;

		// Get joystick direction.
		Vector3 targetDirection = playerInput.joyStick.direction;

		// Calculate velocity.
		Vector3 newVelocity = (_rigidbody.mass * _rigidbody.velocity - differentialEmittedMass * targetDirection * emissionSpeed) / newMass;

		// Apply mass and velocity.
		UpdateMass(newMass);
		_rigidbody.velocity = newVelocity;
	}

	private void CheckBounds() {
		// If player is not in bounds, reflect and dampen the player's velocity.
		if (!playerBounds.Contains(transform.position)) {	
			// Get the required vectors for calculating reflection vector.
			Vector3 closestPoint = playerBounds.ClosestPoint(_rigidbody.position);
			Vector3 reflectionNormal = Vector3.Normalize(closestPoint - _rigidbody.position);
			Vector3 direction = Vector3.Normalize(_rigidbody.velocity);

			// Set the position to the closestpoint on the bounds and the velocity to the dampened reflection.
			_rigidbody.position = closestPoint;
			Vector3 reflection = 2 * Vector3.Dot(reflectionNormal, -direction) * reflectionNormal + direction;
			_rigidbody.velocity = reflection * _rigidbody.velocity.magnitude * velocityDamping;
		}
	}

	private void UpdateMass(float newMass) {
		_rigidbody.mass = newMass;
		inverseMass = 1 / newMass;
	}
}
