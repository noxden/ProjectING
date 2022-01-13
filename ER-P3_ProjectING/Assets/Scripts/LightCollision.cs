using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCollision : MonoBehaviour
{

    private void Awake()
    {
        this.GetComponent<MeshRenderer>().enabled = false;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Lightcone")
        {
            this.GetComponent<MeshRenderer>().enabled=true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Lightcone")
        {
            this.GetComponent<MeshRenderer>().enabled = false;
        }
    }

}
