using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperFunctions : MonoBehaviour {

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
}
