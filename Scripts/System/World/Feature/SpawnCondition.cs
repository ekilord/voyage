using Newtonsoft.Json;
using System.Collections.Generic;

[System.Serializable]
public abstract class SpawnCondition 
{
    [JsonProperty]
    protected List<Biome> Biomes;

    public SpawnCondition()
    {
        Biomes = new List<Biome>();
    }

    protected SpawnCondition(List<Biome> biome)
    {
        Biomes = biome;
    }

    public List<Biome> GetBiomes()
    {
        return Biomes;
    }
}
