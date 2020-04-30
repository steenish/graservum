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
	private float maxEmittedMassFraction = 0.1f;
    [SerializeField]
#pragma warning disable
    private Transform childCollectionTransform;
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
    [SerializeField]
#pragma warning disable
    private GameObject emittedObjectPrefab;
#pragma warning restore

    private bool _instantiateOnNextFixedUpdate;
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
        }

        // Check for mouse held, update slider with new value.
        if (Input.GetMouseButton(0)) {
            _accumulatedTime += Time.deltaTime * sliderSpeed;
            float progress = _accumulatedTime / timeToMaxValue * massSlider.maxValue;
            massSlider.value = Mathf.Clamp(progress, massSlider.minValue, massSlider.maxValue);
        }

        // Check for mouse button release, set emission flag and reset slider.
        if (Input.GetMouseButtonUp(0)) {
            _massSliderProgress = massSlider.value / (massSlider.maxValue - massSlider.minValue);
            massSlider.value = massSlider.minValue;
            _instantiateOnNextFixedUpdate = true;
        }
    }

    // TODO add spring forces for play area bounds
    void FixedUpdate() {
        if (_instantiateOnNextFixedUpdate) {
            // Calculate mass.
            float emittedMass = _massSliderProgress * maxEmittedMassFraction * _rigidbody.mass;
            float newMass = _rigidbody.mass - emittedMass;

            // Get mouse position to target.
            Vector3 targetDirection = -GetMouseTargetDirection();

            // Calculate velocity.
            Vector3 newVelocity = Time.fixedDeltaTime * (_rigidbody.mass * _rigidbody.velocity - emittedMass * targetDirection * emissionSpeed) / newMass;

            // Instantiate child.
            GameObject childObject = Instantiate(emittedObjectPrefab, transform.position + targetDirection * transform.localScale.x, transform.rotation, childCollectionTransform);

            // Update mass and velocities.
            Rigidbody childRigidbody = childObject.GetComponent<Rigidbody>();
            childRigidbody.mass = emittedMass;
            childRigidbody.velocity = targetDirection * emissionSpeed;

            _rigidbody.mass = newMass;
            _rigidbody.velocity += newVelocity;

            _instantiateOnNextFixedUpdate = false;
        }
    }

	// Returns normalized direction vector from the player to the mouse position projected on the plane the player moves in.
    private Vector3 GetMouseTargetDirection() {
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		targetPos.z = transform.position.z;
		return transform.InverseTransformPoint(targetPos);
    }
}
