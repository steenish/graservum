using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles the graphical parts of the engine.
public class EngineController : MonoBehaviour {

#pragma warning disable
    [SerializeField]
    private PlayerInput playerInput;
    [SerializeField]
    private float engineDistanceScaler = 1.46f;
    [SerializeField]
    private ParticleSystem engineExhaustParticles;
    [SerializeField]
    private Color minSpeedColor;
    [SerializeField]
    private Color maxSpeedColor;
    [SerializeField]
    private float particleSpeed = 1.0f;
	[SerializeField]
	private float particleScaleModifier = 1.0f;
#pragma warning restore

    private float onConstant;
    private Gradient currentGradient;
    private GradientColorKey[] colorKeys;
    private GradientAlphaKey[] alphaKeys;
    private ParticleSystem.ColorOverLifetimeModule exhaustColorModule;
    private ParticleSystem.EmissionModule exhaustEmissionModule;
    private ParticleSystem.MainModule exhaustMainModule;
    private ParticleSystem.MinMaxCurve exhaustEmissionRateOverTime;

    void Start() {
        exhaustColorModule = engineExhaustParticles.colorOverLifetime;
        exhaustEmissionModule = engineExhaustParticles.emission;
        exhaustMainModule = engineExhaustParticles.main;
        exhaustEmissionRateOverTime = exhaustEmissionModule.rateOverTime;

        onConstant = exhaustEmissionRateOverTime.constant;
        exhaustEmissionRateOverTime.constant = 0.0f;
        exhaustMainModule.startSpeed = particleSpeed;

        colorKeys = new GradientColorKey[] { new GradientColorKey(minSpeedColor, 0.0f), new GradientColorKey(Color.white, 0.5f) };
        alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) };
        currentGradient = new Gradient();
        currentGradient.SetKeys(colorKeys, alphaKeys);
    }

	private void OnDestroy() {
		AudioManager.instance.Stop("Engine");
	}

	public void UpdateEngine() {
        MoveEngine();
        UpdateParticles();
		UpdateAudio();
    }

    // Moves the engine to the appropriate position according to joystick direction.
    private void MoveEngine() {
		Vector3 targetDirection = playerInput.joystick.direction;
		float angle = Vector3.Angle(Vector3.down, targetDirection);
		transform.localPosition = Vector3.zero + targetDirection * transform.localScale.x * engineDistanceScaler;
		transform.localEulerAngles = new Vector3(0.0f, 0.0f, (targetDirection.x < 0) ? 360 - angle : angle);
    }

    private void UpdateParticles() {
        // Update particle size from ship scale.
        exhaustMainModule.startSize = particleScaleModifier * transform.parent.localScale.x;

        // Control the exhaust emission.
        if (playerInput.burnSlider.sliderProgress > playerInput.burnSlider.minimumValue) {
            if (exhaustEmissionRateOverTime.constant == 0) {
                engineExhaustParticles.Play(true);
                exhaustEmissionRateOverTime.constant = onConstant;
            }

            Color currentAccelerationColor = Color.Lerp(minSpeedColor, maxSpeedColor, playerInput.burnSlider.sliderProgress);
            colorKeys[0].color = currentAccelerationColor;
            currentGradient.SetKeys(colorKeys, alphaKeys);
            exhaustColorModule.color = currentGradient;
        } else {
            engineExhaustParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            exhaustEmissionRateOverTime.constant = 0.0f;
        }
    }

	private void UpdateAudio() {
		float sliderProgress = playerInput.burnSlider.sliderProgress;

		// Play or stop the engine sound effect.
		if (sliderProgress > 0) {
			AudioManager.instance.Play("Engine");
		} else {
			AudioManager.instance.Stop("Engine");
		}

		// Alter the volume and pitch of the engine sound effect.
		float volume = sliderProgress * 0.2f;
		float pitch = sliderProgress * 0.2f;

		AudioManager.instance.ChangeVolume("Engine", volume);
		AudioManager.instance.ChangePitch("Engine", pitch);
	}
}
