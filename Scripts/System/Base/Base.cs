using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Building;

#nullable enable
public class Base
{
	[JsonProperty]
	private Dictionary<BuildingVariant, bool> GatheringBuildings;
	[JsonProperty]
	private Dictionary<BuildingVariant, bool> CombatBuildings;
	[JsonProperty]
	private Dictionary<BuildingVariant, bool> UtilityBuildings;
	[JsonProperty]
	private Dictionary<BuildingVariant, int> Timetable;

	public Base()
	{
		GatheringBuildings = new Dictionary<BuildingVariant, bool>();

		foreach ( var variant in Buildings.GOLD.GetVariants() )
			GatheringBuildings[variant] = false;

		foreach ( var variant in Buildings.WOOD.GetVariants() )
			GatheringBuildings[variant] = false;

		foreach ( var variant in Buildings.STONE.GetVariants() )
			GatheringBuildings[variant] = false;

		foreach ( var variant in Buildings.IRON.GetVariants() )
			GatheringBuildings[variant] = false;

		foreach ( var variant in Buildings.AETHER.GetVariants() )
			GatheringBuildings[variant] = false;

		CombatBuildings = new Dictionary<BuildingVariant, bool>();

		foreach ( var variant in Buildings.ATTACK.GetVariants() )
			CombatBuildings[variant] = false;

		foreach ( var variant in Buildings.ARCANE.GetVariants() )
			CombatBuildings[variant] = false;

		foreach ( var variant in Buildings.DEFENSE.GetVariants() )
			CombatBuildings[variant] = false;

		UtilityBuildings = new Dictionary<BuildingVariant, bool>();

		foreach ( var variant in Buildings.MORALE.GetVariants() )
			UtilityBuildings[variant] = false;

		foreach ( var variant in Buildings.INITIATIVE.GetVariants() )
			UtilityBuildings[variant] = false;

		foreach ( var variant in Buildings.HP.GetVariants() )
			UtilityBuildings[variant] = false;

		Timetable = new();
	}

	public Dictionary<BuildingVariant, int> GetTimetable()
	{
		return Timetable;
	}

	public bool TryToAcquireBuilding( BuildingVariant variant )
	{
		if ( variant.GetCost().IsEnough( PlayerCharacter.GetInventory() ) ) {
			AddBuilding( variant );

			return true;
		}

		return false;
	}

	public void AddBuilding( BuildingVariant variant )
	{
		if ( GatheringBuildings.ContainsKey( variant ) || CombatBuildings.ContainsKey( variant ) || UtilityBuildings.ContainsKey( variant ) ) {
			Timetable.Add( variant, variant.GetRank() );


			return;
		}
	}

	public void AddIncome(BuildingVariant variant)
	{
		Building? building = Buildings.GetBuildingByVariant( variant );

		if ( building == null )
			return;

		if (building.Equals(Buildings.GOLD)) {
			PlayerCharacter.GetIncome().AddResource( RawResource.RawResources.GOLD, variant.GetRank() );
		}
		else if ( building.Equals( Buildings.WOOD ) ) {
			PlayerCharacter.GetIncome().AddResource( RawResource.RawResources.WOOD, variant.GetRank() );
		}
		else if ( building.Equals( Buildings.STONE ) ) {
			PlayerCharacter.GetIncome().AddResource( RawResource.RawResources.STONE, variant.GetRank() );
		}
		else if ( building.Equals( Buildings.IRON ) ) {
			PlayerCharacter.GetIncome().AddResource( RawResource.RawResources.IRON, variant.GetRank() );
		}
		else if ( building.Equals( Buildings.AETHER ) ) {
			PlayerCharacter.GetIncome().AddResource( RawResource.RawResources.AETHER, variant.GetRank() );
		}
	}

	public void PassTime( int amount )
	{
		List<BuildingVariant> toRemove = new();
		List<BuildingVariant> toDecrement = new();

		foreach ( (BuildingVariant key, int value) in Timetable ) {
			if ( Timetable[key] - amount > 0 ) {
                toDecrement.Add( key );
			}
			else {
				AcquireBuilding( key );
				toRemove.Add(key);
			}
		}

        foreach (BuildingVariant variant in toDecrement)
        {
			Timetable[variant] -= amount;
        }

        foreach (BuildingVariant variant in toRemove)
		{
            Timetable.Remove(variant);
        }
	}

	public void AcquireBuilding( BuildingVariant variant )
	{
		if ( GatheringBuildings.ContainsKey( variant ) ) {
			GatheringBuildings[variant] = true;
			AddDeck( variant );
		}
		else if ( CombatBuildings.ContainsKey( variant )) {
			CombatBuildings[variant] = true;
            AddDeck(variant);
        }
		else if ( UtilityBuildings.ContainsKey( variant ) ) {
			UtilityBuildings[variant] = true;
            AddDeck(variant);
        }
	}

	public BuildingVariant? GetHightestTierAcquiredVariant( Building building )
	{
		for ( int i = building.GetVariants().Length - 1; i >= 0; i-- ) {
			if ( GatheringBuildings.ContainsKey( building.GetVariants()[i] ) && GatheringBuildings[building.GetVariants()[i]] )
				return building.GetVariants()[i];
			if ( CombatBuildings.ContainsKey( building.GetVariants()[i] ) && CombatBuildings[building.GetVariants()[i]] )
				return building.GetVariants()[i];
			if ( UtilityBuildings.ContainsKey( building.GetVariants()[i] ) && UtilityBuildings[building.GetVariants()[i]] )
				return building.GetVariants()[i];
		}

		return null;
	}

	private void AddDeck(BuildingVariant variant)
	{
		Deck? deck = Buildings.GetBuildingByVariant(variant)?.GetDeck();

		if (deck != null)
		{
			PlayerCharacter.AddDeck(deck);
		}
    }

	public List<BuildingVariant> GetPossibleUpgrades()
	{
		if (PlayerCharacter.GetCurrentBase().GetLocation() == null) return new();

		List<BuildingVariant> available = new();

		foreach ( Building building in Buildings.GetAllBuildings ) {
			BuildingVariant? variant = GetHightestTierAcquiredVariant( building );

			if ( variant != null ) {
				switch ( variant.GetRank() ) {
					case 1:
					available.Add( building.GetSecondRank() );
					break;
					case 2:
					available.Add( building.GetThirdRank() );
					break;
					default:
					break;
				}
			}
			else
				available.Add( building.GetFirstRank() );
		}

		return available;
	}

	public List<BuildingVariant> GetOwnedBuildings()
	{
		List<BuildingVariant> owned = new();

		foreach ( Building building in Buildings.GetAllBuildings ) {
			for ( int i = building.GetVariants().Length - 1; i >= 0; i-- ) {
				if ( GatheringBuildings.ContainsKey( building.GetVariants()[i] ) && GatheringBuildings[building.GetVariants()[i]] )
					owned.Add( building.GetVariants()[i] );
				if ( CombatBuildings.ContainsKey( building.GetVariants()[i] ) && CombatBuildings[building.GetVariants()[i]] )
					owned.Add( building.GetVariants()[i] );
				if ( UtilityBuildings.ContainsKey( building.GetVariants()[i] ) && UtilityBuildings[building.GetVariants()[i]] )
					owned.Add( building.GetVariants()[i] );
			}
		}

		return owned;
	}

    public List<BuildingVariant> GetBuildingsInTimetable()
    {
        return new List<BuildingVariant>(Timetable.Keys);
    }
}
