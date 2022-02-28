//----------------------------------------------------------------------------------------------
// Institution:   Darmstadt University of Applied Sciences, Expanded Realities
// Class:         Project 3: "Hidden Ventures Beyond The Spotlight" by Prof. Dr. Frank Gabler
// Script by:     Julius Faust (770970)
// Last changed:  17-02-22
//----------------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Cartridge : MonoBehaviour
{

    public bool isGrabbed;
    public bool isInserted;

    public GameObject cartridgeTransform;
    public GameObject thisContainedWorld;
    public Material blackSkybox;
    public Material skybox;



    // Start is called before the first frame update
    void Start()
    {
        isGrabbed = false;
        isInserted = false;
        thisContainedWorld.SetActive(false);    // every cartridge carries a different world, every world is deactivated at the start
        RenderSettings.skybox = blackSkybox;    // a default black skybox for the blockout scene is activated at the start
    }

    // Update is called once per frame
    void Update()
    {
        if(isInserted == true)
        {
            this.transform.rotation = cartridgeTransform.transform.rotation;    // ensures the cartridge stays in place
            this.transform.position = cartridgeTransform.transform.position;
        }

        if (isInserted && isGrabbed)        // allows for retractiong cartridge from Laterna Magica
        {
            isInserted = false; 
            thisContainedWorld.SetActive(false);
            RenderSettings.skybox = blackSkybox;
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "CartridgeTransform" && !isGrabbed)    // if the cartridge triggers a special collider in the Laterna Magica it will be enabled to snap in place
        {
            this.transform.rotation = cartridgeTransform.transform.rotation;    // snap in place using a transform information in the laterna magica
            this.transform.position = cartridgeTransform.transform.position;
            isInserted = true;
            thisContainedWorld.SetActive(true);     // the contained world is set to active
            RenderSettings.skybox = skybox;     // every world can have its own skybox which is activated here
        }
    }



    public void SetIsGrabbed(bool _isGrabbed)
    {
        isGrabbed = _isGrabbed;     // information needed for other scripts
    }

}
