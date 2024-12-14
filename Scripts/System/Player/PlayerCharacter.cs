using System;
using System.Collections.Generic;
using UnityEngine;
using static Attribute;

public static class PlayerCharacter
{
    private static string Name;
    private static Map PlayerMap;
    private static Base PlayerBase;
    private static Inventory Inventory;
    private static Inventory Income;
    private static Experience Experience;
    private static Clock Clock;
    private static Stats Stats;
    private static HashSet<Deck> Decks;
    private static Dictionary<ScoutEntity, int> Scouts;
    private static BaseEntity CurrentBase;
    private static BaseEntity OldBase;
    private static bool InGame;

    public static void CreatePlayer(string name)
    {
        Name = name;
        PlayerMap = new Map();
        PlayerBase = new Base();
        Decks = new HashSet<Deck>()
        {
            Deck.Decks.BASE
        };
        Inventory = new Inventory();
        Income = new Inventory();
        Experience = new Experience();
        Stats = Stats.CreateDefaultStats();
        Clock = new Clock();
        Scouts = new()
        {
            { ScoutEntity.Scouts.NOVICE_SCOUTS, 1 },
            { ScoutEntity.Scouts.VETERAN_SCOUTS, 0 },
            { ScoutEntity.Scouts.ROYAL_SCOUTS, 0 }
        };

        CurrentBase = new BaseEntity(false, null);
        OldBase = new BaseEntity(true, null);
        InGame = true;
    }

    public static void SetPlayer(string name, Map playerMap, Base playerBase, Inventory playerInventory, Inventory income, Experience experience, Stats stats, Clock playerClock, HashSet<Deck> decks, Dictionary<ScoutEntity, int> playerScouts, BaseEntity currentBase, BaseEntity oldBase, bool inGame)
    {
        Name = name;
        PlayerMap = playerMap;
        PlayerBase = playerBase;
        Inventory = playerInventory;
        Income = income;
        Experience = experience;
        Stats = stats;
        Clock = playerClock;
        Decks = decks;
        Scouts = playerScouts;
        CurrentBase = currentBase;
        OldBase = oldBase;
        InGame = inGame;
    }

    public static string GetName()
    {
        return Name;
    }

    public static Map GetMap()
    {
        return PlayerMap;
    }

    public static Base GetBase()
    {
        return PlayerBase;
    }

    public static Inventory GetInventory()
    {
        return Inventory;
    }

    public static Inventory GetIncome()
    {
        return Income;
    }

    public static Experience GetExperience()
    {
        return Experience;
    }

    public static Clock GetClock()
    {
        return Clock;
    }

    public static Stats GetStats()
    {
        return Stats;
    }

    public static HashSet<Deck> GetDecks()
    {
        return Decks;
    }

    public static Dictionary<ScoutEntity, int> GetScouts()
    {
        return Scouts;
    }

    public static BaseEntity GetCurrentBase()
    {
        return CurrentBase;
    }

    public static BaseEntity GetOldBase()
    {
        return OldBase;
    }

    public static bool IsInGame()
    {
        return InGame;
    }

    public static void SetInGame(bool value)
    {
        InGame = value;
    }

    public static void AddDeck(Deck deck)
    {
        Decks.Add(deck);

        foreach(var element in PlayerCharacter.GetDecks())
        {
            Debug.Log(element.GetName());
        }
    }

    public static void AddIncome()
    {
        Inventory.AddContents(Income);
    }


    public static void EndRun()
    {
        GetExperience().EndRun();
        SetInGame(false);
    }

    public static void StartRun()
    {
        SetInGame(true);
        GetMap().RemoveScouts();
        GetMap().PopulateMapWithEntities();
        OldBase = new BaseEntity(true, GetCurrentBase().GetLocation());
        if (GetCurrentBase() != null) GetMap().GetTiles()[GetCurrentBase().GetLocation().GetX(), GetCurrentBase().GetLocation().GetY()].SetEntityOccupation(OldBase);
        CurrentBase = new BaseEntity(false, null);
        GetMap().GenerateNewSpawnPoint();
        GetMap().ExploreTile(GetMap().GetSpawnPoint());
        GetClock().ProgressToNextMorning();
    }

    public static bool TryToAcquireBuilding(BuildingVariant variant)
    {
        if (PlayerBase.TryToAcquireBuilding(variant))
        {
            if (variant.GetEffect() != null && variant.GetEffect().Count > 0)
            {
                foreach (Effect effect in variant.GetEffect())
                {
                    EffectManager.AddEffect(effect);
                }
            }
            return Inventory.RemoveCost(variant.GetCost());
        }

        return false;
    }

    public static bool TryToAcquirePreparation(string name, PreparationType preparationType)
    {
        if (preparationType == PreparationType.PERK)
        {
            if (GetExperience().DecreasePerkPoints(1))
            {
                GetExperience().AddPerk(name);
                EffectManager.AddEffect(Experience.Perks.GetPerkByName(name));
                return true;
            }
            return false;
        }
        else
        {
            GetExperience().AddResearch(name);
            return true;
        }
    }

