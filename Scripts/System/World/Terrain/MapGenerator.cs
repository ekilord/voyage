using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

using static ElevationType;
using Unity.Mathematics;
using static Biome;
using System.Linq;

public class MapGenerator
{
    private readonly int Width;
    private readonly int Height;

    private readonly float ElevationMagnification;

    private readonly float VariationMultiplier;
    private readonly float VariationMagnification;

    private readonly float IslandSize;

    private int ElevationSeed;
    private int VariationSeed;

    private int[,] ElevationMap;
    private float[,] VariationMap;

    private bool[,] Visited;

    public MapGenerator(int width, int height, float magnification, float multiplier, float islandSize, int seed)
    {
        Width = width;
        Height = height;
        ElevationMagnification = magnification;
        VariationMagnification = magnification * multiplier;
        IslandSize = islandSize;
        ElevationSeed = seed;
        VariationSeed = seed + seed * Mathf.RoundToInt(multiplier);
        ElevationMap = new int[Width, Height];
        VariationMap = new float[Width, Height];
        Visited = new bool[Width, Height];
    }

    public Tile[,] GenerateWorld()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                float elevationNoise = GetPrimaryNoise(x, y);
                float variationNoise = GetSecondaryNoise(x, y);

                ElevationMap[x, y] = ChooseElevation(elevationNoise);
                VariationMap[x, y] = variationNoise;
            }
        }

        HashSet<(int, int)> mainContinent = IdentifyMainContinent();
        ReplaceUnconnectedLand(mainContinent);

        Visited = new bool[Width, Height];

        List<HashSet<(int, int)>> oceans = IdentifyOceans();
        HashSet<(int, int)> mainOcean = FindLargestOcean(oceans);
        ReplaceUnconnectedOcean(oceans, mainOcean);

        ReplaceFarAwayBeaches();

        Tile[,] mapTiles = new Tile[Width, Height];

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                mapTiles[x, y] = new Tile( new Coordinate( x, y ), ChooseVariation( ElevationTypes.GetElevationFromNumber( ElevationMap[x, y] ), VariationMap[x, y] ) );
            }
        }

        PlaceConditionalFeatures(mapTiles);
        PlaceRandomFeatures(mapTiles);

        FinalizeLoot(mapTiles);

		return mapTiles;
    }

    private float GenerateNoise(int seed, float magnification, int x, int y)
    {
        return Mathf.PerlinNoise((x / magnification) + seed,
                                 (y / magnification) + seed);
    }

    private float GetPrimaryNoise(int x, int y)
    {
        float noise = GenerateNoise(ElevationSeed, ElevationMagnification, x, y) - FallOffMap(x, y);

        return Mathf.Clamp01(noise);
    }

    private float GetSecondaryNoise(int x, int y)
    {
        return Mathf.Clamp01(GenerateNoise(VariationSeed, VariationMagnification, x, y));
    }

    private float FallOffMap(float x, float y)
    {
        float gradient = 1;

        gradient /= (x * y) / (Width * Height) * (1 - (x / Width)) * (1 - (y / Height));
        gradient -= 16;
        gradient /= IslandSize;

        return gradient;

    }

    private int ChooseElevation(float value)
    {
        float sum = 0f;

        foreach (var biome in ElevationTypes.ALL_ELEVATION_TYPES)
        {
            sum += (float)biome.GetWeight() / (ElevationTypes.WEIGHTS_SUM);

            if (value < sum)
            {
                return biome.GetElevation();
            }
        }

        return 0;
    }

    private Biome ChooseVariation(ElevationType elevation, float value)
    {
        if (elevation.GetName() == ElevationTypes.DEEP_OCEAN.GetName() && !elevation.GetVariants().Contains(Biomes.DEEP_OCEAN))
        {

        }

        if (value < 0)
        {
            return elevation.GetRarestVariant();
        }

        float sum = 0f;
        int weightsSum = 0;

        foreach (var variant in elevation.GetVariants())
        {
            weightsSum += variant.GetWeight();
        }

        foreach (var variant in elevation.GetVariants())
        {
            sum += (float)variant.GetWeight() / (weightsSum);

            if (value < sum)
            {
                return variant;
            }
        }

        return elevation.GetMainVariant();
    }

    private HashSet<(int, int)> IdentifyMainContinent()
    {
        List<HashSet<(int, int)>> continents = new();

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (IsLand(x, y) && !Visited[x, y])
                {
                    var continent = new HashSet<(int, int)>();
                    FloodFill(x, y, continent);
                    continents.Add(continent);
                }
            }
        }

        return FindLargestContinent(continents);
    }

    private HashSet<(int, int)> FindLargestContinent(List<HashSet<(int, int)>> continents)
    {
        HashSet<(int, int)> largestContinent = null;
        int maxSize = 0;

        foreach (var continent in continents)
        {
            if (continent.Count > maxSize)
            {
                maxSize = continent.Count;
                largestContinent = continent;
            }
        }

        return largestContinent;
    }

    private List<HashSet<(int, int)>> IdentifyOceans()
    {
        List<HashSet<(int, int)>> oceans = new();

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (!IsLand(x, y) && !Visited[x, y])
                {
                    var ocean = new HashSet<(int, int)>();
                    FloodFillOcean(x, y, ocean);
                    oceans.Add(ocean);
                }
            }
        }

        return oceans;
    }

    private HashSet<(int, int)> FindLargestOcean(List<HashSet<(int, int)>> oceans)
    {
        HashSet<(int, int)> largestOcean = null;
        int maxSize = 0;

        foreach (var ocean in oceans)
        {
            if (ocean.Count > maxSize)
            {
                maxSize = ocean.Count;
                largestOcean = ocean;
            }
        }

        return largestOcean;
    }

    private bool IsLand(int x, int y)
    {
        return ElevationMap[x, y] > ElevationTypes.OCEAN.GetElevation();
    }

    private void FloodFill(int startX, int startY, HashSet<(int, int)> area)
    {
        var stack = new Stack<(int, int)>();
        stack.Push((startX, startY));

        while (stack.Count > 0)
        {
            var (x, y) = stack.Pop();

            if (x < 0 || x >= Width || y < 0 || y >= Height)
                continue;
            if (!IsLand(x, y) || Visited[x, y])
                continue;

            Visited[x, y] = true;
            area.Add((x, y));

            stack.Push((x + 1, y));
            stack.Push((x - 1, y));
            stack.Push((x, y + 1));
            stack.Push((x, y - 1));
        }
    }

    private void FloodFillOcean(int startX, int startY, HashSet<(int, int)> area)
    {
        var stack = new Stack<(int, int)>();
        stack.Push((startX, startY));

        while (stack.Count > 0)
        {
            var (x, y) = stack.Pop();

            if (x < 0 || x >= Width || y < 0 || y >= Height)
                continue;
            if (IsLand(x, y) || Visited[x, y])
                continue;

            Visited[x, y] = true;
            area.Add((x, y));

            stack.Push((x + 1, y));
            stack.Push((x - 1, y));
            stack.Push((x, y + 1));
            stack.Push((x, y - 1));
        }
    }

    private void ReplaceUnconnectedLand(HashSet<(int, int)> mainContinent)
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (IsLand(x, y) && !mainContinent.Contains((x, y)))
                {
                    ElevationMap[x, y] = 1;
                }
            }
        }
    }

    public void ReplaceUnconnectedOcean(List<HashSet<(int, int)>> oceans, HashSet<(int, int)> mainOcean)
    {
        foreach (var ocean in oceans)
        {
            if (ocean.Count > 30)
            {
                foreach ((int x, int y) tile in ocean)
                {
                    if (!mainOcean.Contains((tile.x, tile.y)))
                    {
                        ElevationMap[tile.x, tile.y] = 3;
                    }
                }
            }
            else
            {
                foreach ((int x, int y) tile in ocean)
                {
                    if (!mainOcean.Contains((tile.x, tile.y)))
                    {
                        ElevationMap[tile.x, tile.y] = 1;
                        VariationMap[tile.x, tile.y] = -1;
                    }
                }
            }
        }
    }

    public void ReplaceFarAwayBeaches()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (ElevationMap[x, y] == ElevationTypes.BEACH.GetElevation())
                {
                    if (!HasNearbyOceanTile(x, y, 2))
                    {
                        ElevationMap[x, y] = 3;
                    }
                }
            }
        }
    }

    private bool HasNearbyOceanTile(int centerX, int centerY, int radius)
    {
        for (int dx = -radius; dx <= radius; dx++)
        {
            for (int dy = -radius; dy <= radius; dy++)
            {
                int nx = centerX + dx;
                int ny = centerY + dy;

                if (nx >= 0 && nx < Width && ny >= 0 && ny < Height && !(dx == 0 && dy == 0))
                {
                    if (ElevationMap[nx, ny] == ElevationTypes.OCEAN.GetElevation() || ElevationMap[nx, ny] == ElevationTypes.DEEP_OCEAN.GetElevation())
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void PlaceConditionalFeatures(Tile[,] mapTiles)
    {
        Dictionary<Biome, NaturalFeature> naturalFeaturesDict = NaturalFeature.Features.GetBiomesToNaturalFeatures();

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Biome biome = mapTiles[x, y].GetBiome();
                if (naturalFeaturesDict.ContainsKey(biome))
                {
                    mapTiles[x, y].SetNaturalFeature(NaturalFeature.Features.Create(naturalFeaturesDict[biome]));
                }
            }
        }
    }

    public void GenerateRandomFeature(Tile[,] mapTiles, AbstractFeature feature)
    {
        RandomSpawnCondition rsc = (RandomSpawnCondition)feature.GetSpawnCondition();
        (int minAmount, int maxAmount) = rsc.GetAmounts();
        int minDistance = rsc.GetMinDistance();

        int numberOfStructures = UnityEngine.Random.Range(minAmount, maxAmount + 1);

        List<Coordinate> placedCoordinates = new List<Coordinate>();

        for (int i = 0; i < numberOfStructures; i++)
        {
            for (int attempts = 0; attempts < 500; attempts++)
            {
                int x = UnityEngine.Random.Range(0, Width);
                int y = UnityEngine.Random.Range(0, Height);

                Tile selectedTile = mapTiles[x, y];

                if (feature is StructureFeature structureFeature)
                {
                    if (IsTileSuitableForStructure(structureFeature, selectedTile, placedCoordinates, minDistance))
                    {
                        selectedTile.SetStructureFeature(StructureFeature.Features.Create(structureFeature));
                        placedCoordinates.Add(selectedTile.GetCoordinate());
                        break;
                    }
                }
                else if (feature is NaturalFeature naturalFeature)
                {
                    if (IsTileSuitableForFeature(naturalFeature, selectedTile, placedCoordinates, minDistance))
                    {
                        selectedTile.SetNaturalFeature(NaturalFeature.Features.Create(naturalFeature));
                        placedCoordinates.Add(selectedTile.GetCoordinate());
                        break;
                    }
                }

            }
        }
    }

    private bool IsTileSuitableForStructure(StructureFeature feature, Tile tile, List<Coordinate> placedCoordinates, int minDistance)
    {
        if (tile.GetNaturalFeature() != null && (tile.GetNaturalFeature().GetFeatureType().Equals(FeatureType.STRUCTURE_BLOCKING) || tile.GetNaturalFeature().GetFeatureType().Equals(FeatureType.BLOCKING)))
            return false;

        if (tile.GetStructureFeature() == null || tile.GetStructureFeature().GetFeatureType().Equals(FeatureType.NON_BLOCKING))
        {
            if (((RandomSpawnCondition)feature.GetSpawnCondition()).GetBiomes().Contains(tile.GetBiome()))
            {
                foreach (Coordinate coord in placedCoordinates)
                {
                    if (GetDistance(tile.GetCoordinate(), coord) < minDistance)
                        return false;
                }

                return true;
            }
        }

        return false;
    }

    private bool IsTileSuitableForFeature(NaturalFeature feature, Tile tile, List<Coordinate> placedCoordinates, int minDistance)
    {
        if (tile.GetStructureFeature() != null && (tile.GetStructureFeature().GetFeatureType().Equals(FeatureType.FEATURE_BLOCKING) || tile.GetStructureFeature().GetFeatureType().Equals(FeatureType.BLOCKING)))
            return false;

        if (tile.GetNaturalFeature() == null || tile.GetNaturalFeature().GetFeatureType().Equals(FeatureType.NON_BLOCKING))
        {
            if (((RandomSpawnCondition)feature.GetSpawnCondition()).GetBiomes().Contains(tile.GetBiome()))
            {
                foreach (Coordinate coord in placedCoordinates)
                {
                    if (GetDistance(tile.GetCoordinate(), coord) < minDistance)
                        return false;
                }

                return true;
            }
        }

        return false;
    }

    private float GetDistance(Coordinate a, Coordinate b)
    {
        return Mathf.Sqrt(Mathf.Pow(a.GetX() - b.GetX(), 2) + Mathf.Pow(a.GetY() - b.GetY(), 2));
    }

    private List<AbstractFeature> GetAllRandomFeatures()
    {
        List<AbstractFeature> allFeatures = new();

        allFeatures.AddRange(NaturalFeature.Features.GetAllNaturalFeatures());
        allFeatures.AddRange(StructureFeature.Features.GetAllStructureFeatures());

        List<AbstractFeature> filteredFeatures = new();

        foreach (var feature in allFeatures)
        {
            if (feature.GetSpawnCondition() is RandomSpawnCondition)
                filteredFeatures.Add(feature);
        }

        return filteredFeatures;
    }

    private void PlaceRandomFeatures(Tile[,] mapTiles)
    {
        List<AbstractFeature> features = GetAllRandomFeatures();

        foreach (var feature in features)
        {
            GenerateRandomFeature(mapTiles, feature);
        }
    }

    private void FinalizeLoot(Tile[,] mapTiles)
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                mapTiles[x, y].RollLoots();
            }
        }
    }
}
