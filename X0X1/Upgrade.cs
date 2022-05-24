using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
public class Upgrade : MonoBehaviour
{
    // player database class
    Player_Progress PG;
    // player attack class calling for update reload time max value in slider
    P_attack p_attack;
    //player's ship healt class for update healt max value in slider
    Saglik saglik; 
    public int maxHealt_UpgradeLimit;
    public int maxReload_UpgradeLimit;
    public GameObject HealtUpgradelimit_text;
    public GameObject ReloadUpgradelimit_text;
    public List<GameObject> AmmolistButtons;
    public GameObject HealtUpgradePanel;
    public GameObject HealtUpgradePanel_text;
    public GameObject ReloadUpgradePanel;
    public GameObject ReloadUpgradePanel_text;
    public List<Upgradelist> healtupgrade_prices;
    public List<Upgradelist> reloadupgrade_prices;
    // not enought currency warning
    public GameObject NocurrencyPanel; 
    public GameObject selectedImage;
    //Ammo Panel Objects
    public GameObject AmmoUpgradePanel;
    public Animator AmmoUpgradeAni;
    public GameObject AmmoNameText;
    public GameObject AmmoDamageExpText;
    public GameObject AmmoDamageText;
    public GameObject AmmoSpeedExpText;
    public GameObject AmmoSpeedText;
    public GameObject AmmoFullExpText;
    public List<AmmoExp> AmmoUpgradelist;
    public int SelectedAmmoId;
    [System.Serializable]
    public class AmmoExp 
    {
        public string AmmoExpString;
        public int AmmoDamage_MaxUpgradeNumber;
        public int AmmoSpeed_MaxUpgradeNumber;
        public int AmmoDamage_UpgradeNumber;
        public int AmmoSpeed_UpgradeNumber;
        public int AmmoDamageUpgradeAmount;
        public int AmmoSpeedUpgradeAmount;
        public int price;
    }
    [System.Serializable]
    //stat upgrade class
    public class Upgradelist  
    {
        public int price;
        public bool isStar; 
        public int healtUpgradeAmount; 
        public float reloadSpeedAmount;
    }
    void Start()
    {
        //assign player classes
        PG = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Progress>(); 
        p_attack = GameObject.FindGameObjectWithTag("Player").GetComponent<P_attack>();
        saglik = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Saglik>();
        AmmoUpgradeAni = AmmoUpgradePanel.GetComponent<Animator>();
        HealtUpgradeCheck();
        ReloadUpgradeCheck();
        SelectedAmmoImage();
    }


