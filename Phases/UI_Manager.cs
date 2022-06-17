using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public GameObject panel;
    public GameObject shop_Main;
    public GameObject shop_All;
    public GameObject upgrade_Upgrade;
    public GameObject upgrade_Inventory;
    public GameObject yellow_coin_textbox;
    public GameObject green_coin_textbox;
    public GameObject currency_Panel;
    public GameObject green_coin_currency_textbox;
    public GameObject green_coin_currency_textbox_change;
    public GameObject yellow_coin_currency_textbox_change;
    public int ratio;
    public Slider currency_slider;
    public bool isCurrencyChangeOpen;
    void Start()
    {
        //i assign this script to close button objects, for not losing performans i have to call CurrencyCheck func. in one object
        //You can check CloseParentPanel func.
        if(gameObject.name == "UIManager")
        {
            CurrencyCheck();
        }
    }
    void Update()
    {
        if(isCurrencyChangeOpen && gameObject.name == "UIManager")
        {
            //Showing players to how much they spend and gain
            green_coin_currency_textbox_change.GetComponent<TMP_Text>().text = (-currency_slider.value).ToString();
            yellow_coin_currency_textbox_change.GetComponent<TMP_Text>().text = (currency_slider.value * ratio).ToString();
        }
    }
    public void OpenPanel()
    {
        Animator panel_animator = panel.GetComponent<Animator>();
        //There is boolean variables in animators which is named Is_open 
        panel_animator.SetBool("Is_open", true);
    }
    public void CloseParentPanel()
    {
        //buttons will start their parent panel animations and close them
        //close buttons are child objects of their panels in hierarchy
        Animator parent_panel_ani = GetComponentInParent<Animator>();       
        parent_panel_ani.SetBool("Is_open", false);
    }
    public void ShopMainButton_Click()
    {
        shop_Main.GetComponentInChildren<Transform>().gameObject.SetActive(true);
        shop_All.GetComponentInChildren<Transform>().gameObject.SetActive(false);
    }
    public void ShopAllButton_Click()
    {
        shop_All.GetComponentInChildren<Transform>().gameObject.SetActive(true);
        shop_Main.GetComponentInChildren<Transform>().gameObject.SetActive(false);
    }
    public void UpgradePanelUpgradeButton_Click()
    {
        upgrade_Upgrade.GetComponentInChildren<Transform>().gameObject.SetActive(true);
        upgrade_Inventory.GetComponentInChildren<Transform>().gameObject.SetActive(false);
    }
    public void UpgradePanelInventoryButton_Click()
    {
        upgrade_Inventory.GetComponentInChildren<Transform>().gameObject.SetActive(true);
        upgrade_Upgrade.GetComponentInChildren<Transform>().gameObject.SetActive(false);
    }
    public void CurrencyCheck()
    {
        //get updated player's currency and show in screen
        int yellow_coin = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Data>().yellow_coin;
        int green_coin = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Data>().green_coin;
        yellow_coin_textbox.GetComponent<TMP_Text>().text = yellow_coin.ToString();
        green_coin_textbox.GetComponent<TMP_Text>().text = green_coin.ToString();
    }
    public void CurrencyPanelOpen()
    {
        Animator panel_animator = currency_Panel.GetComponent<Animator>();
        panel_animator.SetBool("Is_open", true);

        //setting values to ui objects
        int green_coin = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Data>().green_coin;
        green_coin_currency_textbox.GetComponent<TMP_Text>().text = green_coin.ToString();
        currency_slider.maxValue = green_coin;
        currency_slider.value = 0;

        //open control bool for responsive ui
        isCurrencyChangeOpen = true;
    }
    public void CurrencyPanelClose()
    {
        //close control object
        isCurrencyChangeOpen = false;
        Animator panel_animator = currency_Panel.GetComponent<Animator>();
        panel_animator.SetBool("Is_open", false);
    }
    public void CurrencyPay()
    {     
        int green_coin = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Data>().green_coin;
        if (green_coin >= currency_slider.value && currency_slider.value != 0)
        {
            //payment transaction
            green_coin = green_coin - (int)(currency_slider.value);
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Data>().green_coin = green_coin;
            int extra_yellow_coin = (int)currency_slider.value * ratio;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Data>().yellow_coin =
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Data>().yellow_coin + extra_yellow_coin;

            //showing player
            green_coin_currency_textbox.GetComponent<TMP_Text>().text = green_coin.ToString();
            currency_slider.maxValue = green_coin;
            currency_slider.value = 0;
            CurrencyCheck();
            //i can show pop up or effect for added new coins
        }
    }
}
