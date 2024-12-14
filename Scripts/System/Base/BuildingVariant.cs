using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable
[System.Serializable]
public class BuildingVariant
{
    [SerializeField]
    private string Name;
    [SerializeField]
    private string Description;
    [SerializeField]
    private Cost Cost;
    [SerializeField]
    private int Rank;
    [SerializeField]
    private List<Effect>? Effects;

    public BuildingVariant()
    {
        Name = "";
        Description = "";
        Cost = new Cost();
        Rank = 0;
        Effects = null;
    }

    protected BuildingVariant(string name, string description, Cost cost, int rank, List<Effect> effects)
    {
        Name = name;
        Description = description;
        Cost = cost;
        Rank = rank;
        Effects = effects;
    }

    protected BuildingVariant(string name, string description, Cost cost, int rank, Effect effect)
    {
        Name = name;
        Description = description;
        Cost = cost;
        Rank = rank;
        Effects = new() { effect };
    }

    protected BuildingVariant(string name, string description, Cost cost, int rank)
    {
        Name = name;
        Description = description;
        Cost = cost;
        Rank = rank;
        Effects = null;
    }

    public string GetName()
    {
        return Name;
    }

    public string GetDescription()
    {
        return Description;
    }

    public Cost GetCost()
    {
        return Cost;
    }

    public int GetRank()
    {
        return Rank;
    }

    public List<Effect>? GetEffect()
    {
        return Effects;
    }

    public static class BuildingVariants
    {
        // Gold Generating buildings
        public static BuildingVariant LOOTERS_CAMP = new(
            "Looter's Camp",
            "A rough camp generating 1 piece of gold every day from nearby loot.",
            new Cost(8, 3, 0, 0, 0), 1
        );

        public static BuildingVariant PLUNDERERS_OUTPOST = new(
            "Plunderer's Outpost",
            "A base coordinating plundering expeditions, producing 3 pieces of gold daily.",
            new Cost(16, 8, 4, 0, 0), 2
        );

        public static BuildingVariant SPOILS_VAULT = new(
            "Spoils Vault",
            "A fortified vault producing 5 pieces of gold daily from gathered loot.",
            new Cost(30, 12, 6, 3, 0), 3
        );

        // Wood Generating buildings
        public static BuildingVariant LOG_PILE = new(
            "Log Pile",
            "A simple structure that gathers 1 piece of wood daily.",
            new Cost(4, 0, 0, 0, 0), 1
        );

        public static BuildingVariant WOODCUTTERS_SHACK = new(
            "Woodcutter's Shack",
            "A small hut producing 3 pieces of wood daily through steady work.",
            new Cost(12, 4, 0, 0, 0), 2
        );

        public static BuildingVariant SAWMILL = new(
            "Sawmill",
            "An advanced structure efficiently producing 5 pieces of wood daily.",
            new Cost(24, 8, 4, 0, 0), 3
        );

        // Stone Generating buildings
        public static BuildingVariant STONECUTTERS_HUT = new(
            "Stonecutter's Hut",
            "A hut producing 1 piece of stone daily through basic cutting work.",
            new Cost(4, 0, 8, 0, 0), 1
        );

        public static BuildingVariant MASONRY = new(
            "Masonry",
            "A building processing 3 pieces of stone daily into usable blocks.",
            new Cost(12, 4, 12, 0, 0), 2
        );

        public static BuildingVariant QUARRY = new(
            "Quarry",
            "A large site producing 5 pieces of stone daily through extensive excavation.",
            new Cost(28, 8, 20, 0, 0), 3
        );

        // Iron Generating buildings
        public static BuildingVariant ORE_HEAP = new(
            "Ore Heap",
            "A basic pile producing 1 piece of iron ore daily.",
            new Cost(8, 0, 8, 0, 0), 1
        );

        public static BuildingVariant MINERS_CABIN = new(
            "Miner's Cabin",
            "A small cabin producing 3 pieces of iron ore daily from nearby veins.",
            new Cost(16, 4, 12, 4, 0), 2
        );

