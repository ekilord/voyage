using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using static ElevationType;

public class Biome
{
	[JsonProperty]
	private string Name;
	[JsonProperty]
	private string Description;
	[JsonProperty]
	private int Weight;
    [JsonProperty]
    private string ColorHex;

	public Biome()
	{
		Name = "";
		Description = "";
		Weight = 0;
		ColorHex = "#ffffff";
    }

	public Biome( string name, string description, int weight, Color color )
	{
		Name = name;
		Description = description;
		Weight = weight;
		ColorHex = ColorUtility.ToHtmlStringRGBA(color);
	}

	public string GetName()
	{
		return Name;
	}

	public string GetDescription()
	{
		return Description;
	}

	public int GetWeight()
	{
		return Weight;
	}

	public Color GetColor()
	{
        ColorUtility.TryParseHtmlString($"#{ColorHex}", out Color color);
        return color;
    }

	public static class Biomes
	{
		public static readonly Biome DEEP_OCEAN = new( "Deep Ocean", "", 1, new Color( 52 / 255f, 94 / 255f, 132 / 255f ) );
		public static readonly Biome DEAD_SEA = new( "Dead Sea", "", 2, new Color( 95 / 255f, 126 / 255f, 154 / 255f ) );
		public static readonly Biome OCEAN = new( "Ocean", "", 3, new Color( 75 / 255f, 122 / 255f, 152 / 255f ) );
		public static readonly Biome CORAL_REEF = new( "Coral Reef", "", 1, new Color( 126 / 255f, 158 / 255f, 179 / 255f ) );
		public static readonly Biome LAKE = new( "Lake", "", 0, new Color( 52 / 255f, 94 / 255f, 132 / 255f ) );
		public static readonly Biome GRAVEL_BEACH = new( "Gravel Beach", "", 2, new Color( 135 / 255f, 123 / 255f, 107 / 255f ) );
		public static readonly Biome STONY_SHORES = new( "Stony Shores", "", 1, new Color( 96 / 255f, 93 / 255f, 84 / 255f ) );
		public static readonly Biome MARSH = new( "Marsh", "", 1, new Color( 92 / 255f, 123 / 255f, 89 / 255f ) );
		public static readonly Biome GRASSLAND = new( "Grassland", "", 2, new Color( 127 / 255f, 155 / 255f, 104 / 255f ) );
		public static readonly Biome WOODS = new( "Woods", "", 2, new Color( 107 / 255f, 134 / 255f, 99 / 255f ) );
		public static readonly Biome CRAGS = new( "Crags", "", 2, new Color( 112 / 255f, 108 / 255f, 83 / 255f ) );
		public static readonly Biome WOODED_HILLS = new( "Wooded Hills", "", 3, new Color( 117 / 255f, 129 / 255f, 93 / 255f ) );
		public static readonly Biome OLD_GROWTH_FOREST = new( "Old-Growth Forest", "", 2, new Color( 94 / 255f, 125 / 255f, 84 / 255f ) );
		public static readonly Biome STEEP_SLOPES = new( "Steep Slopes", "", 1, new Color( 148 / 255f, 143 / 255f, 113 / 255f ) );
		public static readonly Biome HIGHLANDS = new( "Highlands", "", 1, new Color( 167 / 255f, 150 / 255f, 130 / 255f ) );
		public static readonly Biome JAGGED_PEAKS = new( "Jagged Peaks", "", 1, new Color( 76 / 255f, 77 / 255f, 71 / 255f ) );
		public static readonly Biome BARREN_PEAKS = new( "Barren Peaks", "", 2, new Color( 103 / 255f, 100 / 255f, 85 / 255f ) );

		public static Biome GetBiomeByName(string name)
		{
			foreach (Biome biome in GetAllBiomes()) {
				if ( biome.Name == name ) return biome;
			}

			return DEEP_OCEAN;
		}

		public static List<Biome> GetAllBiomes()
		{
			List<Biome> biomes = new( ElevationTypes.DEEP_OCEAN.GetVariants() );
			biomes.AddRange( ElevationTypes.OCEAN.GetVariants() );
			biomes.AddRange( ElevationTypes.BEACH.GetVariants() );
			biomes.AddRange( ElevationTypes.PLAINS.GetVariants() );
			biomes.AddRange( ElevationTypes.HILLS.GetVariants() );
			biomes.AddRange( ElevationTypes.MOUNTAINS.GetVariants() );
			biomes.AddRange( ElevationTypes.MOUNTAIN_PEAKS.GetVariants() );
			return biomes;
		}

		public static List<Biome> GetPlainsBiomes()
		{
			return new List<Biome>( ElevationTypes.PLAINS.GetVariants() );
		}

		public static List<Biome> GetHillsBiomes()
		{
			return new List<Biome>( ElevationTypes.HILLS.GetVariants() );
		}

		public static List<Biome> GetMountainsBiomes()
		{
			return new List<Biome>( ElevationTypes.MOUNTAINS.GetVariants() );
		}

		public static List<Biome> GetMountainPeaksBiomes()
		{
			return new List<Biome>( ElevationTypes.MOUNTAIN_PEAKS.GetVariants() );
		}

		public static List<Biome> GetWoodedBiomes()
		{
			return new List<Biome>( new Biome[] { Biomes.MARSH, Biomes.WOODS, Biomes.WOODED_HILLS, Biomes.OLD_GROWTH_FOREST } );
		}

		public static List<Biome> GetSparselyWoodedBiomes()
		{
			return new List<Biome>( new Biome[] { Biomes.MARSH } );
		}

		public static List<Biome> GetLushlyWoodedBiomes()
		{
			return new List<Biome>( new Biome[] { Biomes.WOODS, Biomes.WOODED_HILLS } );
		}

		public static List<Biome> GetDenselyWoodedBiomes()
		{
			return new List<Biome>( new Biome[] { Biomes.OLD_GROWTH_FOREST } );
		}

		public static List<Biome> GetGrassyBiomes()
		{
			return new List<Biome>( new Biome[] { Biomes.GRASSLAND, Biomes.HIGHLANDS } );
		}

		public static List<Biome> GetHighElevationBiomes()
		{
			List<Biome> biomes = new( ElevationTypes.MOUNTAINS.GetVariants() );
			biomes.AddRange( ElevationTypes.MOUNTAIN_PEAKS.GetVariants() );
			return biomes;
		}

		public static List<Biome> GetGeneralBiomes()
		{
			List<Biome> biomes = new( ElevationTypes.BEACH.GetVariants() );
			biomes.AddRange( ElevationTypes.PLAINS.GetVariants() );
			biomes.AddRange( ElevationTypes.HILLS.GetVariants() );
			biomes.AddRange( ElevationTypes.MOUNTAINS.GetVariants() );
			return biomes;
		}

		public static List<Biome> GetContinentalBiomes()
		{
			List<Biome> biomes = new( ElevationTypes.PLAINS.GetVariants() );
			biomes.AddRange( ElevationTypes.HILLS.GetVariants() );
			biomes.AddRange( ElevationTypes.MOUNTAINS.GetVariants() );
			return biomes;
		}

		public static List<Biome> GetStoneyBiomes()
		{
			return new List<Biome>( new Biome[] { Biomes.STONY_SHORES, Biomes.GRASSLAND, Biomes.CRAGS, Biomes.STEEP_SLOPES, Biomes.HIGHLANDS, Biomes.JAGGED_PEAKS } );
		}

		public static List<Biome> GetBeachBiomes()
		{
			return new List<Biome>( new Biome[] { Biomes.STONY_SHORES, Biomes.GRAVEL_BEACH } );
		}
	}
}
