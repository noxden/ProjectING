//----------------------------------------------------------------------------------------------
// Institution:   Darmstadt University of Applied Sciences, Expanded Realities
// Class:         Project 3: "Hidden Ventures Beyond The Spotlight" by Prof. Dr. Frank Gabler
// Script by:     Daniel Heilmann (771144)
// Last changed:  17-02-22
//----------------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternHandler : MonoBehaviour
{
    public bool isCartridgeLoaded = false;
    public List<Cartridge> AllCartridges;

    //public Material invertedShaderMat;

    private void Update()
    {
        /*
        if (Cartridges == null)     //< Guard clause for more readable code / less if-clause clustering
            return;

        this.isCartridgeLoaded = false;
        foreach (Cartridge cartridge in Cartridges)     //< If any cartridge has an "isInserted" value of true, then this loop will turn "this.isCartridgeLoaded" to true
        {
            if (this.isCartridgeLoaded != true && cartridge.isInserted == true)     //< If the Lantern has not yet recognized any cartridge being inserted BUT one of them is inserted, then...
                this.isCartridgeLoaded = true;                                       
            else if (this.isCartridgeLoaded == true)                                 //< This foreach loop ends as soon as it found a cartridge that has been inserted. -> might increase performance when there's way more cartridges
                break;
        }
        */

        /*
        //float val = isCartridgeLoaded ? 0f : 1f;
        //invertedShaderMat.SetFloat("Alpha", val);

        if (invertedShaderMat.GetFloat("Alpha") != 1 && isCartridgeLoaded == false)
        {
            invertedShaderMat.SetFloat("Alpha", 1f);
        }
        else if (invertedShaderMat.GetFloat("Alpha") != 0 && isCartridgeLoaded == true)
        {
            invertedShaderMat.SetFloat("Alpha", 0f);
        }

        Debug.Log(invertedShaderMat.GetFloat("Alpha"));
        */
    }   //< All code from in here has been moved to separate functions 

    private void checkForInsertedCartridge()
    {
        if (AllCartridges == null)     //< Guard clause for more readable code / less if-clause clustering
            return;

        this.isCartridgeLoaded = false;
        foreach (Cartridge cartridge in AllCartridges)     //< If any cartridge has an "isInserted" value of true, then this loop will turn "this.isCartridgeLoaded" to true
        {
            if (this.isCartridgeLoaded != true && cartridge.isInserted == true)     //< If the Lantern has not yet recognized any cartridge being inserted BUT one of them is inserted, then...
                this.isCartridgeLoaded = true;
            else if (this.isCartridgeLoaded == true)                                 //< This foreach loop ends as soon as it found a cartridge that has been inserted. -> might increase performance when there's way more cartridges
                break;
        }
    }

    public float getNewMatAlpha()
    {
        checkForInsertedCartridge();
        if (isCartridgeLoaded == true)
        {
            return 0f;
        }
        else
        {
            return 1f;
        }
    }
}
