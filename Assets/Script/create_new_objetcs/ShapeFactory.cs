using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//SOURCE: https://catlikecoding.com/unity/tutorials/object-management/object-variety/

[CreateAssetMenu]
public class ShapeFactory : ScriptableObject {
    //This class allows to generate different objects (with different
    //shapes and materials)

    [SerializeField]
    Shape[] prefabs;

    [SerializeField]
    Material[] materials;


    //To get a specific shape/material
    public Shape Get(int shapeId = 0, int materialId = 0)
    {

        Shape instance = Instantiate(prefabs[shapeId]);
        instance.ShapeId = shapeId;
        instance.SetMaterial(materials[materialId], materialId);
        return instance;
    }

    //To get a specific shape/material with respect to specific transform
    public Shape GetTr(Transform ts,int shapeId = 0, int materialId = 0)
    {

        Shape instance = Instantiate(prefabs[shapeId]);
        instance.transform.parent = ts;
        instance.ShapeId = shapeId;
        instance.SetMaterial(materials[materialId], materialId);
        return instance;
    }

    //To get a random shape
    public Shape GetRandom()
    {

        return Get(

            Random.Range(0, prefabs.Length),
            Random.Range(0, materials.Length)
        );
    }
}