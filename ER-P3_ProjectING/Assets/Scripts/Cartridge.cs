using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cartridge : MonoBehaviour
{

    public bool isGrabbed;
    public bool isInserted;
    public GameObject cartridgeTransform;

    public GameObject thisContainedWorld;

    // Start is called before the first frame update
    void Start()
    {
        isGrabbed = false;
        isInserted = false;
        thisContainedWorld.SetActive(false);
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
        }
    }


    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "CartridgeTransform" && !isGrabbed)
        {
            this.transform.rotation = cartridgeTransform.transform.rotation;
            this.transform.position = cartridgeTransform.transform.position;
            isInserted = true;
        }
    }

    public void SetIsGrabbed(bool _isGrabbed)
    {
        isGrabbed = _isGrabbed;
    }

}
