using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class SendHaptic : MonoBehaviour
{

    public XRController xrControl;

    public bool isGrabbed;


    private void Awake()
    {
        xrControl = (XRController)GameObject.FindObjectOfType(typeof(XRController));
        
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (isGrabbed)
        {
            SendHapticFeedback();
        }
    }

    public void SendHapticFeedback()
    {
        xrControl.SendHapticImpulse(0.7f, 0.01f);
    }

    public void SetIsGrabbed(bool _isGrabbed)
    {
        isGrabbed = _isGrabbed;
    }
}
