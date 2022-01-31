//------------------------INFORMATION----------------------------
// Darmstadt University of Applied Sciences, Expanded Realities
// Project 3 (Prof. Frank Gabler)
// Script by:     Daniel Heilmann
// Last changed:  20-01-22
//---------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerController : MonoBehaviour
{
    private const float fGRAVITY = 9.807f;

    public XRNode inputSourceFormDevice;
    public LayerMask groundLayer;

    private Vector2 input2DAxis;
    private CharacterController myCharacter;
    private XROrigin myXROrigin;
    private float fMovementSpeed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        myCharacter = GetComponent<CharacterController>();
        myXROrigin = GetComponent<XROrigin>();
    }

    private void FixedUpdate()
    {
        Physics.SyncTransforms();   // flushes the position so that you aren't pushed back immediately after teleporting -> enables teleporting again
        Quaternion hmdYaw = Quaternion.Euler(0, myXROrigin.Camera.transform.eulerAngles.y, 0);       // required to make the control orientation adapt to the look direction

        Vector3 direction = hmdYaw * new Vector3(input2DAxis.x, 0, input2DAxis.y);      // by multiplying a vector with a quaternion, you perform a rotation
        myCharacter.Move(direction * Time.fixedDeltaTime * fMovementSpeed);

        if (!isGrounded())
        {
            myCharacter.Move(Vector3.down * Time.fixedDeltaTime * fGRAVITY);
        }
        CharacterFollowHeadset();
    }

    // Update is called once per frame
    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSourceFormDevice);

        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out input2DAxis);
    }

    void CharacterFollowHeadset()
    {
        myCharacter.height = myXROrigin.CameraInOriginSpaceHeight + 0.2f;     // height-adjustment of the capsule
        Vector3 cameraCenter = transform.InverseTransformPoint(myXROrigin.Camera.transform.position);        // converts from local to world space
        myCharacter.center = new Vector3(cameraCenter.x, myCharacter.height / 2.0f + myCharacter.skinWidth, cameraCenter.z);        // update the center of the camera in relation to the scaled height
    }

    private bool isGrounded()
    {
        Vector3 centerPos = transform.TransformPoint(myCharacter.center);
        // ^ when working with children, their transform parameter always refers to their local transform. Hence, we need to convert it to world transform with "TransformPoint".

        float rayLength = myCharacter.center.y + 0.01f;
        return Physics.SphereCast(centerPos, myCharacter.radius, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer);
        // ^ if we'd use a raycast instead of a spherecast, the player would fall through gaps in e.g. a wooden bridge. -> takes player width into account
    }

}
