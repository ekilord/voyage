using System.Collections;
using System.Collections.Generic;
using static Biome;
using static Effect;
using static LootTable;
using UnityEngine;

[System.Serializable]
public class StructureFeature : AbstractFeature
{
	public StructureFeature() : base()
	{
	}
	protected StructureFeature( string name, string description, FeatureType featureType, SpawnCondition spawnCondition, LootTable loot, List<Effect> effects ) : base( name, description, featureType, spawnCondition, loot, effects )
	{
	}

	protected StructureFeature( string name, string description, FeatureType featureType, SpawnCondition spawnCondition, LootTable loot, Effect effect ) : base( name, description, featureType, spawnCondition, loot, effect )
	{
	}

	protected StructureFeature( string name, string description, FeatureType featureType, SpawnCondition spawnCondition, LootTable loot ) : base( name, description, featureType, spawnCondition, loot )
	{
	}

	protected StructureFeature( string name, string description, FeatureType featureType, SpawnCondition spawnCondition  ) : base( name, description, featureType, spawnCondition )
	{
	}

	public static class Features
	{
		//MAGICAL


		//RUINS
		public static readonly StructureFeature RUINS = new( "Ruins", "", FeatureType.STRUCTURE_BLOCKING, new RandomSpawnCondition(Biomes.GetGeneralBiomes(), (6, 12), 3), LootTables.RUINS, ConditionalEffect.Environmental.CURSED_GROUND );
		public static readonly StructureFeature TEMPLE_RUINS = new( "Temple Ruins", "", FeatureType.STRUCTURE_BLOCKING, new RandomSpawnCondition(Biomes.GetContinentalBiomes(), (4, 9), 5), LootTables.TEMPLE_RUINS, ConditionalEffect.Environmental.MYSTICAL_FOG );

        public static StructureFeature Create(StructureFeature feature)
        {
            var originalLootTable = feature.GetLootTable();

            if (originalLootTable != null)
            {
                return new(feature.GetName(), feature.GetDescription(), feature.GetFeatureType(), feature.GetSpawnCondition(), LootTables.Copy(feature.GetLootTable()), feature.GetEffects());
            }

            return new(feature.GetName(), feature.GetDescription(), feature.GetFeatureType(), feature.GetSpawnCondition(), originalLootTable, feature.GetEffects());
        }

		public static StructureFeature CreateWithLootTable(string name, ResourceHolder mainEntry, List<ResourceHolder> secondaryEntries)
		{
			StructureFeature feature = new();

            foreach (StructureFeature currentFeature in GetAllStructureFeatures())
            {
                if (currentFeature.GetName().Equals(name))
                    feature = currentFeature;
            }

            var originalLootTable = feature.GetLootTable();

            if (mainEntry.GetResourceType().Equals(originalLootTable.GetMainEntry().GetResource()))
            {
                LootTable newLootTable = new(mainEntry, secondaryEntries);

                return new(feature.GetName(), feature.GetDescription(), feature.GetFeatureType(), feature.GetSpawnCondition(), LootTables.Copy(newLootTable), feature.GetEffects());
            }

            return null;
        }

		public static StructureFeature? GetStructureFeatureByName( string name )
		{
			foreach ( StructureFeature feature in GetAllStructureFeatures() ) {
				if ( feature.GetName().Equals( name ) )
					return feature;
			}
			return null;
		}

		public static List<StructureFeature> GetAllStructureFeatures()
        {
            List<StructureFeature> features = new();
            features.AddRange(new StructureFeature[] { RUINS, TEMPLE_RUINS });

            return features;
        }
    }
}
