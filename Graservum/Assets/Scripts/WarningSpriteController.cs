using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WarningSpriteController : MonoBehaviour, IPointerClickHandler {

	public AsteroidManager asteroidManager { get; set; }

#pragma warning disable
	[SerializeField]
	private GameObject parentAsteroid;
	[SerializeField]
	private Sprite smallWarningSprite;
	[SerializeField]
	private Sprite criticalWarningSprite;
	[SerializeField]
	private SpriteRenderer spriteRenderer;
	[SerializeField]
	private CircleCollider2D collider;
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

	private bool isCriticalWarning = false;
	private bool isActive = false;
	private Color fromColor;
	private float activationMass;
	private float interpolationParameter = 0.0f; // 0 means nothing has happened, 1 means interpolation is complete.
	private int interpolationDirection = 1;
	private PlayerInput playerInput;
	private Rigidbody parentAsteroidRigidbody;
	private Rigidbody playerAsteroidRigidbody;
	private Vector3 fromScale;
	private Vector3 offsetVector;

	void Start() {
		parentAsteroidRigidbody = parentAsteroid.GetComponent<Rigidbody>();
		playerInput = GameObject.Find("Player")?.GetComponent<PlayerInput>();
		playerAsteroidRigidbody = playerInput?.playerAsteroid.GetComponent<Rigidbody>();

		fromColor = spriteRenderer.color;
		fromScale = transform.localScale;
		activationMass = (playerInput != null) ? playerInput.maxAsteroidMass : Mathf.Infinity;
		offsetVector = new Vector3(0.0f, 0.0f, zOffset);

		spriteRenderer.enabled = false;
		collider.enabled = false;
    }

    void Update() {
		// Update rotation and offset.
		transform.rotation = Quaternion.identity;
		transform.position = parentAsteroid.transform.position + offsetVector * parentAsteroid.transform.localScale.x;
		
		// If the sum of the parent asteroid's mass and the player asteroid's mass is greater than the activation mass, set isActive.
		isActive = ((playerAsteroidRigidbody != null) ? playerAsteroidRigidbody.mass : 0) + parentAsteroidRigidbody.mass > activationMass;
	
		// If the parent asteroid's mass itself is greater than the activation mass, set criticalWarning.
		isCriticalWarning = parentAsteroidRigidbody.mass > activationMass;

		// Pick sprite depending on type of warning.
		if (isCriticalWarning) {
			spriteRenderer.sprite = criticalWarningSprite;
		} else {
			spriteRenderer.sprite = smallWarningSprite;
		}

		if (isActive) {
			// Enable sprite renderer and collider.
			spriteRenderer.enabled = true;
			collider.enabled = true;

			// Update interpolation parameter.
			// Turn interpolation direction.
			if (interpolationParameter <= 0.0f || interpolationParameter >= 1.0f) {
				interpolationDirection *= -1;
			}
			interpolationParameter += interpolationDirection * pulseSpeed * Time.deltaTime;
			interpolationParameter = Mathf.Clamp(interpolationParameter, 0.0f, 1.0f);

			// Update pulse status.
			SetAnimation(interpolationParameter);
		} else {
			spriteRenderer.enabled = false;
			spriteRenderer.enabled = false;
		}
    }

	private void SetAnimation(float parameter) {
		// Interpolate color according to parameter.
		spriteRenderer.color = Color.Lerp(fromColor, toColor, parameter);

		// Interpolate scale according to parameter.
		transform.localScale = Vector3.Lerp(fromScale, toScale, parameter);
	}
	
	public void OnPointerClick(PointerEventData eventData) {
		if (isCriticalWarning) {
			SplitParentAsteroid();
		}
	}

	// Destroys this asteroid and creates two new asteroids.
	private void SplitParentAsteroid() {
		// Find direction for velocities added to new asteroids.
		Vector3 directionToPlayer = Vector3.Normalize(parentAsteroid.transform.position - ((playerInput != null) ? playerInput.playerAsteroid.transform.position : Vector3.zero));
		// New velocity is perpendicular to direction to player plus the velocity of the original asteroid.
		Vector3 perpendicularVelocity = Vector3.Cross(directionToPlayer, Vector3.back) * newAsteroidSpeed;

		// Project the parent asteroid's velocity on the direction to the player.
		// Using this component of the velocity ensures more beautiful splits.
		Vector3 projectedVelocity = Vector3.Project(parentAsteroidRigidbody.velocity, directionToPlayer);

		// Asteroids have equal momentum, but half the original mass, so they both get the original velocity.
		// p = p/2 + p/2 = mv/2 + mv/2 = v(m/2) + v(m/2)
		float newMass = parentAsteroidRigidbody.mass / 2;
		Vector3 newVelocity1 = projectedVelocity + perpendicularVelocity;
		Vector3 newVelocity2 = projectedVelocity - perpendicularVelocity;

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
		newRigidbody1.angularVelocity = parentAsteroidRigidbody.angularVelocity;

		Rigidbody newRigidbody2 = newAsteroid2.GetComponent<Rigidbody>();
		newRigidbody2.mass = newMass;
		newRigidbody2.velocity = newVelocity2;
		newRigidbody2.angularVelocity = -parentAsteroidRigidbody.angularVelocity;

		// Destroy the parent asteroid.
		Destroy(parentAsteroid);
		asteroidManager.AsteroidDestroyed();
	}
}
