//----------------------------------------------------------------------------------------------
// Institution:   Darmstadt University of Applied Sciences, Expanded Realities
// Class:         Project 3: "Hidden Ventures Beyond The Spotlight" by Prof. Dr. Frank Gabler
// Script by:     Yanina Koulakovski (771310) & Julius Faust (770970)
// Last changed:  17-02-22
//----------------------------------------------------------------------------------------------


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
        
        if (collisionSound != null)     // playing a simple sound when colliding with objects
        {
            if (this.GetComponent<Cartridge>().isInserted == false && this.GetComponent<Cartridge>().isGrabbed == true) // only when grabbed and not inserted to avoid constand feedback
            {
                collisionSound.volume = 0.3f;
                collisionSound.Play();
            }
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
