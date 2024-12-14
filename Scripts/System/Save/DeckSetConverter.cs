using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public class DeckSetConverter : JsonConverter<HashSet<Deck>>
{
    public override void WriteJson(JsonWriter writer, HashSet<Deck> value, JsonSerializer serializer)
    {
        writer.WriteStartArray();

        foreach (var deck in value)
        {
            writer.WriteValue(deck.GetName());
        }

        writer.WriteEndArray();
    }

    public override HashSet<Deck> ReadJson(JsonReader reader, Type objectType, HashSet<Deck>? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new HashSet<Deck>();

        if (reader.TokenType == JsonToken.StartArray)
        {
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.EndArray)
                    break;

                if (reader.TokenType == JsonToken.String)
                {
                    string name = reader.Value.ToString();

                    var deck = Deck.Decks.GetDeckByName(name);
                    if (deck != null)
                    {
                        result.Add(deck);
                    }
                }
            }
        }

        return result;
    }
}
