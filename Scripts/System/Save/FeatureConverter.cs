using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FeatureConverter : JsonConverter<AbstractFeature>
{
    public override void WriteJson(JsonWriter writer, AbstractFeature value, JsonSerializer serializer)
    {
        writer.WriteStartObject();

        writer.WritePropertyName("Name");
        writer.WriteValue(value.GetName());

        writer.WritePropertyName("MainEntry");
        var mainEntry = value.GetLootTable()?.GetMainEntry();
        if (mainEntry != null)
        {
            var resourceHolder = new ResourceHolder(mainEntry.GetResource(), mainEntry.GetAmount() == null ? 0 : (int)mainEntry.GetAmount());
            serializer.Serialize(writer, resourceHolder);
        }
        else
        {
            writer.WriteNull();
        }

        writer.WritePropertyName("SecondaryEntries");
        var secondaryEntries = value.GetLootTable()?.GetAllSecondaryEntries();
        if (secondaryEntries != null)
        {
            writer.WriteStartArray();
            foreach (var entry in secondaryEntries)
            {
                var resourceHolder = new ResourceHolder(entry.GetResource(), entry.GetAmount() == null ? 0 : (int)entry.GetAmount());
                serializer.Serialize(writer, resourceHolder);
            }
            writer.WriteEndArray();
        }
        else
        {
            writer.WriteNull();
        }

        writer.WriteEndObject();
    }

    public override AbstractFeature ReadJson(JsonReader reader, Type objectType, AbstractFeature existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
        {
            return null;
        }

        JObject obj = JObject.Load(reader);

        string name = obj["Name"]?.ToString() ?? string.Empty;

        ResourceHolder? mainEntry = null;
        if (obj["MainEntry"] != null && obj["MainEntry"].Type != JTokenType.Null)
        {
            mainEntry = obj["MainEntry"].ToObject<ResourceHolder>(serializer);
        }

        List<ResourceHolder> secondaryEntries = new();
        if (obj["SecondaryEntries"] != null && obj["SecondaryEntries"].Type == JTokenType.Array)
        {
            foreach (var entry in obj["SecondaryEntries"])
            {
                var resourceHolder = entry.ToObject<ResourceHolder>(serializer);
                if (resourceHolder != null)
                {
                    secondaryEntries.Add(resourceHolder);
                }
            }
        }

        if (NaturalFeature.Features.GetNaturalFeatureByName(name) != null)
        {
            return NaturalFeature.Features.CreateWithLootTable(name, mainEntry, secondaryEntries);
        }
        else if (StructureFeature.Features.GetStructureFeatureByName(name) != null)
        {
            return StructureFeature.Features.CreateWithLootTable(name, mainEntry, secondaryEntries);
        }
        else
        {
            throw new JsonSerializationException($"Unknown feature name: {name}");
        }
    }

}
