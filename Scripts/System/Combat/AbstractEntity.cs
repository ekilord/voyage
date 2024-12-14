using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class AbstractEntity
{
	[JsonProperty]
	protected string Name;
	[JsonProperty]
	protected PlayerRelation PlayerRelation;
	[JsonProperty]
    protected string ColorHex;

    public AbstractEntity()
    {
        Name = "";
        PlayerRelation = PlayerRelation.NONE;
        ColorHex = "#ffffff";
    }

    public AbstractEntity(string name, PlayerRelation playerRelation, Color color)
    {
        Name = name;
        PlayerRelation = playerRelation;
        ColorHex = ColorUtility.ToHtmlStringRGBA(color);
    }

    public string GetName()
    {
        return Name;
    }

    public PlayerRelation GetPlayerRelation()
    {
        return PlayerRelation;
    }

    public Color GetColor()
    {
        ColorUtility.TryParseHtmlString($"#{ColorHex}", out Color color);
        return color;
    }
}
