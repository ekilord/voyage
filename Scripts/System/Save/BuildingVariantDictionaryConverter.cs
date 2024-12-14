using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public class BuildingVariantDictionaryConverter : JsonConverter<Dictionary<BuildingVariant, bool>>
{
    public override void WriteJson(JsonWriter writer, Dictionary<BuildingVariant, bool> value, JsonSerializer serializer)
    {
        writer.WriteStartObject();

        foreach (var kvp in value)
        {
            writer.WritePropertyName(kvp.Key.GetName()); // Use the BuildingVariant name
            writer.WriteValue(kvp.Value);              // Write the boolean value
        }

        writer.WriteEndObject();
    }

    public override Dictionary<BuildingVariant, bool> ReadJson(JsonReader reader, Type objectType, Dictionary<BuildingVariant, bool>? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new Dictionary<BuildingVariant, bool>();

        if (reader.TokenType == JsonToken.StartObject)
        {
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.EndObject)
                    break;

                if (reader.TokenType == JsonToken.PropertyName)
                {
                    string name = reader.Value.ToString();
                    reader.Read();

                    bool value = reader.TokenType == JsonToken.Boolean && (bool)reader.Value;

                    // Use GetBuildingVariantByName to fetch the BuildingVariant by name
                    var variant = BuildingVariant.BuildingVariants.GetBuildingVariantByName(name);
                    result[variant] = value;
                }
            }
        }

        return result;
    }
}
