using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneDetection : MonoBehaviour {

    public GameObject camera;
    public Plane current;
    public Vector3 axes = new Vector3(0,0,0);
    public float alphaScalar = 5.0f;
    private MeshRenderer renderer;
    public float power = 1.0f;

	
    void Start()
    {
        renderer = GetComponent<MeshRenderer>();
    }
	// Update is called once per frame
	void Update () {
        float distances = Vector3.Dot(axes, (this.transform.position - camera.transform.position));
        float distance = Mathf.Abs(distances);
        print("Distance");
        print(distance);
        float alpha = Mathf.Max(0.0f,Mathf.Min(255.0f,255.0f - Mathf.Pow(distance, power)))/255.0f;
        print(alpha);
        Color color = renderer.material.color;
        renderer.material.color = new Color(color[0], color[1], color[2], alpha);

    }
}
