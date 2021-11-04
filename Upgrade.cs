using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
public class Upgrade : MonoBehaviour
{
    Player_Progress PG; // player database class
    P_saldiri p_Saldiri; // player attack class calling for update reload time max value in slider
    Saglik saglik; //player's ship healt class for update healt max value in slider
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
    public GameObject NocurrencyPanel; // not enought currency warning
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
    public class Upgradelist  //healt and reload speed upgrade list class
    {
        public int price;
        public bool isStar; 
        public int healtUpgradeAmount; 
        public float reloadSpeedAmount;
    }
    void Start()
    {
        PG = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Progress>(); //assign player database
        p_Saldiri = GameObject.FindGameObjectWithTag("Player").GetComponent<P_saldiri>();
        saglik = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Saglik>();
        AmmoUpgradeAni = AmmoUpgradePanel.GetComponent<Animator>();
        HealtUpgradeCheck();
        ReloadUpgradeCheck();
        SelectedAmmoImage();
    }


    void HealtUpgradeCheck()  // assign upgrade number of healt
    {
        HealtUpgradelimit_text.GetComponent<TMP_Text>().text = PG.HealtUpgradeNumber + "/" + maxHealt_UpgradeLimit;
    }
    void ReloadUpgradeCheck() // assign upgrade number of reloadspeed 
    {
        ReloadUpgradelimit_text.GetComponent<TMP_Text>().text = PG.ReloadUpgradeNumber + "/" + maxReload_UpgradeLimit;
    }
    public void MaxHealtUgradePanel() //open healt upgrade panel
    {
        if (PG.HealtUpgradeNumber < maxHealt_UpgradeLimit)
        {
            HealtUpgradePanel.SetActive(true);
            string currency;
            if (healtupgrade_prices[PG.HealtUpgradeNumber].isStar)  //check the next upgrade currency and warning the player
                currency = "star";
            else
                currency = "credit"; 
            HealtUpgradePanel_text.GetComponent<TMP_Text>().text = "Are you sure pay "+ healtupgrade_prices[PG.HealtUpgradeNumber].price+" "+
                 currency +"  for upgrade max healt? (" + healtupgrade_prices[PG.HealtUpgradeNumber].healtUpgradeAmount + " healt)";
            //there are two options in the panel, named pay and cancel
        }
    }
    public void ReloadUgradePanel() //similar of MaxHealtUgradePanel 
    {
        if (PG.ReloadUpgradeNumber < maxReload_UpgradeLimit)
        {
            ReloadUpgradePanel.SetActive(true);
            string currency;
            if (reloadupgrade_prices[PG.ReloadUpgradeNumber].isStar)
                currency = "star";
            else
                currency = "credit";
            ReloadUpgradePanel_text.GetComponent<TMP_Text>().text = "Are you sure pay " + reloadupgrade_prices[PG.ReloadUpgradeNumber].price + " " +
                currency +"  for upgrade reload speed? (" + reloadupgrade_prices[PG.ReloadUpgradeNumber].reloadSpeedAmount + " reload speed)";
        }
    }
    public void MaxHealtUP_Pay() // assign the pay button 
    {
        if (healtupgrade_prices[PG.HealtUpgradeNumber].isStar == true) //checking the currency type
        {
            if (PG.star > healtupgrade_prices[PG.HealtUpgradeNumber].price)
            {
                PG.star = PG.star - healtupgrade_prices[PG.HealtUpgradeNumber].price;
                PG.PlayerMaxcan = PG.PlayerMaxcan + healtupgrade_prices[PG.HealtUpgradeNumber].healtUpgradeAmount;
                PG.Playercan = PG.Playercan + healtupgrade_prices[PG.HealtUpgradeNumber].healtUpgradeAmount; // upgrade healt with max healt
                PG.StarCheck(); //refresh star currency number in the screen
                PG.HealtUpgradeNumber += 1;
                HealtUpgradeCheck(); // refresh the upgrade number for the upgrade panel
                saglik.slider.maxValue = PG.PlayerMaxcan; // adapt healt sliders max value
                HealtUpgradePanel.SetActive(false);
            }
            else
            {
                NocurrencyPanel.GetComponent<Nocurrency>().isStar = true; //warning for not enough currency
                NocurrencyPanel.SetActive(true);
                HealtUpgradePanel.SetActive(false);
            }
        }
        else if (healtupgrade_prices[PG.HealtUpgradeNumber].isStar == false)  //same progress if the next upgrade currency is credit instead of star
        {
            if (PG.credit > healtupgrade_prices[PG.HealtUpgradeNumber].price)
            {
                PG.credit = PG.credit - healtupgrade_prices[PG.HealtUpgradeNumber].price;
                PG.PlayerMaxcan = PG.PlayerMaxcan + healtupgrade_prices[PG.HealtUpgradeNumber].healtUpgradeAmount;
                PG.Playercan = PG.Playercan + healtupgrade_prices[PG.HealtUpgradeNumber].healtUpgradeAmount;
                PG.CreditCheck();
                PG.HealtUpgradeNumber += 1;
                HealtUpgradeCheck();
                saglik.slider.maxValue = PG.PlayerMaxcan;
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
    public void ReloadtUP_Pay() //similar of maxhealtpay
    {
        if (reloadupgrade_prices[PG.ReloadUpgradeNumber].isStar == true)
        {
            if (PG.star > reloadupgrade_prices[PG.ReloadUpgradeNumber].price)
            {
                PG.star = PG.star - reloadupgrade_prices[PG.ReloadUpgradeNumber].price;
                PG.Reloadtime = PG.Reloadtime + reloadupgrade_prices[PG.ReloadUpgradeNumber].reloadSpeedAmount; //adding negative value in editor
                PG.StarCheck();
                PG.ReloadUpgradeNumber += 1;
                ReloadUpgradeCheck();
                p_Saldiri.atis.maxValue = PG.Reloadtime; // changing reload time max value 
                ReloadUpgradePanel.SetActive(false);
            }
            else
            {
                NocurrencyPanel.GetComponent<Nocurrency>().isStar = true;
                NocurrencyPanel.SetActive(true);
                ReloadUpgradePanel.SetActive(false);
            }
        }
        else if (reloadupgrade_prices[PG.ReloadUpgradeNumber].isStar == false)
        {
            if (PG.credit > reloadupgrade_prices[PG.ReloadUpgradeNumber].price)
            {
                PG.credit = PG.credit - reloadupgrade_prices[PG.ReloadUpgradeNumber].price;
                PG.Reloadtime = PG.Reloadtime + reloadupgrade_prices[PG.ReloadUpgradeNumber].reloadSpeedAmount;
                PG.CreditCheck();
                PG.ReloadUpgradeNumber += 1;
                ReloadUpgradeCheck();
                p_Saldiri.atis.maxValue = PG.Reloadtime;
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
    public void ReloadUP_Cancel() //??
    {
        ReloadUpgradePanel.SetActive(false);
    }
    public void SelectedAmmoImage() //show player to which ammo is active right now
    {
        selectedImage.transform.position = new Vector3(AmmolistButtons[PG.selectedAmmo].transform.position.x, selectedImage.transform.position.y, transform.position.z);
    }   
    public void AmmoUpgrade() //one function for every unique ammo upgrade button
    {
        var go = EventSystem.current.currentSelectedGameObject; //find witch button is pressed ????
        if (AmmoUpgradeAni != null) 
        {
            bool isopen = AmmoUpgradeAni.GetBool("open");
            AmmoUpgradeAni.SetBool("open", !isopen);
        }
        for (int i = 0; i < AmmolistButtons.Count; i++) 
        {
            if (go.name == AmmolistButtons[i].name) //check button list and find the button
            {
                SelectedAmmoId = i; //define ammo id for using in upgrade methods
                AmmoNameText.GetComponent<TMP_Text>().text = PG.Ammolist[SelectedAmmoId].name;
                if (PG.AmmoIsOpen[i] == true) //and if this button is unlock from player data base, add panel values from AmmoUpgradelist
                {
                    AmmoFullExpText.GetComponent<TMP_Text>().text = AmmoUpgradelist[SelectedAmmoId].AmmoExpString;
                    AmmoDamageText.GetComponent<TMP_Text>().text = PG.Ammolist[SelectedAmmoId].GetComponent<gitme>().hasar.ToString();
                    AmmoSpeedText.GetComponent<TMP_Text>().text = PG.Ammolist[SelectedAmmoId].GetComponent<gitme>().hiz.ToString();
                    AmmoDamageExpText.GetComponent<TMP_Text>().text = "(" + AmmoUpgradelist[SelectedAmmoId].AmmoDamage_UpgradeNumber + "/" + AmmoUpgradelist[SelectedAmmoId].AmmoDamage_MaxUpgradeNumber + ")";
                    AmmoSpeedExpText.GetComponent<TMP_Text>().text = "(" + AmmoUpgradelist[SelectedAmmoId].AmmoSpeed_UpgradeNumber + "/" + AmmoUpgradelist[SelectedAmmoId].AmmoSpeed_MaxUpgradeNumber + ")";

                }
                else // if button not unlock warning the player
                {
                    AmmoFullExpText.GetComponent<TMP_Text>().text = "You need to unlock this ammo from somewhere"; //change
                    AmmoDamageText.GetComponent<TMP_Text>().text = PG.Ammolist[SelectedAmmoId].GetComponent<gitme>().hasar.ToString();
                    AmmoSpeedText.GetComponent<TMP_Text>().text = PG.Ammolist[SelectedAmmoId].GetComponent<gitme>().hiz.ToString();
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
