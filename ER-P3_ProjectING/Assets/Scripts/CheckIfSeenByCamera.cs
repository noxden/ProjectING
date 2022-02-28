//----------------------------------------------------------------------------------------------
// Institution:   Darmstadt University of Applied Sciences, Expanded Realities
// Class:         Project 3: "Hidden Ventures Beyond The Spotlight" by Prof. Dr. Frank Gabler
// Script by:     Prof. Dr. Frank Gabler, Julius Faust and Daniel Heilmann (771144)
// Created:       28-01-22
// Last changed:  17-02-22 (by Julius Faust & Daniel Heilmann)
// Purpose:       Shows Objects only if in camera view
//----------------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfSeenByCamera : MonoBehaviour
{
    public LanternHandler LanternHandler;   //< I know that this is very bad code, but for now it works. If we're lucky, I'm gonna do it properly somewhen later.
    public Camera displayCamera;
    public GameObject[] HideableObjects;
    public bool showBlends = false;
    private enum Blendtype { btLEFT, btRIGHT, btTOP, btBOTTOM };

    private GameObject leftBlend;
    private GameObject rightBlend;
    private GameObject topBlend;
    private GameObject bottomBlend;
    // pivot used as an hinge. Pivots are child to the camera
    private GameObject _PivotLeft;
    private GameObject _PivotRight;
    private GameObject _PivotTop;
    private GameObject _PivotBottom;
    private float _depth = 100;

    private void Awake()
    {
        HideableObjects = GameObject.FindGameObjectsWithTag("HideableObject");
    }

    // Start is called before the first frame update
    void Start()
    {
        
        //HideableObjects = GameObject.FindGameObjectsWithTag("HideableObject");

        _PivotLeft = GameObject.Find("PivotLeft");
        _PivotRight = GameObject.Find("PivotRight");
        _PivotTop = GameObject.Find("PivotTop");
        _PivotBottom = GameObject.Find("PivotBottom");

        // create the right blend:
        CreateBlend(_PivotLeft, out leftBlend, Blendtype.btLEFT);
        // create the right blend:
        CreateBlend(_PivotRight, out rightBlend, Blendtype.btRIGHT);
        // create the top blend:
        CreateBlend(_PivotTop, out topBlend, Blendtype.btTOP);
        // create the bottom blend:
        CreateBlend(_PivotBottom, out bottomBlend, Blendtype.btBOTTOM);

        _depth = displayCamera.farClipPlane;
        Debug.Log(displayCamera);
    }

    // Update is called once per frame
    void Update()
    {


        // loop over all tagged and found objects in scene
        foreach (GameObject go in HideableObjects)
        {
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(displayCamera);
            // check if GameObject is within Camera view:
            if (GeometryUtility.TestPlanesAABB(planes, go.GetComponent<Collider>().bounds))
            {
                // if within view: then show it
                HideObjects(go, false);
            }
            else
            {
                // else hide it
                HideObjects(go, true);
            }
        }

        //Debug.Log($"Frustrum depth: {_depth}");
    }

    private void CreateBlend(GameObject pivot, out GameObject blend, Blendtype _btype)
    {
        // create the right blend:
        blend = GameObject.CreatePrimitive(PrimitiveType.Quad);
        blend.transform.localScale = new Vector3(_depth, 6, 1);
        blend.transform.position = new Vector3(pivot.transform.position.x, pivot.transform.position.y, pivot.transform.position.z + _depth / 2.0f);


        // rotate blends towards camera
        Quaternion qrotation = Quaternion.Euler(0, 90, 0);
        blend.transform.rotation = qrotation * displayCamera.transform.rotation;
        // make a pivot for the blades with an empty game object attached to the camera
        blend.transform.SetParent(pivot.transform);
        blend.transform.GetComponent<Renderer>().enabled = showBlends;
        blend.transform.GetComponent<MeshCollider>().enabled = false;

        switch (_btype)
        {

            case Blendtype.btLEFT:
                qrotation = Quaternion.Euler(0, (displayCamera.fieldOfView * displayCamera.aspect) / 2.0f, 0);
                break;
            case Blendtype.btRIGHT:
                // turn by 180° in z-axis to make the normal vectors point inwards
                qrotation = Quaternion.Euler(0, -(displayCamera.fieldOfView * displayCamera.aspect) / 2.0f, 180f); // 180?
                break;

            case Blendtype.btTOP:
                qrotation = Quaternion.Euler((displayCamera.fieldOfView * displayCamera.aspect) / 2.0f, 0, -90f); // 180?
                break; // for top and bottom blends if needed
            case Blendtype.btBOTTOM:
                qrotation = Quaternion.Euler(-(displayCamera.fieldOfView * displayCamera.aspect) / 2.0f, 0, 90f); // 180?
                break;
            default:
                qrotation = Quaternion.Euler(0, 0, 0);
                break;
        }
        // rotate pivot instead of blends, otherwise you would turn around the center
        pivot.transform.rotation = qrotation * pivot.transform.rotation;
        // switch of shadows for the blends - you don't want them in the scene
        blend.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

    }

    private void UpdateMaterial(Material mat)
    {
        // put current position and normal vector to the shader:
        mat.SetVector("_PlanePosition1", leftBlend.transform.position);
        mat.SetVector("_PlaneNormal1", leftBlend.transform.forward);

        mat.SetVector("_PlanePosition2", rightBlend.transform.position);
        mat.SetVector("_PlaneNormal2", rightBlend.transform.forward);

        mat.SetVector("_PlanePosition3", topBlend.transform.position);
        mat.SetVector("_PlaneNormal3", topBlend.transform.forward);

        mat.SetVector("_PlanePosition4", bottomBlend.transform.position);
        mat.SetVector("_PlaneNormal4", bottomBlend.transform.forward);

        mat.SetFloat("_Alpha", LanternHandler.getNewMatAlpha());
    }

    private void HideObjects(GameObject go, bool hideIt)
    {
        // Update the two shaders (materials) about current position of the planes:
        if (go)
        {
            Renderer[] rend =  go.GetComponents<Renderer>();
            foreach (Renderer rd in rend)
            {
                UpdateMaterial(rd.material);
            }

            // needed for composit objects
            Renderer[] RenderChild = go.GetComponentsInChildren<Renderer>();
            foreach (Renderer rd in RenderChild)
            {
                UpdateMaterial(rd.material);
            }

          /*  FadeOut script = go.GetComponent<FadeOut>();
            // check first if script is provided by the object (only those with fresnelshader)
            if (script)
                script.StartFade(hideIt);*/

            // deaktivate colliders to not run into non visible objects
            Collider[] listColliders = go.GetComponents<Collider>();
            foreach (Collider co in listColliders)
            {
                co.enabled = hideIt;
            }
        }
    }
}