    public static AbstractEntity? CheckForEntityNearBase()
    {
        if ( GetCurrentBase() == null )
            return null;

		int radius = 2;
		int width = GetMap().GetTiles().GetLength( 0 );
		int height = GetMap().GetTiles().GetLength( 1 );

		List<Coordinate> validCoordinates = new();

		for ( int dx = -radius; dx <= radius; dx++ ) {
			for ( int dy = -radius; dy <= radius; dy++ ) {
				int targetX = GetCurrentBase().GetLocation().GetX() + dx;
				int targetY = GetCurrentBase().GetLocation().GetY() + dy;

				if ( targetX >= 0 && targetX < width && targetY >= 0 && targetY < height && ( dx != 0 || dy != 0 ) ) {
					Tile targetTile = GetMap().GetTiles()[targetX, targetY];

					if ( targetTile != null && targetTile.GetEntityOccupation() != null && targetTile.GetEntityOccupation() is not ScoutEntity && targetTile.GetEntityOccupation() is not BaseEntity) {
						validCoordinates.Add( new Coordinate( targetX, targetY ) );
					}
				}
			}
		}

		if ( validCoordinates.Count > 0 ) {
			Coordinate randomDestination = validCoordinates[UnityEngine.Random.Range( 0, validCoordinates.Count )];
            AbstractEntity entity = GetMap().GetTiles()[randomDestination.GetX(), randomDestination.GetY()].GetEntityOccupation();

            if (entity != null) Debug.Log(entity.GetName());

			return entity ?? null;
		}

        return null;
	}

    public static AttributeHolder GetHealth()
    {
        return new AttributeHolder(Attributes.HEALTH, Mathf.Round(Stats.GetBaseHealth().GetValue() * (1 + EffectManager.GetAllModifiersFor(Attributes.HEALTH, PlayerRelation.PLAYER).GetValue())));
    }

    public static AttributeHolder GetActions()
    {
        return new AttributeHolder(Attributes.ACTIONS, Mathf.Round(Stats.GetBaseActions().GetValue() * (1 + EffectManager.GetAllModifiersFor(Attributes.ACTIONS, PlayerRelation.PLAYER).GetValue())));
    }

    public static AttributeHolder GetPhysicalPower()
    {
        return new AttributeHolder(Attributes.PHYSICAL_POWER, Stats.GetBasePhysicalPower().GetValue() * (1 + EffectManager.GetAllModifiersFor(Attributes.PHYSICAL_POWER, PlayerRelation.PLAYER).GetValue()));
    }

    public static AttributeHolder GetArcanePower()
    {
        return new AttributeHolder(Attributes.ARCANE_POWER, Stats.GetBaseArcanePower().GetValue() * (1 + EffectManager.GetAllModifiersFor(Attributes.ARCANE_POWER, PlayerRelation.PLAYER).GetValue()));
    }

    public static AttributeHolder GetDefense()
    {
        return new AttributeHolder(Attributes.DEFENSE, Stats.GetBaseDefense().GetValue() * (1 + EffectManager.GetAllModifiersFor(Attributes.DEFENSE, PlayerRelation.PLAYER).GetValue()));
    }

    public static AttributeHolder GetInitiative()
    {
        return new AttributeHolder(Attributes.INITIATIVE, Stats.GetBaseInitiative().GetValue() * (1 + EffectManager.GetAllModifiersFor(Attributes.INITIATIVE, PlayerRelation.PLAYER).GetValue()));
    }

    public static AttributeHolder GetMorale()
    {
        return new AttributeHolder(Attributes.MORALE, Stats.GetBaseMorale().GetValue() * (1 + EffectManager.GetAllModifiersFor(Attributes.MORALE, PlayerRelation.PLAYER).GetValue()));
    }

    public static AttributeHolder GetAccuracy()
    {
        return new AttributeHolder(Attributes.ACCURACY, Stats.GetBaseAccuracy().GetValue() * (1 + EffectManager.GetAllModifiersFor(Attributes.ACCURACY, PlayerRelation.PLAYER).GetValue()));
    }

    public static AttributeHolder GetSanity()
    {
        return new AttributeHolder(Attributes.SANITY, Stats.GetBaseSanity().GetValue() * (1 + EffectManager.GetAllModifiersFor(Attributes.SANITY, PlayerRelation.PLAYER).GetValue()));
    }

    public static int GetHealthValue()
    {
        return (int)Math.Round(GetHealth().GetValue());
    }

    public static int GetActionsValue()
    {
        return (int)Math.Round(GetActions().GetValue());
    }

    public static float GetPhysicalPowerValue()
    {
        return GetPhysicalPower().GetValue();
    }

    public static float GetArcanePowerValue()
    {
        return GetArcanePower().GetValue();
    }

    public static float GetDefenseValue()
    {
        return GetDefense().GetValue();
    }

    public static float GetInitiativeValue()
    {
        return GetInitiative().GetValue();
    }

    public static float GetMoraleValue()
    {
        return GetMorale().GetValue();
    }

    public static float GetAccuracyValue()
    {
        return GetAccuracy().GetValue();
    }

    public static float GetSanityValue()
    {
        return GetSanity().GetValue();
    }

    public static void RepairBase()
    {
        if (CurrentBase.GetLocation() != null) GetMap().GetTiles()[CurrentBase.GetLocation().GetX(), CurrentBase.GetLocation().GetY()].SetEntityOccupation(null);
        if (OldBase.GetLocation() != null)
        {
            GetMap().GetTiles()[OldBase.GetLocation().GetX(), OldBase.GetLocation().GetY()].SetEntityOccupation(CurrentBase);
            CurrentBase.SetLocation(OldBase.GetLocation());
            OldBase.SetLocation(null);
        }
        
    }
}
