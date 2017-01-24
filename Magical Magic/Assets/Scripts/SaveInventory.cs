using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.IO;

public class SaveInventory : MonoBehaviour {
    
    //JSON
    public List<Cards> database = new List<Cards>();
    private JsonData itemsData;

    //GameObjects
    public GameObject inventoryCardPull;
    public GameObject workshopCardPull;
    public GameObject emptyCardPrefab;
    public GameObject cardPrefab;
    
    void Awake()
    {
        itemsData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Saves.json"));
    }

	void Start () {
        //ConstructItemDataBase();
    }
	
	void Update () {
	
	}

/*
    void ConstructItemDataBase()
    {
        for (int i = 0; i < itemsData.Count; i++)
        {
            database.Add(new Cards(

                (int)itemsData[i]["id"], 
                itemsData[i]["cardName"].ToString(), 
                itemsData[i]["name"].ToString(),

                (int)itemsData[i]["encht"]["slot1"],
                (int)itemsData[i]["encht"]["slot2"],
                (int)itemsData[i]["encht"]["slot3"],
                (int)itemsData[i]["encht"]["slot4"],
                (int)itemsData[i]["encht"]["slot5"],
                (int)itemsData[i]["encht"]["slot6"],
                (int)itemsData[i]["encht"]["slot7"],
                (int)itemsData[i]["encht"]["slot8"],

                (int)itemsData[i]["encht_lvl"]["encht1_lvl"],
                (int)itemsData[i]["encht_lvl"]["encht2_lvl"],
                (int)itemsData[i]["encht_lvl"]["encht3_lvl"],
                (int)itemsData[i]["encht_lvl"]["encht4_lvl"],
                (int)itemsData[i]["encht_lvl"]["encht5_lvl"],
                (int)itemsData[i]["encht_lvl"]["encht6_lvl"],
                (int)itemsData[i]["encht_lvl"]["encht7_lvl"],
                (int)itemsData[i]["encht_lvl"]["encht8_lvl"]
            ));
        }
    }
    */

    public void LoadInventoryCards()
    {
        if (itemsData["Inventory"].Count == 0)
            LoadEmptyInvestoryCards(8);
        else
        {
            for(int i = 0; i < itemsData["Inventory"].Count; i++)
            {
                GameObject newCard = Instantiate(Resources.Load(itemsData["Inventory"][i]["cardName"].ToString()) as GameObject);
                newCard.transform.SetParent(inventoryCardPull.transform, false);
                // TODO: Load card image and stuff
            }
            if (itemsData["Inventory"].Count < 8)
            {
                int am = 8 - itemsData["Inventory"].Count;
                LoadEmptyInvestoryCards(am);
            }
            else
                LoadEmptyInvestoryCards(1);
        }
    }

    public void LoadWorkshopCards()
    {
        if (itemsData["Inventory"].Count == 0)
            LoadEmptyWorkshopCards(8);
        else
        {
            for (int i = 0; i < itemsData["Inventory"].Count; i++)
            {
                GameObject newCard = Instantiate(Resources.Load(itemsData["Inventory"][i]["cardName"].ToString()) as GameObject);
                newCard.transform.SetParent(workshopCardPull.transform, false);
            }
            if (itemsData["Inventory"].Count < 8)
            {
                int am = 8 - itemsData["Inventory"].Count;
                LoadEmptyWorkshopCards(am);
            }
            else
                LoadEmptyWorkshopCards(1);
        }
    }

    void LoadEmptyInvestoryCards(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject newCard = Instantiate(emptyCardPrefab);
            newCard.transform.SetParent(inventoryCardPull.transform, false);
        }
    }

    void LoadEmptyWorkshopCards(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject newCard = Instantiate(emptyCardPrefab);
            newCard.transform.SetParent(workshopCardPull.transform, false);
        }
    }
}

[System.Serializable]
public class Cards
{
    public int Id { get; set; }
    public string CardName { get; set; }
    public string Name { get; set; }
        public int Slot1 { get; set; }
        public int Slot2 { get; set; }
        public int Slot3 { get; set; }
        public int Slot4 { get; set; }
        public int Slot5 { get; set; }
        public int Slot6 { get; set; }
        public int Slot7 { get; set; }
        public int Slot8 { get; set; }

        public int Encht1_lvl { get; set; }
        public int Encht2_lvl { get; set; }
        public int Encht3_lvl { get; set; }
        public int Encht4_lvl { get; set; }
        public int Encht5_lvl { get; set; }
        public int Encht6_lvl { get; set; }
        public int Encht7_lvl { get; set; }
        public int Encht8_lvl { get; set; }

    public Cards(int id, string cardName, string name, int slot1, int slot2, int slot3, int slot4, int slot5, int slot6, int slot7, int slot8, 
        int encht1_lvl, int encht2_lvl, int encht3_lvl, int encht4_lvl, int encht5_lvl, int encht6_lvl, int encht7_lvl, int encht8_lvl)
    {
        this.Id = id;
        this.CardName = cardName;
        this.Name = name;
        //encht slots
            this.Slot1 = slot1;
            this.Slot2 = slot2;
            this.Slot3 = slot3;
            this.Slot4 = slot4;
            this.Slot5 = slot5;
            this.Slot6 = slot6;
            this.Slot7 = slot7;
            this.Slot8 = slot8;
        //ench_lvl 
            this.Encht1_lvl = encht1_lvl;
            this.Encht2_lvl = encht1_lvl;
            this.Encht3_lvl = encht1_lvl;
            this.Encht4_lvl = encht1_lvl;
            this.Encht5_lvl = encht1_lvl;
            this.Encht6_lvl = encht1_lvl;
            this.Encht7_lvl = encht1_lvl;
            this.Encht8_lvl = encht1_lvl;
    }
}
