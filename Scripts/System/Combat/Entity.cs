using System;
using System.Collections.Generic;
using UnityEngine;
using static Attribute;

public class Entity : AbstractEntity
{
	protected Stats Stats;
	protected Deck Deck;

    public Entity() : base()
    {
        Stats = Stats.CreateDefaultStats();
        Deck = Deck.Decks.BASE;
    }

	public Entity(string name, PlayerRelation playerRelation, Color color, Stats stats, Deck deck ) : base(name, playerRelation, color)
	{
		Name = name;
        PlayerRelation = playerRelation;
        Stats = stats;
		Deck = deck;
	}

    public Stats GetStats()
    {
        return Stats;
    }

    public Deck GetDeck()
    {
        return Deck;
    }

	public AttributeHolder GetHealth()
	{
        return new AttributeHolder( Attributes.HEALTH, Mathf.Round(Stats.GetBaseHealth().GetValue() * (1 + EffectManager.GetAllModifiersFor(Attributes.HEALTH, PlayerRelation).GetValue())));
    }

    public AttributeHolder GetActions()
    {
        return new AttributeHolder(Attributes.ACTIONS, Mathf.Round(Stats.GetBaseActions().GetValue() * (1 + EffectManager.GetAllModifiersFor(Attributes.ACTIONS, PlayerRelation).GetValue())));
    }

    public AttributeHolder GetPhysicalPower()
    {
        return new AttributeHolder(Attributes.PHYSICAL_POWER, Stats.GetBasePhysicalPower().GetValue() * (1 + EffectManager.GetAllModifiersFor(Attributes.PHYSICAL_POWER, PlayerRelation).GetValue()));
    }

    public AttributeHolder GetArcanePower()
    {
        return new AttributeHolder(Attributes.ARCANE_POWER, Stats.GetBaseArcanePower().GetValue() * (1 + EffectManager.GetAllModifiersFor(Attributes.ARCANE_POWER, PlayerRelation).GetValue()));
    }

    public AttributeHolder GetDefense()
    {
        return new AttributeHolder(Attributes.DEFENSE, Stats.GetBaseDefense().GetValue() * (1 + EffectManager.GetAllModifiersFor(Attributes.DEFENSE, PlayerRelation).GetValue()));
    }

    public AttributeHolder GetInitiative()
    {
        return new AttributeHolder(Attributes.INITIATIVE, Stats.GetBaseInitiative().GetValue() * (1 + EffectManager.GetAllModifiersFor(Attributes.INITIATIVE, PlayerRelation).GetValue()));
    }

    public AttributeHolder GetMorale()
    {
        return new AttributeHolder(Attributes.MORALE, Stats.GetBaseMorale().GetValue() * (1 + EffectManager.GetAllModifiersFor(Attributes.MORALE, PlayerRelation).GetValue()));
    }

    public AttributeHolder GetAccuracy()
    {
        return new AttributeHolder(Attributes.ACCURACY, Stats.GetBaseAccuracy().GetValue() * (1 + EffectManager.GetAllModifiersFor(Attributes.ACCURACY, PlayerRelation).GetValue()));
    }

    public AttributeHolder GetSanity()
    {
        return new AttributeHolder(Attributes.SANITY, Stats.GetBaseSanity().GetValue() * (1 + EffectManager.GetAllModifiersFor(Attributes.SANITY, PlayerRelation).GetValue()));
    }

    public int GetHealthValue()
    {
        return (int)Math.Round(GetHealth().GetValue());
    }

    public int GetActionsValue()
    {
        return (int)Math.Round(GetActions().GetValue());
    }

    public float GetPhysicalPowerValue()
    {
        return GetPhysicalPower().GetValue();
    }

    public float GetArcanePowerValue()
    {
        return GetArcanePower().GetValue();
    }

    public float GetDefenseValue()
    {
        return GetDefense().GetValue();
    }

    public float GetInitiativeValue()
    {
        return GetInitiative().GetValue();
    }

    public float GetMoraleValue()
    {
        return GetMorale().GetValue();
    }

    public float GetAccuracyValue()
    {
        return GetAccuracy().GetValue();
    }

    public float GetSanityValue()
    {
        return GetSanity().GetValue();
    }

    public Entity Copy()
    {
        return new Entity(GetName(), GetPlayerRelation(), GetColor(), GetStats(), GetDeck());
    }

    public static class Entities
    {
		public static Entity GHOULS = new( "Ghouls", PlayerRelation.HOSTILE, UIUtils.Colors.DarkRed, Stats.CreateGhoulsStats(), Deck.Decks.GHOULS );
		public static Entity GOBLINS = new( "Goblins", PlayerRelation.HOSTILE, UIUtils.Colors.Crimson, Stats.CreateGoblinsStats(), Deck.Decks.GOBLINS );
		public static Entity SKELETONS = new( "Skeletons", PlayerRelation.HOSTILE, UIUtils.Colors.DeepRed, Stats.CreateSkeletonsStats(), Deck.Decks.SKELETONS );
		public static Entity GHOSTS = new( "Ghosts", PlayerRelation.HOSTILE, UIUtils.Colors.LightRed, Stats.CreateGhostsStats(), Deck.Decks.GHOSTS );
		public static Entity WOLF_PACK = new( "Wolf Pack", PlayerRelation.HOSTILE, UIUtils.Colors.BloodRed, Stats.CreateWolfPackStats(), Deck.Decks.WOLF_PACK );
		public static Entity FEROCIOUS_BEAR = new( "Ferocious Bear", PlayerRelation.HOSTILE, UIUtils.Colors.Scarlet, Stats.CreateFerociousBearStats(), Deck.Decks.FEROCIOUS_BEAR );

		public static Entity? GetEntityByName( string name )
		{
			foreach ( Entity feature in GetAllEntities() ) {
				if ( feature.GetName().Equals( name ) )
					return feature;
			}
			return null;
		}

        public static List<AbstractEntity> GetAllEntities()
        {
            return new List<AbstractEntity>() { 
                GHOULS,
                GOBLINS,
                SKELETONS,
                GHOSTS,
                WOLF_PACK,
                FEROCIOUS_BEAR,
			};

        }

		public static Entity GetEnemyEntityBasedOnStrength( int strength )
        {
			return strength switch {
				1 => GHOULS,
				2 => GOBLINS,
				3 => SKELETONS,
				4 => GHOSTS,
				5 => WOLF_PACK,
				6 => FEROCIOUS_BEAR,
				_ => GHOULS,
			};
		}
	}
}
