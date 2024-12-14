using System.Collections;
using System.Collections.Generic;
using static Attribute;

[System.Serializable]
public class ConditionalEffect : Effect
{
    public ConditionalEffect() : base()
    { 
    }

    protected ConditionalEffect(string name, string description, EffectSource source, PlayerRelation target, EffectType type, List<AttributeHolder> modifiers) : base(name, description, source, target, type, modifiers)
    {
    }

    protected ConditionalEffect(string name, string description, EffectSource source, PlayerRelation target, EffectType type, AttributeHolder modifier) : base(name, description, source, target, type, modifier)
    {
    }

    public static class Environmental
    {
        public static readonly ConditionalEffect LUSH_VEGETATION = new("Lush Vegetation", "", EffectSource.ENVIRONMENTAL, PlayerRelation.NONE, EffectType.CONDITIONAL, new List<AttributeHolder>() {
            new(Attributes.ACCURACY, PENALTY.Medium),
            new(Attributes.INITIATIVE, PENALTY.Small)
        });

        public static readonly ConditionalEffect DENSE_VEGETATION = new("Dense Vegetation", "", EffectSource.ENVIRONMENTAL, PlayerRelation.NONE, EffectType.CONDITIONAL, new List<AttributeHolder>() {
            new(Attributes.ACCURACY, PENALTY.Medium),
            new(Attributes.INITIATIVE, PENALTY.Large)
        });

        public static readonly ConditionalEffect MYSTICAL_FOG = new("Mystical Fog", "", EffectSource.ENVIRONMENTAL, PlayerRelation.NONE, EffectType.CONDITIONAL, new List<AttributeHolder>() {
            new(Attributes.ARCANE_POWER, BOON.Medium),
            new(Attributes.ACCURACY, PENALTY.Medium),
            new(Attributes.INITIATIVE, PENALTY.Medium)
        });

        public static readonly ConditionalEffect CURSED_GROUND = new("Cursed Ground", "", EffectSource.ENVIRONMENTAL, PlayerRelation.NONE, EffectType.CONDITIONAL, new List<AttributeHolder>() {
            new(Attributes.DEFENSE, PENALTY.Medium),
            new(Attributes.PHYSICAL_POWER, PENALTY.Medium),
            new(Attributes.MORALE, PENALTY.Medium),
            new(Attributes.SANITY, PENALTY.Small)
        });

        public static readonly ConditionalEffect SACRED_GROUND = new("Sacred Ground", "", EffectSource.ENVIRONMENTAL, PlayerRelation.NONE, EffectType.CONDITIONAL, new List<AttributeHolder>() {
            new(Attributes.DEFENSE, BOON.Medium),
            new(Attributes.PHYSICAL_POWER, BOON.Medium),
            new(Attributes.MORALE, BOON.Large),
            new(Attributes.SANITY, BOON.Medium)
        });

        public static readonly ConditionalEffect LEY_LINES = new("Ley Lines", "", EffectSource.ENVIRONMENTAL, PlayerRelation.NONE, EffectType.CONDITIONAL, new List<AttributeHolder>() {
            new(Attributes.ARCANE_POWER, BOON.Large),
            new(Attributes.SANITY, PENALTY.ExtraSmall)
        });
    }

    public static class Weather
    {
        public static readonly ConditionalEffect FOG = new("Fog", "", EffectSource.WEATHER, PlayerRelation.NONE, EffectType.CONDITIONAL, new List<AttributeHolder>() {
            new(Attributes.ACCURACY, PENALTY.ExtraLarge),
            new(Attributes.INITIATIVE, PENALTY.ExtraLarge)
        });

        public static readonly ConditionalEffect BONE_CHILLING_WIND = new("Bone-Chilling Wind", "", EffectSource.WEATHER, PlayerRelation.NONE, EffectType.CONDITIONAL, new List<AttributeHolder>() {
            new(Attributes.DEFENSE, PENALTY.Large),
            new(Attributes.INITIATIVE, PENALTY.Medium)
        });

        public static readonly ConditionalEffect BLIZZARD = new("Blizzard", "", EffectSource.WEATHER, PlayerRelation.NONE, EffectType.CONDITIONAL, new List<AttributeHolder>() {
            new(Attributes.ACCURACY, PENALTY.Large),
            new(Attributes.INITIATIVE, PENALTY.Medium),
            new(Attributes.DEFENSE, PENALTY.Medium),
            new(Attributes.MORALE, PENALTY.Large)
        });
    }
}
