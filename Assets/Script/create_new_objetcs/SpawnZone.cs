 using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//SOURCE : https://catlikecoding.com/unity/tutorials/object-management/spawn-zones/

public class SpawnZone : MonoBehaviour
{
    //This class defines the reference points for the objects and initialize the shapes parameters
    [SerializeField]
    bool surfaceOnly;
    public Transform prefab;
    public ShapeFactory shapeFactory;

    public Vector3 SpawnPoint
    {
        get
        {
            return transform.parent.transform.TransformPoint(
                surfaceOnly ? Random.onUnitSphere : Random.insideUnitSphere
            );
        }
    }

    //Initialize the shapes parameters
    public virtual void ConfigureSpawn(Shape shape)
    {
        Transform t = shape.transform;
        t.localPosition = SpawnPoint;
        t.localRotation = Random.rotation;
        t.localScale = Vector3.one/2 * Random.Range(0.1f, 1f);
        shape.SetColor(Random.ColorHSV(
            hueMin: 0f, hueMax: 1f,
            saturationMin: 0.5f, saturationMax: 1f,
            valueMin: 0.25f, valueMax: 1f,
            alphaMin: 1f, alphaMax: 1f
        ));
        shape.AngularVelocity = Random.onUnitSphere * Random.Range(0f, 90f);
        shape.Velocity = transform.forward * Random.Range(0f, 2f);
        shape.phase =  Random.Range(0.0f, 2 * 3.14f);
   
    }















}