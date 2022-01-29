using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Cartridge : MonoBehaviour
{

    public bool isGrabbed;
    public bool isInserted;

    public GameObject cartridgeTransform;
    public GameObject thisContainedWorld;
    public GameObject lightCone;
    public Light SpotLight;
    public Light mainLight;
    public Material blackSkybox;



    // Start is called before the first frame update
    void Start()
    {
        isGrabbed = false;
        isInserted = false;
        thisContainedWorld.SetActive(false);
        //RenderSettings.skybox = blackSkybox;
        mainLight.enabled = false;
        lightCone.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(isInserted == true)
        {
            this.transform.rotation = cartridgeTransform.transform.rotation;
            this.transform.position = cartridgeTransform.transform.position;
        }

        if (isInserted && isGrabbed)
        {
            isInserted = false;
            thisContainedWorld.SetActive(false);
            mainLight.enabled = false;
            SpotLight.enabled = true;
            lightCone.SetActive(false);
        }

        Debug.Log(isGrabbed);
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "CartridgeTransform" && !isGrabbed)
        {
            this.transform.rotation = cartridgeTransform.transform.rotation;
            this.transform.position = cartridgeTransform.transform.position;
            isInserted = true;
            thisContainedWorld.SetActive(true);
            mainLight.enabled = true;
            SpotLight.enabled = false;
            lightCone.SetActive(true);
        }
    }


    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "CartridgeTransform" && !isGrabbed)
        {
            this.transform.rotation = cartridgeTransform.transform.rotation;
            this.transform.position = cartridgeTransform.transform.position;
            isInserted = true;
            thisContainedWorld.SetActive(true);
            mainLight.enabled = true;
            SpotLight.enabled = false;
            lightCone.SetActive(true);
        }
    }



    public void SetIsGrabbed(bool _isGrabbed)
    {
        isGrabbed = _isGrabbed;
    }

}
