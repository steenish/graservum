using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float emissionSpeed = 10.0f;
    [SerializeField]
    private Transform childCollectionTransform;
    [SerializeField]
    private LineRenderer line;
    [SerializeField]
    [Range(1.0f, 10.0f)]
    private float timeToMaxValue = 2.0f;
    [SerializeField]
    private Slider massSlider;

    private bool _instantiateOnNextFixedUpdate;
    private float _timeClicked;
    private float _accumulatedTime;
    private Rigidbody _rigidbody;
    private Vector3 _emissionVelocityDirection;

    void Start() {
        _rigidbody = GetComponent<Rigidbody>();
        _emissionVelocityDirection = Vector3.Normalize(-transform.forward);
    }

    void Update() {
        // Get mouse position to target.
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPos.z = transform.position.z;
        targetPos = 100 * transform.InverseTransformPoint(targetPos);

        // Set line direction.
        line.SetPosition(1, targetPos);

        // Check for new clicks, start slider timer.
        if (Input.GetMouseButtonDown(0)) {
            _timeClicked = Time.time;
            _accumulatedTime = 0.0f;
        }

        // Check for mouse held, update slider with new value.
        if (Input.GetMouseButton(0)) {
            _accumulatedTime += Time.deltaTime;
            float progress = _accumulatedTime / timeToMaxValue * massSlider.maxValue;
            Debug.Log("progress = " + progress);
            massSlider.value = Mathf.Clamp(progress, massSlider.minValue, massSlider.maxValue);
        }

        // Check for mouse button release, set emission flag and reset slider.
        if (Input.GetMouseButtonUp(0)) {
            _instantiateOnNextFixedUpdate = true;
            massSlider.value = massSlider.minValue;
        }
    }

    void FixedUpdate() {
        //if (_instantiateOnNextFixedUpdate) {
        //    // Calculate mass.
        //    float newMass = _rigidbody.mass - emittedMass;
        //    Debug.Log("newMass = " + newMass);

        //    // Calculate velocity.
        //    Vector3 newVelocity = Time.fixedDeltaTime * (_rigidbody.mass * _rigidbody.velocity - emittedMass * _emissionVelocityDirection * emissionSpeed) / newMass;

        //    // Instantiate child.
        //    GameObject childObject = Instantiate(gameObject, _emissionVelocityDirection * transform.localScale.x, transform.rotation, childCollectionTransform);
        //    Destroy(childObject.GetComponent<PlayerController>());
        //    Destroy(childObject.GetComponent<MomentumController>());

        //    // Update mass and velocities.
        //    Rigidbody childRigidbody = childObject.GetComponent<Rigidbody>();
        //    childRigidbody.mass = emittedMass;
        //    childRigidbody.velocity = _emissionVelocityDirection * emissionSpeed;

        //    _rigidbody.mass = newMass;
        //    _rigidbody.velocity = newVelocity;

        //    _instantiateOnNextFixedUpdate = false;
        //}
    }
}
