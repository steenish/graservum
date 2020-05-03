using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float emissionSpeed = 10.0f;
	[SerializeField]
	[Range(0.01f, 1.0f)]
	private float maxEmittedMassPerSecondFraction = 0.01f;
#pragma warning disable
    [SerializeField]
    private LineRenderer line;
#pragma warning restore
    [SerializeField]
    [Range(1.0f, 10.0f)]
    private float timeToMaxValue = 2.0f;
    [SerializeField]
#pragma warning disable
    private Slider massSlider;
#pragma warning restore
    [SerializeField]
	[Range(1.0f, 5.0f)]
	private float sliderSpeed = 2.0f;

    private bool currentlyAccelerating;
    private float _accumulatedTime;
    private float _massSliderProgress;
    private Rigidbody _rigidbody;

    void Start() {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update() {
        // Get mouse position to target.
        Vector3 targetPosition = 100 * GetMouseTargetDirection();

		// Set line direction.
		line.SetPosition(1, targetPosition);
		Debug.DrawLine(transform.position, targetPosition);

        // Check for new clicks, start slider timer.
        if (Input.GetMouseButtonDown(0)) {
            _accumulatedTime = 0.0f;
            currentlyAccelerating = true;
        }

        // Check for mouse held, update slider with new value.
        if (Input.GetMouseButton(0)) {
            _accumulatedTime += Time.deltaTime * sliderSpeed;
            float progress = _accumulatedTime / timeToMaxValue * massSlider.maxValue;
            massSlider.value = Mathf.Clamp(progress, massSlider.minValue, massSlider.maxValue);
            _massSliderProgress = massSlider.value / (massSlider.maxValue - massSlider.minValue);
        }

        // Check for mouse button release, set emission flag and reset slider.
        if (Input.GetMouseButtonUp(0)) {
            _massSliderProgress = massSlider.value / (massSlider.maxValue - massSlider.minValue);
            massSlider.value = massSlider.minValue;
            currentlyAccelerating = false;
        }
    }

    // TODO add spring forces for play area bounds
    void FixedUpdate() {
        if (currentlyAccelerating) {
            // Calculate mass.
            float differentialEmittedMass = _massSliderProgress * maxEmittedMassPerSecondFraction * _rigidbody.mass * Time.fixedDeltaTime;
            float newMass = _rigidbody.mass - differentialEmittedMass;

            // Get mouse position to target.
            Vector3 targetDirection = GetMouseTargetDirection();

            // Calculate velocity.
            Vector3 newVelocity = (_rigidbody.mass * _rigidbody.velocity - differentialEmittedMass * targetDirection * emissionSpeed) / newMass;

            _rigidbody.mass = newMass;
            _rigidbody.velocity = newVelocity;
        }
    }

	// Returns normalized direction vector from the player to the mouse position projected on the plane the player moves in.
    private Vector3 GetMouseTargetDirection() {
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		targetPos.z = transform.position.z;
		return transform.InverseTransformPoint(targetPos);
    }
}
