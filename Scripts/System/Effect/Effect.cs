using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public enum EffectSource
{
	ENVIRONMENTAL,
	WEATHER,
	NEUTRAL,
	PLAYER,
	ENEMY
}

public enum EffectType
{
	TEMPORARY,
	CONDITIONAL,
	PERMANENT
}

public class Effect
{
    [JsonProperty]
    protected string Name;
    protected string Description;
	protected EffectSource Source;
	protected PlayerRelation Target;
	protected EffectType Type;
	protected List<AttributeHolder> Modifiers;

	public Effect()
	{
		Name = "";
		Description = "";
		Source = EffectSource.PLAYER;
		Target = PlayerRelation.PLAYER;
		Type = EffectType.TEMPORARY;
		Modifiers = new List<AttributeHolder>();
	}

	protected Effect( string name, string description, EffectSource source, PlayerRelation target, EffectType type, List<AttributeHolder> modifiers )
	{
		Name = name;
		Description = description;
		Source = source;
		Target = target;
		Type = type;
		Modifiers = modifiers;
	}

	protected Effect( string name, string description, EffectSource source, PlayerRelation target, EffectType type, AttributeHolder modifier )
	{
		Name = name;
		Description = description;
		Source = source;
		Target = target;
		Type = type;
		Modifiers = new List<AttributeHolder>() { modifier };
	}

	public string GetName()
	{
		return Name;
	}

    public string GetDescription()
    {
        return Description;
    }

    public EffectSource GetEffectSource()
    {
        return Source;
    }

    public PlayerRelation GetEffectTarget()
    {
        return Target;
    }

    public EffectType GetEffectType()
	{
		return Type;
	}

	public List<AttributeHolder> GetModifiers()
	{
		return Modifiers;
	}

	public bool ContainsAttribute(Attribute attribute)
	{
		foreach (AttributeHolder modifier in Modifiers) if (modifier.GetAttribute().Equals(attribute)) return true;
		return false;
    }

	public float GetAttributeValue(Attribute attribute)
	{
        foreach (AttributeHolder modifier in Modifiers) if (modifier.GetAttribute().Equals(attribute)) return modifier.GetValue();
		return 0f;
    }

	protected readonly static (float ExtraSmall, float Small, float Medium, float Large, float ExtraLarge) PENALTY =
		(-0.025f, -0.05f, -0.1f, -0.15f, -0.2f);

	protected readonly static (float ExtraSmall, float Small, float Medium, float Large, float ExtraLarge) BOON =
		(0.025f, 0.05f, 0.1f, 0.15f, 0.2f);
}
