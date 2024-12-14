using UnityEngine;
using static Attribute;

[System.Serializable]
public enum CardType
{
    DAMAGE,
    SUPPORT,
    UTILITY
}

#nullable enable
public class Card
{
	private string Name;
	private string Description;
	private int Cost;
	private CardType CardType;
    private DamageType? CardDamageType;
	private int? DamageAmount;
    private int? HealAmount;
    private Effect? Effect;

    protected Card(string name, string description, int cost, CardType cardType)
    {
        Name = name;
        Description = description;
        Cost = cost;
        CardType = cardType;
    }

    protected Card(string name, string description, int cost, CardType cardType, DamageType? damageType , int? damageAmount, int? healAmount, Effect? effect)
    {
        Name = name;
        Description = description;
        Cost = cost;
        CardType = cardType;
        CardDamageType = damageType;
        DamageAmount = damageAmount;
        HealAmount = healAmount;
        Effect = effect;
    }

    public string GetName()
    {
        return Name;
    }

    public string GetDescription()
    {
        return Description;
    }

    public int GetCost()
    {
        return Cost;
    }

    public CardType GetCardType()
    {
        return CardType;
    }

    public DamageType? GetDamageType()
    {
        return CardDamageType;
    }

    public int? GetDamageAmount()
    {
        return DamageAmount;
    }

    public int? GetHealAmount()
    {
        return HealAmount;
    }

    public Effect? GetEffect()
    {
        return Effect;
    }

    public static class Cards
    {
        // Woodcutters - DAMAGE
        public static readonly Card AXE_SWING = new("Axe Swing", "A powerful swing with an axe dealing 10 damage.", 3, CardType.DAMAGE, DamageType.PHYSICAL, 10, 0, null);
        public static readonly Card CHOP = new("Chop", "A quick chop that inflicts 6 damage.", 1, CardType.DAMAGE, DamageType.PHYSICAL, 6, 0, null);
        public static readonly Card CLEAVE = new("Cleave", "Wide cleaving attack hitting multiple foes for 8 damage each.", 2, CardType.DAMAGE, DamageType.PHYSICAL, 8, 0, null);

        // Woodcutters - SUPPORT
        public static readonly Card HARDENED_SKIN = new("Hardened Skin", "Increases defense temporarily.", 2, CardType.SUPPORT, null, 0, 0,
            new TemporaryEffect("Hardened Skin", "Defense increased by 10% for 2 turns.", EffectSource.PLAYER, PlayerRelation.PLAYER, EffectType.TEMPORARY,
            new AttributeHolder(Attributes.DEFENSE, 0.1f), 2));
        public static readonly Card BANDAGE = new("Bandage", "Restores 5 HP.", 1, CardType.SUPPORT, null, 0, 5, null);
        public static readonly Card REST = new("Rest", "Resteroes 10 HP.", 3, CardType.SUPPORT, null, 0, 10, null);

        // Woodcutters - UTILITY
        public static readonly Card INTIMIDATE = new("Intimidate", "Lowers enemy morale.", 2, CardType.UTILITY, null, 0, 0,
            new TemporaryEffect("Intimidate", "Morale reduced by 10% for 2 turns.", EffectSource.PLAYER, PlayerRelation.HOSTILE, EffectType.TEMPORARY,
            new AttributeHolder(Attributes.MORALE, -0.1f), 2));
        public static readonly Card BARK_ARMOR = new("Bark Armor", "Provides a temporary defense buff.", 2, CardType.UTILITY, null, 0, 0,
            new TemporaryEffect("Bark Armor", "Defense increased by 15% for 3 turns.", EffectSource.PLAYER, PlayerRelation.PLAYER, EffectType.TEMPORARY,
            new AttributeHolder(Attributes.DEFENSE, 0.15f), 3));
        public static readonly Card ROAR = new("Roar", "Decreases enemy initiative.", 2, CardType.UTILITY, null, 0, 0,
            new TemporaryEffect("Roar", "Initiative decreased by 20% for 1 turn.", EffectSource.PLAYER, PlayerRelation.HOSTILE, EffectType.TEMPORARY,
            new AttributeHolder(Attributes.INITIATIVE, -0.2f), 1));

