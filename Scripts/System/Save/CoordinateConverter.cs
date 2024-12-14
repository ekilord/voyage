using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using Unity.VisualScripting;

public class CoordinateConverter : JsonConverter<Coordinate>
{
    public override void WriteJson(JsonWriter writer, Coordinate value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }


        writer.WriteValue(value.GetX() + ";" + value.GetY());

    }

    public override Coordinate ReadJson(JsonReader reader, Type objectType, Coordinate existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
        {
            return null;
        }

        if (reader.TokenType != JsonToken.String)
        {
            throw new JsonSerializationException($"Unexpected token type: {reader.TokenType}");
        }

        var coordinateString = reader.Value.ToString();

        var parts = coordinateString.Split(';');
        if (parts.Length != 2)
        {
            throw new JsonSerializationException($"Invalid coordinate format: {coordinateString}");
        }

        if (!int.TryParse(parts[0], out int x))
        {
            throw new JsonSerializationException($"Invalid X value: {parts[0]}");
        }

        if (!int.TryParse(parts[1], out int y))
        {
            throw new JsonSerializationException($"Invalid Y value: {parts[1]}");
        }

        return new Coordinate(x, y);
    }
}