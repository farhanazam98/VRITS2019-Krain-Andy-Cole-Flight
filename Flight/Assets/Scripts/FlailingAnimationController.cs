using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem; 

public class FlailingAnimationController : MonoBehaviour
{

    Throwable throwable;
    Animator m_Animator;
    Rigidbody m_Rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        throwable = GetComponent<Throwable>();
    }

    void FixedUpdate()
    {
        bool isGrabbed = throwable.attached;
        m_Animator.SetBool("IsGrabbed", isGrabbed);
    }
}
