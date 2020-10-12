using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
public class cube : MonoBehaviour
{
    // Colliders/Renderer
    BoxCollider collider;
    Renderer rend;

    //give a tag to the cubes
    public GameObject respawnPrefab;
    public GameObject respawn;


    // Cube 
    bool collisionEffect;
    bool endcollision;
    bool toTop;
    float gravity = 8f;
    float jumpForce = 10f;
    float rot;
    Vector3 initalPosition;
    Vector3 moveDir = Vector3.zero;
    public Color Level_Colour;

    //to deform the cube
    public Mesh mesh;
    public Vector3[] verts;
    public Vector3[] originalpoints;
    
    // Cube hitpoint
    Vector3 cubeButtomPoint = new Vector3(0f,-0.5f,0f);
    Vector3 hitPoint = new Vector3(0.0f, 0.0f, 0.0f);
    float hitRadius=1.5f;

    // Coin variables
    GameObject coin;
    Vector3 coinInitialPosition;
    bool coinEffect = false;

    // Star variables
    GameObject star;
    bool starWasTop = false;
    bool starWasBottom = true;
    Vector3 starInitialPosition;


    bool objectAppeared = false;
    //float tag;

    // Start is called before the first frame update

    

    void Start()
    {

        // Collider settings
        collider = GetComponent<BoxCollider>();
        initalPosition = transform.position;
        
        // Coin settings
        coin = transform.GetChild (0).gameObject;
        coin.transform.localScale  = new Vector3(0,0,0);
        coinInitialPosition  = coin.transform.position;
        coin.GetComponent<Rigidbody>().detectCollisions = false; 
        coin.GetComponent<SphereCollider>().isTrigger = false;
    
       // Star settings
        star = transform.GetChild (1).gameObject;
        star.transform.localScale  = new Vector3(0,0,0);
        starInitialPosition = star.transform.position;
        star.GetComponent<Rigidbody>().detectCollisions = false; 
        star.GetComponent<SphereCollider>().isTrigger = false;
        
        // Cube mesh settings
        mesh = GetComponent<MeshFilter>().mesh;
        verts = mesh.vertices;
        originalpoints = mesh.vertices;

        // Cube renderer settings
        rend = GetComponent<Renderer>();
        if(Level_Colour != Color.clear)
        {
            rend.material.color = Level_Colour;
            rend.material.SetColor("_SpecColor", Level_Colour);
        }

    }



    void OnCollisionEnter(Collision collision)
    {
        print("Cube has detected Mario!");
        if (collision.gameObject.name == "Mario")
        {
            // random between coin and star
            int idxChild = Random.Range(0,5);

            // make a star or coin appear
            if (idxChild != 0)
            {
                if (coin.transform.localScale.x == 0 && objectAppeared == false)
                {

                    collisionEffect = true;
                    objectAppeared = true;
                    coinEffect = true;
                    coin.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                    // update the canvas coin score
                    FindObjectOfType<Score>().updateScore();
                    
                }
            }
            else
            {
                if (star.transform.localScale.x == 0 && objectAppeared == false)
                {
                    objectAppeared = true;
                    collisionEffect = true;
                    star.GetComponent<Rigidbody>().detectCollisions = true; 
                    coin.GetComponent<SphereCollider>().isTrigger = true;
                    star.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
                }
            }
            
        }

        hitPoint = transform.InverseTransformPoint(collision.contacts[0].point);

    }

    // Update the score and make the star disappears
    public void CollisionCoinDetected(Coin childScript)
     {
          if (coin.transform.localScale.x != 0f)
          {
              print("Coin touched!");
              coin.transform.localScale = new Vector3(0,0,0);
              coin.GetComponent<Rigidbody>().detectCollisions = false;
              coin.GetComponent<SphereCollider>().isTrigger = true; 
              FindObjectOfType<Score>().updateScore();
          }     
          
     } 

