using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidCheck : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "VoidCheck")
        {
            this.transform.position += new Vector3(0, 10, 0);
        }
    }
}
