using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

/*
This code allows for a participant to fly throughout a city either as ironman or superman. 
To allow the user to begin flying, press the space key. 
The user will begin flying as superman. To fly as ironman, you can press 's' to toggle between flight modes. 
*/
    public class ThrustController : MonoBehaviour {

    public GameObject lController;
    public GameObject rController;
    public GameObject hmd;
    public SteamVR_Input_Sources leftHand;
    public SteamVR_Input_Sources rightHand;
    public SteamVR_Action_Boolean trigger;
    public float thrust = 7.0f;
    public float cap = 15.0f;
    public float maxSpeed = 5.0f;
    public float speedScalar = 6.0f;
    public FlightAudio flightAudio;
    public Haptics haptic;

    private Rigidbody rb;
    private bool isSuperman = true; //Determines whether user flies as superman or ironman. Press 's' to switch. 
    private bool canStart = false;
    private float hapticScalar = 0;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            // User cannot begin flying until experimenter presses the space key. 
            canStart = !canStart;
        }
        
        if (Input.GetKeyDown("s"))
        {
            // User will onyl fly as in the traditional flight simulation until experimenter presses the 's' key. 
            isSuperman = !isSuperman;
            rb.velocity = Vector3.zero;
        }

        if (canStart)
        {
            haptic.Pulse(1, 400, 0.2f * hapticScalar);
        }
    }

	void FixedUpdate () {
        if (canStart == false)
        { 
            //pass do nothing because we have not started
        }  
        // ironman flight simulation 
        else if (isSuperman == false)
        {
    
            rb.useGravity = true;
            if (rb.velocity.magnitude > cap)
            {
                rb.velocity = rb.velocity.normalized * cap;
            }

            // hold triggers to activate thrusters. Each hand is independently controlled.  
            addThruster(leftHand, lController.transform.rotation);
            addThruster(rightHand, rController.transform.rotation);
            if(!flightAudio.isAmbionic)
            {
                flightAudio.playSoundsRegular(cap, rb.velocity.magnitude, isSuperman);
            } else
            {
                //edit to add ambionic sound system
            }
            hapticScalar = rb.velocity.magnitude / cap; 
        }
        // Superman flight simulation. 
        else
        {
            rb.useGravity = false;

            float distance = Vector3.Distance(lController.transform.position, rController.transform.position);
            float speed = Mathf.Max(maxSpeed - distance * speedScalar, 0.01f);

            Vector3 heading = (lController.transform.position + rController.transform.position) / 2.0f - hmd.transform.position;
            Vector3 direction = heading.normalized;
           
            Vector3 speedVector = speed * direction;
            rb.velocity = speedVector;
            if (!flightAudio.isAmbionic)
            {
                flightAudio.playSoundsRegular(maxSpeed, speed, isSuperman);
            }
            else
            {
                //play ambionic sound system;
            }
            hapticScalar = speed / maxSpeed; 
        }
    }

    
    private void addThruster(SteamVR_Input_Sources hand, Quaternion rot)
    {
        if (trigger.GetState(hand))
        {
            Vector3 forward = rot * Vector3.forward;
            rb.AddForce(-1.0f * forward * thrust);
        }
    }
}
