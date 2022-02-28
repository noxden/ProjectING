//----------------------------------------------------------------------------------------------
// Institution:   Darmstadt University of Applied Sciences, Expanded Realities
// Class:         Project 3: "Hidden Ventures Beyond The Spotlight" by Prof. Dr. Frank Gabler
// Script by:     Julius Faust (770970)
// Last changed:  2021
//----------------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class lectureController : MonoBehaviour
{

    private InputDevice targetDevice;
    public InputDeviceCharacteristics controllerCharacteristics;
    public List<GameObject> controllerPrefabs;
    private GameObject myController;
    public GameObject myHandPrefab;
    private GameObject myHand;
    public bool showController = true;

    public Animator handAnimator;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!targetDevice.isValid)
        {
            InitializeController();
        } else
        {
            if (showController)
            {
                myHand.SetActive(false);
                myController.SetActive(true);
            }
            else{
                myHand.SetActive(true);
                myController.SetActive(false);
            }




            if (targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue))
            {
                //Debug.Log("Primary Button was pressed");
            }
            if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
            {
                //Debug.Log("Trigger Button was pressed" + triggerValue);
                handAnimator.SetFloat("valTrigger", triggerValue);
            }
            else
            {
                handAnimator.SetFloat("valTrigger", 0);
            }

            if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
            {
                //Debug.Log("gripValue" + gripValue);
                handAnimator.SetFloat("valGrip", gripValue);
            }
            else
            {
                handAnimator.SetFloat("valGrip", 0);
            }

            if (targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 primary2DValue))
            {
                //Debug.Log("Touch" + primary2DValue);
            }

        }




    }

    void InitializeController()
    {
        List<InputDevice> attached_Devices = new List<InputDevice>();

       // controllerCharacteristics = (InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right);

        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, attached_Devices);

        foreach (var item in attached_Devices)
        {
            Debug.Log(item.name + item.characteristics);
        }

        if(attached_Devices.Count > 0)
        {
            targetDevice = attached_Devices[0];

            GameObject prefab = controllerPrefabs.Find(controller => controller.name == targetDevice.name);

            if (prefab)
            {
                myController = Instantiate(prefab, transform);
            }
            else
            {
                Debug.Log("Controller not found");
            }

            if(myHand == null)
            {
                myHand = Instantiate(myHandPrefab, transform);
            }


            handAnimator = myHand.GetComponent<Animator>();
        }

    }





}
