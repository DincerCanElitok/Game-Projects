using UnityEngine;

public class Player_movement : MonoBehaviour
{
    public GameObject motor1;
    public GameObject motor2;
    public ParticleSystem particle1;
    public ParticleSystem particle2;
    Rigidbody rb;
    Player_scenemanager ps;

    void Start()
    {
        if(motor1 != null || motor2 != null)
        {
            particle1 = motor1.GetComponent<ParticleSystem>();
            particle2 = motor2.GetComponent<ParticleSystem>();
        }
        rb = GetComponent<Rigidbody>();
        ps = GetComponent<Player_scenemanager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Checking player movement
        if(rb.velocity.magnitude>1 )
        {
            if(!particle1.isPlaying || !particle2.isPlaying)
            {
                particle1.Play();
                particle2.Play();
            }
            
        }
        else
        {
            if (particle1.isPlaying || particle2.isPlaying)
            {
                particle1.Stop();
                particle2.Stop();
            }
         
        }
        if(ps.giris==true ||ps.cikis==true)
        {
            if (particle1.isPlaying || particle2.isPlaying)
            {
                particle1.Stop();
                particle2.Stop();
            }
        }
    }
}
