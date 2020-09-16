using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public Transform groundCheck;
    public LayerMask groundMask;

    public Animator animator;
    public float speed = 6f;
    public float groundDistance = 0.4f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float pushForce = 3f;

    public float turnSmoothTime = 0.1f;

    public GameObject gameOverPanel;   


    public GameObject winPanel;
    public GameObject winCinemachineCam;
    public GameObject TPPCinemachineCam;
    public GameObject spawner;

    
    public AudioSource levelMusic, winMusic;


    Vector3 velocity;

    float turnSmoothVelocity;
    bool isOnGround;
    bool isOnGroundLastFrame = false;

    private void Start()
    {
        levelMusic.Play();        
    }

    private void Update()
    {
        
        isOnGround = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);       

        if(isOnGround && velocity.y < 0f)
        {
            velocity.y = -2f;
        }

        if (isOnGround && !isOnGroundLastFrame)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isRunning", true);
        }
        isOnGroundLastFrame = isOnGround;

        if (isOnGround && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetBool("isJumping", true);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);


        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontalInput, 0, verticalInput).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
            animator.SetBool("isRunning", true);             
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
                

        if(transform.position.y < -10f)
        {
            gameOver();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LevelWinTrigger"))
        {
            speed = 0;
            jumpHeight = 0;
            animator.SetBool("dance", true);
            winGame();            
            Debug.Log("Winner");
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("RotatingCylinder"))
        {
            transform.parent = other.transform;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("RotatingCylinder"))
        {
            transform.parent = null;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        if(body == null || body.isKinematic)
        {
            return;
        }
        
        if (hit.gameObject.CompareTag("UnstablePlatform"))
        {
            Debug.Log(hit.moveDirection);            
            Vector3 pushDir = new Vector3(0, hit.moveDirection.y, 0);
            body.velocity = pushDir * pushForce;
        }
        else
        {
            Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
            body.velocity = pushForce * pushDir;
        }      

        if (hit.gameObject.CompareTag("Spheres"))
        {
            gameOver();
            Debug.Log("game over");
        }

    }

    void gameOver()
    {        
        gameOverPanel.SetActive(true);
        TPPCinemachineCam.SetActive(false);
    }

    void winGame()
    {
        levelMusic.Stop();
        winMusic.Play();
        spawner.SetActive(false);        
        TPPCinemachineCam.SetActive(false);
        winCinemachineCam.SetActive(true);
        winPanel.SetActive(true);
    }
}
