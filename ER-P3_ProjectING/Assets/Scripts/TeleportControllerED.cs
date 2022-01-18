// inspired by VALEM
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportControllerED : MonoBehaviour // will be attached to XRRig
{
    // access to our teleportation rays:
    // access to left and right controller
    public XRController leftTeleportRay;
    public XRController rightTeleportRay;
    // Which button activates the teleport?
    public InputHelpers.Button teleportActivationButton;
    // how much is needed (e.g. trigger value), when it is active
    public float activationThreshold = 0.2f;
    
    // get/set to link it to an event
    public bool EnableLeftTeleport { get; set; } = true;
    public bool EnableRightTeleport { get; set; } = true;

    // Update is called once per frame
    void Update()
    {
        //XRRayInteractor component shows the ray for teleport

        if(leftTeleportRay)
        {
            leftTeleportRay.GetComponent<XRRayInteractor>().enabled = EnableLeftTeleport && CheckIfActivated(leftTeleportRay);
        }

        if (rightTeleportRay)
        {
            rightTeleportRay.GetComponent<XRRayInteractor>().enabled = EnableRightTeleport && CheckIfActivated(rightTeleportRay);
        }
    }

    public bool CheckIfActivated(XRController controller) 
    {
        // check if a specific button (controller feature) was pressed:
        // inputhelpers is library that provides functions connected to the input system, for example for controllers
        InputHelpers.IsPressed(controller.inputDevice, teleportActivationButton, out bool isActivated, activationThreshold);
        return isActivated;
    }
}
