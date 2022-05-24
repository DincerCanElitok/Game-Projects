using UnityEngine;
//source : Blackthornprod / RANDOM DUNGEON GENERATION https://www.youtube.com/watch?v=qAf9axsyijY
// i just used his idea in 3d game project and make a minimap with that idea but i realize using two camera much more logical
// OntriggerEnter and OnTriggerStay functions should not be corrcet i will check later
public class RoomSpawner : MonoBehaviour
{
    public int openinDirection;
    //1 need bottom
    //2 top need
    //3 left need
    //4 right need
    private RoomTemplates templates;
    private int rand;
    private bool spawned=false;
    public float waitTime = 1f;
    void Start()
    {
        //Destroy empty game object after the wait time
        Destroy(gameObject, waitTime);
        //taking room prefabs from room templates class
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("Spawn", 0.1f);
    }
    void Spawn()
    {
        //if this game object not spawn a room check its direction and spawn a random room in same direction section
        if(spawned==false)
        {
            if (openinDirection == 2)
            {
                rand = Random.Range(0, templates.bottomrooms.Length);
                Instantiate(templates.bottomrooms[rand], transform.position, templates.bottomrooms[rand].transform.rotation);
            }
            else if (openinDirection == 1)
            {
                rand = Random.Range(0, templates.toprooms.Length);
                Instantiate(templates.toprooms[rand], transform.position, templates.toprooms[rand].transform.rotation);
            }
            else if (openinDirection == 4)
            {
                rand = Random.Range(0, templates.leftrooms.Length);
                Instantiate(templates.leftrooms[rand], transform.position, templates.leftrooms[rand].transform.rotation);
            }
            else if (openinDirection == 3)
            {
                rand = Random.Range(0, templates.rightrooms.Length);
                Instantiate(templates.rightrooms[rand], transform.position, templates.rightrooms[rand].transform.rotation);
            }
            spawned = true;
        }
        
    }
    //checking if more than one room spawner gameobject in the same location
    //??? Check this code later
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("SpawnPoint"))
        {
            if(other.GetComponent<RoomSpawner>().spawned==false && spawned==false)
            {
                Instantiate(templates.closedroom, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            spawned = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("SpawnPoint"))
        {
            if (other.GetComponent<RoomSpawner>().spawned == false && spawned == false)
            {
                Instantiate(templates.closedroom, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            spawned = true;
        }
    }
}
