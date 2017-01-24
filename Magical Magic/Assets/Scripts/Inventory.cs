using UnityEngine;
using System.Collections;
using LitJson;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text;

public class Inventory : MonoBehaviour {

        #region Variables
        private SaveManager saveManager;

        //json
        public List<Item> database = new List<Item>();
        private JsonData itemData;

        //card shop
        public GameObject[] videoList;
        public GameObject[] basicCardPull;

        //tabs
        public GameObject[] tabButtons;
        public GameObject[] _gui;
        public int buttonActive;
        public int _guiActive;
        public GameObject[] panelSwitchButtons;
        public Text[] damageTexts;

        //search
        public Text searchedText;
        public GameObject[] cardPulls;
        public int currentCardPullSearch;

        //Buy
        public bool canBuy = true;
        public Text title;
        public Text shortDescription;
        public GameObject videoPanel;
        public GameObject detailsPanel;
        public GameObject damagePanel;
        private int activeCard;
        public Text priceText;
        #endregion


    void Awake()
    {
        
    }

	void Start () {
        itemData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Items.json"));
        ConstructItemDataBase();

        FirstShopOpenLoad();
    }

    void Update () {
       /* if (Input.GetKeyDown(KeyCode.K))
        {
            // check something
        }*/
    }

    void ConstructItemDataBase()
    {
        for (int i=0; i < itemData["ShopBaseItems"].Count; i++)
        {
            database.Add(new Item(
                (int)itemData["ShopBaseItems"][i]["id"], itemData["ShopBaseItems"][i]["title"].ToString(), (int)itemData["ShopBaseItems"][i]["price"],
                itemData["ShopBaseItems"][i]["description"].ToString(), itemData["ShopBaseItems"][i]["nature"].ToString(),
                itemData["ShopBaseItems"][i]["type"].ToString(), itemData["ShopBaseItems"][i]["shortDescription"].ToString(),
                itemData["ShopBaseItems"][i]["video"].ToString(),
                (int)itemData["ShopBaseItems"][i]["damageType"]["magic"], (int)itemData["ShopBaseItems"][i]["damageType"]["light"],
                (int)itemData["ShopBaseItems"][i]["damageType"]["dark"], (int)itemData["ShopBaseItems"][i]["damageType"]["blood"],
                (int)itemData["ShopBaseItems"][i]["damageType"]["fire"], (int)itemData["ShopBaseItems"][i]["damageType"]["water"],
                (int)itemData["ShopBaseItems"][i]["damageType"]["earth"], (int)itemData["ShopBaseItems"][i]["damageType"]["wind"],
                (int)itemData["ShopBaseItems"][i]["damageType"]["toxin"], (int)itemData["ShopBaseItems"][i]["damageType"]["lightning"],
                (int)itemData["ShopBaseItems"][i]["critChance"], (int)itemData["ShopBaseItems"][i]["costHp"],
                (int)itemData["ShopBaseItems"][i]["costMp"], (int)itemData["ShopBaseItems"][i]["costSouls"]
                ));
        }
    }

    void OnOffShopCards(bool sw)
    {
        if (sw == false)
            for(int i=0; i < basicCardPull.Length; i++)
                basicCardPull[i].SetActive(false);
        else
            for (int i = 0; i < basicCardPull.Length; i++)
                basicCardPull[i].SetActive(true);
    }

    public void BuyCard()
    {
        //TODO play buy sound

        if(canBuy == true && GM.cashAmount >= database[activeCard].Price)
        {
            canBuy = false;
            saveManager.db[0].Souls = GM.cashAmount - database[activeCard].Price;
            saveManager.db[0].UnboughtCards[activeCard] = false;
            saveManager.LoadSoulsAmount();
            canBuy = true;
        }
    }

    public void BuyEnchant()
    {
        if (canBuy==true && GM.cashAmount>=database[activeCard].Price)
        {
            canBuy = false;
            saveManager.db[0].Souls = GM.cashAmount - database[activeCard].Price;
            saveManager.AddEnchant(activeCard);
            saveManager.LoadSoulsAmount();
            canBuy = true;
        }
    }

