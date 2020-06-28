using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Class for taking player input and signalling player components to update.
public class PlayerInput : MonoBehaviour {

    // --- Inspector-exposd public properties ---

    [SerializeField]
    private GameObject _playerAsteroid;
    public GameObject playerAsteroid { get => _playerAsteroid; private set => _playerAsteroid = value; }

    [SerializeField]
    private EngineController _engineController;
    public EngineController engineController { get => _engineController; private set => _engineController = value; }

    [SerializeField]
    private PlayerPhysicsController _playerPhysicsController;
    public PlayerPhysicsController playerPhysicsController { get => _playerPhysicsController; private set => _playerPhysicsController = value; }

    [SerializeField]
    private EasySlider _burnSlider;
    public EasySlider burnSlider { get => _burnSlider; private set => _burnSlider = value; }

    // --- Inspector-exposed private fields ---

#pragma warning disable
    [SerializeField]
    [Range(1.0f, 10.0f)]
    private float timeToMaxValue = 2.0f;
    [SerializeField]
    private Text scoreText;
#pragma warning restore

    // --- Public properties ---

    public float score { get; set; } = 0.0f;

    // --- Private fields ---

    private float accumulatedTime = 0.0f;

    void OnDestroy() {
        Debug.Log("Game Over!");
    }

    void Update() {

        // Check for new clicks, start slider timer.
        if (Input.GetMouseButtonDown(0)) {
            playerPhysicsController.currentlyAccelerating = true;
        }

        // Check for mouse held, update the accumulated time.
        if (Input.GetMouseButton(0)) {
            accumulatedTime += Time.deltaTime * playerPhysicsController.accelerationRate;
        } else {
            accumulatedTime -= Time.deltaTime * playerPhysicsController.cooldownRate;
        }
        accumulatedTime = Mathf.Clamp(accumulatedTime, 0.0f, timeToMaxValue);

        // Update slider with new value.
        float progress = accumulatedTime / timeToMaxValue * burnSlider.maximumValue;
        burnSlider.sliderValue = progress;

        // Check for mouse button release, set emission flag and reset slider.
        if (Input.GetMouseButtonUp(0)) {
            playerPhysicsController.currentlyAccelerating = false;
        }

        engineController.UpdateEngine();

        // Update score text.
        if (scoreText != null) {
            scoreText.text = ((int) score).ToString();
        }
    }
}
