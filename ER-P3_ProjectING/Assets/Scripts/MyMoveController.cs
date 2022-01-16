using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// include XR Interaction Toolkit libraries to access controllers 
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class MyMoveController : MonoBehaviour // script put onto XR Rig
                                              // the script enables movement via characterController component that is synced with touchpad info on hand cntroller and manual movement of headset (ducking, room scale...)
{

    // XR Node: sources from the connected device
    // XR nodes represent the physical points of reference in the XR system:
    // For example, the user’s head position, their right and left hands, and a tracking reference such as an Oculus camera are all XR nodes: LeftEye, RightHand etc.
    public XRNode inputSourceFromDevice; // we put in LeftHand

    private XRRig myXRRig; // reference to out XR rig - an XR Rig is the user’s eyes, ears, and hands in the virtual world.

    private Vector2 input2DAxis; // read touchpad movement info on (left) controller
                                 // to store the 2D vector data coming from the (left) controller

    private CharacterController myCharacter;  // character controller component attached to our XR Rig to move it physically correct (collisions, slopes, stairs)
                                              // physical object of virtual body, the collider: since attached to XR rig it is used to move character around
    private float fSpeed = 1.0f; // movement speed:  1 meter per second
    

    // we need to define a constant for the gravity - so that XR Rig/ I as playr can fall down
    // const = you can not change it during runtime!
    private const float fGravity = 9.807f; 
    public LayerMask groundLayer; // for spherecast, define a layer to check whether we are in air or on ground


    // ? why nothing assigned to public Layer? Ground object is default layer?


   

    // Start is called before the first frame update
    void Start()
    {
        // search for the needed components of our XR Rig and store the references:

        myCharacter = GetComponent<CharacterController>();
        myXRRig = GetComponent<XRRig>();
    }

    // Use Fixed Update here to calculate movement each time an update on the physics happens
    private void FixedUpdate() 
    {
        Physics.SyncTransforms(); // update position in case of teleportation, otherwise pushed back to original position prior to when trying to teleport, could add condition to call it only after teleport
                                  // Apply Transform changes to the physics engine:
                                  // When a Transform component changes, any Rigidbody or Collider on that Transform or its children may need to be repositioned, rotated or scaled depending on the change to the Transform.
                                  // Use this function to flush those changes to the physics engine manually.


        // we need the yaw of our XR Rig and to use this as an offent for our movement
        // What is meant by yaw, pitch and roll :
        // Movements of an object that are measured as angles.Pitch is up and down like a box lid.Yaw is left and right like a door on hinges, and roll is rotation
        // I get the rotation of the camera in y axis (how much rotated to left/ right) in euler angles (cause it is local)
         Quaternion hmdYaw = Quaternion.Euler(0, myXRRig.cameraGameObject.transform.eulerAngles.y, 0); 

        // movement direction should be according to rotation of headset
        // Move function (beneath) needs a Vector3 - therefore 2D vector mapped into y-z plane
        // Multilpied by the Quaterinon (hmdYaw) it will rotate our movement towards where we are looking at
        // Multiplying a vector with a quaterinion always is a rotation
        Vector3 direction = hmdYaw * new Vector3( input2DAxis.x, 0 , input2DAxis.y); // x and y info from 2D vector, touchpad info, match it with 3D space, rotation happens here

        // now move the CharacterController 
        myCharacter.Move(direction * Time.fixedDeltaTime * fSpeed); // fixed time, since inside fixedUpdate, continuous movement, move character in correct direction, rotated correctly

        // check if Character is in air. If yes start falling down
        if (!IsGrounded ())
        {
            myCharacter.Move(Vector3.down * Time.fixedDeltaTime * fGravity); // fixed time, since inside fixedUpdate, continuous movement, fall dwn if no solid ground beneath
        }

        CharacterFollowHeadset();

    }

    // Update is called once per frame
    void Update() //value from controller (input device) needed - read input device: thus we look for reference
    {
        // get the reference to the device from the selected node
        // link to device established via node, it is the LeftHand (set on public variable from above)
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSourceFromDevice);
        // read the Joystick/Touchpad and store the information in the 2D vector for further use
        // common usages is> grip, trigger, primary touch etc...
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out input2DAxis); 
    }

    //1:27:10
    void CharacterFollowHeadset() // character controller works by touchpad info, but it should follow the headset position when I move manually through room:
                                  // for examle needed for room scale, also if you want to duck the virtual camera height should adapt and become lower etc
    {
        // the information from XRRig camera child component (containing informatin about headset position etc.) passed to the character controller

        //y/ height position of character controller needs to be adjusted:
        myCharacter.height = myXRRig.cameraInRigSpaceHeight + 0.2f; // height adjustment - height position of headset/ camera (hmd) stored into the height of character controller, just little higher (seems to give better feeling)


        //x and z position of character controller in global space needs to be adjusted:
        Vector3 cameraCenter = transform.InverseTransformPoint(myXRRig.cameraGameObject.transform.position); // when we move (need camera position of XRRig to determine where to mve) we are in global space: transform from local to global
                                                                                                             // (since camera is child of XRRig need to transform local to global)

        myCharacter.center = new Vector3(cameraCenter.x, myCharacter.height / 2.0f + myCharacter.skinWidth, cameraCenter.z); // character controller position adjusted to headset position
                                                                                                                             // apply the new position
                                                                                                                             // character need to be moved now

        // ? is the height of real headset or just camera inside XRRig (unity) used in l. 100?
        // ? what is mycharacter.center for anyway?
    }


    private bool IsGrounded()
    {
        // return the position of the character in world space:
        // function transforms position from local space to world space
        // orignally was local coordinate system since character controller is child (?) of XRRig, need to transform into global coordinate system therefore
        // where is character? need character controller's position
        Vector3 centerPos = transform.TransformPoint(myCharacter.center);
        // length of character needed now:
        // the length of the ray should be at least the height position of the character (0.9, it is the height of the center point) plus a little gap so that it really reached the floor:
        // send down a ray with characters half length and little gap, so that bit longer, make sure we interact with surface underneath us
        // want to do raycast downwards according to height of character
        float rayLength = myCharacter.center.y + 0.02f;
        // Do a sphere cast here (not a simple ray cast) to have a certain diminesion e.g. radius of Character - e.g beneath character maybe just bit of floor missing ut character's radius big enough so that still should not fall through
        // groundLayer: defines what is a ground as a layer
        // spherecast with radius in size f character, more proper detection if really no floor beneath, returns true if grounded
        // check on specific layer we set up
        return Physics.SphereCast(centerPos, myCharacter.radius, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer);
    }

}
