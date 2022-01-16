using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class lectureController : MonoBehaviour // script to assign needed hand controller prefabs and to trigger certain animations of hands models
{
    // to store the device we want to read/use
    private InputDevice targetDevice;

    // A set of bit flags describing InputDevice characteristics.
    public InputDeviceCharacteristics controllerCharacteristics; // in inspector assigned controller and either Left or Right (attached to myControllerRight and myControllerLeft)

    // controller prefab
    // create a list to store all the different controller models and pick them dynamically 
    public List<GameObject> controllerPrefabs;
    // store the reference to the prefab of the instantiated controller 
    private GameObject myController;

    // hand prefab
    // store the reference to the prefab of the hand
    public GameObject myHandPrefab;
    // store the reference to the instantiated hand
    private GameObject myHand; 


    public bool showController = true; //default show controller, except I want the hand, then need to recheck in inspector

    // reference to the animator compnent of the hand
    private Animator handAnimator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // check if device was already selected 
        if (!targetDevice.isValid)
        {
            InitializeController();
        }
        else
        {

            if (showController)
            {
                myHand.SetActive(false);
                myController.SetActive(true);
            }
            else
            {
                myHand.SetActive(true);
                myController.SetActive(false);
            }

            // if statement to check if feature is supported by the controller used
            if (targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primarayButtonValue))
            {
                Debug.Log("Primary Button was pressed");
            }

            if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue)
                && (triggerValue > 0.1f))
            {
                Debug.Log("Trigger value: " + triggerValue);
                handAnimator.SetFloat("valTrigger", triggerValue);
            }
            else
            {
                handAnimator.SetFloat("valTrigger", 0f);
            }

            if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue)
                && (gripValue > 0.1f))
            {
                Debug.Log("Grip value: " + gripValue);
                handAnimator.SetFloat("valGrip", gripValue);
            }
            else
            {
                handAnimator.SetFloat("valGrip", 0f);
            }


            if (targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 Primary2DAxisValue)
                && (Primary2DAxisValue != Vector2.zero))
            {
                Debug.Log("Primary 2D Axis (Touch): " + Primary2DAxisValue);
            }
        }
    }

    void InitializeController() //check if controller is there, at start do not check since might appear later during run time
    {
        // instantiate list of type that contains devices we want to use/ read from 
        List<InputDevice> attached_devices = new List<InputDevice>();

        // Set the search bit-mask: this will give you only the right controller:
        // controllerCharacteristics = (InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right);
        // attacheddevices list gets filled with desired characteristics, if available devices are found - I guess the real necessary hand controller
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, attached_devices);

        // loop over the list and show the name and the characteristics of the device
        foreach (var item in attached_devices)
        {
            Debug.Log(item.name + item.characteristics);
        }

        // check first if there are any entries in the list of devices
        if (attached_devices.Count > 0)
        {
            // grab the first device from the list and store it
            targetDevice = attached_devices[0];

            // search for a fitting name. Watchout! Must exactly fit to the name of the attached controller ( targetDevice.name)
            GameObject prefab = controllerPrefabs.Find(controller => controller.name == targetDevice.name); // possible controller prefab model name compared to connected real controller device name (targetdevice)

            if (prefab != null)
            {
                myController = Instantiate(prefab, transform);
            }

            else
            {
                Debug.Log("Error");
                // not found, just pick the first one in the list
                myController = Instantiate(controllerPrefabs[0], transform);
            }

            myHand = Instantiate(myHandPrefab, transform);

            handAnimator = myHand.GetComponent<Animator>();
        }
    }
}

//? do the real controllers fit the controllerCharacteristics description so that they get assigned to list (in line 106)? 
//? what transform is used for myController and myHand? how is it connected to the real coordinates of hand controllers in real life? due to XRController script?