        public static BuildingVariant MINESHAFT = new(
            "Mineshaft",
            "An advanced structure producing 5 pieces of iron ore daily through deep mining.",
            new Cost(32, 8, 24, 8, 0), 3
        );

        // Aether Generating buildings
        public static BuildingVariant SCAVENGING_STATION = new(
            "Scavenging Station",
            "A post gathering 1 piece of aether daily.",
            new Cost(8, 4, 0, 0, 2), 1
        );

        public static BuildingVariant CRYSTAL_HARVESTER = new(
            "Crystal Harvester",
            "A machine collecting 3 pieces of aether daily.",
            new Cost(20, 8, 0, 4, 8), 2
        );

        public static BuildingVariant ARCANE_SYNTHESIZER = new(
            "Arcane Synthesizer",
            "A structure refining 5 pieces of aether daily.",
            new Cost(40, 12, 4, 8, 20), 3
        );

        public static BuildingVariant TRAINING_YARD = new(
            "Training Yard",
            "A basic facility where troops gain a slight increase in combat readiness.",
            new Cost(10, 5, 0, 0, 0), 1,
            new PermanentEffect(
                "Attack Power Boost (Small)",
                "Slightly increases attack power.",
                EffectSource.PLAYER,
                PlayerRelation.PLAYER,
                EffectType.PERMANENT,
                new AttributeHolder(Attribute.Attributes.PHYSICAL_POWER, 0.10f)
            )
        );

        public static BuildingVariant BARRACKS = new(
            "Barracks",
            "A dedicated barracks training soldiers for the battlefield.",
            new Cost(20, 10, 5, 0, 0), 2,
            new PermanentEffect(
                "Attack Power Boost (Medium)",
                "Moderately increases attack power.",
                EffectSource.PLAYER,
                PlayerRelation.PLAYER,
                EffectType.PERMANENT,
                new AttributeHolder(Attribute.Attributes.PHYSICAL_POWER, 0.20f)
            )
        );

        public static BuildingVariant COMMAND_CENTER = new(
            "Command Center",
            "An elite training center honing the might of your army.",
            new Cost(40, 20, 10, 5, 0), 3,
            new PermanentEffect(
                "Attack Power Boost (Large)",
                "Greatly increases attack power.",
                EffectSource.PLAYER,
                PlayerRelation.PLAYER,
                EffectType.PERMANENT,
                new AttributeHolder(Attribute.Attributes.PHYSICAL_POWER, 0.30f)
            )
        );


        public static BuildingVariant SMITHY = new(
            "Smithy",
            "A humble smithy forging basic protective gear to enhance defensive capabilities.",
            new Cost(10, 6, 0, 0, 0), 1,
            new PermanentEffect(
                "Defense Boost (Small)",
                "Slightly increases defense.",
                EffectSource.PLAYER,
                PlayerRelation.PLAYER,
                EffectType.PERMANENT,
                new AttributeHolder(Attribute.Attributes.DEFENSE, 0.10f)
            )
        );

        public static BuildingVariant FORGE = new(
            "Forge",
            "A well-equipped forge producing advanced plate armor for soldiers.",
            new Cost(20, 12, 5, 0, 0), 2,
            new PermanentEffect(
                "Defense Boost (Medium)",
                "Moderately increases defense.",
                EffectSource.PLAYER,
                PlayerRelation.PLAYER,
                EffectType.PERMANENT,
                new AttributeHolder(Attribute.Attributes.DEFENSE, 0.20f)
            )
        );

        public static BuildingVariant ARMORY = new(
            "Armory",
            "A massive armory producing the finest protective gear for an impenetrable defense.",
            new Cost(40, 20, 10, 5, 0), 3,
            new PermanentEffect(
                "Defense Boost (Large)",
                "Greatly increases defense.",
                EffectSource.PLAYER,
                PlayerRelation.PLAYER,
                EffectType.PERMANENT,
                new AttributeHolder(Attribute.Attributes.DEFENSE, 0.30f)
            )
        );

