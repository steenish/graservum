using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public Bounds cameraBounds { get; private set; }

	[SerializeField]
	private bool showDebugBoundsVis = false;

#pragma warning disable
	[SerializeField]
	private GameObject debugBoundsVis;
#pragma warning restore

	void Awake() {
		Vector3 center = Vector3.zero;
		float verticalSize = 2 * GetComponent<Camera>().orthographicSize;
		float horizontalSize = verticalSize * Screen.width / Screen.height;
		Vector3 size = new Vector3(horizontalSize, verticalSize, 100.0f);
		cameraBounds = new Bounds(center, size);
		
		if (showDebugBoundsVis) {
			Debug.Log(cameraBounds);
			GameObject cube = Instantiate(debugBoundsVis, cameraBounds.center, Quaternion.identity, transform);
			cube.transform.localScale = cameraBounds.size;
		}
	}
}
