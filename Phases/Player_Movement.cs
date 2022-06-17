using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Movement : MonoBehaviour
{
    public float speed;
    public FloatingJoystick floatingJoystick;
    public Rigidbody2D rb;
    public float joystick_deadzone;
    Animator animator;
    private bool face_right = true;
    private void Start()
    {
        //i am using Joystick Pack package on asset store 
        floatingJoystick = GameObject.Find("Floating Joystick").GetComponent<FloatingJoystick>();
        animator = GetComponent<Animator>();
        //checking screen for playing main menu animation
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            animator.SetBool("IsMain", true);
        }
        else
        {
            animator.SetBool("IsMain", false);
        }
    }
    public void FixedUpdate()
    {
        //i put the player gameobject to main menu for showing users, i did not make another gameobject for saving my time
        //because of that i have to check scene name for save some performans 
        if (SceneManager.GetActiveScene().name !="MainMenu")
        {
            if (floatingJoystick.Horizontal > joystick_deadzone || floatingJoystick.Horizontal < -joystick_deadzone ||
                floatingJoystick.Vertical > joystick_deadzone || floatingJoystick.Vertical < -joystick_deadzone)
            {
                Vector2 direction = Vector2.up * floatingJoystick.Vertical + Vector2.right * floatingJoystick.Horizontal;
                rb.velocity=(direction * speed * Time.deltaTime);
            }
            if(rb.velocity.magnitude > 0.5f)
            {
                animator.SetBool("IsWalking", true);
            }
            else
            {
                animator.SetBool("IsWalking", false);
            }
            //checking flip controller boolean and direction of player for flip player gameobject
            if (rb.velocity.x > 0 && !face_right)
            {
                Flip();
            }
            else if (rb.velocity.x <0 && face_right)
            {
                Flip();
            }
            
        }
    }
    public void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;
        face_right = !face_right;
    }
}