        public static BuildingVariant MYSTIC_CIRCLE = new(
            "Mystic Circle",
            "A sacred circle channeling ambient magical energy.",
            new Cost(12, 6, 0, 0, 2), 1,
            new PermanentEffect(
                "Arcane Power Boost (Small)",
                "Slightly increases arcane power.",
                EffectSource.PLAYER,
                PlayerRelation.PLAYER,
                EffectType.PERMANENT,
                new AttributeHolder(Attribute.Attributes.ARCANE_POWER, 0.10f)
            )
        );

        public static BuildingVariant ENCHANTERS_TENT = new(
            "Enchanter's Tent",
            "A tent where enchanters amplify arcane energy for your troops.",
            new Cost(24, 12, 4, 0, 8), 2,
            new PermanentEffect(
                "Arcane Power Boost (Medium)",
                "Moderately increases arcane power.",
                EffectSource.PLAYER,
                PlayerRelation.PLAYER,
                EffectType.PERMANENT,
                new AttributeHolder(Attribute.Attributes.ARCANE_POWER, 0.20f)
            )
        );

        public static BuildingVariant OBELISK = new(
            "Obelisk of Power",
            "A towering monument brimming with arcane potency.",
            new Cost(48, 24, 8, 4, 16), 3,
            new PermanentEffect(
                "Arcane Power Boost (Large)",
                "Greatly increases arcane power.",
                EffectSource.PLAYER,
                PlayerRelation.PLAYER,
                EffectType.PERMANENT,
                new AttributeHolder(Attribute.Attributes.ARCANE_POWER, 0.30f)
            )
        );


        // Morale-Boosting Buildings
        public static BuildingVariant CAMPFIRE_CIRCLE = new(
            "Campfire Circle",
            "A cozy gathering place where stories are shared, boosting morale.",
            new Cost(8, 3, 0, 0, 0), 1,
            new PermanentEffect(
                "Morale Boost (Small)",
                "Increases morale slightly.",
                EffectSource.PLAYER,
                PlayerRelation.PLAYER,
                EffectType.PERMANENT,
                new AttributeHolder(Attribute.Attributes.MORALE, 0.10f)
            )
        );

        public static BuildingVariant CHAPEL = new(
            "Chapel",
            "A sacred space offering spiritual support to uplift the spirits.",
            new Cost(15, 6, 0, 0, 2), 2,
            new PermanentEffect(
                "Morale Boost (Medium)",
                "Moderately increases morale.",
                EffectSource.PLAYER,
                PlayerRelation.PLAYER,
                EffectType.PERMANENT,
                new AttributeHolder(Attribute.Attributes.MORALE, 0.20f)
            )
        );

        public static BuildingVariant TEMPLE = new(
            "Temple",
            "A site of worship, inspiring unmatched morale among your people.",
            new Cost(30, 12, 5, 0, 4), 3,
            new PermanentEffect(
                "Morale Boost (Large)",
                "Greatly increases morale.",
                EffectSource.PLAYER,
                PlayerRelation.PLAYER,
                EffectType.PERMANENT,
                new AttributeHolder(Attribute.Attributes.MORALE, 0.30f)
            )
        );

        // Initiative-Boosting Buildings
        public static BuildingVariant WATCHTOWER = new(
            "Watchtower",
            "A high vantage point ensuring quick reaction to approaching threats.",
            new Cost(8, 4, 0, 0, 0), 1,
            new PermanentEffect(
                "Initiative Boost (Small)",
                "Increases initiative slightly.",
                EffectSource.PLAYER,
                PlayerRelation.PLAYER,
                EffectType.PERMANENT,
                new AttributeHolder(Attribute.Attributes.INITIATIVE, 0.10f)
            )
        );

