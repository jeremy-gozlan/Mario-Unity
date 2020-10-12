using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//SOURCE : https://catlikecoding.com/unity/tutorials/object-management/object-variety/

public class Shape : MonoBehaviour
{
    int shapeId = int.MinValue;

    //Shape property initialized in SpawnZone
    public float phase;
    public Vector3 Velocity { get; set; }
    public Vector3 position { get; set; }
    public bool will_fall;
    public Vector3 AngularVelocity { get; set; }
    public int MaterialId { get; private set; }

    //Used in Disperse()
    float v;
    float y;
    float g = 9.8f;

    //Utils
    Color color;
    static int colorPropertyId = Shader.PropertyToID("_Color");
    MeshRenderer meshRenderer;
    MeshCollider meshCollider;
    static MaterialPropertyBlock sharedPropertyBlock;

    void Awake()
    {
        if (meshCollider == null)
        {
            meshCollider = new MeshCollider();
        }
        meshRenderer = GetComponent<MeshRenderer>();}

    //ACTIONS
    public void GoUp()
    {
        //In order to make an helicoidal movement
        transform.Rotate(1/2*AngularVelocity * Time.deltaTime);
        transform.localPosition += Velocity/2 * Time.deltaTime;
    }

    public void MakeCircle()
    {
        //In order to make an helicoidal movement
        Vector3 temp = new Vector3();
        temp.x = 0.015f * Mathf.Cos((1.5f * Time.fixedTime + phase));
        temp.y = 0;
        temp.z = 0.015f * Mathf.Sin((1.5f * Time.fixedTime + phase));
        transform.localPosition += temp;
    }


    public void Orbit(GameObject perso)
    {
        //Made for Mario's lives (hearts in orbit)
        Vector3 orbit = new Vector3(Mathf.Cos((1.5f * Time.time + phase)), 0.5f, Mathf.Sin((1.5f * Time.time + phase)))*2.5f;
        transform.position = perso.transform.position+orbit;
    }

    public void Disperse()
    {
        //Dispersion of the tube's flying bubbles if Mario touches them
        y = y + v * (Time.deltaTime);
        v = v - g * (Time.deltaTime);
        transform.Translate(0, y, 0);
    }

    void OnCollisionEnter(Collision collision)
    {
        //Disperse() is called if will_fall = true;
        if (collision.gameObject.name == "Mario")
        { will_fall = true; }
    }


    public int ShapeId
    {
        get { return shapeId; }
        set {if (shapeId == int.MinValue && value != int.MinValue) { shapeId = value;
            }
            else { Debug.LogError("Not allowed to change shapeId."); }
        }
    }

    public void SetMaterial(Material material, int materialId)
    {
        GetComponent<MeshRenderer>().material = material;
        MaterialId = materialId;
    }


    public void SetColor(Color color)
    {
        this.color = color;
        if (sharedPropertyBlock == null)
        {
            sharedPropertyBlock = new MaterialPropertyBlock();
        }
        sharedPropertyBlock.SetColor(colorPropertyId, color);
        meshRenderer.SetPropertyBlock(sharedPropertyBlock);
    }


    public void SetPosition(Vector3 pos)
    {
        transform.localPosition = pos;
    }


}