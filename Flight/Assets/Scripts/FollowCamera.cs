using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {

    public GameObject camera;

	// Update is called once per frame
	void Update () {
        Vector3 cameraPos = camera.transform.position;
        transform.position = new Vector3(cameraPos[0], transform.position[1], cameraPos[2]);
	}
}
