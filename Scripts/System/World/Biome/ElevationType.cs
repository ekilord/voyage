using System;
using System.Collections.Generic;
using static Biome;

public class ElevationType
{
	private readonly string Name;
	private readonly string Description;
	private readonly int Elevation;
	private readonly int Weight;
	private readonly Biome[] Variants;

	protected ElevationType( string name, string description, int elevation, int weight, Biome[] variants )
	{
		Name = name;
		Description = description;
		Elevation = elevation;
		Weight = weight;
		Variants = variants;
	}

	public string GetName()
	{
		return Name;
	}

	public string GetDescription()
	{
		return Description;
	}


	public int GetElevation()
	{
		return Elevation;
	}

	public int GetWeight()
	{
		return Weight;
	}

	public Biome[] GetVariants()
	{
		return Variants;
	}

	public Biome GetMainVariant()
	{
		return Variants.Length > 1 ? Variants[1] : Variants[0];
	}

	public Biome GetRarestVariant()
	{
		int rarity = Variants[0].GetWeight();
		Biome rarest = Variants[0];

		foreach ( Biome biome in Variants ) {
			if ( biome.GetWeight() <= rarity ) {
				rarity = biome.GetWeight();
				rarest = biome;
			}
		}

		return rarest;
	}

	public static class ElevationTypes
	{
		public static readonly ElevationType DEEP_OCEAN = new( "Deep Ocean", "", 0, 10, new Biome[] { Biomes.DEEP_OCEAN } );
		public static readonly ElevationType OCEAN = new( "Ocean", "", 1, 12, new Biome[] { Biomes.DEAD_SEA, Biomes.OCEAN, Biomes.CORAL_REEF, Biomes.LAKE } );
		public static readonly ElevationType BEACH = new( "Beach", "", 2, 5, new Biome[] { Biomes.GRAVEL_BEACH, Biomes.STONY_SHORES } );
		public static readonly ElevationType PLAINS = new( "Plains", "", 3, 16, new Biome[] { Biomes.MARSH, Biomes.GRASSLAND, Biomes.WOODS } );
		public static readonly ElevationType HILLS = new( "Hills", "", 4, 16, new Biome[] { Biomes.CRAGS, Biomes.WOODED_HILLS, Biomes.OLD_GROWTH_FOREST } );
		public static readonly ElevationType MOUNTAINS = new( "Mountains", "", 5, 10, new Biome[] { Biomes.STEEP_SLOPES, Biomes.HIGHLANDS } );
		public static readonly ElevationType MOUNTAIN_PEAKS = new( "Mountain Peaks", "", 6, 18, new Biome[] { Biomes.JAGGED_PEAKS, Biomes.BARREN_PEAKS } );

		public static readonly List<ElevationType> ALL_ELEVATION_TYPES = new() {
			DEEP_OCEAN,
			OCEAN,
			BEACH,
			PLAINS,
			HILLS,
			MOUNTAINS,
			MOUNTAIN_PEAKS
		};

		public static readonly int WEIGHTS_SUM = GetSumOfWeights();

		private static int GetSumOfWeights()
		{
			int sum = 0;

			foreach ( var elevation in ALL_ELEVATION_TYPES ) {
				sum += elevation.Weight;
			}

			return sum;
		}

		public static ElevationType GetElevationFromNumber( int elevation )
		{
			foreach ( var elevationType in ALL_ELEVATION_TYPES )
				if ( elevationType.GetElevation() == elevation )
					return elevationType;
			throw new ArgumentException( "No biome corresponding to given elevation" );
		}

		public static ElevationType GetElevationTypeFromBiome( Biome biome )
		{
			foreach ( var elevationType in ALL_ELEVATION_TYPES )
				foreach (var variant in elevationType.GetVariants() ) {
					if (variant == biome) return elevationType;
				}
			throw new ArgumentException( "No biome corresponding to given elevation" );
		}
	}
}
