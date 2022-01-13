using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class MyMoveController : MonoBehaviour
{

    public XRNode intputSourceFromDevice;
    private Vector2 input2DAxis;
    private CharacterController myCharacter;
    private float speed = 1.0F;
    private XRRig myXRRig;
    private const float fGravity = 9.807f;
    public LayerMask groundLayer; 

    // Start is called before the first frame update
    void Start()
    {
        myCharacter = GetComponent<CharacterController>();
        myXRRig = GetComponent<XRRig>();
    }

    private void FixedUpdate()
    {
        Physics.SyncTransforms();   //GameEvent OnTeleportHasHappened

        Quaternion hmdYaw = Quaternion.Euler(0, myXRRig.cameraGameObject.transform.eulerAngles.y, 0);

        Vector3 direction = hmdYaw * new Vector3(input2DAxis.x,0,input2DAxis.y);
        myCharacter.Move(direction * Time.fixedDeltaTime * speed);

        if (!isGrounded())
        {
            myCharacter.Move(Vector3.down * Time.fixedDeltaTime * fGravity);
        }

        CharacterFollowHmd();
    }

    // Update is called once per frame
    void Update()
    {


        InputDevice device = InputDevices.GetDeviceAtXRNode(intputSourceFromDevice);
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out input2DAxis);
    }

    private void CharacterFollowHmd()
    {
        myCharacter.height = myXRRig.cameraInRigSpaceHeight + 0.2f;
        Vector3 cameraCenter = transform.InverseTransformPoint(myXRRig.cameraGameObject.transform.position);
        myCharacter.center = new Vector3(cameraCenter.x, myCharacter.height / 2.0f + myCharacter.skinWidth, cameraCenter.z);
    }

    private bool isGrounded()
    {
        Vector3 centerPos = transform.TransformPoint(myCharacter.center);
        float rayLength = myCharacter.center.y + 0.01f;
        return Physics.SphereCast(centerPos, myCharacter.radius, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer);
    }

}
