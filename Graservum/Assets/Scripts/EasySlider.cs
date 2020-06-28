using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasySlider : MonoBehaviour {

    // The numerical value of the slider.
    [SerializeField]
    private float _sliderValue = 0.0f;
    public float sliderValue {
        get => _sliderValue;
        set { _sliderValue = Mathf.Clamp(value, minimumValue, maximumValue); }
    }

    // The numerical minimum value of the slider.
    [SerializeField]
    private float _minimumValue = 0.0f;
    public float minimumValue { get => _minimumValue; private set => _minimumValue = value; }

    // The numerical maximum value of the slider.
    [SerializeField]
    private float _maximumValue = 100.0f;
    public float maximumValue { get => _maximumValue; private set => _maximumValue = value; }

    // Read-only property returning where between sliderValue is between minimumValue and maximumValue
    // represented as a fraction between 0 and 1 (inclusive, 0 is at minimumvalue, 1 is at maximumValue).
    public float sliderProgress {
        get { return sliderValue / (maximumValue - minimumValue); }
    }
}