        // Stonemasons - DAMAGE
        public static readonly Card ROCK_THROW = new("Rock Throw", "Throws a heavy rock dealing 9 damage.", 2, CardType.DAMAGE, DamageType.PHYSICAL, 9, 0, null);
        public static readonly Card HAMMER_STRIKE = new("Hammer Strike", "A powerful hammer strike for 12 damage.", 3, CardType.DAMAGE, DamageType.PHYSICAL, 12, 0, null);
        public static readonly Card BOULDER_CRUSH = new("Boulder Crush", "Crushes the enemy with a boulder, inflicting 14 damage.", 4, CardType.DAMAGE, DamageType.PHYSICAL, 14, 0, null);

        // Stonemasons - SUPPORT
        public static readonly Card STONE_SKIN = new("Stone Skin", "Enhances defense with a stone layer.", 2, CardType.SUPPORT, null, 0, 0,
            new TemporaryEffect("Stone Skin", "Defense increased by 12% for 2 turns.", EffectSource.PLAYER, PlayerRelation.PLAYER, EffectType.TEMPORARY,
            new AttributeHolder(Attributes.DEFENSE, 0.12f), 2));
        public static readonly Card MEND = new("Mend", "Restores 6 HP.", 1, CardType.SUPPORT, null, 0, 6, null);
        public static readonly Card REINFORCE = new("Reinforce", "Increases defense for multiple turns.", 3, CardType.SUPPORT, null, 0, 0,
            new TemporaryEffect("Reinforce", "Defense increased by 8% for 4 turns.", EffectSource.PLAYER, PlayerRelation.PLAYER, EffectType.TEMPORARY,
            new AttributeHolder(Attributes.DEFENSE, 0.08f), 4));

        // Stonemasons - UTILITY
        public static readonly Card QUAKE = new("Quake", "Shakes enemy morale.", 2, CardType.UTILITY, null, 0, 0,
            new TemporaryEffect("Quake", "Morale reduced by 15% for 2 turns.", EffectSource.PLAYER, PlayerRelation.HOSTILE, EffectType.TEMPORARY,
            new AttributeHolder(Attributes.MORALE, -0.15f), 2));
        public static readonly Card FORTIFY = new("Fortify", "Increases own defense temporarily.", 3, CardType.UTILITY, null, 0, 0,
            new TemporaryEffect("Fortify", "Defense increased by 20% for 3 turns.", EffectSource.PLAYER, PlayerRelation.PLAYER, EffectType.TEMPORARY,
            new AttributeHolder(Attributes.DEFENSE, 0.2f), 3));
        public static readonly Card RUMBLE = new("Rumble", "Decreases enemy initiative.", 2, CardType.UTILITY, null, 0, 0,
            new TemporaryEffect("Rumble", "Initiative decreased by 15% for 2 turns.", EffectSource.PLAYER, PlayerRelation.HOSTILE, EffectType.TEMPORARY,
            new AttributeHolder(Attributes.INITIATIVE, -0.15f), 2));

        // Plunderers - DAMAGE
        public static readonly Card SLASH = new("Slash", "A swift slash with a dagger, dealing 7 damage.", 1, CardType.DAMAGE, DamageType.PHYSICAL, 7, 0, null);
        public static readonly Card AMBUSH = new("Ambush", "A surprise attack from the shadows, dealing 10 damage.", 3, CardType.DAMAGE, DamageType.PHYSICAL, 10, 0, null);
        public static readonly Card RANSACK = new("Ransack", "Brutal looting hit causing 12 damage.", 3, CardType.DAMAGE, DamageType.PHYSICAL, 12, 0, null);

