using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    public Vector3 direction { get; private set; } = Vector3.down;

#pragma warning disable
    [SerializeField]
    private GameObject stick;
    [SerializeField]
    private Image sprite;
    [SerializeField]
    private Color grabColor;
    [SerializeField]
    private float distanceScaler = 1.0f;
#pragma warning restore

    private bool isDragging;
    private Color releaseColor;

    void Start() {
        releaseColor = sprite.color;
    }
    
    void Update() {
        if (isDragging) {
            // Get direction to pointer from joystick.
            Vector3 targetVector = HelperFunctions.GetMouseTargetDirectionRaw(transform.position);

            // Calculate the distance to pointer from joystick, clamped to the maximum distance.
            float distance = Mathf.Clamp(targetVector.magnitude, 0, transform.localScale.x * distanceScaler);

            // Move the stick.
            stick.transform.position = transform.position + targetVector.normalized * distance;

            // Update normalized joystick direction.
            direction = targetVector.normalized;
        }
    }

    public bool GetInput() {
        return isDragging;
    }

    public void OnPointerDown(PointerEventData eventData) {
        sprite.color = grabColor;
        isDragging = true;
    }

    public void OnPointerUp(PointerEventData eventData) {
        sprite.color = releaseColor;
        isDragging = false;
        stick.transform.position = transform.position;
    }
}
