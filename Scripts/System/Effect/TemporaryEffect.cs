using System.Collections.Generic;
using UnityEngine;

public class TemporaryEffect : Effect
{
	private int Duration;
    private int CurrentDuration;

	public TemporaryEffect( string name, string description, EffectSource source, PlayerRelation target, EffectType type, List<AttributeHolder> modifiers, int duration ) : base( name, description, source, target, type, modifiers )
	{
		Duration = duration;
		CurrentDuration = duration;
	}

	public TemporaryEffect( string name, string description, EffectSource source, PlayerRelation target, EffectType type, AttributeHolder modifier, int duration ) : base( name, description, source, target, type, modifier )
	{
		Duration = duration;
        CurrentDuration = duration;
    }

	public bool DecreaseDuration()
	{
		if (CurrentDuration > 0)
		{
			CurrentDuration--;
			return false;
		}
		else
		{
			CurrentDuration = Duration;
			return true;
		}
	}
}