        // Plunderers - SUPPORT
        public static readonly Card BATTLE_CRY = new("Battle Cry", "Boosts morale temporarily.", 2, CardType.SUPPORT, null, 0, 0,
            new TemporaryEffect("Battle Cry", "Morale increased by 10% for 2 turns.", EffectSource.PLAYER, PlayerRelation.PLAYER, EffectType.TEMPORARY,
            new AttributeHolder(Attributes.MORALE, 0.1f), 2));
        public static readonly Card SHIELD_UP = new("Shield Up", "Boosts defense temporarily.", 1, CardType.SUPPORT, null, 0, 0,
            new TemporaryEffect("Shield Up", "Defense increased by 5% for 1 turn.", EffectSource.PLAYER, PlayerRelation.PLAYER, EffectType.TEMPORARY,
            new AttributeHolder(Attributes.DEFENSE, 0.05f), 1));
        public static readonly Card RESTORATION = new("Restoration", "Restores 10 HP.", 3, CardType.SUPPORT, null, 0, 0, null);

        // Plunderers - UTILITY
        public static readonly Card DISORIENT = new("Disorient", "Lowers enemy accuracy.", 2, CardType.UTILITY, null, 0, 0,
            new TemporaryEffect("Disorient", "Accuracy reduced by 10% for 2 turns.", EffectSource.PLAYER, PlayerRelation.HOSTILE, EffectType.TEMPORARY,
            new AttributeHolder(Attributes.ACCURACY, -0.1f), 2));
        public static readonly Card BLINDING_POWDER = new("Blinding Powder", "Decreases enemy initiative.", 1, CardType.UTILITY, null, 0, 0,
            new TemporaryEffect("Blinding Powder", "Initiative decreased by 20% for 1 turn.", EffectSource.PLAYER, PlayerRelation.HOSTILE, EffectType.TEMPORARY,
            new AttributeHolder(Attributes.INITIATIVE, -0.2f), 1));
        public static readonly Card BERSERK = new("Berserk", "Increases own attack power temporarily.", 3, CardType.UTILITY, null, 0, 0,
            new TemporaryEffect("Berserk", "Attack increased by 15% for 3 turns.", EffectSource.PLAYER, PlayerRelation.PLAYER, EffectType.TEMPORARY,
            new AttributeHolder(Attributes.PHYSICAL_POWER, 0.15f), 3));

        // Iron Miners - DAMAGE
        public static readonly Card PICKAXE_STRIKE = new("Pickaxe Strike", "Strikes with a pickaxe, dealing 8 damage.", 2, CardType.DAMAGE, DamageType.PHYSICAL, 8, 0, null);
        public static readonly Card TUNNEL_COLLAPSE = new("Tunnel Collapse", "Collapses a tunnel on enemies, dealing 15 damage.", 4, CardType.DAMAGE, DamageType.PHYSICAL, 15, 0, null);
        public static readonly Card RUSTY_BLADE = new("Rusty Blade", "Stabs with a rusty blade, inflicting 5 damage.", 1, CardType.DAMAGE, DamageType.PHYSICAL, 5, 0, null);

        // Iron Miners - SUPPORT
        public static readonly Card IRON_SKIN = new("Iron Skin", "Boosts defense.", 2, CardType.SUPPORT, null, 0, 0,
            new TemporaryEffect("Iron Skin", "Defense increased by 12% for 3 turns.", EffectSource.PLAYER, PlayerRelation.PLAYER, EffectType.TEMPORARY,
            new AttributeHolder(Attributes.DEFENSE, 0.12f), 3));
        public static readonly Card FIRST_AID = new("First Aid", "Restores 6 HP.", 1, CardType.SUPPORT, null, 0, 6, null);
        public static readonly Card DEFENSIVE_STANCE = new("Defensive Stance", "Increases defense.", 3, CardType.SUPPORT, null, 0, 0,
            new TemporaryEffect("Defensive Stance", "Defense increased by 10% for 4 turns.", EffectSource.PLAYER, PlayerRelation.PLAYER, EffectType.TEMPORARY,
            new AttributeHolder(Attributes.DEFENSE, 0.1f), 4));

