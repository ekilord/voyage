using System.Collections.Generic;
using static Attribute;

public static class EnemyAI
{
    public static int EnemyTurnDecision(CombatantEntity playerEntity, CombatantEntity enemyEntity)
    {
        int bestIndex = int.MinValue;
        float bestScore = float.MinValue;

        for (int i = 0; i < enemyEntity.GetHand().Length; i++)
        {
            float score = EvaluateCard(enemyEntity.GetCardFromHand(i), playerEntity.GetRemainingHealth(), enemyEntity);
            if (score > bestScore)
            {
                bestScore = score;
                bestIndex = i;
            }
        }

        return bestIndex;
    }

    private static float EvaluateCard(Card card, int playerHealth, CombatantEntity enemyEntity)
    {
        float score = 0;

        if (card == null) return float.MinValue;

        if (card.GetCost() > enemyEntity.GetRemainingActions()) return float.MinValue;

        switch (card.GetCardType())
        {
            case CardType.DAMAGE:
                int damage = card.GetDamageAmount() ?? 0;
                score = playerHealth > damage ? damage : playerHealth * 1.2f;
                break;

            case CardType.SUPPORT:
                if (card.GetHealAmount() != null && card.GetHealAmount() > 0)
                    score = (enemyEntity.GetRemainingHealth() < enemyEntity.GetMaxHealthWithModifiers() * 0.5) ? 20 : 5;
                else if (card.GetEffect() != null && card.GetEffect().ContainsAttribute(Attributes.DEFENSE))
                    score = enemyEntity.GetRemainingHealth() < playerHealth ? 15 : 5;
                break;

            case CardType.UTILITY:
                if (card.GetEffect() != null) score = card.GetEffect().GetEffectTarget() == PlayerRelation.PLAYER ? 10 : 5;
                break;
        }

        return score;
    }

    public static AbstractEntity? ChooseNightTimeEnemy()
    {
		if ( PlayerCharacter.GetCurrentBase().GetLocation() != null ) {
			AbstractEntity entity = PlayerCharacter.CheckForEntityNearBase();

			return entity;
        }
        else {
            return Entity.Entities.GHOSTS;
        }
    }
}
