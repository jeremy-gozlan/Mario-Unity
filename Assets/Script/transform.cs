
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class transform : MonoBehaviour
{


    CharacterController controller;
    Animator animator;

    //to change the outfits of mario
    Renderer[] renderers;
    public Color col;
    int color = 0;
    bool isGoldenMode;
    float totalTime = 0f;
    int changing_big = -1;
    int changing_small = -1;

    Vector3 lTemp;


    //initialize values 
    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
        lTemp = transform.localScale;
    }


    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // IF MARIO TOUCHES A STAR, BECOME GOLD MODE
        if (hit.gameObject.name == "Star")
        {
            be_a_star();
        }

        // IF MARIO TOUCHES A CUBE AFTER BEING GOD MODE, COME BACK TO NORMAL
        if (hit.gameObject.GetComponent("cube") != null)
        {
            Color col = hit.gameObject.GetComponent<Renderer>().material.color;
            Color basic = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            if (col == basic)
            {
                getBackNormal();
            }
            else { changeColor(hit.gameObject.GetComponent<Renderer>().material.color); }
        }

        // BECOMES BIG
        if (hit.gameObject.name == "Capsule_get_big")
        {
            Destroy(hit.gameObject);
            changing_big = 0;

        }
        // BECOMES SMALL
        if (hit.gameObject.name == "Capsule_get_small")
        {
            Destroy(hit.gameObject);
            changing_small = 0;

        }

    }

    void transform_scale(float scale)
    {
        lTemp.y *= scale;
        lTemp.x *= scale;
        transform.localScale = lTemp;

    }



    void changeColor(Color col)
    {

        foreach (var r in renderers)
        {
            
            r.material.SetColor("_SpecColor", col);
            r.material.EnableKeyword("_EMISSION");
            r.material.SetColor("_EmissionColor", col);
        }
    }



    void be_a_star()
    {
       
        getBackNormal();
        foreach (var r in renderers)
        {
           
            r.material.DisableKeyword("_SPECULARHIGHLIGHTS_OFF");
            isGoldenMode = true;
            r.material.EnableKeyword("_EMISSION");
            r.material.SetColor("_EmissionColor", Color.yellow);
        }
    }

    void getBackNormal()
    {
        foreach (var r in renderers)
        {
            r.material.DisableKeyword("_SPECULARHIGHLIGHTS_OFF");
            r.material.SetColor("_SpecColor", Color.black);
            r.material.SetColor("_EmissionColor", Color.black);
            isGoldenMode = false;
        }
    }



    void Update()
    {

        // CHANGE GOLD MODE COLOR OVER TIME
        if (isGoldenMode == true)
        {
            foreach (var r in renderers)
            {
                r.material.SetColor("_EmissionColor", Color.yellow * ((totalTime % 1f) + 0.2f));
            }
        }

        

        // TRANSFORM MARIO SIZE OVER TIME 
        if (changing_big<28 && changing_big >=0)
        {
            if (changing_big < 28 && changing_big >= 10)
            {
                transform_scale(1.1f);

            }
            if (changing_big % 5 ==0)
            {
                changeColor(Color.white);
            }
            else { getBackNormal(); }
            changing_big += 1 ;

        }
        if (changing_small <28 && changing_small >=0)
        {
            if (changing_small < 28 && changing_small >= 10)
                transform_scale((1/1.1f));

            if (changing_small%5==0)
            {
                changeColor(Color.white);
            }
            else { getBackNormal(); }
            changing_small += 1;
        }


        totalTime += Time.deltaTime;


    }


}