        // Iron Miners - UTILITY
        public static readonly Card BATTLECRY = new("Battlecry", "Lowers enemy morale.", 2, CardType.UTILITY, null, 0, 0,
            new TemporaryEffect("Taunt", "Morale reduced by 10% for 2 turns.", EffectSource.PLAYER, PlayerRelation.HOSTILE, EffectType.TEMPORARY,
            new AttributeHolder(Attributes.MORALE, -0.1f), 2));
        public static readonly Card HARDENED_WILL = new("Hardened Will", "Increases own morale temporarily.", 3, CardType.UTILITY, null, 0, 0,
            new TemporaryEffect("Hardened Will", "Morale increased by 15% for 3 turns.", EffectSource.PLAYER, PlayerRelation.PLAYER, EffectType.TEMPORARY,
            new AttributeHolder(Attributes.MORALE, 0.15f), 3));
        public static readonly Card CHALLENGE = new("Challenge", "Decreases enemy initiative.", 2, CardType.UTILITY, null, 0, 0,
            new TemporaryEffect("Challenge", "Initiative decreased by 15% for 2 turns.", EffectSource.PLAYER, PlayerRelation.HOSTILE, EffectType.TEMPORARY,
            new AttributeHolder(Attributes.INITIATIVE, -0.15f), 2));

        // Arcanists - DAMAGE
        public static readonly Card FIREBALL = new("Fireball", "Casts a fireball dealing 12 damage.", 3, CardType.DAMAGE, DamageType.ARCANE, 12, 0, null);
        public static readonly Card ICE_SPIKE = new("Ice Spike", "Shoots an ice spike for 8 damage.", 2, CardType.DAMAGE, DamageType.ARCANE, 8, 0, null);
        public static readonly Card ARCANE_BOLT = new("Arcane Bolt", "Hurls a bolt of arcane energy, inflicting 10 damage.", 3, CardType.DAMAGE, DamageType.ARCANE, 10, 0, null);

        // Arcanists - SUPPORT
        public static readonly Card MANA_SHIELD = new("Mana Shield", "Increases defense with magic.", 2, CardType.SUPPORT, null, 0, 0,
            new TemporaryEffect("Mana Shield", "Defense increased by 15% for 3 turns.", EffectSource.PLAYER, PlayerRelation.PLAYER, EffectType.TEMPORARY,
            new AttributeHolder(Attributes.DEFENSE, 0.15f), 3));
        public static readonly Card HEALING_AURA = new("Healing Aura", "Restores 12 HP.", 3, CardType.SUPPORT, null, 0, 12, null);
        public static readonly Card MAGIC_BARRIER = new("Magic Barrier", "Increases defense briefly.", 1, CardType.SUPPORT, null, 0, 0,
            new TemporaryEffect("Magic Barrier", "Defense increased by 10% for 2 turns.", EffectSource.PLAYER, PlayerRelation.PLAYER, EffectType.TEMPORARY,
            new AttributeHolder(Attributes.DEFENSE, 0.1f), 2));

        // Arcanists - UTILITY
        public static readonly Card WEAKEN = new("Weaken", "Lowers enemy physical power.", 2, CardType.UTILITY, null, 0, 0,
            new TemporaryEffect("Weaken", "Physical power reduced by 12% for 2 turns.", EffectSource.PLAYER, PlayerRelation.HOSTILE, EffectType.TEMPORARY,
            new AttributeHolder(Attributes.PHYSICAL_POWER, -0.12f), 2));
        public static readonly Card ENFEEBLE = new("Enfeeble", "Decreases enemy initiative.", 2, CardType.UTILITY, null, 0, 0,
            new TemporaryEffect("Enfeeble", "Initiative decreased by 15% for 2 turns.", EffectSource.PLAYER, PlayerRelation.HOSTILE, EffectType.TEMPORARY,
            new AttributeHolder(Attributes.INITIATIVE, -0.15f), 2));
        public static readonly Card BLESSING = new("Blessing", "Boosts morale temporarily.", 3, CardType.UTILITY, null, 0, 0,
            new TemporaryEffect("Blessing", "Morale increased by 20% for 3 turns.", EffectSource.PLAYER, PlayerRelation.PLAYER, EffectType.TEMPORARY,
            new AttributeHolder(Attributes.MORALE, 0.2f), 3));

