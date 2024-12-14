using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BuildingVariant;

#nullable enable
public class Building
{
    private BuildingVariant[] Variants;
    private Deck? Deck;

    protected Building(BuildingVariant[] variants, Deck? deck = null)
    {
        Variants = variants;
        Deck = deck;
    }

    public BuildingVariant[] GetVariants()
    {
        return Variants;
    }

    public BuildingVariant GetFirstRank()
    {
        return Variants[0];
    }

    public BuildingVariant GetSecondRank()
    {
        return Variants[1];
    }

    public BuildingVariant GetThirdRank()
    {
        return Variants[2];
    }

    public Deck? GetDeck()
    {
        return Deck;
    }

    public static class Buildings
    {
        public static readonly Building GOLD = new(new BuildingVariant[] { BuildingVariants.LOOTERS_CAMP, BuildingVariants.PLUNDERERS_OUTPOST, BuildingVariants.SPOILS_VAULT }, Deck.Decks.PLUNDERERS);
        public static readonly Building WOOD = new(new BuildingVariant[] { BuildingVariants.LOG_PILE, BuildingVariants.WOODCUTTERS_SHACK, BuildingVariants.SAWMILL }, Deck.Decks.WOODCUTTERS);
        public static readonly Building STONE = new(new BuildingVariant[] { BuildingVariants.STONECUTTERS_HUT, BuildingVariants.MASONRY, BuildingVariants.QUARRY }, Deck.Decks.STONEMASONS);
        public static readonly Building IRON = new(new BuildingVariant[] { BuildingVariants.ORE_HEAP, BuildingVariants.MINERS_CABIN, BuildingVariants.MINESHAFT }, Deck.Decks.MINERS);
        public static readonly Building AETHER = new(new BuildingVariant[] { BuildingVariants.SCAVENGING_STATION, BuildingVariants.CRYSTAL_HARVESTER, BuildingVariants.ARCANE_SYNTHESIZER }, Deck.Decks.ARCANISTS);

        public static readonly Building ATTACK = new(new BuildingVariant[] { BuildingVariants.TRAINING_YARD, BuildingVariants.BARRACKS, BuildingVariants.COMMAND_CENTER });
        public static readonly Building ARCANE = new(new BuildingVariant[] { BuildingVariants.MYSTIC_CIRCLE, BuildingVariants.ENCHANTERS_TENT, BuildingVariants.OBELISK });
        public static readonly Building DEFENSE = new(new BuildingVariant[] { BuildingVariants.SMITHY, BuildingVariants.FORGE, BuildingVariants.ARMORY });

        public static readonly Building MORALE = new(new BuildingVariant[] { BuildingVariants.CAMPFIRE_CIRCLE, BuildingVariants.CHAPEL, BuildingVariants.TEMPLE });
        public static readonly Building INITIATIVE = new(new BuildingVariant[] { BuildingVariants.WATCHTOWER, BuildingVariants.SIGNAL_POST, BuildingVariants.TACTICAL_COMMAND_TENT });
        public static readonly Building HP = new(new BuildingVariant[] { BuildingVariants.FIRST_AID_STATION, BuildingVariants.FIELD_HOSPITAL, BuildingVariants.CLINIC });

        public static readonly List<Building> GetAllBuildings = new() { GOLD, WOOD, STONE, IRON, AETHER, ATTACK, ARCANE, DEFENSE, MORALE, INITIATIVE, HP };

        public static Building? GetBuildingByVariant(BuildingVariant variant)
        {
            foreach (var building in GetAllBuildings)
            {
                foreach (var buildingVariant in building.GetVariants())
                {
                    if (buildingVariant == variant)
                    {
                        return building;
                    }
                }
            }

            return null;
        }
    }
}