        public static BuildingVariant SIGNAL_POST = new(
            "Signal Post",
            "Equipped with flags and horns to coordinate fast responses.",
            new Cost(12, 6, 3, 0, 0), 2,
            new PermanentEffect(
                "Initiative Boost (Medium)",
                "Moderately increases initiative.",
                EffectSource.PLAYER,
                PlayerRelation.PLAYER,
                EffectType.PERMANENT,
                new AttributeHolder(Attribute.Attributes.INITIATIVE, 0.20f)
            )
        );

        public static BuildingVariant TACTICAL_COMMAND_TENT = new(
            "Tactical Command Tent",
            "A nerve center for coordinated planning and rapid execution.",
            new Cost(25, 10, 5, 2, 0), 3,
            new PermanentEffect(
                "Initiative Boost (Large)",
                "Greatly increases initiative.",
                EffectSource.PLAYER,
                PlayerRelation.PLAYER,
                EffectType.PERMANENT,
                new AttributeHolder(Attribute.Attributes.INITIATIVE, 0.30f)
            )
        );

        // HP-Boosting Buildings
        public static BuildingVariant FIRST_AID_STATION = new(
            "First Aid Station",
            "A basic tent equipped to handle minor injuries and boost resilience.",
            new Cost(6, 3, 0, 0, 1), 1,
            new PermanentEffect(
                "HP Boost (Small)",
                "Slightly increases HP.",
                EffectSource.PLAYER,
                PlayerRelation.PLAYER,
                EffectType.PERMANENT,
                new AttributeHolder(Attribute.Attributes.HEALTH, 20)
            )
        );

        public static BuildingVariant FIELD_HOSPITAL = new(
            "Field Hospital",
            "A fully equipped facility for treating wounds and fortifying endurance.",
            new Cost(15, 8, 0, 0, 3), 2,
            new PermanentEffect(
                "HP Boost (Medium)",
                "Moderately increases HP.",
                EffectSource.PLAYER,
                PlayerRelation.PLAYER,
                EffectType.PERMANENT,
                new AttributeHolder(Attribute.Attributes.HEALTH, 30)
            )
        );

        public static BuildingVariant CLINIC = new(
            "Clinic",
            "A structure offering unparalleled healing and protection.",
            new Cost(30, 15, 6, 3, 6), 3,
            new PermanentEffect(
                "HP Boost (Large)",
                "Greatly increases HP.",
                EffectSource.PLAYER,
                PlayerRelation.PLAYER,
                EffectType.PERMANENT,
                new AttributeHolder(Attribute.Attributes.HEALTH, 50)
            )
        );



        public static List<BuildingVariant> GetAllBuildingVariants()
        {
            return new List<BuildingVariant>()
            {
                LOOTERS_CAMP,
                PLUNDERERS_OUTPOST,
                SPOILS_VAULT,

                LOG_PILE,
                WOODCUTTERS_SHACK,
                SAWMILL,

                STONECUTTERS_HUT,
                MASONRY,
                QUARRY,

                ORE_HEAP,
                MINERS_CABIN,
                MINESHAFT,

                SCAVENGING_STATION,
                CRYSTAL_HARVESTER,
                ARCANE_SYNTHESIZER,

                TRAINING_YARD,
                BARRACKS,
                COMMAND_CENTER,

                SMITHY,
                FORGE,
                ARMORY,

                MYSTIC_CIRCLE,
                ENCHANTERS_TENT,
                OBELISK,

                CAMPFIRE_CIRCLE,
                CHAPEL,
                TEMPLE,

                WATCHTOWER,
                SIGNAL_POST,
                TACTICAL_COMMAND_TENT,

                FIRST_AID_STATION,
                FIELD_HOSPITAL,
                CLINIC
            };
        }

        public static BuildingVariant GetBuildingVariantByName(string name)
        {
            foreach (BuildingVariant variant in GetAllBuildingVariants())
            {
                if (name == variant.GetName()) return variant;
            }

            return LOOTERS_CAMP;
        }
    }
}
