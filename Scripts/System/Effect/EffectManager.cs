using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class EffectManager
{
    private static readonly List<Effect> SelfEffects = new();
    private static readonly List<Effect> FriendlyEffects = new();
    private static readonly List<Effect> NeutralEffects = new();
    private static readonly List<Effect> HostileEffects = new();
    private static readonly List<Effect> GeneralEffects = new();

    public static void AddEffect(Effect effect)
    {
        Debug.Log("Effect added");
        switch (effect.GetEffectTarget())
        {
            case PlayerRelation.PLAYER:
                SelfEffects.Add(effect);
                break;
            case PlayerRelation.FRIENDLY:
                FriendlyEffects.Add(effect);
                break;
            case PlayerRelation.NEUTRAL:
                NeutralEffects.Add(effect);
                break;
            case PlayerRelation.HOSTILE:
                HostileEffects.Add(effect);
                break;
            case PlayerRelation.NONE:
                GeneralEffects.Add(effect);
                break;
            default:
                throw new ArgumentException("Effect's target is not defined");
        }
    }

    public static void RemoveEffect(Effect effect)
    {
        switch (effect.GetEffectTarget())
        {
            case PlayerRelation.PLAYER:
                SelfEffects.Remove(effect);
                break;
            case PlayerRelation.FRIENDLY:
                FriendlyEffects.Remove(effect);
                break;
            case PlayerRelation.NEUTRAL:
                NeutralEffects.Remove(effect);
                break;
            case PlayerRelation.HOSTILE:
                HostileEffects.Remove(effect);
                break;
            case PlayerRelation.NONE:
                GeneralEffects.Remove(effect);
                break;
            default:
                throw new ArgumentException("Effect's target is not defined");
        }
    }

    public static AttributeHolder GetAllModifiersFor(Attribute attribute, PlayerRelation target)
    {
        float modifierAmount = 0f;

        switch (target)
        {
            case PlayerRelation.PLAYER:
                foreach (var effect in SelfEffects) modifierAmount += effect.GetAttributeValue(attribute);
                break;
            case PlayerRelation.FRIENDLY:
                foreach (var effect in FriendlyEffects) modifierAmount += effect.GetAttributeValue(attribute);
                break;
            case PlayerRelation.NEUTRAL:
                foreach (var effect in NeutralEffects) modifierAmount += effect.GetAttributeValue(attribute);
                break;
            case PlayerRelation.HOSTILE:
                foreach (var effect in HostileEffects) modifierAmount += effect.GetAttributeValue(attribute);
                break;
        }

        return new AttributeHolder(attribute, modifierAmount);
    }

    public static void DecreaseEffectDurations()
    {
        List<List<Effect>> effects = new()
        {
            SelfEffects,
            FriendlyEffects,
            NeutralEffects,
            HostileEffects,
            GeneralEffects
        };

        for (int i = 0; i < effects.Count; i++)
        {
            for (int k = effects[i].Count - 1; k >= 0; k--)
            {
                Effect effect = effects[i][k];
                if (effect is TemporaryEffect temporaryEffect)
                {
                    if (temporaryEffect.DecreaseDuration())
                    {
                        RemoveEffect(effect);
                    }
                }
            }
        }
    }
}
