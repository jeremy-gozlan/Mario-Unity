using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used when Mario gets bigger/smaller (the capsule disappears)
public class capsule : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Mario")
        {
            Destroy(gameObject);
        }
    }
}