    // assign upgrade number of healt
    void HealtUpgradeCheck()  
    {
        HealtUpgradelimit_text.GetComponent<TMP_Text>().text = PG.healtUpgradeNumber + "/" + maxHealt_UpgradeLimit;
    }
    // assign upgrade number of reloadspeed
    void ReloadUpgradeCheck() 
    {
        ReloadUpgradelimit_text.GetComponent<TMP_Text>().text = PG.reloadUpgradeNumber + "/" + maxReload_UpgradeLimit;
    }
    //open healt upgrade panel
    public void MaxHealtUgradePanel() 
    {
        if (PG.healtUpgradeNumber < maxHealt_UpgradeLimit)
        {
            HealtUpgradePanel.SetActive(true);
            string currency;
            //check the next upgrade currency and warning the player
            if (healtupgrade_prices[PG.healtUpgradeNumber].isStar)  
                currency = "star";
            else
                currency = "credit"; 
            HealtUpgradePanel_text.GetComponent<TMP_Text>().text = "Are you sure pay "+ healtupgrade_prices[PG.healtUpgradeNumber].price+" "+
                 currency +"  for upgrade max healt? (" + healtupgrade_prices[PG.healtUpgradeNumber].healtUpgradeAmount + " healt)";
            //there are two options in the panel, named pay and cancel
        }
    }
    //similar of MaxHealtUgradePanel 
    public void ReloadUgradePanel() 
    {
        if (PG.reloadUpgradeNumber < maxReload_UpgradeLimit)
        {
            ReloadUpgradePanel.SetActive(true);
            string currency;
            if (reloadupgrade_prices[PG.reloadUpgradeNumber].isStar)
                currency = "star";
            else
                currency = "credit";
            ReloadUpgradePanel_text.GetComponent<TMP_Text>().text = "Are you sure pay " + reloadupgrade_prices[PG.reloadUpgradeNumber].price + " " +
                currency +"  for upgrade reload speed? (" + reloadupgrade_prices[PG.reloadUpgradeNumber].reloadSpeedAmount + " reload speed)";
        }
    }
    // assign the pay button 
    public void MaxHealtUP_Pay()
    {
        //checking the currency type
        if (healtupgrade_prices[PG.healtUpgradeNumber].isStar == true) 
        {
            if (PG.star > healtupgrade_prices[PG.healtUpgradeNumber].price)
            {
                PG.star = PG.star - healtupgrade_prices[PG.healtUpgradeNumber].price;
                PG.playerMaxcan = PG.playerMaxcan + healtupgrade_prices[PG.healtUpgradeNumber].healtUpgradeAmount;
                PG.playercan = PG.playercan + healtupgrade_prices[PG.healtUpgradeNumber].healtUpgradeAmount; 
                PG.StarCheck(); 
                PG.healtUpgradeNumber += 1;
                HealtUpgradeCheck();
                // adapt healt sliders max value
                saglik.slider.maxValue = PG.playerMaxcan; 
                HealtUpgradePanel.SetActive(false);
            }
            else
            {
                //warning for not enough currency
                NocurrencyPanel.GetComponent<Nocurrency>().isStar = true; 
                NocurrencyPanel.SetActive(true);
                HealtUpgradePanel.SetActive(false);
            }
        }
        //same progress if the next upgrade currency is credit instead of star
        else if (healtupgrade_prices[PG.healtUpgradeNumber].isStar == false)  
        {
            if (PG.credit > healtupgrade_prices[PG.healtUpgradeNumber].price)
            {
                PG.credit = PG.credit - healtupgrade_prices[PG.healtUpgradeNumber].price;
                PG.playerMaxcan = PG.playerMaxcan + healtupgrade_prices[PG.healtUpgradeNumber].healtUpgradeAmount;
                PG.playercan = PG.playercan + healtupgrade_prices[PG.healtUpgradeNumber].healtUpgradeAmount;
                PG.CreditCheck();
                PG.healtUpgradeNumber += 1;
                HealtUpgradeCheck();
                saglik.slider.maxValue = PG.playerMaxcan;
                HealtUpgradePanel.SetActive(false);
            }
            else
            {
                HealtUpgradePanel.SetActive(false);
                NocurrencyPanel.GetComponent<Nocurrency>().isStar = false;
                NocurrencyPanel.SetActive(true);
            }
        }

    }
    public void MaxHealtUP_cancel()
    {
        HealtUpgradePanel.SetActive(false);
        
    }
    //similar of maxhealtpay
    public void ReloadtUP_Pay() 
    {
        if (reloadupgrade_prices[PG.reloadUpgradeNumber].isStar == true)
        {
            if (PG.star > reloadupgrade_prices[PG.reloadUpgradeNumber].price)
            {
                PG.star = PG.star - reloadupgrade_prices[PG.reloadUpgradeNumber].price;
                //adding negative value in editor
                PG.reloadtime = PG.reloadtime + reloadupgrade_prices[PG.reloadUpgradeNumber].reloadSpeedAmount; 
                PG.StarCheck();
                PG.reloadUpgradeNumber += 1;
                ReloadUpgradeCheck();
                // changing reload time max value 
                p_attack.atis.maxValue = PG.reloadtime;
                ReloadUpgradePanel.SetActive(false);
            }
            else
            {
                NocurrencyPanel.GetComponent<Nocurrency>().isStar = true;
                NocurrencyPanel.SetActive(true);
                ReloadUpgradePanel.SetActive(false);
            }
        }
        else if (reloadupgrade_prices[PG.reloadUpgradeNumber].isStar == false)
        {
            if (PG.credit > reloadupgrade_prices[PG.reloadUpgradeNumber].price)
            {
                PG.credit = PG.credit - reloadupgrade_prices[PG.reloadUpgradeNumber].price;
                PG.reloadtime = PG.reloadtime + reloadupgrade_prices[PG.reloadUpgradeNumber].reloadSpeedAmount;
                PG.CreditCheck();
                PG.reloadUpgradeNumber += 1;
                ReloadUpgradeCheck();
                p_attack.atis.maxValue = PG.reloadtime;
                ReloadUpgradePanel.SetActive(false);
            }
            else
            {
                NocurrencyPanel.GetComponent<Nocurrency>().isStar = false;
                NocurrencyPanel.SetActive(true);
                ReloadUpgradePanel.SetActive(false);
            }
        }
    }
    public void ReloadUP_Cancel() 
    {
        ReloadUpgradePanel.SetActive(false);
    }
    //show player to which ammo is active right now
    public void SelectedAmmoImage() 
    {
        selectedImage.transform.position = new Vector3(AmmolistButtons[PG.selectedAmmo].transform.position.x, selectedImage.transform.position.y, transform.position.z);
    }
    //one function for every unique ammo upgrade button
    public void AmmoUpgrade() 
    {
        var go = EventSystem.current.currentSelectedGameObject; //find witch button is used
        if (AmmoUpgradeAni != null) 
        {
            bool isopen = AmmoUpgradeAni.GetBool("open");
            AmmoUpgradeAni.SetBool("open", !isopen);
        }
        //check button list and find the button
        for (int i = 0; i < AmmolistButtons.Count; i++) 
        {
            if (go.name == AmmolistButtons[i].name) 
            {
                //define ammo id for using in upgrade methods
                SelectedAmmoId = i; 
                AmmoNameText.GetComponent<TMP_Text>().text = PG.ammolist[SelectedAmmoId].name;
                //and if this button is unlock from player data base, add panel values from AmmoUpgradelist
                if (PG.ammoIsOpen[i] == true) 
                {
                    AmmoFullExpText.GetComponent<TMP_Text>().text = AmmoUpgradelist[SelectedAmmoId].AmmoExpString;
                    AmmoDamageText.GetComponent<TMP_Text>().text = PG.ammolist[SelectedAmmoId].GetComponent<gitme>().hasar.ToString();
                    AmmoSpeedText.GetComponent<TMP_Text>().text = PG.ammolist[SelectedAmmoId].GetComponent<gitme>().hiz.ToString();
                    AmmoDamageExpText.GetComponent<TMP_Text>().text = "(" + AmmoUpgradelist[SelectedAmmoId].AmmoDamage_UpgradeNumber + "/" + AmmoUpgradelist[SelectedAmmoId].AmmoDamage_MaxUpgradeNumber + ")";
                    AmmoSpeedExpText.GetComponent<TMP_Text>().text = "(" + AmmoUpgradelist[SelectedAmmoId].AmmoSpeed_UpgradeNumber + "/" + AmmoUpgradelist[SelectedAmmoId].AmmoSpeed_MaxUpgradeNumber + ")";

                }
                // if button not unlock warn the player
                else
                {
                    AmmoFullExpText.GetComponent<TMP_Text>().text = "You need to unlock this ammo from somewhere"; //change
                    AmmoDamageText.GetComponent<TMP_Text>().text = PG.ammolist[SelectedAmmoId].GetComponent<gitme>().hasar.ToString();
                    AmmoSpeedText.GetComponent<TMP_Text>().text = PG.ammolist[SelectedAmmoId].GetComponent<gitme>().hiz.ToString();
                    AmmoDamageExpText.GetComponent<TMP_Text>().text = "(" + AmmoUpgradelist[SelectedAmmoId].AmmoDamage_UpgradeNumber + "/" + AmmoUpgradelist[SelectedAmmoId].AmmoDamage_MaxUpgradeNumber + ")";
                    AmmoSpeedExpText.GetComponent<TMP_Text>().text = "(" + AmmoUpgradelist[SelectedAmmoId].AmmoSpeed_UpgradeNumber + "/" + AmmoUpgradelist[SelectedAmmoId].AmmoSpeed_MaxUpgradeNumber + ")";
                }
            }
        }

    }
    public void AmmoUpgradeClose()
    {
        if (AmmoUpgradeAni != null)
        {
            AmmoUpgradeAni.SetBool("open", false);
        }
        SelectedAmmoId = -1;
    }
    public void AmmoDamageUpPanel()
    {

    }
    public void AmmoSpeedUpPanel()
    {

    }
    public void AmmoDamageUp()
    {

    }
    public void AmmoSpeedUp()
    {

    }
}
