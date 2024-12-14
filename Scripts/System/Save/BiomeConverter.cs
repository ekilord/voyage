using Newtonsoft.Json;
using System;
using UnityEngine;

public class BiomeConverter : JsonConverter<Biome>
{
    public override void WriteJson(JsonWriter writer, Biome value, JsonSerializer serializer)
    {
        writer.WriteValue(value.GetName());
    }

    public override Biome ReadJson(JsonReader reader, Type objectType, Biome existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.String)
        {
            string biomeName = (string)reader.Value;     
            return Biome.Biomes.GetBiomeByName(biomeName);
        }

        throw new JsonSerializationException("Unexpected token type when deserializing Biome.");
    }
}
