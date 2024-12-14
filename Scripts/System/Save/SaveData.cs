using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    public string Name;
    public Map PlayerMap;
    public Base PlayerBase;
    public Inventory Inventory;
    public Inventory Income;
    public Experience Experience;
    public Clock Clock;
    public Stats Stats;
    public HashSet<Deck> Decks;
    public Dictionary<ScoutEntity, int> Scouts;
    public BaseEntity CurrentBase;
    public BaseEntity OldBase;
    public bool InGame;

    public SaveData()
    {
        Name = null;
        PlayerMap = null;
        PlayerBase = null;
        Inventory = null;
        Income = null;
        Experience = null;
        Clock = null;
        Stats = null;
        Decks = null;
        Scouts = null;
        CurrentBase = null;
        OldBase = null;
        InGame = false;
    }

    public SaveData(bool test)
	{
        Name = PlayerCharacter.GetName();
		PlayerMap = PlayerCharacter.GetMap();
		PlayerBase = PlayerCharacter.GetBase();
		Inventory = PlayerCharacter.GetInventory();
        Income = PlayerCharacter.GetIncome();
        Experience = PlayerCharacter.GetExperience();
        Stats = PlayerCharacter.GetStats();
		Clock = PlayerCharacter.GetClock();
        Stats = PlayerCharacter.GetStats();
        Decks = PlayerCharacter.GetDecks();
		Scouts = new Dictionary<ScoutEntity, int>( PlayerCharacter.GetScouts() );
        CurrentBase = PlayerCharacter.GetCurrentBase();
        OldBase = PlayerCharacter.GetOldBase();
        InGame = PlayerCharacter.IsInGame();
	}
}
