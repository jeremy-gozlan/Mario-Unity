using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mario : MonoBehaviour
{
    // Forces/movement
    float Speed;
    float gravity = 20;
    float rot ;
    float rotSpeed = 120;
    float jumpForce = 15f;
    float previousHeight ;
    Vector3 moveDir = Vector3.zero;

    // Dust
    public ParticleSystem dust;
    bool dustEffect;

    // Lives
    public int lives=3;
    float reboot_time;

    // Controller 
    CharacterController controller;

    Animator animator;
    bool animGround ;
    bool animTop ;
    bool animationWater ;
    float waveHeight ;

    // Current ground
    public Vector3 planePosition;
    Ground currentPlane;

    // scripts
    public Waves wavescript;

    void Start()
    {
        // Settings
        reboot_time = 0f;
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        wavescript = FindObjectOfType(typeof(Waves)) as Waves;
    }

     void OnControllerColliderHit(ControllerColliderHit hit)
    {
        
        if (hit.gameObject.name != "Plane")
        {
            if (hit.point.y >= transform.position.y + controller.height)
            {
                animator.SetTrigger("JumpTop");
                animTop = true;
                moveDir = new Vector3(0, 0, 0);
                moveDir.y = -transform.position.y;
                moveDir = transform.TransformDirection(moveDir);

            }
        }

        if (hit.gameObject.GetComponent("PlaneWater") != null)
        {
                if(animationWater == false)
                {

                    animator.SetTrigger("Water");
                    animationWater = true;
                }
        }

        if (hit.gameObject.name == "goomba" && reboot_time > 1f)
        {
            LooseLife();
        }

        if (hit.gameObject.GetComponent("Ghost")!= null && reboot_time > 1f)
        {
            LooseLife();
        }

        if (hit.gameObject.GetComponent<Ground>()!= null)
        {
            currentPlane = hit.gameObject.GetComponent<Ground>();
        }

        

    }

    void simulateDust()
    {
        dust.Play();
    }


    void LooseLife()
    {
        reboot_time = 0f;
        if (lives > 0)
        {
            lives--;
        }
        if (lives == 0)
        {

            animator.SetTrigger("Die");
        }
    }

    // Update is called once per frame
    void Update()
    {
        reboot_time += Time.deltaTime;
        
        // MARIO IS GROUNDED
        if (controller.isGrounded)
        {
            // if Mario is now grounded and was jumping, do the grounded animation
            if (animTop == true)
            {
                animator.SetTrigger("Grounded");
                animTop = false;
                currentPlane.deformMeshAtLocation();
             
            }
            else
            {
                previousHeight = 0.0f;
            }


            // KEY INPUTS

            // MOVING/RUNNING FORWARD
            if (Input.GetKey(KeyCode.W))
            {
                if (Speed < 15f)
                {
                     Speed += 0.1f;
                }
                animator.SetFloat("Speed", Speed);
                moveDir = new Vector3(0,0,1);
                moveDir *= Speed;
                moveDir = transform.TransformDirection(moveDir);

                // IF + SPACE KEY => JUMP FORWARD
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    animator.SetTrigger("Jump");
                    moveDir = new Vector3(0,0,1);

                    moveDir.y = 0.3f*jumpForce;
                    moveDir.z = 0.2f*jumpForce;
                    moveDir *= Mathf.Sqrt(Speed);

                    moveDir = transform.TransformDirection(moveDir);
                    currentPlane.deformMeshAtLocation();
                }
            }
            // MOVING BACKWARD
            else if(Input.GetKey(KeyCode.S))
            {
                if (Speed > 0f)
                {
                     Speed -= 0.1f;
                }
                else 
                {
                    if (Speed > -2f)
                    {
                        Speed -= 0.1f;
                    }

                    animator.SetFloat("Speed", Speed);
                    moveDir = new Vector3(0,0,-1);
                    moveDir *= -Speed;
                    moveDir = transform.TransformDirection(moveDir);
 
                }              
            }
            // STRAIGHT JUMP
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                   animator.SetTrigger("Jump");
                   moveDir = new Vector3(0,0,0);
                   moveDir.y = jumpForce;
                   //jumpHeight = moveDir.y;
                   moveDir = transform.TransformDirection(moveDir);
                   currentPlane.deformMeshAtLocation();

            }
            // IF NOTHING IS PRESSED, DECREASE THE SPEED
            else
            {
                
                if (Speed >= 0.2f)
                {
                     Speed -= 0.2f;
                }
                else if (Speed > 0)
                {
                    Speed -= 0.1f;
                }
                else if (Speed <0)
                {
                    Speed += 0.1f;
                }
                moveDir = new Vector3(0,0,1);
                animator.SetFloat("Speed", Speed);
                moveDir *= Speed;
                moveDir = transform.TransformDirection(moveDir);
            }
            

        }
        // MARIO IS IN THE AIR 
        else
        {
            // mario achieved its jump peak, do the animation
            if ( transform.position.y < previousHeight && animTop == false)
            {
                animator.SetTrigger("JumpTop");
                animTop = true;
            }
            else
            {
                previousHeight = transform.position.y;
            }

            // decrease the speed as mario is in the air
            if (Speed >= 0.2f)
            {
                Speed -= 0.20f;
            }
            else if (Speed > 0f)
            {
                Speed -= 0.10f;
            }
            animator.SetFloat("Speed", Speed);

        }
       
        // Dust effect started in case mario is running 
        if (Speed >= 3 && dustEffect == false)
        {
           
            dust.Play();
            dustEffect = true;
        }
        // Dust effect stopped when mario is jumping
        else if (Speed <3 || !controller.isGrounded)
        {   //print("Stop");
            dust.Stop();
            dustEffect = false;
        }


        // MARIO KEY INPUT ROTATION 
        rot += Input.GetAxis("Horizontal")*rotSpeed *Time.deltaTime;
        transform.eulerAngles = new Vector3(0,rot,0);
        
        moveDir.y -= gravity*Time.deltaTime;

        // MARIO IS IN THE WATER
        if(animationWater)
        {  
              moveDir.x = 0f;
              moveDir.z = 0f;
              float newWaveHeight = wavescript.getWavesHeight();

              if (newWaveHeight > waveHeight)
              {
                  moveDir.y = newWaveHeight - waveHeight;
              }
              else
              { 
                  moveDir.y = -(waveHeight-newWaveHeight);
              }
              waveHeight = newWaveHeight;

              controller.Move(moveDir);
        }
        // MARIO IS SMALL
        else if (transform.localScale.y < 1)
        {
             controller.Move(5f*moveDir*Time.deltaTime);
             print(Speed);
        }
        // OTHERWISE
        else
        {
            controller.Move(1.5f*moveDir*Time.deltaTime);

        }



    }
}

