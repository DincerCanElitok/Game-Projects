using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Player_scenemanager : MonoBehaviour
{
    public string file = "/Save.json";
    public bool giris;
    public bool cikis;
    public bool dungeonacikis;
    public bool girisecikis;
    public Material Primary;
    public Material Secondary;
    public Material Cam;
    public GameObject p_healt;
    public GameObject ctrl_ui;
    public GameObject currency;
    public GameObject option;
    float fade=-0.1f; //min - max 1

    void FixedUpdate()
    {
        //Changing custom material variable value over time for make a small animation 
        if (giris==true)
        {
            fade -=Time.deltaTime;
            Primary.SetFloat("_Fade", fade);
            Secondary.SetFloat("_Fade", fade);
            Cam.SetFloat("_Fade", fade);
            if (fade <= -0.1f)
                giris = false; 
        }
        if(cikis==true)
        {
            fade += Time.deltaTime;
            Primary.SetFloat("_Fade", fade);
            Secondary.SetFloat("_Fade", fade);
            Cam.SetFloat("_Fade", fade);
            if (fade >= 1f)
            {
                if(dungeonacikis==true)
                {
                    if (p_healt != null)
                        DontDestroyOnLoad(p_healt);
                    if (ctrl_ui != null)
                        DontDestroyOnLoad(ctrl_ui);
                    if (currency != null)
                        DontDestroyOnLoad(currency);
                    if (option != null)
                        DontDestroyOnLoad(option);
                    DontDestroyOnLoad(gameObject);
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
                    
                else if(girisecikis==true)
                {
                    if (p_healt != null)
                        DontDestroyOnLoad(p_healt);
                    if (ctrl_ui != null)
                        DontDestroyOnLoad(ctrl_ui);
                    if (currency != null)
                        DontDestroyOnLoad(currency);
                    if (option != null)
                        DontDestroyOnLoad(option);
                    DontDestroyOnLoad(gameObject);
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
                }
                             
            }
                
        }
    }
    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Level Loaded");
        if (scene.name == "giris")
        {          
            gameObject.transform.position = new Vector3(0, 0, -13);
            fade = 1;
            giris = true;
        }
        else if (scene.name == "dungeon")
        {
            gameObject.transform.position = new Vector3(0, 0, 0);
            fade = 1;
            giris = true;
        }
        else if(scene.name == "char" && File.Exists(Application.persistentDataPath + file))
        {
            p_healt.SetActive(true);
            ctrl_ui.SetActive(true);
            currency.SetActive(true);
            option.SetActive(true);
            DontDestroyOnLoad(p_healt);
            DontDestroyOnLoad(ctrl_ui);
            DontDestroyOnLoad(currency);
            DontDestroyOnLoad(option);
            DontDestroyOnLoad(gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        dungeonacikis = false;
        girisecikis = false;
        cikis = false;
    }
    public void teleport()
    {
        Player_scenemanager ps = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_scenemanager>();
        ps.cikis = true;
        ps.dungeonacikis = true;
    }
    public void tpback()
    {
        cikis = true;
        girisecikis = true;
    }
}
