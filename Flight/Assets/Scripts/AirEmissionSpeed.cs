using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirEmissionSpeed : MonoBehaviour
{
    public GameObject playSpace;
    private ParticleSystem p;
    private Rigidbody rb;	
    public float rate = 10.0f;
    public float lifetime = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
	rb = playSpace.GetComponent<Rigidbody>();
        p = gameObject.GetComponent<ParticleSystem>();
        p.startLifetime = lifetime;

    }

    // Update is called once per frame
    void Update()
    {

        var speed = rb.velocity.magnitude;

       


        var emission = p.emission;


        if (speed == 0.0f)
        {
            emission.enabled = false;
            p.Clear();
        }
        else
        {
            emission.enabled = true;
        }

        emission.rateOverTime = speed*rate;
    }
}
