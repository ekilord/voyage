using Newtonsoft.Json;
using System;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

#nullable enable
public class Tile
{
	[JsonProperty]
    private Coordinate Coordinate;
	[JsonProperty]
    private Biome Biome;
	[JsonProperty]
    private NaturalFeature? NaturalFeature;
	[JsonProperty]
    private StructureFeature? StructureFeature;
	[JsonProperty]
    private AbstractEntity? EntityOccupation;
	[JsonProperty]
	private bool Explored;

	public Tile()
	{
		Coordinate = new Coordinate( 0, 0 );
		Biome = Biome.Biomes.DEEP_OCEAN;
		NaturalFeature = null;
		StructureFeature = null;
		EntityOccupation = null;
		Explored = false;
	}

	public Tile( Coordinate coordinate, Biome biome, NaturalFeature naturalFeature, StructureFeature structureFeature, AbstractEntity entityOccupation, bool explored )//, Occupation occupation)
	{
		Coordinate = coordinate;
		Biome = biome;
		NaturalFeature = naturalFeature;
		StructureFeature = structureFeature;
		EntityOccupation = entityOccupation;
		//Occupation = occupation;
		Explored = explored;
	}

	public Tile( Coordinate coordinate, Biome biome )
	{
		Coordinate = coordinate;
		Biome = biome;
		NaturalFeature = null;
		StructureFeature = null;
		EntityOccupation = null;
		Explored = false;
	}

	public Coordinate GetCoordinate()
	{
		return Coordinate;
	}

	public Biome GetBiome()
	{
		return Biome;
	}

	public NaturalFeature? GetNaturalFeature()
	{
		return NaturalFeature;
	}

	public StructureFeature? GetStructureFeature()
	{
		return StructureFeature;
	}

	public AbstractEntity? GetEntityOccupation()
	{
		return EntityOccupation;
	}

	public bool IsExplored()
	{
		return Explored;
	}

	public void SetToExplored()
	{
		Explored = true;
	}

	public void SetNaturalFeature( NaturalFeature naturalFeature )
	{
		if ( NaturalFeature == null )
			NaturalFeature = NaturalFeature.Features.Create( naturalFeature );
		else
			throw new InvalidOperationException( "Cannot set NaturalFeature attribute more than once" );
	}

	public void SetStructureFeature( StructureFeature structureFeature )
	{
		if ( StructureFeature == null )
			StructureFeature = StructureFeature.Features.Create( structureFeature );
		else
			throw new InvalidOperationException( "Cannot set StructureFeature attribute more than once" );
	}

	public void SetEntityOccupation( AbstractEntity abstractEntity )
	{
		EntityOccupation = abstractEntity;
	}

	public void RollLoots()
	{
		NaturalFeature?.RollLootTable();
		StructureFeature?.RollLootTable();
	}

    public int GetTotalResources()
    {
        int totalResources = 0;

        if (NaturalFeature != null)
        {
            totalResources += GetResourcesFromLootTable(NaturalFeature.GetLootTable());
        }

        if (StructureFeature != null)
        {
            totalResources += GetResourcesFromLootTable(StructureFeature.GetLootTable());
        }

        return totalResources;
    }

    private int GetResourcesFromLootTable(LootTable? lootTable)
    {
        int resourceAmount = 0;

        if (lootTable == null) return resourceAmount;

        var mainEntry = lootTable.GetMainEntry();
        if (mainEntry != null)
        {
            resourceAmount += mainEntry.GetAmount() != null ? (int)mainEntry.GetAmount() : 0;
        }

        var secondaryEntries = lootTable.GetAllSecondaryEntries();
        if (secondaryEntries != null)
        {
            foreach (var entry in secondaryEntries)
            {
                resourceAmount += entry.GetAmount() != null ? (int)entry.GetAmount() : 0;
            }
        }

        return resourceAmount;
    }

	public Inventory CollectResourcesFromTile()
	{
		Inventory collectedInventory = new Inventory();

		if ( GetNaturalFeature() != null ) {
			LootTable? naturalLootTable = GetNaturalFeature()?.GetLootTable();
			if ( naturalLootTable != null ) {
				var mainEntry = naturalLootTable.GetMainEntry();
				if ( mainEntry != null ) {
					collectedInventory.AddResource( mainEntry.GetResource(), mainEntry.GetAmount() ?? 0 );
				}

				var secondaryEntries = naturalLootTable.GetAllSecondaryEntries();
				if ( secondaryEntries != null ) {
					foreach ( var entry in secondaryEntries ) {
						collectedInventory.AddResource( entry.GetResource(), entry.GetAmount() ?? 0 );
					}
				}

				GetNaturalFeature()?.ClearLootTable();
			}
		}

		if ( GetStructureFeature() != null ) {
			LootTable? structureLootTable = GetStructureFeature()?.GetLootTable();
			if ( structureLootTable != null ) {
				var mainEntry = structureLootTable.GetMainEntry();
				if ( mainEntry != null ) {
					collectedInventory.AddResource( mainEntry.GetResource(), mainEntry.GetAmount() ?? 0 );
				}

				var secondaryEntries = structureLootTable.GetAllSecondaryEntries();
				if ( secondaryEntries != null ) {
					foreach ( var entry in secondaryEntries ) {
						collectedInventory.AddResource( entry.GetResource(), entry.GetAmount() ?? 0 );
					}
				}

				GetStructureFeature()?.ClearLootTable();
			}
		}

		return collectedInventory;
	}
}
