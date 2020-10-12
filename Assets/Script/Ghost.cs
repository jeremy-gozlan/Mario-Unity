using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    // Start is called before the first frame update

     bool animationForPosition = true;
     Vector3 positionAmplitude = Vector3.one;
     Vector3 positionSpeed = Vector3.one;
 
     bool animationForRotation = true;
     Vector3 rotationAmplitude = Vector3.one*20;
     Vector3 rotationSpeed = Vector3.one;
 
     Vector3 originalPosition;
     Vector3 originalRotation;
 
     private float startAnimOffset ;
 


    void Start()
    {

         originalPosition = transform.position;
         originalRotation = transform.eulerAngles;
         startAnimOffset = Random.Range(0f, 540f);  
    }

    // Update is called once per frame
    void Update()
    {
            if(animationForPosition == true) {
             Vector3 position;
             position.x = originalPosition.x + positionAmplitude.x*Mathf.Sin(positionSpeed.x*Time.time + startAnimOffset);
             position.y = originalPosition.y + positionAmplitude.y*Mathf.Sin(positionSpeed.y*Time.time + startAnimOffset);
             position.z = originalPosition.z + positionAmplitude.z*Mathf.Cos(positionSpeed.z*Time.time + startAnimOffset);
             transform.position = position;
            }

            if(animationForRotation == true) {
             Vector3 rotation;
             rotation.x = originalRotation.x + rotationAmplitude.x*Mathf.Sin(rotationSpeed.x*Time.time + startAnimOffset);
             rotation.y = originalRotation.y + rotationAmplitude.y*Mathf.Sin(rotationSpeed.y*Time.time + startAnimOffset);
             rotation.z = originalRotation.z + rotationAmplitude.z*Mathf.Sin(rotationSpeed.z*Time.time + startAnimOffset);
             transform.eulerAngles = rotation;
         }
            
    }

       void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.name == "Mario")
        {
            print("Mario has touched a ghost!");
        }



    }
}

