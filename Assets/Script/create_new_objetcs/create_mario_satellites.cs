using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//SOURCE : https://catlikecoding.com/unity/tutorials/object-management/persisting-objects/

public class create_mario_satellites : MonoBehaviour
{
    List<Shape> shapes;
    GameObject Mario;
    public SpawnZone spawnZone;
    public ShapeFactory shapeFactory;


    void Awake()
    {
        //Create hearts around Mario
        Mario = GameObject.Find("Mario");
        shapes = new List<Shape>();
        for(int i=0; i < Mario.GetComponent<Mario>().lives; i++)
        {CreateShape(i);}
    }

    void FixedUpdate()
    {
        //Create the orbits for the hearts
        for (int i = 0; i < shapes.Count; i++)
        {  shapes[i].Orbit(Mario); }

    }

    void Update()
    {
        //Destroy one heart if Mario touches on enemy
        if (Mario.GetComponent<Mario>().lives != shapes.Count)
        {
            Destroy(shapes[0].gameObject);
            shapes.RemoveAt(0);

        }
    }

    void CreateShape(int id)
    {
        //Create one heart
        Shape shape = shapeFactory.GetTr(Mario.transform,3,0);
        Transform t = shape.transform;
        t.localScale = Vector3.one * 0.01f;
        shape.Velocity = transform.forward * Random.Range(0f, 2f);
        Vector3 newPosition = new Vector3(transform.localPosition.x, transform.localPosition.y,transform.localPosition.z );
        shape.position = newPosition;
        shape.phase = (1/3f)*id*2f*3.14f;
        shape.SetColor(Color.red);
        shapes.Add(shape);
    }

}

