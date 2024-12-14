using Newtonsoft.Json;
using System;
using UnityEngine;

public class AbstractEntityConverter : JsonConverter<AbstractEntity>
{
    public override void WriteJson(JsonWriter writer, AbstractEntity value, JsonSerializer serializer)
    {
        string code = "";

        if (value.GetName().Contains("Encampment"))
        {
            if (((BaseEntity)value).GetLocation() != null) {
                code += $"Base;{((BaseEntity)value).IsOld()};{((BaseEntity)value).GetLocation().GetX()},{((BaseEntity)value).GetLocation().GetY()};";
            }
            else code += $"Base;{((BaseEntity)value).IsOld()};-1,-1;";

        }

        if (value.GetPlayerRelation() == PlayerRelation.PLAYER)
        {
            code += "Scout;";
        }
        else
        {
            code += "Entity;";
        }

        code += value.GetName();

        writer.WriteValue(code);
    }

    public override AbstractEntity ReadJson(JsonReader reader, Type objectType, AbstractEntity existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
        {
            return null;
        }

        if (reader.TokenType != JsonToken.String)
        {
            throw new JsonSerializationException($"Unexpected token type: {reader.TokenType}");
        }

        var entityString = reader.Value.ToString();

        var parts = entityString.Split(';');
        if (parts.Length < 2)
        {
            throw new JsonSerializationException($"Invalid format: {entityString}");
        }

        return parts[0] switch
        {
            "Entity" => Entity.Entities.GetEntityByName(parts[1]),
            "Scout" => ScoutEntity.Scouts.GetScoutEntityByName(parts[1]),
            "Base" => CreateFromScratch(parts),
            _ => throw new JsonSerializationException("Unexpected token type when deserializing Entity."),
        };
    }

    private BaseEntity CreateFromScratch(string[] scratch) 
    {
        bool isOld = bool.Parse(scratch[1]);

        string[] coordinateString = scratch[2].Split(',');
        Coordinate coords = new(int.Parse(coordinateString[0]), int.Parse(coordinateString[1]));

        return new BaseEntity(isOld, coords.GetX() == -1 ? null : coords);
    }
}
