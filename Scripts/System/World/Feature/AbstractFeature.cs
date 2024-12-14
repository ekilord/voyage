using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public enum FeatureType
{
	NON_BLOCKING,
	FEATURE_BLOCKING,
	STRUCTURE_BLOCKING,
	BLOCKING
}

#nullable enable
public class AbstractFeature
{
    [JsonProperty]
    protected string Name;
    [JsonProperty]
    protected string Description;
	[JsonProperty]
	protected FeatureType FeatureType;
	[JsonProperty]
	protected SpawnCondition SpawnCondition;
	[JsonProperty]
	protected LootTable? LootTable;
	[JsonProperty]
	protected List<Effect>? Effects;

	public AbstractFeature()
	{
		Name = "";
		Description = "";
		FeatureType = FeatureType.NON_BLOCKING;
		SpawnCondition = new ConditionalSpawnCondition();
		LootTable = null;
		Effects = null;
	}

	protected AbstractFeature(string name, string description, FeatureType featureType, SpawnCondition spawnCondition, LootTable loot, List<Effect> effects)
	{
		Name = name;
		Description = description;
		FeatureType = featureType;
		SpawnCondition = spawnCondition;
		LootTable = loot;
		Effects = effects;
	}

	protected AbstractFeature(string name, string description, FeatureType featureType, SpawnCondition spawnCondition, LootTable loot, Effect effect)
	{
		Name = name;
		Description = description;
		FeatureType = featureType;
		SpawnCondition = spawnCondition;
		LootTable = loot;
		Effects = new List<Effect>() { effect };
	}

	protected AbstractFeature(string name, string description, FeatureType featureType, SpawnCondition spawnCondition, LootTable loot)
	{
		Name = name;
		Description = description;
		FeatureType = featureType;
		SpawnCondition = spawnCondition;
		LootTable = loot;
		Effects = null;
	}

	protected AbstractFeature(string name, string description, FeatureType featureType, SpawnCondition spawnCondition)
	{
		Name = name;
		Description = description;
		FeatureType = featureType;
		SpawnCondition = spawnCondition;
		LootTable = null;
		Effects = null;
	}

	public string GetName()
	{
		return Name;
	}

    public string GetDescription()
    {
        return Description;
    }

    public FeatureType GetFeatureType()
    {
        return FeatureType;
    }

    public SpawnCondition GetSpawnCondition()
    {
        return SpawnCondition;
    }

    public LootTable? GetLootTable()
    {
        return LootTable;
    }

	public LootTableEntry? GetMainEntry()
	{
		return LootTable?.GetMainEntry();
	}

	public RawResource? GetMainResource()
	{
		return LootTable?.GetMainEntry().GetResource();
	}

	public int? GetMainResourceAmount()
	{
		return LootTable?.GetMainEntry().GetAmount();
	}

	public List<LootTableEntry>? GetAllSecondaryEntries()
	{
		return LootTable?.GetAllSecondaryEntries();
	}

	public LootTableEntry? GetSecondaryResource( RawResource resource )
	{
		return LootTable?.GetSecondaryEntry( resource );
	}

	public List<Effect>? GetEffects()
	{
		return Effects;
	}

	public void RollLootTable()
	{
		LootTable?.RollResults();
	}

	public void ClearLootTable()
	{
		LootTable = null;
	}
}