        // Default - DAMAGE
        public static readonly Card QUICK_STRIKE = new("Quick Strike", "A swift hit dealing 4 damage.", 1, CardType.DAMAGE, DamageType.PHYSICAL, 4, 0, null);
        public static readonly Card CRUDE_SLASH = new("Crude Slash", "A basic slash causing 5 damage.", 2, CardType.DAMAGE, DamageType.PHYSICAL, 5, 0, null);
        public static readonly Card HEAVY_SWING = new("Heavy Swing", "A strong blow for 6 damage.", 2, CardType.DAMAGE, DamageType.PHYSICAL, 6, 0, null);

        // Default - SUPPORT
        public static readonly Card SELF_BANDAGE = new("Self Bandage", "Restores 3 HP.", 1, CardType.SUPPORT, null, 0, 3, null);
        public static readonly Card GUARD = new("Guard", "Slightly increases defense by 5% for 2 turns.", 2, CardType.SUPPORT, null, 0, 0,
            new TemporaryEffect("Guard", "Defense increased by 5% for 2 turns.", EffectSource.PLAYER, PlayerRelation.PLAYER, EffectType.TEMPORARY,
            new AttributeHolder(Attributes.DEFENSE, 0.05f), 2));
        public static readonly Card FOCUS = new("Focus", "Boosts accuracy by 5% for 2 turns.", 3, CardType.SUPPORT, null, 0, 0,
            new TemporaryEffect("Focus", "Accuracy increased by 5% for 2 turns.", EffectSource.PLAYER, PlayerRelation.PLAYER, EffectType.TEMPORARY,
            new AttributeHolder(Attributes.ACCURACY, 0.05f), 2));

        // Default - UTILITY
        public static readonly Card DISTRACT = new("Distract", "Reduces enemy accuracy by 5% for 1 turn.", 1, CardType.UTILITY, null, 0, 0,
            new TemporaryEffect("Distract", "Accuracy decreased by 5% for 1 turn.", EffectSource.PLAYER, PlayerRelation.HOSTILE, EffectType.TEMPORARY,
            new AttributeHolder(Attributes.ACCURACY, -0.05f), 1));
        public static readonly Card TAUNT = new("Taunt", "Lowers enemy morale by 5% for 2 turns.", 2, CardType.UTILITY, null, 0, 0,
            new TemporaryEffect("Taunt", "Morale reduced by 5% for 2 turns.", EffectSource.PLAYER, PlayerRelation.HOSTILE, EffectType.TEMPORARY,
            new AttributeHolder(Attributes.MORALE, -0.05f), 2));
        public static readonly Card HINDER = new("Hinder", "Decreases enemy initiative by 5% for 1 turn.", 3, CardType.UTILITY, null, 0, 0,
            new TemporaryEffect("Hinder", "Initiative decreased by 5% for 1 turn.", EffectSource.PLAYER, PlayerRelation.HOSTILE, EffectType.TEMPORARY,
            new AttributeHolder(Attributes.INITIATIVE, -0.05f), 1));

        // Pack of Wolves - DAMAGE
        public static readonly Card FANGS = new("Fangs", "A vicious bite that deals 8 damage.", 2, CardType.DAMAGE, DamageType.PHYSICAL, 8, 0, null);
        public static readonly Card CLAW_SWIPE = new("Claw Swipe", "A swipe of sharp claws, dealing 6 damage.", 1, CardType.DAMAGE, DamageType.PHYSICAL, 6, 0, null);
        public static readonly Card PACK_HUNT = new("Pack Hunt", "A coordinated attack by multiple wolves, inflicting 10 damage.", 3, CardType.DAMAGE, DamageType.PHYSICAL, 10, 0, null);

