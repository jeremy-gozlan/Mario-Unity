using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using Vector3 = UnityEngine.Vector3;

public class PlaneWater : MonoBehaviour
{
     // Start is called before the first frame update
    protected Rigidbody rigidbody;
    protected BoxCollider BoxCollider;

    Vector3 hitPoint = new Vector3(0.0f, 0.0f, 0.0f);
    Vector3 hitDir = new Vector3(0.0f, 0.0f, 0.0f);

    void Start()
    {
        // settings
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.detectCollisions = true;
        BoxCollider = GetComponent<BoxCollider>();

    }

     // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        print("Mario is in the water");

        if (collision.gameObject.name == "Mario")
        {
             hitPoint = transform.InverseTransformPoint(collision.contacts[0].point);
             hitDir = transform.InverseTransformDirection(-collision.contacts[0].normal);
             transform.parent.GetComponent<Waves>().CollisionMarioDetected(this,hitPoint);
        }


    }


}