    // Make the star disappears
    public void CollisionStarDetected(Star childScript)
     {
          if (star.transform.localScale.x != 0f)
          {
              print("Star touched!");
              star.transform.localScale = new Vector3(0,0,0);
              star.GetComponent<Rigidbody>().detectCollisions = false; 
              star.GetComponent<SphereCollider>().isTrigger = true;
          }     
          
     } 

    // Update is called once per frame
    void Update()
    {

        // Move the cube up and down when a collision happened
        if (collisionEffect ==  true)
        {
            if (toTop == false)
            {
                moveDir.y = jumpForce;
                transform.Translate(moveDir*2*Time.deltaTime);
                if (transform.position.y >= initalPosition.y + 4)
                {
                    toTop = true;
                }
       
            }
            else
            {

               moveDir.y = -jumpForce;
               transform.Translate(moveDir*2*Time.deltaTime);
               if(transform.position.y <= initalPosition.y)
               {
                   transform.position = initalPosition;
                   collisionEffect = false;
                   toTop = false;
                   endcollision = true;
               }
            }

        }
        
        
        // COIN ROTATION EFFECT
        rot = 300*Time.deltaTime;
        Vector3 rotation = new Vector3(0,rot,0);
        coin.transform.Rotate(rotation);
        float coinSpeed = 5*Time.deltaTime;

        if (coin.transform.localScale.y > 0f && coinEffect == true)
        {
            coin.transform.position = new Vector3(coin.transform.position.x,coin.transform.position.y - coinSpeed, coin.transform.position.z);
            if (coin.transform.position.y < coinInitialPosition.y - 2.0)
            {
                coinEffect = false;
                coin.transform.localScale = new Vector3(0,0,0);
                coin.transform.position = coinInitialPosition;
                objectAppeared = false;
            }  
        }


        // STAR MOVING EFFECT
        float starSpeed =  10*Time.deltaTime;

        if (starWasTop == false && starWasBottom == true && star.transform.localScale.y > 0f)
        {
            star.transform.position = new Vector3(star.transform.position.x, star.transform.position.y + starSpeed, star.transform.position.z);
            if (star.transform.position.y > starInitialPosition.y + 1.5)
            {
                starWasTop = true;
                starWasBottom = false;
            }
        }
        if (starWasTop == true && starWasBottom == false && star.transform.localScale.y > 0f)
        {
            star.transform.position = new Vector3(star.transform.position.x,star.transform.position.y - starSpeed, star.transform.position.z);
            if (star.transform.position.y < starInitialPosition.y)
            {
                starWasBottom = true;
                starWasTop = false;
            }  
        }

       

        // CUBE DEFORMATION EFFECT
        if (collisionEffect == true)
        {
            Vector3 top = new Vector3(0, 1.0f, 0);

            hitPoint = new Vector3(0,-0.5f,0);
    
            for (var i = 0; i < verts.Length; i++)
            {
                float distance = Vector3.Distance(verts[i], hitPoint);
                Vector3 dir = (verts[i] - hitPoint);

                if (dir.magnitude < hitRadius)
                {
                    
                    float amount = (1 - dir.magnitude / hitRadius);
                    Vector3 vertMove = dir*amount*0.03f;
                    verts[i] += vertMove;
                }
            }
            GetComponent<MeshFilter>().mesh.vertices = verts;
            mesh.RecalculateBounds();
        }
        //Returns to normal at the end of the collision
        if(endcollision == true){
            for (var i = 0; i < verts.Length; i++)
            {
                float distance = Vector3.Distance(originalpoints[i], verts[i]);
                Vector3 dir = (originalpoints[i]- verts[i]);
                float amount =  30*Time.deltaTime *  (1 - dir.magnitude / hitRadius);
                Vector3 vertMove = (dir * amount*2f);
                verts[i] += vertMove;
            }

         }
        GetComponent<MeshFilter>().mesh.vertices = verts;
        mesh.RecalculateBounds();


    }


    
}
