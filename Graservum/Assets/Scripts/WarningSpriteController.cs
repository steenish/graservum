using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningSpriteController : MonoBehaviour {

#pragma warning disable
	[SerializeField]
	private GameObject parentAsteroid;
	[SerializeField]
	private PlayerInput playerInput;
	[SerializeField]
	private SpriteRenderer spriteRenderer;
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
		offsetVector = new Vector3(0.0f, 0.0f, -zOffset);

		spriteRenderer.enabled = false;
    }

    void Update() {
		// If the parent asteroid's mass is greater than the activation mass, set isActive.
		//isActive = asteroidRigidbody.mass > activationMass;
		isActive = true;

		if (isActive) {
			// Enable sprite renderer.
			spriteRenderer.enabled = true;

			// Update rotation and offset.
			transform.rotation = Quaternion.identity;
			transform.position = parentAsteroid.transform.position + offsetVector * parentAsteroid.transform.localScale.x;

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
}
