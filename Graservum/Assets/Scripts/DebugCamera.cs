using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCamera : MonoBehaviour {

    public float camSpeed;

    // Update is called once per frame
    void Update() {

        // Movement
        if (Input.GetKey(KeyCode.W)) {
            transform.position += transform.forward * Time.deltaTime * camSpeed;
        }

        if (Input.GetKey(KeyCode.S)) {
            transform.position += -transform.forward * Time.deltaTime * camSpeed;
        }

        if (Input.GetKey(KeyCode.Space)) {
            transform.position += Vector3.up * Time.deltaTime * camSpeed;
        }

        if (Input.GetKey(KeyCode.LeftControl)) {
            transform.position += Vector3.down * Time.deltaTime * camSpeed;
        }

        if (Input.GetKey(KeyCode.A)) {
            transform.position += Vector3.left * Time.deltaTime * camSpeed;
        }

        if (Input.GetKey(KeyCode.D)) {
            transform.position += Vector3.right * Time.deltaTime * camSpeed;
        }

        if (Input.GetKey(KeyCode.D)) {
            transform.position += Vector3.right * Time.deltaTime * camSpeed;
        }


        // Rotate horizontally
        if (Input.GetKey(KeyCode.Q)) {
            transform.Rotate(new Vector3(0, -2, 0));
        }

        if (Input.GetKey(KeyCode.E)) {
            transform.Rotate(new Vector3(0, 2, 0));
        }
    }
}
