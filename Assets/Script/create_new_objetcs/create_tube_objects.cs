using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//SOURCE : https://catlikecoding.com/unity/tutorials/object-management/persisting-objects/

public class create_tube_objects : MonoBehaviour
{
    List<Shape> shapes;
    Vector3 lTemp;
    int create_new ;
    bool global_fall ;

    public SpawnZone spawnZone;
    public ShapeFactory shapeFactory;
    public SpawnZone SpawnZoneOfLevel { get; set; }


    void Awake()
    {shapes = new List<Shape>();}

    void Update()
    {
        //Create new_shape every 8 frame
        create_new += 1;
        if (create_new ==8 && !global_fall)
        {
            create_new = 0;
            CreateShape();

        }
    }

    void CreateShape()
    {
        //Create some shapes
        Shape instance = shapeFactory.GetTr(transform,1,2);
        spawnZone.ConfigureSpawn(instance);
        instance.SetColor(random_green());
        lTemp = new Vector3(0f,0f,0f);
        lTemp.x+= Random.Range(-0.5f,0.5f);
        lTemp.y += 2f;
        lTemp.z += Random.Range(-0.5f, 0.5f);
        instance.SetPosition(lTemp);
        instance.Velocity = transform.up  *Random.Range(0.8f, 1.8f);
        shapes.Add(instance);
    }


    void FixedUpdate()
    {
        for (int i = 0; i < shapes.Count; i++)
        {
           //If Mario touches one shape a global_fall variable will trigger a general dispersion
           if (shapes[i].will_fall){
                global_fall = true;

           }
            if (!global_fall)
            {
                //Helicoidal movement for the shapes
                shapes[i].GoUp();
                shapes[i].MakeCircle();
            }
            else
            {
                //Disperse movement
                shapes[i].SetColor(new Color(0, 0, 0));
                shapes[i].Disperse();
            }
            //Destroy shapes that are too high
            if (shapes[i].transform.position.y > 30)
            {
                Destroy(shapes[i].gameObject);
                shapes.RemoveAt(i);
                i--;
            }
        }

    }

    static Color random_green()
    {
        float H, S, V;
        Color g = new Color(0, Random.Range(0.8f, 1f), 0);
        Color.RGBToHSV(g, out H, out S, out V);
        Color rand_green =Random.ColorHSV(
                    hueMin: H - 0.1f, hueMax: H + 0.1f,
                    saturationMin: (S - 0.1f) / 2, saturationMax: (S + 0.1f) / 2,
                    valueMin: V - 0.1f, valueMax: V + 0.1f,
                    alphaMin: 0.0f, alphaMax: 0.8f
                );
        return rand_green;
    }



}

