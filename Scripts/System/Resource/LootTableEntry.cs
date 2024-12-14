using Newtonsoft.Json;
using System;

public class LootTableEntry
{
	[JsonProperty]
	private RawResource Resource;
	[JsonProperty]
	private int MinAmount;
	[JsonProperty]
	private int MaxAmount;
	[JsonProperty]
	private float Chance;

	[JsonProperty]
	private int? Amount;

    public LootTableEntry()
    {
        Resource = RawResource.RawResources.GOLD;
        MinAmount = 0;
        MaxAmount = 0;
        Chance = 0f;
        Amount = null;
    }

    public LootTableEntry(RawResource resource, int minAmount, int maxAmount, float chance)
    {
        Resource = resource;
        MinAmount = minAmount;
        MaxAmount = maxAmount;
        Chance = chance;
    }

    public LootTableEntry(RawResource resource, (int MinAmount, int MaxAmount) range, float chance)
    {
        Resource = resource;
        MinAmount = range.MinAmount;
        MaxAmount = range.MaxAmount;
        Chance = chance;
    }

    public LootTableEntry(RawResource resource, int amount)
    {
        Resource = resource;
        MinAmount = 0;
        MaxAmount = 0;
        Chance = 0;
        Amount = amount;
    }

    public RawResource GetResource()
    {
        return Resource;
    }

    public int GetMinAmount()
    {
        return MinAmount;
    }

    public int GetMaxAmount()
    {
        return MaxAmount;
    }

    public float GetChance()
    {
        return Chance;
    }

    public void RollResult()
    {
        var random = new System.Random();
        var result = random.NextDouble();

        if (result < Chance)
        {
            Amount = random.Next(MinAmount, MaxAmount + 1);
            return;
        }

        Amount = 0;
    }
      
    public void SetAmount(int amount)
    {
        Amount = amount;
    }

    public int? GetAmount()
    {
        return Amount;
    }
}
