using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float emissionSpeed = 10.0f;
	[SerializeField]
	[Range(0.01f, 1.0f)]
	private float maxEmittedMassPerSecondFraction = 0.01f;
    [SerializeField]
    [Range(1.0f, 10.0f)]
    private float timeToMaxValue = 2.0f;
    [SerializeField]
#pragma warning disable
    private Slider massSlider;
#pragma warning restore
    [SerializeField]
	[Range(1.0f, 5.0f)]
	private float accelerationRate = 2.0f;
    [SerializeField]
    [Range(1.0f, 5.0f)]
    private float cooldownRate = 2.0f;
    [SerializeField]
#pragma warning disable
    private Transform engineTransform;
    [SerializeField]
    private ParticleSystem engineExhaustParticles;
    [SerializeField]
    private Color minSpeedColor;
    [SerializeField]
    private Color maxSpeedColor;
#pragma warning restore

    private bool currentlyAccelerating;
    private float _accumulatedTime = 0.0f;
    private float _massSliderProgress;
    private Gradient currentGradient;
    private GradientColorKey[] colorKeys;
    private GradientAlphaKey[] alphaKeys;
    private ParticleSystem.ColorOverLifetimeModule exhaustColorModule;
    private ParticleSystem.MainModule exhaustMainModule;
    private Rigidbody _rigidbody;

    void Start() {
        _rigidbody = GetComponent<Rigidbody>();
        exhaustColorModule = engineExhaustParticles.colorOverLifetime;
        exhaustMainModule = engineExhaustParticles.main;
            
        exhaustMainModule.startSpeed = emissionSpeed;

        colorKeys = new GradientColorKey[] { new GradientColorKey(minSpeedColor, 0.0f), new GradientColorKey(Color.white, 0.5f) };
        alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) };
        currentGradient = new Gradient();
        currentGradient.SetKeys(colorKeys, alphaKeys);
    }

    void Update() {

        Debug.Log(engineExhaustParticles.isPlaying);

        MoveEngine();

        // Check for new clicks, start slider timer.
        if (Input.GetMouseButtonDown(0)) {
            currentlyAccelerating = true;
        }

        // Check for mouse held, update the accumulated time.
        if (Input.GetMouseButton(0)) {
            _accumulatedTime += Time.deltaTime * accelerationRate;
        } else {
            _accumulatedTime -= Time.deltaTime * cooldownRate;
        }
        _accumulatedTime = Mathf.Clamp(_accumulatedTime, 0.0f, timeToMaxValue);

        // Update slider with new value.
        float progress = _accumulatedTime / timeToMaxValue * massSlider.maxValue;
        massSlider.value = Mathf.Clamp(progress, massSlider.minValue, massSlider.maxValue);
        _massSliderProgress = massSlider.value / (massSlider.maxValue - massSlider.minValue);

        // Update particle size from asteroid scale.
        exhaustMainModule.startSize = transform.localScale.x;

        // Check for mouse button release, set emission flag and reset slider.
        if (Input.GetMouseButtonUp(0)) {
            currentlyAccelerating = false;
        }

        // Update exhaust. TODO debug this stuff
        if (_massSliderProgress > massSlider.minValue) {
            if (!engineExhaustParticles.isPlaying) {
                engineExhaustParticles.Play();
            }

            Color currentAccelerationColor = Color.Lerp(minSpeedColor, maxSpeedColor, _massSliderProgress);
            colorKeys[0].color = currentAccelerationColor;
            currentGradient.SetKeys(colorKeys, alphaKeys);
            exhaustColorModule.color = currentGradient;
        } else {
            engineExhaustParticles.Stop();
        }
    }

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
		return transform.InverseTransformPoint(targetPos).normalized;
    }

    // Moves the engine to the appropriate position on the asteroid's surface according to mouse position.
    private void MoveEngine() {
        Vector3 targetDirection = GetMouseTargetDirection();
        float angle = Vector3.Angle(Vector3.down, targetDirection);
        engineTransform.position = transform.position + targetDirection * transform.localScale.x;
        engineTransform.localEulerAngles = new Vector3(0.0f, 0.0f, (targetDirection.x < 0) ? 360 - angle : angle);
    }
}
