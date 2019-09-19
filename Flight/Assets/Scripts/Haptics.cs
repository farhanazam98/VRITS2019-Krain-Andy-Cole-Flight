using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Haptics : MonoBehaviour
{
    public SteamVR_Action_Vibration hapticAction;
    public SteamVR_Input_Sources source;

    /*
    // Update is called once per frame
    void Update()
    {
        if (trigger.GetState(SteamVR_Input_Sources.LeftHand))
        {
            Pulse(1, 150, 75, SteamVR_Input_Sources.LeftHand);
        }

        if (trigger.GetState(SteamVR_Input_Sources.RightHand))
        {
            Pulse(1, 150, 75, SteamVR_Input_Sources.RightHand);
        }
    }
    */

    public void Pulse(float duration, float frequency, float amplitude)
    {
        hapticAction.Execute(0, duration, frequency, amplitude, source);
    }
}
