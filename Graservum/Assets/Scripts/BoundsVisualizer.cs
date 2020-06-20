using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BoundsVisualizer : MonoBehaviour {

#pragma warning disable
    // Array of strings formatted as such: GameObjectName.ComponentName.PropertyName
    [SerializeField]
    private string[] boundsNames;
    [SerializeField]
    private GameObject debugBoundsVis;
#pragma warning restore

    private List<Bounds> boundsToVisualize;
    private List<string> boundsToVisualizeNames;

    void Start() {
        StartCoroutine(GetBoundsLater());
    }

    IEnumerator GetBoundsLater() {
        yield return new WaitForSecondsRealtime(1.0f);

        boundsToVisualize = new List<Bounds>();
        boundsToVisualizeNames = new List<string>();

        // Go through the specially formatted strings.
        foreach (string boundsString in boundsNames) {
            // Find all the different parts of the string.
            string[] parts = boundsString.Split('.');
            // If there are enough parts of the string.
            if (parts.Length == 3) {
                // Find the specified GameObject.
                GameObject gameObjectWithBounds = GameObject.Find(parts[0]);
                if (gameObjectWithBounds != null) {
                    // Find the specified Component.
                    Component boundsComponent = gameObjectWithBounds.GetComponent(parts[1]);
                    if (boundsComponent != null) {
                        // Get the value of the property. Look for private property.
                        object boundsObject = HelperFunctions.GetPrivateProperty(boundsComponent, parts[2]);

                        // Look for public property value if no private property was found.
                        if (boundsObject == null) {
                            System.Reflection.PropertyInfo propertyInfo = boundsComponent.GetType().GetProperty(parts[2]);
                            boundsObject = propertyInfo == null ? null : propertyInfo.GetValue(boundsComponent);
                        }

                        // If the value is not null and of type Bounds.
                        if (boundsObject != null && boundsObject.GetType() == typeof(Bounds)) {
                            // Add the Bounds to the list.
                            boundsToVisualize.Add((Bounds) boundsObject);
                            boundsToVisualizeNames.Add(parts[2]);
                        }
                    }
                }
            }
        }

        // Visualize each Bounds in turn.
        for (int i = 0; i < boundsToVisualize.Count; ++i) {
            Bounds bounds = boundsToVisualize[i];
            Debug.Log(boundsToVisualizeNames[i] + ": " + bounds);
            GameObject cube = Instantiate(debugBoundsVis, bounds.center, Quaternion.identity, transform);
            cube.transform.localScale = bounds.size;
        }
    }
}