    public void LoadSelectedCardShop(int id)
    {
        activeCard = id;

        //for (int i=0; i < database.Count; i++)
        //{
        //    if ((int)database[i].ID == id) 
         //   {
                StringBuilder sBuilder = new StringBuilder((database[id].CostMp.ToString() + "/" + database[id].CostHp.ToString() + "/" + database[id].CostSouls.ToString()));  //StringBuilder allow to connect difirent strings with html like text styles
                sBuilder.Insert(0, "<color=blue>");                                                                                                           // coloring spell's cost colors
                sBuilder.Insert(12 + database[id].CostMp.ToString().Length, "</color>");
                sBuilder.Insert(21 + database[id].CostMp.ToString().Length, "<color=red>");
                sBuilder.Insert(32 + database[id].CostMp.ToString().Length + database[id].CostHp.ToString().Length, "</color>");
                sBuilder.Insert(41 + database[id].CostMp.ToString().Length + database[id].CostHp.ToString().Length, "<color=gray>");
                sBuilder.Insert(53 + database[id].CostMp.ToString().Length + database[id].CostHp.ToString().Length + database[id].CostSouls.ToString().Length, "</color>");

                title.GetComponent<Text>().text = database[id].Title;
                shortDescription.GetComponent<Text>().text = database[id].ShortDescription;
                detailsPanel.GetComponent<Text>().text = database[id].Description;

                damageTexts[0].text = database[id].Magic.ToString();
                damageTexts[1].text = database[id].Light.ToString();
                damageTexts[2].text = database[id].Dark.ToString();
                damageTexts[3].text = database[id].Blood.ToString();
                damageTexts[4].text = database[id].CritChance.ToString();
                damageTexts[5].text = sBuilder.ToString();                                                                                                      // putting edited (by StringBuilder) string to display
                damageTexts[6].text = database[id].Type;
                damageTexts[7].text = database[id].Fire.ToString();
                damageTexts[8].text = database[id].Water.ToString();
                damageTexts[9].text = database[id].Earth.ToString();
                damageTexts[10].text = database[id].Wind.ToString();
                damageTexts[11].text = database[id].Toxin.ToString();
                damageTexts[12].text = database[id].Lightning.ToString();
                priceText.text = (database[id].Price.ToString() + " $");
                return;
      //      }
      //  }
    }

    public void FirstShopOpenLoad()
    {
        tabButtons[0].GetComponent<Button>().interactable = false;
        _guiActive = 0;
        buttonActive = 0;
        currentCardPullSearch = 0;
        gameObject.GetComponent<SaveInventory>().LoadInventoryCards();
    }
    
    public void ShopTabs(int i)
    {
        tabButtons[buttonActive].GetComponent<Button>().interactable = true;
        
        _gui[_guiActive].SetActive(false);
        _guiActive = i;

        #region Switch GUI and buttons active
        switch (i)
        {
            case 0:
                foreach (Transform c in cardPulls[0].transform)
                {
                    c.gameObject.SetActive(false);
                    Destroy(c.gameObject);
                }
                tabButtons[i].GetComponent<Button>().interactable = false;
                buttonActive = 0;
                currentCardPullSearch = 0;
                _gui[i].SetActive(true);
                gameObject.GetComponent<SaveInventory>().LoadInventoryCards();
                break;

            case 1:
                foreach (Transform c in cardPulls[1].transform)
                {
                    c.gameObject.SetActive(false);
                    Destroy(c.gameObject);
                }
                tabButtons[i].GetComponent<Button>().interactable = false;
                buttonActive = 1;
                currentCardPullSearch = 1;
                _gui[i].SetActive(true);
                gameObject.GetComponent<SaveInventory>().LoadWorkshopCards();
                break;

            case 2:
                tabButtons[i].GetComponent<Button>().interactable = false;
                buttonActive = 2;
                currentCardPullSearch = 2;
                _gui[i].SetActive(true);
                break;

            case 3:
                tabButtons[i].GetComponent<Button>().interactable = false;
                buttonActive = 3;
                currentCardPullSearch = 3;
                _gui[i].SetActive(true);
                break;
            default:
                Debug.Log("ShopTabs Switch error");
                break;
        }
        #endregion

        
    }

