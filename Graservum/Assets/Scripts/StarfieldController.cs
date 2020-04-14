using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarfieldController : MonoBehaviour {

	[SerializeField]
	private GameObject starPrefab;
	[SerializeField]
	private Transform starParent;
	[SerializeField]
	private Bounds fieldBounds;
	[SerializeField]
	[Range(0, 10000)]
	private int numStars = 100;

	private GameObject[] stars;

    void Start() {
		// Instantiate star objects.
		stars = new GameObject[numStars];
		for (int i = 0; i < numStars; ++i) {
			Vector3 randomizedPosition = new Vector3(
				Random.Range(fieldBounds.min.x, fieldBounds.max.x),
				Random.Range(fieldBounds.min.y, fieldBounds.max.y),
				Random.Range(fieldBounds.min.z, fieldBounds.max.z));
			stars[i] = Instantiate(starPrefab, randomizedPosition, Quaternion.identity, starParent);
		}
    }
	
    void Update() {
        // Check bounds and wrap position.
		//foreach (GameObject star in stars) {
		//	Transform starSpace = starParent.transform;
		//	Vector3 position = star.transform.position;
		//	if (position.x < starSpace.TransformPoint(fieldBounds.min).x) {
		//		position += new Vector3(fieldBounds.extents.x, 0, 0);
		//	} else if (position.x > starSpace.TransformPoint(fieldBounds.max).x) {
		//		position -= new Vector3(fieldBounds.extents.x, 0, 0);
		//	}

		//	if (position.y < starSpace.TransformPoint(fieldBounds.min).y) {
		//		position += new Vector3(0, fieldBounds.extents.y, 0);
		//	} else if (position.y > starSpace.TransformPoint(fieldBounds.max).y) {
		//		position -= new Vector3(0, fieldBounds.extents.y, 0);
		//	}
		//}
    }
}
