using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class ShipDeathController : MonoBehaviour {

#pragma warning disable
	[SerializeField]
	private Vector3 angularVelocity;
	[SerializeField]
	private float speed;
	[SerializeField]
	private float gameOverDelay;
#pragma warning restore

	private void Start() {
		Camera mainCamera = Camera.main;
		Bounds cameraBounds = mainCamera.GetComponent<CameraController>().cameraBounds;
		Vector3 target = cameraBounds.center + 
						 (Random.Range(0, 2) > 0 ? -1 : 1) * Vector3.up * cameraBounds.extents.y +
						 (Random.Range(0, 2) > 0 ? -1 : 1) * Vector3.right * cameraBounds.extents.x;
		target.z = -mainCamera.transform.position.z;

		Rigidbody rigidbody = GetComponent<Rigidbody>();
		rigidbody.velocity = speed * Vector3.Normalize(target - rigidbody.position);
		rigidbody.angularVelocity = angularVelocity;

		AudioManager.instance.Play("Explosion");

		StartCoroutine(WaitForGameOver());
	}

	IEnumerator WaitForGameOver() {
		yield return new WaitForSeconds(gameOverDelay);

		GameObject UICanvas = GameObject.Find("UICanvas");

		if (UICanvas != null) {
			GameObject gameOverUI = UICanvas.transform.GetChild(2).gameObject;

			if (gameOverUI != null) {
				gameOverUI.SetActive(true);
			}
		}

		if (Application.isEditor) Debug.Log("Game Over!");
	}
}
