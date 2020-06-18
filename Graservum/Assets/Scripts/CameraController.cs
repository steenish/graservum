using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public Bounds cameraBounds {
		get {
			Vector3 center = Vector3.zero;
			float verticalSize = 2 * GetComponent<Camera>().orthographicSize;
			float horizontalSize = verticalSize * Screen.width / Screen.height;
			Vector3 size = new Vector3(horizontalSize, verticalSize, 100.0f);
			return new Bounds(center, size);
		}
	}
}
