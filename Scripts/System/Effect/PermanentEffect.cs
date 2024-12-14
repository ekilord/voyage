using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PermanentEffect : Effect
{
    public PermanentEffect() : base()
    {
    }

    public PermanentEffect(string name, string description, EffectSource source, PlayerRelation target, EffectType type, List<AttributeHolder> modifiers) : base(name, description, source, target, type, modifiers)
    {
    }

    public PermanentEffect(string name, string description, EffectSource source, PlayerRelation target, EffectType type, AttributeHolder modifier) : base(name, description, source, target, type, modifier)
    {
    }
}
