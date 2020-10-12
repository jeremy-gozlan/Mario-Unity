using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Waves : MonoBehaviour
{
    // Start is called before the first frame update
    protected MeshFilter MeshFilter;
    protected Rigidbody rigidbody;
    protected BoxCollider BoxCollider;
    protected Mesh Mesh;

    public int Dimensions =  10;
    public Octave[] Octaves;
    public float UVScale;
    int indexMario = 100000;

    void Start()
    {
        Mesh = new Mesh(); 
        Mesh.name = gameObject.name;

        Mesh.vertices = GenerateVertices();
        Mesh.triangles = GenerateTriangles();
        Mesh.uv = GenerateUVs();
        Mesh.RecalculateBounds();
        Mesh.RecalculateNormals();

        rigidbody = GetComponent<Rigidbody>();
        rigidbody.detectCollisions = true;
        MeshFilter = gameObject.AddComponent<MeshFilter>();
        BoxCollider = GetComponent<BoxCollider>();
        MeshFilter.mesh  = Mesh;

    }

 
    public void CollisionMarioDetected(PlaneWater childScript, Vector3 hitPointMario)
     {
         print("MARIO ON WAVES");  
         float x = Mathf.Round((hitPointMario.x + 0.5f)*10.0f);
         float z = Mathf.Round((hitPointMario.z + 0.5f)*10.0f);
         indexMario = index((int)x,(int)z);
          
     } 

    public float getWavesHeight()
    {
        if (indexMario != 100000)
        {
            float waterHeight = Mesh.vertices[indexMario].y;
            return waterHeight;
        }
        return 0f;
    }

    Vector3[] GenerateVertices()
    {
        var vertices = new Vector3[(Dimensions + 1)* (Dimensions + 1)];

        for (int x = 0; x <= Dimensions; x++)
        {
            for (int z = 0; z <= Dimensions; z++)
            {
                vertices[index(x,z)] = new Vector3(x,0,z);
            }
        }
        
        return vertices;
    }

    int index(int x, int z)
    {
        return x*(Dimensions + 1) + z;
    }

    int[] GenerateTriangles()
    {
        var triangles = new int[Mesh.vertices.Length * 6];

        for (int x = 0 ; x < Dimensions; x++)
        {
            for (int z = 0 ; z < Dimensions; z++)
            {

                triangles[index(x,z) * 6 + 0] = index(x,z);
                triangles[index(x,z) * 6 + 1] = index(x+1,z+1);
                triangles[index(x,z) * 6 + 2] = index(x+1,z);
                triangles[index(x,z) * 6 + 3] = index(x,z);
                triangles[index(x,z) * 6 + 4] = index(x,z+1);
                triangles[index(x,z) * 6 + 5] = index(x+1,z+1);
            }
        }
        return triangles;
    }

    private Vector2[] GenerateUVs()
    {
        var uvs = new Vector2[Mesh.vertices.Length];

        for (int x=0; x <= Dimensions; x++)
        {
             for (int z=0; z <= Dimensions; z++)
            {
                var vec = new Vector2((x / UVScale) % 2, (z/UVScale)%2);
                uvs[index(x,z)] = new Vector2(vec.x <= 1? vec.x :2 - vec.x,vec.y <=1 ? vec.y :2 - vec.y);
            }
        }
        return uvs;
    }

     // Update is called once per frame
    void Update()
    {
        var vertices = Mesh.vertices;
         for (int x = 0 ; x < Dimensions; x++)
        {
            for (int z = 0 ; z < Dimensions; z++)
            {
                var y = 0f;
                vertices[index(x,z)] = new Vector3(x,y,z);
                
                for (int o = 0; o < Octaves.Length; o++)
                {
                    if (Octaves[o].alternate)
                    {
                        var perl = Mathf.PerlinNoise((x*Octaves[o].scale.x)/Dimensions, 
                                                  (z*Octaves[o].scale.y)/Dimensions) * Mathf.PI*2f;
                        y += Mathf.Cos(perl + Octaves[o].speed.magnitude * Time.time) * Octaves[o].height;
                    }
                    else
                    {
                        var perl = Mathf.PerlinNoise((x*Octaves[o].scale.x + Time.time*Octaves[o].speed.x)/Dimensions, 
                                                     (z*Octaves[o].scale.y + Time.time*Octaves[o].speed.y)/Dimensions) - 0.5f;
                        y += perl * Octaves[o].height;
                    }
                    
                }
                vertices[index(x,z)] = new Vector3(x,y,z);

            }
        }
        Mesh.vertices = vertices;
        Mesh.RecalculateNormals();
    }
}


[Serializable]
public struct Octave 
{
    public Vector2 speed;
    public Vector2 scale;
    public float height;
    public bool alternate;

}
