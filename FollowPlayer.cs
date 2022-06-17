using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    Rigidbody2D rb;
    GameObject player;
    public float movespeed;
    private Vector2 movement;

    //push mechanic variables
    public float power;
    Vector2 directionForPlayer;
    Vector2 directionForMonster;
    private bool pushing;
    public float pushtimerA;
    private float pushtimerB;

    Play_Manager play_Manager;
    void Start()
    {
        //pushtimerA is the input which is setted by us 
        //pushtimerB will change in runtime
        pushtimerB = pushtimerA;
        rb = gameObject.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        play_Manager = GameObject.Find("Play_Manager").GetComponent<Play_Manager>();
    }

    private void FixedUpdate()
    {
        //checking pushing control
        if (!pushing)
        {
            Vector3 direction = player.transform.position - transform.position;
            //if you wanna rotate while following to player
            //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            //rb.rotation = angle;
            direction.Normalize();
            movement = direction;
            MovementToPlayer(movement);
        }
        else if (pushing && pushtimerB > 0)
        {
            pushtimerB -= Time.deltaTime;
        }
        else
        {
            pushing = false;
        }              
    }
    private void MovementToPlayer(Vector2 direction)
    {
        //moving monster object to player
        rb.MovePosition((Vector2)transform.position + (direction * movespeed * Time.deltaTime));
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {     
        if (collision.collider.CompareTag("Player"))
        {
            pushing = true;
            pushtimerB = pushtimerA;
            //finding directions for monster and player
            directionForPlayer = collision.transform.position - transform.position;
            directionForMonster = transform.position - collision.transform.position;
            //push player and monster
            collision.collider.GetComponent<Rigidbody2D>().AddForce(directionForPlayer * power);
            rb.AddForce(directionForMonster * power);
            //game mechanics
            collision.collider.GetComponent<Player_Data>().healthPoint -= 1;
            play_Manager.UpdateHealthBars();
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("PlayerAttack"))
        {
            pushing = true;
            pushtimerB = pushtimerA;
            directionForMonster = transform.position - collision.transform.position;
            rb.AddForce(directionForMonster * power);
            play_Manager.CheckMonsters(gameObject);
            Destroy(gameObject, 2);
            
        }
    }
}
