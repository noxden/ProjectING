using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidCheck : MonoBehaviour
{
    public GameObject lantern;

    private Transform initialLanternTransform;

    private void Start ()
    {
        initialLanternTransform = lantern.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        //other.transform.position += new Vector3(0, 10, 0);

        if (other.tag == Lantern)
        {
            lantern.transform.position = initialLanternTransform;
        }
    }
}
