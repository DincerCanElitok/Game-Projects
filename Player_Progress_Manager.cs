using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Player_Progress_Manager : MonoBehaviour
{
    Player_Data player_Data;
    public string file = "/Save.json";
    void Start()
    {
        player_Data = GetComponent<Player_Data>();
        Load();
    }

    //i worked with json format
    //I can write the json file with my Player_Data class but i cant deserialize json file because Player_Data class is MonoBehaviour
    //for fix this i made this class with same variables
    public class SaveClass
    {
        public int yellow_coin;
        public int green_coin;
        public int indungeon_yellow_coin;
        public int active_phase;
        public int limit_phase;
        public List<bool> skins;
        public List<bool> upgrades;
        public bool adspass;
        public int healthPoint;
        public float attacktime;
    }
    public void Save()
    {
        SaveClass savedFile = new SaveClass
        {
            yellow_coin = player_Data.yellow_coin,
            green_coin = player_Data.green_coin,
            indungeon_yellow_coin = player_Data.indungeon_yellow_coin,
            active_phase = player_Data.active_phase,
            limit_phase = player_Data.limit_phase,
            skins = player_Data.skins,
            upgrades = player_Data.upgrades,
            adspass = player_Data.adspass,
            healthPoint = player_Data.healthPoint,
            attacktime = player_Data.attacktime
        };

        var json = JsonUtility.ToJson(savedFile);
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(json);
        //encryption, you can use any encryption which you want
        var b64 = System.Convert.ToBase64String(plainTextBytes);
        File.WriteAllText(Application.persistentDataPath + file, b64);
    }
    public void Load()
    {
        //if saved file exixst load the file
        if(File.Exists(Application.persistentDataPath + file))
        {
            var b64 = File.ReadAllText(Application.persistentDataPath + file);
            var plainTextBytes = System.Convert.FromBase64String(b64);
            var savedJson = System.Text.Encoding.UTF8.GetString(plainTextBytes);
            SaveClass savedFile = JsonUtility.FromJson<SaveClass>(savedJson);
            player_Data.yellow_coin = savedFile.yellow_coin;
            player_Data.green_coin = savedFile.green_coin;
            player_Data.indungeon_yellow_coin = savedFile.indungeon_yellow_coin;
            player_Data.active_phase = savedFile.active_phase;
            player_Data.limit_phase = savedFile.limit_phase;
            player_Data.skins = savedFile.skins;
            player_Data.upgrades = savedFile.upgrades;
            player_Data.adspass = savedFile.adspass;
            player_Data.healthPoint = savedFile.healthPoint;
            player_Data.attacktime = savedFile.attacktime;
        }
        else
        {
            Debug.Log("file not found");
        }
        
    }
    public void WatchAd()
    {
        // yellow coin  += 2x in dungeon yellow coin
    }
}
