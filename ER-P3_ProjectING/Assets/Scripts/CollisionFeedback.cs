using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;


[RequireComponent(typeof(XRBaseInteractable))]
public class CollisionFeedback : MonoBehaviour
{
    public AudioSource collisionSound;

    private void Start()
    {
        collisionSound = GetComponent<AudioSource>();
    }


    [System.Obsolete]


    private void OnCollisionEnter(Collision collision)
    {
        SendHaptics();
        if (collisionSound != null)
        {
            collisionSound.volume = 0.3f;
            collisionSound.Play();
        }
    }

    [System.Obsolete]
    public void SendHaptics()
    {
        XRBaseControllerInteractor hand = GetComponent<XRBaseInteractable>().selectingInteractor as XRBaseControllerInteractor;
        if(hand != null)
        {
            hand.SendHapticImpulse(0.7f, 0.01f);
        } 
    }
}