        // Pack of Wolves - SUPPORT
        public static readonly Card HOWL = new("Howl", "Increases morale, boosting damage temporarily.", 2, CardType.SUPPORT, null, 0, 0,
            new TemporaryEffect("Howl", "Damage increased by 10% for 2 turns.", EffectSource.PLAYER, PlayerRelation.PLAYER, EffectType.TEMPORARY,
            new AttributeHolder(Attributes.PHYSICAL_POWER, 0.1f), 2));
        public static readonly Card LICK_WOUNDS = new("Lick Wounds", "Restores 4 HP", 1, CardType.SUPPORT, null, 0, 4, null);
        public static readonly Card PACK_FORMATION = new("Pack Formation", "Strengthens defense temporarily.", 3, CardType.SUPPORT, null, 0, 0,
            new TemporaryEffect("Pack Formation", "Defense increased by 15% for 3 turns.", EffectSource.PLAYER, PlayerRelation.PLAYER, EffectType.TEMPORARY,
            new AttributeHolder(Attributes.DEFENSE, 0.15f), 3));

        // Pack of Wolves - UTILITY
        public static readonly Card INTIMIDATE_HOWL = new("Intimidating Howl", "Instills fear in enemies.", 2, CardType.UTILITY, null, 0, 0,
            new TemporaryEffect("Intimidating Howl", "Morale reduced by 15% for 2 turns.", EffectSource.PLAYER, PlayerRelation.HOSTILE, EffectType.TEMPORARY,
            new AttributeHolder(Attributes.MORALE, -0.15f), 2));
        public static readonly Card SCENT_TRACKING = new("Scent Tracking", "Increases own initiative temporarily.", 1, CardType.UTILITY, null, 0, 0,
            new TemporaryEffect("Scent Tracking", "Initiative increased by 10% for 1 turn.", EffectSource.PLAYER, PlayerRelation.PLAYER, EffectType.TEMPORARY,
            new AttributeHolder(Attributes.INITIATIVE, 0.1f), 1));
        public static readonly Card FEINT = new("Feint", "Deceives the enemy, reducing their accuracy.", 3, CardType.UTILITY, null, 0, 0,
            new TemporaryEffect("Feint", "Accuracy reduced by 15% for 2 turns.", EffectSource.PLAYER, PlayerRelation.HOSTILE, EffectType.TEMPORARY,
            new AttributeHolder(Attributes.ACCURACY, -0.15f), 2));

        // Hordes of Undead - DAMAGE
        public static readonly Card ROTTEN_BITE = new("Rotten Bite", "A decayed bite dealing 7 damage.", 2, CardType.DAMAGE, DamageType.PHYSICAL, 7, 0, null);
        public static readonly Card BONE_CLUB = new("Bone Club", "A strike with a bone club, dealing 9 damage.", 2, CardType.DAMAGE, DamageType.PHYSICAL, 9, 0, null);
        public static readonly Card DISEASED_STRIKE = new("Diseased Strike", "A toxic hit inflicting 12 damage.", 3, CardType.DAMAGE, DamageType.ARCANE, 12, 0, null);

        // Hordes of Undead - SUPPORT
        public static readonly Card REANIMATE = new("Reanimate", "Restores 5HP.", 1, CardType.SUPPORT, null, 0, 5, null);
        public static readonly Card SHADOW_SHROUD = new("Shadow Shroud", "Cloaks in darkness, increasing defense.", 3, CardType.SUPPORT, null, 0, 0,
            new TemporaryEffect("Shadow Shroud", "Defense increased by 15% for 3 turns.", EffectSource.PLAYER, PlayerRelation.PLAYER, EffectType.TEMPORARY,
            new AttributeHolder(Attributes.DEFENSE, 0.15f), 3));
        public static readonly Card GRAVE_TOUCH = new("Grave Touch", "Grants temporary resilience.", 2, CardType.SUPPORT, null, 0, 0,
            new TemporaryEffect("Grave Touch", "Defense increased by 10% for 2 turns.", EffectSource.PLAYER, PlayerRelation.PLAYER, EffectType.TEMPORARY,
            new AttributeHolder(Attributes.DEFENSE, 0.1f), 2));

