using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    // Start is called before the first frame update

    // Ground 
    float hitRadius = 0.5f;
    Vector3 jumpPoint;

    // Ground mesh
    public Mesh mesh;
    public Vector3[] originalVertices;
    public Vector3[] deformedVertices;

    void Start()
    {
        // mesh settings
        mesh = GetComponent<MeshFilter>().mesh;
        originalVertices = mesh.vertices;
        deformedVertices = mesh.vertices;

    }

    // Update is called once per frame
    void Update()
    { 
        
    }
    
    void OnCollisionEnter(Collision collision)
    {
         jumpPoint = transform.InverseTransformPoint(collision.contacts[0].point);
    }
    
    public void deformMeshAtLocation()
    {
          // deform the mesh at the last collision location
          for (var i = 0; i < originalVertices.Length; i++)
          { 
                // distance from the vertex to the hitpoint
                float distance = Vector3.Distance(originalVertices[i], jumpPoint);
                Vector3 dir = (originalVertices[i] - jumpPoint);

                if (distance < 0.5f)
                {
                    Vector3 vertMove = originalVertices[i];
                    vertMove.y = -1.5f * distance;
                    deformedVertices[i] = vertMove;
                }
            }
            mesh.vertices = deformedVertices;
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            GetComponent<MeshCollider>().sharedMesh = mesh;
    }
    
}