    public void LoadShopCardsOnOff(int v)
    {
        switch (v)
        {
            case 1:
                for (int i = 0; basicCardPull.Length > i; i++)
                {
                    basicCardPull[i].SetActive(true);
                }
                break;

            case 2:
                for (int i = 0; basicCardPull.Length > i; i++)
                {
                    basicCardPull[i].SetActive(false);
                }
                break;
        }
    }

    public void ValueChangedCheck()
    {
        StopCoroutine(LateSearch(searchedText, currentCardPullSearch));
        StartCoroutine(LateSearch(searchedText, currentCardPullSearch));
    }
    
    public void SwitchShopPanels(int x)
    {
        switch(x)
        {
            case 0:
                panelSwitchButtons[0].GetComponent<Button>().interactable = false;
                panelSwitchButtons[1].GetComponent<Button>().interactable = true;
                panelSwitchButtons[2].GetComponent<Button>().interactable = true;
                
                damagePanel.SetActive(false);
                videoPanel.SetActive(false);
                detailsPanel.SetActive(true);
                break;
                
            case 1:
                panelSwitchButtons[1].GetComponent<Button>().interactable = false;
                panelSwitchButtons[2].GetComponent<Button>().interactable = true;
                panelSwitchButtons[0].GetComponent<Button>().interactable = true;

                videoPanel.SetActive(false);
                detailsPanel.SetActive(false);
                damagePanel.SetActive(true);
                break;

            case 2:
                panelSwitchButtons[2].GetComponent<Button>().interactable = false;
                panelSwitchButtons[0].GetComponent<Button>().interactable = true;
                panelSwitchButtons[1].GetComponent<Button>().interactable = true;

                damagePanel.SetActive(false);
                detailsPanel.SetActive(false);
                videoPanel.SetActive(true);
                break;
        }
    }

    IEnumerator LateSearch(Text textToPrint, int cur)
    {
        yield return new WaitForSeconds(0.1f);

        for(int i=0; i < cardPulls[cur].transform.childCount; i++)
        {
            if (cardPulls[cur].transform.GetChild(i).GetComponent<CardInfo>()._name.Contains(textToPrint.text.ToLower()))
            {
                cardPulls[cur].transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                cardPulls[cur].transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}

[System.Serializable]
public class Item
{
    public int ID { get; set; }
    public string Title { get; set; }
    public int Price { get; set; }
    public string Description { get; set; }
    public string Nature { get; set; }
    public string Type { get; set; }
    public string ShortDescription { get; set; }
    public string Video { get; set; }
    public int Magic { get; set; }
    public int Light { get; set; }
    public int Dark { get; set; }
    public int Blood { get; set; }
    public int Fire { get; set; }
    public int Water { get; set; }
    public int Earth { get; set; }
    public int Wind { get; set; }
    public int Toxin { get; set; }
    public int Lightning { get; set; }
    public int CritChance { get; set; }
    public int CostHp { get; set; }
    public int CostMp { get; set; }
    public int CostSouls { get; set; }


    public Item(int id, string title, int price, string description, string nature, string type, string shortDescription, string video,
        int magic, int light, int dark, int blood, int fire, int water, int earth, int wind, int toxin, int lightning, int critChance, 
        int costHp, int costMp, int costSouls)
    {
        this.ID = id;
        this.Title = title;
        this.Price = price;
        this.Description = description;
        this.Nature = nature;
        this.Type = type;
        this.ShortDescription = shortDescription;
        this.Video = video;
        this.Magic = magic;
        this.Light = light;
        this.Dark = dark;
        this.Blood = blood;
        this.Fire = fire;
        this.Water = water;
        this.Earth = earth;
        this.Wind = wind;
        this.Toxin = toxin;
        this.Lightning = lightning;
        this.CritChance = critChance;
        this.CostHp = costHp;
        this.CostMp = costMp;
        this.CostSouls = costSouls;
    }
}