        // Hordes of Undead - UTILITY
        public static readonly Card DREAD_AURA = new("Dread Aura", "Weakens enemy morale.", 2, CardType.UTILITY, null, 0, 0,
            new TemporaryEffect("Dread Aura", "Morale reduced by 20% for 2 turns.", EffectSource.PLAYER, PlayerRelation.HOSTILE, EffectType.TEMPORARY,
            new AttributeHolder(Attributes.MORALE, -0.2f), 2));
        public static readonly Card LIFE_DRAIN = new("Life Drain", "Saps enemy health, restoring 2 HP.", 1, CardType.UTILITY, null, 2, 2, null);
        public static readonly Card CURSED_GAZE = new("Cursed Gaze", "Reduces enemy initiative.", 3, CardType.UTILITY, null, 0, 0,
            new TemporaryEffect("Cursed Gaze", "Initiative decreased by 15% for 2 turns.", EffectSource.PLAYER, PlayerRelation.HOSTILE, EffectType.TEMPORARY,
            new AttributeHolder(Attributes.INITIATIVE, -0.15f), 2));

        public static readonly Card COSMIC_RAY = new("Cosmic Ray", "A powerful blast dealing 10 damage.", 3, CardType.DAMAGE, DamageType.ARCANE, 10, 0, null);
        public static readonly Card TENTACLE_LASH = new("Tentacle Lash", "A whip-like attack for 8 damage.", 2, CardType.DAMAGE, DamageType.ARCANE, 8, 0, null);
        public static readonly Card MIND_SPIKE = new("Mind Spike", "A psychic attack inflicting 6 damage.", 1, CardType.DAMAGE, DamageType.ARCANE, 6, 0, null);

        public static readonly Card ASTRAL_SHIELD = new("Astral Shield", "Protective aura boosting defense by 15%.", 3, CardType.SUPPORT, null, 0, 0,
            new TemporaryEffect("Astral Shield", "Defense increased by 15% for 3 turns.", EffectSource.PLAYER, PlayerRelation.PLAYER, EffectType.TEMPORARY,
            new AttributeHolder(Attributes.DEFENSE, 0.15f), 3));
        public static readonly Card HEALING_GLOW = new("Healing Glow", "Restores 8 HP.", 2, CardType.SUPPORT, null, 0, 8, null);
        public static readonly Card STAR_INSIGHT = new("Star Insight", "Increases accuracy temporarily.", 1, CardType.SUPPORT, null, 0, 0,
            new TemporaryEffect("Star Insight", "Accuracy increased by 10% for 1 turn.", EffectSource.PLAYER, PlayerRelation.PLAYER, EffectType.TEMPORARY,
            new AttributeHolder(Attributes.ACCURACY, 0.1f), 1));

        public static readonly Card VOID_GAZE = new("Void Gaze", "Reduces enemy morale.", 2, CardType.UTILITY, null, 0, 0,
            new TemporaryEffect("Void Gaze", "Morale reduced by 15% for 2 turns.", EffectSource.PLAYER, PlayerRelation.HOSTILE, EffectType.TEMPORARY,
            new AttributeHolder(Attributes.MORALE, -0.15f), 2));
        public static readonly Card PHASE_SHIFT = new("Phase Shift", "Increases own initiative temporarily.", 3, CardType.UTILITY, null, 0, 0,
            new TemporaryEffect("Phase Shift", "Initiative increased by 15% for 2 turns.", EffectSource.PLAYER, PlayerRelation.PLAYER, EffectType.TEMPORARY,
            new AttributeHolder(Attributes.INITIATIVE, 0.15f), 2));
        public static readonly Card MIND_WARP = new("Mind Warp", "Distorts enemy accuracy.", 1, CardType.UTILITY, null, 0, 0,
            new TemporaryEffect("Mind Warp", "Accuracy decreased by 10% for 1 turn.", EffectSource.PLAYER, PlayerRelation.HOSTILE, EffectType.TEMPORARY,
            new AttributeHolder(Attributes.ACCURACY, -0.1f), 1));
    }
}
