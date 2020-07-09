using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    public Vector3 joyStickDirection { get; private set; }

#pragma warning disable
    [SerializeField]
    private Image sprite;
    [SerializeField]
    private Color grabColor;
    [SerializeField]
    private float maxDistance = 1.0f;
#pragma warning restore

    private bool isDragging;
    private Color releaseColor;

    void Start() {
        releaseColor = sprite.color;
    }
    
    void Update() {
        if (isDragging) {
            Vector3 targetVector = HelperFunctions.GetMouseTargetDirectionRaw(transform.parent.position);
            float distance = Mathf.Clamp(targetVector.magnitude, 0, transform.parent.localScale.x);
            transform.position = transform.parent.position + targetVector.normalized * distance;
            joyStickDirection = targetVector.normalized;
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        sprite.color = grabColor;
        isDragging = true;
    }

    public void OnPointerUp(PointerEventData eventData) {
        sprite.color = releaseColor;
        isDragging = false;
        transform.position = transform.parent.position;
    }
}
