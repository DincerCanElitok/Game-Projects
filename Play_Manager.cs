using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class Play_Manager : MonoBehaviour
{
    public GameObject Title_Textbox;
    public int phaseGap;
    GameObject player;
    Player_Progress_Manager PG_Manager;
    public GameObject ingamecoin_textbox;
    public GameObject help_close_button;
    public GameObject help_attack_panel;
    public GameObject help_movement_panel;
    public GameObject warning_panel;
    Room_Templates room_Templates;
    public List<GameObject> livingMonsters;

    //player ui variables
    public List<GameObject> healthbars;
    public GameObject playerAttack;
    private float playerattacktime;
    public Slider attacktimeSlider;

    private int randomMapID;
    public List<GameObject> spawn_points;
    public GameObject boss_spawn_point;
    private int playerActivePhase;
    void Start()
    {
        livingMonsters.Clear();
        room_Templates = GameObject.Find("Room_templates").GetComponent<Room_Templates>(); ;
        player = GameObject.FindGameObjectWithTag("Player").gameObject;
        playerActivePhase = player.GetComponent<Player_Data>().active_phase;
        PG_Manager = player.GetComponent<Player_Progress_Manager>();
        randomMapID = Random.Range(0, room_Templates.randomWorldCounts);
        //Generate room and summon monsters
        //you can make new worlds like lava rooms just put another case to switch
        switch (randomMapID) 
        {
            //arane room
            case 0: 
                Instantiate(room_Templates.arcane_Rooms[Random.Range(0, room_Templates.arcane_Rooms.Count)]);
                if(playerActivePhase % phaseGap == 0 && playerActivePhase != 0)
                    SummonBoss();
                else
                    SummonMonster(randomMapID);
                break;
            //earth room
            case 1:
                Instantiate(room_Templates.earth_Rooms[Random.Range(0, room_Templates.earth_Rooms.Count)]);
                if (playerActivePhase % phaseGap == 0 && playerActivePhase != 0)
                    SummonBoss();
                else
                    SummonMonster(randomMapID);
                break;
        }
            


        //Make a cooldown for player attack and show to users with a slider
        playerattacktime = player.GetComponent<Player_Data>().attacktime;
        attacktimeSlider.maxValue = playerattacktime;
        attacktimeSlider.value = attacktimeSlider.maxValue;

        if (playerActivePhase == 0)
        {
            //changing game title 
            Title_Textbox.GetComponent<TMP_Text>().text = "Tutorýal"; 
            //tutorial codes
        }       
        else
        {
            Title_Textbox.GetComponent<TMP_Text>().text = "Phase " + playerActivePhase;
        }
        if(playerActivePhase % phaseGap == 0 && playerActivePhase != 0)
        {
            //setting player progress, might be boss phases every x * phaseGap level
            player.GetComponent<Player_Data>().limit_phase = playerActivePhase;
        }
        UpdateHealthBars();
    }

    private void FixedUpdate()
    {
        //when animation ends changing animation boolean to false for continue other animations
        if (player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Char_Jump"))
        {            
            player.GetComponent<Animator>().SetBool("IsJumping", false);  
        }

        //when users press attack button show them cooldown for next attack
        if(playerattacktime < attacktimeSlider.maxValue)
        {
            attacktimeSlider.value = playerattacktime;
            playerattacktime += Time.deltaTime;
        }
        
    }
    public void UpdateIngameCoin()
    {
        //trigger when coin picked up
        ingamecoin_textbox.GetComponent<TMP_Text>().text = player.GetComponent<Player_Data>().indungeon_yellow_coin.ToString();
    }
    public void UpdateHealthBars()
    {
        //trigger on hit by enemy
        if(player.GetComponent<Player_Data>().healthPoint < healthbars.Count)
            healthbars[player.GetComponent<Player_Data>().healthPoint].SetActive(false);
    }
    public void PlayerAttack()
    {       
        if(playerattacktime >= attacktimeSlider.maxValue)
        {
            //calling attack effect
            var attackeffect = Instantiate(playerAttack, player.transform);
            attackeffect.transform.parent = null;
            attackeffect.transform.position = attackeffect.transform.position + new Vector3(0,1.9f,0);
            player.GetComponent<Animator>().SetBool("IsJumping", true);
            playerattacktime = 0;
        }            
    }

    public void OpenRooms()
    {
        foreach (var obj in room_Templates.left_removable_bordes)
            obj.SetActive(false);
        foreach (var obj in room_Templates.right_removable_bordes)
            obj.SetActive(false);
        foreach (var obj in room_Templates.top_removable_bordes)
            obj.SetActive(false);
        room_Templates.left_room_door.SetActive(true);
        room_Templates.top_room_door.SetActive(true);
        room_Templates.right_room_door.SetActive(true);
    }
    public void SummonMonster(int randomMapID)
    {       
        int diffucult = 0;
        int ran = 0;
        //level mechanics
        if (playerActivePhase < phaseGap)
        {
            diffucult = 1;
            ran = 0;
        }           
        else if (playerActivePhase > phaseGap && playerActivePhase < phaseGap * 2)
        {
            diffucult = 2;
            ran = Random.Range(0, 2);
        }
            
        else if (playerActivePhase > phaseGap && playerActivePhase < phaseGap * 3)
        {
            diffucult = 3;
            ran = Random.Range(1, 3);
        }           
        else
        {
            diffucult = 4;
            ran = Random.Range(1, 3);
        }
        //0 arcane / 1 earth
        if (randomMapID == 0)
        {
            for (int i = 0; i < diffucult; i++)
            {
                var monster = Instantiate(room_Templates.arcane_Monsters[ran], gameObject.transform);
                monster.transform.parent = null;
                monster.transform.position = spawn_points[Random.Range(0, spawn_points.Count)].transform.position;
                livingMonsters.Add(monster);
            }
        }
        else if(randomMapID == 1)
        {
            for (int i = 0; i < diffucult; i++)
            {
                var monster = Instantiate(room_Templates.earth_Monsters[ran], gameObject.transform);
                monster.transform.parent = null;
                monster.transform.position = spawn_points[Random.Range(0, spawn_points.Count)].transform.position;
                livingMonsters.Add(monster);
            }
        }
    }
    public void CheckMonsters(GameObject monster)
    {
        //this function will be called when a monster dies
        for (int i = 0; i < livingMonsters.Count; i++)
        {
            if (monster == livingMonsters[i])
            {
                livingMonsters.RemoveAt(i);
            }
        }
        //if there is no monsters open door gates for next level
        if (livingMonsters.Count == 0)
            OpenRooms();
    }
    public void SummonBoss()
    {
            var boss = Instantiate(room_Templates.bosses[(playerActivePhase / phaseGap) -1], gameObject.transform);
            boss.transform.parent = null;
            boss.transform.position = boss_spawn_point.transform.position;
            livingMonsters.Add(boss);           
    }
    public void HelpOpen()
    {
        help_attack_panel.SetActive(true);
        help_close_button.SetActive(true);
        help_movement_panel.SetActive(true);
    }
    public void HelpClose()
    {
        help_attack_panel.SetActive(false);
        help_close_button.SetActive(false);
        help_movement_panel.SetActive(false);
    }
    public void MainMenuButton_Click()
    {
        warning_panel.SetActive(true);
    }
    public void WarningPanel_No()
    {
        warning_panel.SetActive(false);
    }
    public void WarnigPanel_Yes()
    {
        GameEnd();
    }
    public void GameEnd()
    {
        Debug.Log("Game End");
        PG_Manager.Save();
    }
}
