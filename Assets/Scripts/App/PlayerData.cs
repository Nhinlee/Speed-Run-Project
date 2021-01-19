using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    private static int numSlot = 3;

    // Game Slot
    public GameSlot[] gameSlots = new GameSlot[numSlot];
    // Items
    public Dictionary<Item, int> Items = new Dictionary<Item, int>();

    public void LoadPlayerDataFromFile()
    {
        // TODO: Load Player Data
        Debug.Log("Load Player Data");
    }
}

public class GameSlot
{
    public int CurrentLevel { get; set; }
}