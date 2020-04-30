using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperFunctions : MonoBehaviour {

    public static readonly float EPSILON = 0.001f;

    // Returns an array of GameObjects that are on the layer which name was specified by the argument. Returns null if no such layer was found.
    public static GameObject[] FindGameObjectsOnLayer(string layerName) {
        int layer = LayerMask.NameToLayer(layerName);

        if (layer > -1) {
            GameObject[] candidates = GetAllGameObjectsInScene();
            List<GameObject> result = new List<GameObject>();

            foreach (GameObject candidate in candidates) {
                if (candidate.layer == layer) {
                    result.Add(candidate);
                }
            }

            return result.ToArray();
        }

        return null;
    }

    // Returns an array of all GameObjects that are active in the Scene Hierarchy.
    public static GameObject[] GetAllGameObjectsInScene() {
        GameObject[] candidates = FindObjectsOfType<GameObject>();
        List<GameObject> result = new List<GameObject>();

        foreach (GameObject candidate in candidates) {
            if (candidate.activeInHierarchy) {
                result.Add(candidate);
            }
        }

        return result.ToArray();
    }

    public static Vector3 RandomPointInBounds(Bounds bounds) {
        return new Vector3(Random.Range(bounds.min.x, bounds.max.x),
                           Random.Range(bounds.min.y, bounds.max.y),
                           Random.Range(bounds.min.z, bounds.max.z));
    }

    public static Vector3 RandomPointOutsideBounds(Bounds bounds, float distanceOutside) {
        bool negativeX = (Random.Range(0.0f, 1.0f) < 0.5f) ? true : false;
        bool negativeY = (Random.Range(0.0f, 1.0f) < 0.5f) ? true : false;
        bool negativeZ = (Random.Range(0.0f, 1.0f) < 0.5f) ? true : false;

        float x = negativeX ? Random.Range(-Mathf.Infinity, bounds.min.x) : Random.Range(bounds.max.x, Mathf.Infinity);
        float y = negativeY ? Random.Range(-Mathf.Infinity, bounds.min.y) : Random.Range(bounds.max.y, Mathf.Infinity);
        float z = negativeZ ? Random.Range(-Mathf.Infinity, bounds.min.z) : Random.Range(bounds.max.z, Mathf.Infinity);

        Vector3 point = new Vector3(x, y, z);
        Vector3 pointOnBox = bounds.ClosestPoint(point);
        Vector3 outwardDirection = (point - pointOnBox).normalized;

        return pointOnBox + outwardDirection * distanceOutside;
    }

    public static Vector3 RandomPointOutsideBounds2d(Bounds bounds, float distanceOutside) {
        bool negativeX = (Random.Range(0.0f, 1.0f) < 0.5f) ? true : false;
        bool negativeY = (Random.Range(0.0f, 1.0f) < 0.5f) ? true : false;

        float x = negativeX ? Random.Range(bounds.min.x - bounds.size.x, bounds.min.x) : Random.Range(bounds.max.x, bounds.max.x + bounds.size.x);
        float y = negativeY ? Random.Range(bounds.min.y - bounds.size.y, bounds.min.y) : Random.Range(bounds.max.y, bounds.max.y + bounds.size.y);

        Vector3 point = new Vector3(x, y, 0);
        Vector3 pointOnBox = bounds.ClosestPoint(point);
        Vector3 outwardDirection = (point - pointOnBox).normalized;

        return pointOnBox + outwardDirection * distanceOutside;
    }
}
