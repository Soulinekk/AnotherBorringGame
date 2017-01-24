using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveManager : MonoBehaviour
{

        public List<Database> db = new List<Database>();



    void Awake()
    {

    }

    void Start()
    {

    }

    void Update()
    {

    }

    void ConstructDatabase()
    {
        if (File.Exists(Application.dataPath + "/StreamingAssets/Save.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.dataPath + "/StreamingAssets/Save.dat", FileMode.Open);
            db = (List<Database>)bf.Deserialize(file);
            file.Close();
            Debug.Log(db[0].Souls);
        }
        else
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.dataPath + "/StreamingAssets/Save.dat");
            CreateNewSave();
            bf.Serialize(file, db);
            file.Close();
            Debug.Log("New Game Save Created");
            LoadSoulsAmount();
        }
    }

    void CreateNewSave()
    {
        bool[] UnboughtCards = new bool[10];
        for (int i = 0; i <= 9; i++)
        {
            UnboughtCards[i] = true;
        }
        int[] cardsInSlotsAndLevels = new int[8];


        for (int i = 0; i <= 7; i++)
        {
            cardsInSlotsAndLevels[i] = -1;
        }
        db.Add(new Database(100, 0, 0, 0, UnboughtCards, "Preset 0", cardsInSlotsAndLevels, cardsInSlotsAndLevels, 0));
    }


    void DeleteSave()
    {
        File.Delete(Application.dataPath + "/StreamingAssets/Save.dat");
        Debug.Log("Save deleted");
    }

    void SaveToFile()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.dataPath + "/StreamingAssets/Save.dat");
        bf.Serialize(file, db);
        file.Close();
    }

    public void LoadSoulsAmount()
    {
        GM.cashAmount = db[0].Souls;
    }

    public void AddEnchant(int id)
    {
        db.Add(new Database(id));
    }               // Used in Investory.cs


}


public class Database
{

    // Database layer 0

    public int Souls { get; set; }
    public int LevelUnlocked { get; set; }
    public int PresetActive { get; set; }
    public int DungeonLevel { get; set; }
    public bool[/*Card_Id*/] UnboughtCards { get; set; }

    // Database.Add

        // Preset

    public string PresetName { get; set; }
    public int[] CardInSlot { get; set; }
    public int[] CardsLevel { get; set; }

        // Enchants

    public int UnusedEnchants { get; set; }

        // Cards

    public string CardName { get; set; }
    public int BaseCard { get; set; }
    public int[/*Slot*/][/*Enchant*/] EnchantsUsed { get; set; }
    public int Sequence_Id { get; set; }

    public Database(int souls, int levelUnlocked, int presetActive, int dungeonLevel, bool[] unboughtCards, string presetName, int[] cardInSlot, int[] cardsLevel, int unusedEnchants)
    {
        Souls = souls;
        LevelUnlocked = levelUnlocked;
        PresetActive = presetActive;
        DungeonLevel = dungeonLevel;
        UnboughtCards = unboughtCards;
        PresetName = presetName;
        CardInSlot = cardInSlot;
        CardsLevel = cardsLevel;
        UnusedEnchants = unusedEnchants;
    }

    public Database(int unusedEnchants)
    {
        UnusedEnchants = unusedEnchants;
    }
}