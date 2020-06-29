using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperFunctions : MonoBehaviour {

    public static readonly float EPSILON = 0.001f;

    private static float spareGaussian;
    private static bool hasSpareGaussian = false;

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

    // Uses the Marsaglia polar method to generate a Gaussian distributed number with given mean and standard deviation.
    // Optimized to use both generated numbers from Marsaglia.
    public static float GenerateGaussian(float mean, float standardDeviation) {
        if (hasSpareGaussian) {
            hasSpareGaussian = false;
            return spareGaussian * standardDeviation + mean;
        } else {
            float u, v, s;
            do {
                u = Random.Range(0.0f, 1.0f);
                v = Random.Range(0.0f, 1.0f);
                s = u * u + v * v;
            } while (s >= 1 || s == 0);
            s = Mathf.Sqrt((float) -2.0 * Mathf.Log(s) / s);
            spareGaussian = v * s;
            hasSpareGaussian = true;
            return mean + standardDeviation * u * s;
        }
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

    // Returns normalized direction vector from the transform to the mouse position projected on the plane the object moves in.
    public static Vector3 GetMouseTargetDirection(Transform contextTransform) {
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPos.z = contextTransform.position.z;
        return contextTransform.InverseTransformPoint(targetPos).normalized;
    }

    // Gets a private property value of obj with name propertyName.
    public static object GetPrivateProperty(Object obj, string propertyName) {
        if (obj == null) return null;
        System.Reflection.PropertyInfo propertyInfo = obj.GetType().GetProperty(propertyName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        return propertyInfo == null ? null : propertyInfo.GetValue(obj, null);
    }

    public static Vector3 RandomPointInBounds(Bounds bounds) {
        return new Vector3(Random.Range(bounds.min.x, bounds.max.x),
                           Random.Range(bounds.min.y, bounds.max.y),
                           Random.Range(bounds.min.z, bounds.max.z));
    }

    // Calculates a random point somewhere at most a distance distanceOutside outside the bounds. 
    public static Vector3 RandomPointOutsideBounds(Bounds bounds, float distanceOutside) {
        bool negativeX = (Random.Range(0, 2) == 0) ? true : false;
        bool negativeY = (Random.Range(0, 2) == 0) ? true : false;
        bool negativeZ = (Random.Range(0, 2) == 0) ? true : false;

        float xMagnitude = Random.Range(0.0f, distanceOutside);
        float yMagnitude = Random.Range(0.0f, distanceOutside);
        float zMagnitude = Random.Range(0.0f, distanceOutside);

        float x = negativeX ? bounds.min.x - xMagnitude : bounds.max.x + xMagnitude;
        float y = negativeY ? bounds.min.y - yMagnitude : bounds.max.y + yMagnitude;
        float z = negativeZ ? bounds.min.z - zMagnitude : bounds.max.z + zMagnitude;

        return new Vector3(x, y, z);
    }

    // Calculates a random point somewhere at most a distance distanceOutside outside the bounds. The z-value of the point is always 0.

    public static Vector3 RandomPointOutsideBounds2d(Bounds bounds, float distanceOutside) {
        bool negativeX = (Random.Range(0, 2) == 0) ? true : false;
        bool negativeY = (Random.Range(0, 2) == 0) ? true : false;

        float xMagnitude = Random.Range(0.0f, distanceOutside);
        float yMagnitude = Random.Range(0.0f, distanceOutside);

        float x = negativeX ? bounds.min.x - xMagnitude : bounds.max.x + xMagnitude;
        float y = negativeY ? bounds.min.y - yMagnitude : bounds.max.y + yMagnitude;

        return new Vector3(x, y, 0);
    }

    public static Vector3 RandomPointInBoundsOutsideBounds(Bounds inner, Bounds outer) {
        Vector3 result = Vector3.zero;

        do {
            result = RandomPointInBounds(outer);
        } while (inner.Contains(result));

        return result;
    }
}
