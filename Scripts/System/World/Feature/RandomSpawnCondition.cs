using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawnCondition : SpawnCondition
{
    [JsonProperty]
    private (int MinAmount, int MaxAmount) Amounts;
	[JsonProperty]
	protected int MinDistance;

    public RandomSpawnCondition() : base()
    {
        Amounts = (0, 0);
        MinDistance = 0;
    }

    public RandomSpawnCondition(List<Biome> biome, (int, int) amounts, int minDistance) : base(biome)
    {
        Amounts = amounts;
        MinDistance = minDistance;
    }

    public (int MinAmount, int MaxAmount) GetAmounts()
    {
        return Amounts;
    }

    public int GetMinDistance()
    {
        return MinDistance;
    }
}
