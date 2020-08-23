using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWarningSpriteController : MonoBehaviour {

#pragma warning disable
    [SerializeField]
    private PlayerInput playerInput;
    [SerializeField]
    private Rigidbody playerAsteroidRigidbody;
    [SerializeField]
    private SpriteRenderer warningSpriteRenderer;
    [SerializeField]
    private float massFromMaxSpriteEnabler;
#pragma warning restore

    void Start() {
        
    }

    void Update() {
        if (!warningSpriteRenderer.enabled && playerAsteroidRigidbody.mass > playerInput.maxAsteroidMass - massFromMaxSpriteEnabler) {
            warningSpriteRenderer.enabled = true;
        }

        if (warningSpriteRenderer.enabled && playerAsteroidRigidbody.mass <= playerInput.maxAsteroidMass - massFromMaxSpriteEnabler) {
            warningSpriteRenderer.enabled = false;
        }
    }
}
