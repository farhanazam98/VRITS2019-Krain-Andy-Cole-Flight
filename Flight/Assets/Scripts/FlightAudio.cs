using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

using System.Net;
using System.Net.Sockets;
using System.Text;

public class FlightAudio : MonoBehaviour {

    public SteamVR_Input_Sources hands;
    public SteamVR_Action_Boolean trigger;

    public AmbisonicsSystem ambSys;
    public AudioSource flyingWind;

    public bool isAmbionic = false;
    //private float flyingWindVolume = 0.5f; //takes on values only from 0 to 1
    private bool playSound = false;

    void Start()
    {
        isAmbionic = ambSys.StartSystem();
        isAmbionic = false;
    }

    public void playSoundsAmbionic()
    {

    }

    public void playSoundsRegular(float maxSpeed, float speed, bool isSuperman)
    {
        if (isSuperman || (!isSuperman && trigger.GetState(hands)))
        {
            float volume = speed / maxSpeed;
            if (!isSuperman)
            {
                volume = Mathf.Max(0.2f, volume);
            }
            flyingWind.volume = volume;
            if (!flyingWind.isPlaying)
            {
                flyingWind.Play();
            }
        } 
    }

}
