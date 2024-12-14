using Newtonsoft.Json;
using System;
using UnityEngine;

public class ResourceHolderConverter : JsonConverter<ResourceHolder>
{
    public override void WriteJson(JsonWriter writer, ResourceHolder value, JsonSerializer serializer)
    {
        writer.WriteStartObject();

        writer.WritePropertyName("Resource");
        writer.WriteValue(value.GetResourceType().GetResourceType());

        writer.WritePropertyName("Amount");
        writer.WriteValue(value.GetAmount());

        writer.WriteEndObject();
    }

    public override ResourceHolder ReadJson(JsonReader reader, Type objectType, ResourceHolder existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        string resourceName = string.Empty;
        int amount = 0;

        while (reader.Read())
        {
            if (reader.TokenType == JsonToken.EndObject)
                break;

            if (reader.TokenType == JsonToken.PropertyName)
            {
                string propertyName = reader.Value.ToString();
                reader.Read();

                switch (propertyName)
                {
                    case "Resource":
                        resourceName = reader.Value.ToString();
                        break;
                    case "Amount":
                        amount = Convert.ToInt32(reader.Value);
                        break;
                }
            }
        }

        RawResource resource = RawResource.RawResources.GetResourceFromName(resourceName);

        return new ResourceHolder(resource, amount);
    }
}
