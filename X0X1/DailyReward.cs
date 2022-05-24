using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using System;

public class DailyReward : MonoBehaviour
{
    public List<Dailyrewards> Rewards;
    public GameObject DayNumberText;
    public GameObject CollectButton;
    public GameObject RewardText;
    public GameObject NextDayText;
    public DateTime today;
    public API_time time;
    public int nextday; 
    private bool unlockreward=false;
    Player_Progress PG;
    public IEnumerator numerator;
    DateTime rewardday;
    public GameObject sliderobj;
    public Slider slider;

    [System.Serializable]
    public class Dailyrewards
    {
        public bool Isstar;
        public int price;
    }
    //API values
    public class API_time 
    {
        public string abbreviation;
        public string datetime;
        public int day_of_week;
        public int day_of_year;
        public bool dst;
        public string dst_from;
        public int dst_offset;
        public string dst_until;
        public int raw_offset;
        public string timezone;
        public int unixtime;
        public string utc_datetime;
        public string utc_offset;
        public int week_number;
    }
    
    void Start()
    {
        slider =sliderobj.GetComponent<Slider>();
        PG = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Progress>();
        nextday = PG.day_number + 1;
        if (numerator != null)
            StopCoroutine(Checktime());
        numerator = Checktime();
        StartCoroutine(numerator);
    }

    public IEnumerator Checktime()
    {
        DayNumberText.GetComponent<TMP_Text>().text = "Checking";
        RewardText.GetComponent<TMP_Text>().text = "Internet";
        NextDayText.GetComponent<TMP_Text>().text = "Connection";
        Color32 alpha = CollectButton.GetComponent<Image>().color;
        alpha.a = (byte)(0.1f * 255);
        CollectButton.GetComponent<Image>().color = alpha;
        //request api
        var request = UnityWebRequest.Get("http://worldtimeapi.org/api/timezone/Etc/UTC");
        yield return request.SendWebRequest();
        //checking connection
        if (request.isHttpError || request.isNetworkError)
        {
            DayNumberText.GetComponent<TMP_Text>().text = "Connection Error";
            RewardText.GetComponent<TMP_Text>().text = "Pelease check your internet connection";
            NextDayText.GetComponent<TMP_Text>().text = "";
            yield break;
        }
        var json = request.downloadHandler.text;    
        time = JsonUtility.FromJson<API_time>(json);
        today = Convert.ToDateTime(time.datetime);
        Debug.Log(today.Date);
        rewardday = DateTime.Parse(PG.rewardday);
        if (rewardday.Date < today.Date)
        {
            DayNumberText.GetComponent<TMP_Text>().text = "Day: " + PG.day_number;
            if (Rewards[PG.day_number - 1].Isstar == true)
            {
                RewardText.GetComponent<TMP_Text>().text = "Reward: " + Rewards[PG.day_number - 1].price + " Star";
            }
            else
            {
                RewardText.GetComponent<TMP_Text>().text = "Reward: " + Rewards[PG.day_number - 1].price + " Credit";
            }
            if (nextday - 1 == Rewards.Count)
                nextday = 1;
            if (Rewards[nextday - 1].Isstar == true)
            {
                NextDayText.GetComponent<TMP_Text>().text = "Next day: " + Rewards[nextday - 1].price + " Star";
            }
            else
            {
                NextDayText.GetComponent<TMP_Text>().text = "Next day: " + Rewards[nextday - 1].price + " Credit";
            }

            alpha.a = (byte)(1f * 255);
            CollectButton.GetComponent<Image>().color = alpha;
            slider.value = 24;
            unlockreward = true;
        }
        else if (rewardday.Date >= today.Date)
        {
            DayNumberText.GetComponent<TMP_Text>().text = "Next Day";
            if (Rewards[nextday - 2].Isstar == true)
            {
                RewardText.GetComponent<TMP_Text>().text = Rewards[nextday - 2].price + " Star";
            }
            else
            {
                RewardText.GetComponent<TMP_Text>().text = Rewards[nextday - 2].price + " Credit";
            }
            slider.value = today.Hour;      
            NextDayText.GetComponent<TMP_Text>().text = "Less then " + (24-today.Hour) +" Hours / UTC";
        }       
    }
    public void UnlockRewardButton()
    {
        if (unlockreward == true)
        {
            PG.rewardday = today.Date.ToString() ;
            if(Rewards[PG.day_number-1].Isstar == true)
            {
                PG.star = PG.star + Rewards[PG.day_number-1].price;
                PG.StarCheck();
            }
            if (Rewards[PG.day_number-1].Isstar == false)
            {
                PG.credit = PG.credit + Rewards[PG.day_number-1].price;
                PG.CreditCheck();
            }
            PG.day_number = PG.day_number + 1;
            if (PG.day_number == Rewards.Count+1)
            {
                PG.day_number = 1;
            }
            nextday = PG.day_number + 1;
            if(nextday == Rewards.Count+2)
            {
                nextday = 1;
            }
            if (numerator != null)
                StopCoroutine(Checktime());
            numerator = Checktime();
            StartCoroutine(numerator);
            unlockreward = false;
        }
    }
}
