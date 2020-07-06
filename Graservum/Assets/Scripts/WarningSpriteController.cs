using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningSpriteController : MonoBehaviour {

	public AsteroidManager asteroidManager { get; set; }

#pragma warning disable
	[SerializeField]
	private GameObject parentAsteroid;
	[SerializeField]
	private PlayerInput playerInput;
	[SerializeField]
	private SpriteRenderer spriteRenderer;
	[SerializeField]
	private GameObject asteroidPrefab;
	[SerializeField]
	private float newAsteroidSpeed = 20.0f;
	[SerializeField]
	private float outsideDistanceModifier = 1.0f;
	[SerializeField]
	private float zOffset;
	[SerializeField]
	private float pulseSpeed = 1.0f;
	[SerializeField]
	private Color toColor;
	[SerializeField]
	private Vector3 toScale;
#pragma warning disable

	private bool isActive = false;
	private Color fromColor;
	private float activationMass;
	private float interpolationParameter = 0.0f; // 0 means nothing has happened, 1 means interpolation is complete.
	private int interpolationDirection = 1;
	private Rigidbody asteroidRigidbody;
	private Vector3 fromScale;
	private Vector3 offsetVector;

	void Start() {
		asteroidRigidbody = parentAsteroid.GetComponent<Rigidbody>();

		fromColor = spriteRenderer.color;
		fromScale = transform.localScale;
		activationMass = playerInput.maxAsteroidMass;
		offsetVector = new Vector3(0.0f, 0.0f, zOffset);

		spriteRenderer.enabled = false;
    }

    void Update() {
		// Update rotation and offset.
		transform.rotation = Quaternion.identity;
		transform.position = parentAsteroid.transform.position + offsetVector * parentAsteroid.transform.localScale.x;

		// If the parent asteroid's mass is greater than the activation mass, set isActive.
		isActive = asteroidRigidbody.mass > activationMass;

		if (isActive) {
			// Enable sprite renderer.
			spriteRenderer.enabled = true;

			// Update interpolation parameter.
			// Turn interpolation direction.
			if (interpolationParameter <= 0.0f || interpolationParameter >= 1.0f) {
				interpolationDirection *= -1;
			}
			interpolationParameter += interpolationDirection * pulseSpeed * Time.deltaTime;

			// Update pulse status.
			SetAnimation(interpolationParameter);
		} else {
			spriteRenderer.enabled = false;
		}
    }

	private void SetAnimation(float parameter) {
		// Interpolate color according to parameter.
		spriteRenderer.color = Color.Lerp(fromColor, toColor, parameter);

		// Interpolate scale according to parameter.
		transform.localScale = Vector3.Lerp(fromScale, toScale, parameter);
	}

	private void OnMouseDown() {
		SplitParentAsteroid();
	}

	// Destroys this asteroid and creates two new asteroids.
	private void SplitParentAsteroid() {
		// Find direction for velocities added to new asteroids.
		Vector3 directionToPlayer = Vector3.Normalize(parentAsteroid.transform.position - playerInput.transform.position);
		// New velocity is perpendicular to direction to player plus the velocity of the original asteroid.
		Vector3 perpendicularVelocity = Vector3.Cross(directionToPlayer, Vector3.back) * newAsteroidSpeed;

		// Asteroids have equal momentum, but half the original mass, so they both get the original velocity.
		// p = p/2 + p/2 = mv/2 + mv/2 = v(m/2) + v(m/2)
		float newMass = asteroidRigidbody.mass / 2;
		Vector3 newVelocity1 = asteroidRigidbody.velocity + perpendicularVelocity;
		Vector3 newVelocity2 = asteroidRigidbody.velocity - perpendicularVelocity;

		// Instantiate the new asteroids outside the original asteroid along their new velocities.
		Transform asteroidParent = GameObject.Find("AsteroidParent").transform;
		float outsideDistance = parentAsteroid.transform.localScale.x;
		GameObject newAsteroid1 = Instantiate(asteroidPrefab, parentAsteroid.transform.position + outsideDistance * newVelocity1.normalized * outsideDistanceModifier, Quaternion.identity, asteroidParent);
		asteroidManager.AsteroidCreated();
		newAsteroid1.GetComponentInChildren<WarningSpriteController>().asteroidManager = asteroidManager;
		GameObject newAsteroid2 = Instantiate(asteroidPrefab, parentAsteroid.transform.position + outsideDistance * newVelocity2.normalized * outsideDistanceModifier, Quaternion.identity, asteroidParent);
		asteroidManager.AsteroidCreated();
		newAsteroid2.GetComponentInChildren<WarningSpriteController>().asteroidManager = asteroidManager;

		// Set the mass, velocity and angular velocity for the two new asteroids.
		Rigidbody newRigidbody1 = newAsteroid1.GetComponent<Rigidbody>();
		newRigidbody1.mass = newMass;
		newRigidbody1.velocity = newVelocity1;
		newRigidbody1.angularVelocity = asteroidRigidbody.angularVelocity;

		Rigidbody newRigidbody2 = newAsteroid2.GetComponent<Rigidbody>();
		newRigidbody2.mass = newMass;
		newRigidbody2.velocity = newVelocity2;
		newRigidbody2.angularVelocity = -asteroidRigidbody.angularVelocity;

		// Destroy the parent asteroid.
		Destroy(parentAsteroid);
		asteroidManager.AsteroidDestroyed();
	}
}
