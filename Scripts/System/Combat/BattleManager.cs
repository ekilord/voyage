using System.Collections.Generic;
using UnityEngine;

public enum Turns
{
    PLAYER,
    ENEMY
}

public enum DamageType
{
    PHYSICAL,
    ARCANE
}

public class BattleManager
{
    private CombatantEntity PlayerEntity;
    private CombatantEntity EnemyEntity;

    private Turns CurrentTurn;

    private bool HasEnded;

    public BattleManager(Entity enemyEntity)
    {
        PlayerEntity = new CombatantEntity(PlayerCharacter.GetName(), PlayerRelation.PLAYER, Color.green, PlayerCharacter.GetStats(), PlayerCharacter.GetDecks());
        EnemyEntity = new CombatantEntity(enemyEntity);
        ShuffleDecks();

        if (PlayerEntity.GetInitiative().GetValue() >= EnemyEntity.GetInitiative().GetValue())
        {
            CurrentTurn = Turns.PLAYER;
        }
        else
        {
            CurrentTurn = Turns.ENEMY;
        }
    }

    public CombatantEntity GetPlayerEntity()
    {
        return PlayerEntity;
    }

    public CombatantEntity GetEnemyEntity()
    {
        return EnemyEntity;
    }

    public Turns GetCurrentTurn()
    {
        return CurrentTurn;
    }

    public CombatantEntity GetCurrentEntity()
    {
        return IsPlayerTurn() ? PlayerEntity : EnemyEntity;
    }

    public CombatantEntity GetOppositeEntity()
    {
        return IsEnemyTurn() ? PlayerEntity : EnemyEntity;
    }

    public bool IsPlayerTurn()
    {
        return CurrentTurn == Turns.PLAYER;
    }

    public bool IsEnemyTurn()
    {
        return CurrentTurn == Turns.ENEMY;
    }

    private (bool successful, bool lethal) DealDamage(int amount, DamageType damageType)
    {
        var random = new System.Random();
        var rng = random.NextDouble();
        CombatantEntity entity = GetCurrentEntity();

        if (entity.GetAccuracy().GetValue() > rng)
        {
            bool lethal = false;

            if (damageType == DamageType.PHYSICAL)
            {
                float damageMultiplier = entity.GetPhysicalPower().GetValue();

                int calculatedDamage = (int)Mathf.Round(amount + amount * damageMultiplier);

                lethal = ReceiveDamage(calculatedDamage);
            }
            else
            {
                float damageMultiplier = entity.GetArcanePower().GetValue();

                int calculatedDamage = (int)Mathf.Round(amount + amount * damageMultiplier);

                lethal = ReceiveDamage(calculatedDamage);
            }

            return (true, lethal);
        }

        return (false, false);
    }

    public bool ReceiveDamage(int amount)
    {
        CombatantEntity opposite = GetOppositeEntity();

        float defense = opposite.GetDefense().GetValue();
        int calculatedDamage = (int)Mathf.Round(amount - amount * defense);
        return opposite.DecreaseHealth(calculatedDamage);
    }

    public void HealHP(int amount)
    {
        CombatantEntity entity = GetCurrentEntity();

        float arcanePower = entity.GetArcanePowerValue();
        int calculatedHeal = (int)Mathf.Round(amount + amount * arcanePower);
        entity.IncreaseHealth(calculatedHeal);
    }

    public (bool canPlay, bool successful, bool lethal) PlayCard(int index)
    {
        CombatantEntity entity = GetCurrentEntity();

        if (entity.PlayCard(index))
        {
            (bool successful, bool lethal) = UseCard(entity.GetCardFromHand(index));
            entity.RemoveCard(index);
            Debug.Log($"S: {successful}, L: {lethal}");
            return (true, successful, lethal);
        }

        return (false, false, false);
    }

    private (bool successful, bool lethal) UseCard(Card card)
    {
        switch (card.GetCardType())
        {
            case CardType.DAMAGE:
                (bool successful1, bool lethal1) = DealDamage((int)card.GetDamageAmount(), (DamageType)card.GetDamageType());
                return (successful1, lethal1);
            case CardType.SUPPORT:
                if (card.GetEffect() != null)
                {
                    EffectManager.AddEffect(card.GetEffect());
                }
                if (card.GetHealAmount() != null)
                {
                    HealHP((int)card.GetHealAmount());
                }
                if (card.GetDamageAmount() != null && card.GetDamageType() != null)
                {
                    (bool successful2, bool lethal2) = DealDamage((int)card.GetDamageAmount(), (DamageType)card.GetDamageType());
                    return (successful2, lethal2);
                }
                return (true, false);
            case CardType.UTILITY:
                if (card.GetEffect() != null)
                {
                    EffectManager.AddEffect(card.GetEffect());
                }
                if (card.GetHealAmount() != null)
                {
                    HealHP((int)card.GetHealAmount());
                }
                if (card.GetDamageAmount() != null && card.GetDamageType() != null)
                {
                    (bool successful3, bool lethal3) = DealDamage((int)card.GetDamageAmount(), (DamageType)card.GetDamageType());
                    return (successful3, lethal3);
                }
                return (true, false);
        }

        return (false, false);
    }
    private void ShuffleDecks()
    {
        PlayerEntity.GetDeck().Shuffle();
        EnemyEntity.GetDeck().Shuffle();
    }

    public (List<int> PlayerHand, List<int> EnemyHand) StartBattle()
    {
        List<int> playerHand = PlayerEntity.DrawCards();
        List<int> enemyHand = EnemyEntity.DrawCards();
        return (playerHand, enemyHand);
    }

    public List<int> ChangeTurn()
    {
        NextTurn();
        EffectManager.DecreaseEffectDurations();
        return StartTurn();
    }

    private List<int> StartTurn()
    {
        CombatantEntity entity = GetCurrentEntity();

        entity.ResetActions();
        return entity.DrawCards();
    }

    public void NextTurn()
    {
        if (IsPlayerTurn())
        {
            CurrentTurn = Turns.ENEMY;
        }
        else
        {
            CurrentTurn = Turns.PLAYER;
        }
    }

    public int EnemyMakeDecision()
    {
        return EnemyAI.EnemyTurnDecision(PlayerEntity, EnemyEntity);
    }
}
