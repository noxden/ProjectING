using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskObject : MonoBehaviour
{

    public Collider lightCol;

    private void Start()
    {
        GetComponent<MeshRenderer>().material.renderQueue = 2001;
    }

    private void Update()
    {
      
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (lightCol != null)
        {
            if (collision.collider.tag == "Lightcone")
            {
                GetComponent<MeshRenderer>().material.renderQueue = 2000;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (lightCol != null)
        {
            if (collision.collider.tag == "Lightcone")
            {
                GetComponent<MeshRenderer>().material.renderQueue = 2001;
            }
        }
    }
}
