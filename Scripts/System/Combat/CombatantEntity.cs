using System;
using System.Collections.Generic;
using UnityEngine;
using static Attribute;
using static Card;

public class CombatantEntity : Entity
{
    private int MaxHealth;
    private int MaxActions;
    private int RemainingHealth;
    private int RemainingActions;

    private Card[] Hand;

    public CombatantEntity(Entity entity) : base(entity.GetName(), entity.GetPlayerRelation(), entity.GetColor(), entity.GetStats(), entity.GetDeck())
    {
        int maxHealth = GetMaxHealthWithModifiers();
        int maxActions = GetMaxActionsWithModifiers();

        MaxHealth = maxHealth;
        RemainingHealth = maxHealth;
        MaxActions = maxActions;
        RemainingActions = maxActions;

        Hand = new Card[5];
    }

    public CombatantEntity(string name, PlayerRelation relation, Color color, Stats stats, HashSet<Deck> decks) : base(name, relation, color, stats, Deck.Decks.MergeDecks(decks))
    {
        int maxHealth = GetMaxHealthWithModifiers();
        int maxActions = GetMaxActionsWithModifiers();

        MaxHealth = maxHealth;
        RemainingHealth = maxHealth;
        MaxActions = maxActions;
        RemainingActions = maxActions;

        Hand = new Card[5];
    }

    public int GetRemainingHealth()
    {
        return RemainingHealth;
    }

    public int GetRemainingActions()
    {
        return RemainingActions;
    }

    public int GetMaxHealthWithModifiers()
    {
        return (int)Math.Round(Stats.GetBaseHealth().GetValue() * (1 + EffectManager.GetAllModifiersFor(Attributes.HEALTH, PlayerRelation).GetValue()));
    }

    public int GetMaxActionsWithModifiers()
    {
        return (int)Math.Round(Stats.GetBaseActions().GetValue() * (1 + EffectManager.GetAllModifiersFor(Attributes.ACTIONS, PlayerRelation).GetValue()));
    }

    public void UpdateMaxStats()
    {
        int newMaxHealth = GetMaxHealthWithModifiers();
        int newMaxActions = GetMaxActionsWithModifiers();

        if (newMaxHealth > MaxHealth)
        {
            RemainingHealth += (newMaxHealth - MaxHealth);
            MaxHealth = newMaxHealth;
        }

        if (newMaxActions > MaxActions)
        {
            RemainingActions += (newMaxActions - MaxActions);
            MaxActions = newMaxActions;
        }
    }

    public void ResetHealth()
    {
        RemainingHealth = MaxHealth;
    }

    public void IncreaseHealth(int amount)
    {
        if (RemainingHealth + amount > MaxHealth)
        {
            RemainingHealth = MaxHealth;
        }
        else
            RemainingHealth += amount;
    }

    public bool DecreaseHealth(int amount)
    {
        if (RemainingHealth - amount <= 0)
        {
            Debug.Log("LETHAL");
            RemainingHealth = 0;
            return true;
        }
        RemainingHealth -= amount;
        return false;
    }

    public void ResetActions()
    {
        RemainingActions = MaxActions;
    }

    public void IncreaseActions(int amount)
    {
        if (RemainingActions + amount > MaxActions)
        {
            RemainingActions = MaxActions;
        }
        else
            RemainingActions += amount;
    }

    public bool DecreaseActions(int amount)
    {
        if (RemainingActions - amount < 0)
            return false;
        RemainingActions -= amount;
        return true;
    }

    public bool UseAction()
    {
        if (RemainingActions >= 1)
        {
            RemainingActions--;
            return true;
        }
        return false;
    }

    public Card[] GetHand()
    {
        return Hand;
    }

    public List<int> DrawCards()
    {
        List<int> indexes = new();

        for (int i = 0; i < Hand.Length; i++)
        {
            if (Hand[i] == null)
            {
                Hand[i] = Deck.DrawCard();
                indexes.Add(i);
            }
        }

        return indexes;
    }

    public Card GetCardFromHand(int index)
    {
        return Hand[index];
    }

    public bool PlayCard(int index)
    {
        return DecreaseActions(Hand[index].GetCost());
    }

    public void RemoveCard(int index)
    {
        Hand[index] = null;
    }
}
