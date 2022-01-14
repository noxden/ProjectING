using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class CartridgeWorldChange : MonoBehaviour
{
    private GameObject World1;
    private GameObject World2;


    // Start is called before the first frame update
    void Start()
    {
        World1 = GameObject.Find("PrototypeScene1");
        World2 = GameObject.Find("PrototypeScene2");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Cartridge1")
        {
            World1.GetComponent<Renderer>().enabled = true; // cartridge entered, thus according world enabled
            World2.GetComponent<Renderer>().enabled = false; // other wolrd cannot be active simultaneously
        }

        if (other.gameObject.tag == "Cartridge2")
        {
            World1.GetComponent<Renderer>().enabled = false; // other wolrd cannot be active simultaneously
            World2.GetComponent<Renderer>().enabled = true; // cartridge entered, thus according world enabled
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Cartridge1")
        {
            World1.GetComponent<Renderer>().enabled = false; // cartridge left, thus according world disabled
            World2.GetComponent<Renderer>().enabled = false; // maybe not needed
        }

        if (other.gameObject.tag == "Cartridge2")
        {
            World1.GetComponent<Renderer>().enabled = false; // maybe not needed
            World2.GetComponent<Renderer>().enabled = false; // cartridge left, thus according world disabled
        }
    }
}
