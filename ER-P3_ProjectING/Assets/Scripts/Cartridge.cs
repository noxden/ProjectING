using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Cartridge : MonoBehaviour
{

    public bool isGrabbed;
    public bool isInserted;

    public GameObject cartridgeTransform;
    public GameObject thisContainedWorld;
    //public GameObject Table;
    public Light SpotLight;
    public Light mainLight;
    public Material thisSkybox;
    public Material blackSkybox;



    // Start is called before the first frame update
    void Start()
    {
        isGrabbed = false;
        isInserted = false;
        thisContainedWorld.SetActive(false);
        RenderSettings.skybox = blackSkybox;
        mainLight.enabled = false;
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
            //Table.SetActive(true);
            //Table.GetComponent<MeshRenderer>().enabled = true; // --> only disable Mesh so cartridges on the table do not fall into it when switchiong it off
            SpotLight.enabled = true;
            RenderSettings.skybox = blackSkybox; // changing the Skybox works, but it doesnt get masked, so no good for our idea
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
            //Table.SetActive(false);
            //Table.GetComponent<MeshRenderer>().enabled = false; // --> only disable Mesh so cartridges on the table do not fall into it when switchiong it off
            SpotLight.enabled = false;
            RenderSettings.skybox = thisSkybox;
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
            //Table.SetActive(false);
            //Table.GetComponent<MeshRenderer>().enabled = false;
            SpotLight.enabled = false;
            RenderSettings.skybox = thisSkybox;
        }
    }



    public void SetIsGrabbed(bool _isGrabbed)
    {
        isGrabbed = _isGrabbed;
    }

}
