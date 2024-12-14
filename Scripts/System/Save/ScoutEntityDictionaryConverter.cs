using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public class ScoutEntityDictionaryConverter : JsonConverter<Dictionary<ScoutEntity, int>>
{
    public override void WriteJson(JsonWriter writer, Dictionary<ScoutEntity, int> value, JsonSerializer serializer)
    {
        writer.WriteStartObject();

        foreach (var kvp in value)
        {
            writer.WritePropertyName(kvp.Key.GetName());
            writer.WriteValue(kvp.Value);
        }

        writer.WriteEndObject();
    }

    public override Dictionary<ScoutEntity, int> ReadJson(JsonReader reader, Type objectType, Dictionary<ScoutEntity, int>? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new Dictionary<ScoutEntity, int>();

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

                    if (reader.TokenType == JsonToken.Integer)
                    {
                        int value = Convert.ToInt32(reader.Value);

                        var scout = ScoutEntity.Scouts.GetScoutEntityByName(name);
                        if (scout != null)
                        {
                            result[scout] = value;
                        }
                    }
                }
            }
        }

        return result;
    }
}
