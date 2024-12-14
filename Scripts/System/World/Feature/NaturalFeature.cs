using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Effect;
using static LootTable;
using static Biome;

[System.Serializable]
public class NaturalFeature : AbstractFeature
{
	public NaturalFeature() : base()
	{
	}

	protected NaturalFeature( string name, string description, FeatureType featureType, SpawnCondition spawnCondition, LootTable loot, List<Effect> effects ) : base( name, description, featureType, spawnCondition, loot, effects )
	{
	}

	protected NaturalFeature( string name, string description, FeatureType featureType, SpawnCondition spawnCondition, LootTable loot, Effect effect ) : base( name, description, featureType, spawnCondition, loot, effect )
	{
	}

	protected NaturalFeature( string name, string description, FeatureType featureType, SpawnCondition spawnCondition, LootTable loot ) : base( name, description, featureType, spawnCondition, loot )
	{
	}

	protected NaturalFeature( string name, string description, FeatureType featureType, SpawnCondition spawnCondition ) : base( name, description, featureType, spawnCondition )
	{
	}

	public static class Features
	{
		//FORESTS
		public static readonly NaturalFeature SPARSE_FOREST = new( "Sparse Forest", "", FeatureType.FEATURE_BLOCKING, new ConditionalSpawnCondition(Biomes.GetSparselyWoodedBiomes()), LootTables.SPARSE_FOREST );
		public static readonly NaturalFeature LUSH_FOREST = new( "Lush Forest", "", FeatureType.FEATURE_BLOCKING, new ConditionalSpawnCondition(Biomes.GetLushlyWoodedBiomes()), LootTables.FOREST, ConditionalEffect.Environmental.LUSH_VEGETATION );
		public static readonly NaturalFeature DENSE_FOREST = new( "Dense Forest", "", FeatureType.FEATURE_BLOCKING, new ConditionalSpawnCondition(Biomes.GetDenselyWoodedBiomes()), LootTables.DENSE_FOREST, ConditionalEffect.Environmental.DENSE_VEGETATION );

		//DEPOSITS
		public static readonly NaturalFeature STONE_DEPOSIT = new( "Stone Deposit", "", FeatureType.FEATURE_BLOCKING, new RandomSpawnCondition(Biomes.GetStoneyBiomes(), (10, 20), 2), LootTables.STONE_DEPOSIT );
		public static readonly NaturalFeature IRON_DEPOSIT = new( "Iron Deposit", "", FeatureType.FEATURE_BLOCKING, new RandomSpawnCondition(Biomes.GetStoneyBiomes(), (10, 20), 2), LootTables.IRON_DEPOSIT );
		public static readonly NaturalFeature GOLD_DEPOSIT = new( "Gold Deposit", "", FeatureType.FEATURE_BLOCKING, new RandomSpawnCondition(Biomes.GetStoneyBiomes(), (8, 16), 2), LootTables.GOLD_DEPOSIT );
		public static readonly NaturalFeature AETHER_DEPOSIT = new( "Aether Deposit", "", FeatureType.FEATURE_BLOCKING, new RandomSpawnCondition(Biomes.GetStoneyBiomes(), (7, 14), 3), LootTables.AETHER_DEPOSIT );

		public static NaturalFeature Create(NaturalFeature feature)
		{
            var originalLootTable = feature.GetLootTable();

			if (originalLootTable != null)
			{
				return new(feature.GetName(), feature.GetDescription(), feature.GetFeatureType(), feature.GetSpawnCondition(), LootTables.Copy(feature.GetLootTable()), feature.GetEffects());
			}

			return new(feature.GetName(), feature.GetDescription(), feature.GetFeatureType(), feature.GetSpawnCondition(), originalLootTable, feature.GetEffects());

        }

        public static NaturalFeature CreateWithLootTable(string name, ResourceHolder mainEntry, List<ResourceHolder> secondaryEntries)
        {
            NaturalFeature feature = null;

            foreach (NaturalFeature currentFeature in GetAllNaturalFeatures())
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

        public static NaturalFeature? GetNaturalFeatureByName(string name)
		{
			foreach (NaturalFeature feature in GetAllNaturalFeatures()) {
				if (feature.GetName().Equals(name)) return feature;
			}
			return null;
		}

		public static List<NaturalFeature> GetAllNaturalFeatures()
		{
			List<NaturalFeature> features = new();
			features.AddRange(new NaturalFeature[] { SPARSE_FOREST, LUSH_FOREST, DENSE_FOREST });
            features.AddRange(new NaturalFeature[] { STONE_DEPOSIT, IRON_DEPOSIT, GOLD_DEPOSIT, AETHER_DEPOSIT });

            return features;
		}

		public static Dictionary<Biome, NaturalFeature> GetBiomesToNaturalFeatures()
		{
			Dictionary<Biome, NaturalFeature> dict = new();

            List<NaturalFeature> features = GetAllNaturalFeatures();

			foreach(var feature in features)
			{ 
				if (feature.GetSpawnCondition() is ConditionalSpawnCondition)
				{
					foreach (var biome in feature.GetSpawnCondition().GetBiomes())
					{
						dict.Add(biome, feature);
					}
				}
			}

			return dict;
		}
	}
}
