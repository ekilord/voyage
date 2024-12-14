using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConditionalSpawnCondition : SpawnCondition
{
    public ConditionalSpawnCondition() : base()
    {
    }

    public ConditionalSpawnCondition(List<Biome> biome) : base(biome)
    {
    }
}
