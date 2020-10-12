using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    // Start is called before the first frame update

    Rigidbody rigid;

     void OnCollisionEnter(Collision collision)
     {
         transform.parent.GetComponent<cube>().CollisionStarDetected(this);
     }
 
    void Start()
    {
         rigid = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}