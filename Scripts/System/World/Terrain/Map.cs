using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using static Biome;
using static ElevationType;

public class Map
{
    //[SerializeField]
    //private SerializableTileArray MapTilesWrapper;

    [JsonProperty]
    private Tile[,] MapTiles;
	[JsonProperty]
	private int Seed;
	[JsonProperty]
	private Coordinate SpawnPoint;

	public Map()
	{
		System.Random random = new();
		int randomNumber = random.Next( 0, 100000000 );

		Seed = Mathf.RoundToInt( Mathf.Sqrt( randomNumber ) );
		MapGenerator generator = new( 100, 50, 19.5f, 0.5f, 60f, Seed );

		MapTiles = generator.GenerateWorld();

        PopulateMapWithEntities();

        SpawnPoint = GenerateSpawnPoint();

		ExploreTile( SpawnPoint );
	}

	public Map(Tile[,] mapTiles, int seed, Coordinate spawnPoint)
	{
		MapTiles = mapTiles;
		Seed = seed;
		SpawnPoint = spawnPoint;
	}


	public Tile[,] GetTiles()
	{
		return MapTiles;
	}

	public Coordinate GetSpawnPoint()
	{
		return SpawnPoint;
	}

	public void ExploreTile( Coordinate coords )
	{
		ExploreTile( coords.GetX(), coords.GetY() );
	}

	public void ExploreTile( int xOrigin, int yOrigin )
	{
		int radius = 2;
		int radiusSquared = radius * radius;

		int width = MapTiles.GetLength( 0 );
		int height = MapTiles.GetLength( 1 );

		for ( int x = xOrigin - radius; x <= xOrigin + radius; x++ ) {
			for ( int y = yOrigin - radius; y <= yOrigin + radius; y++ ) {
				if ( x >= 0 && x < width && y >= 0 && y < height ) {
					int dx = x - xOrigin;
					int dy = y - yOrigin;

					if ( dx * dx + dy * dy <= radiusSquared ) {
						MapTiles[x, y].SetToExplored();
                        Debug.Log($"SetToExplored: {x}, {y}");
					}
				}
			}
		}
	}

	public (bool isEmpty, bool startCombat) ShouldMoveEntity( int xOrigin, int yOrigin, int xDestination, int yDestination )
	{
		int width = MapTiles.GetLength( 0 );
		int height = MapTiles.GetLength( 1 );

		if ( xOrigin < 0 || xOrigin >= width || yOrigin < 0 || yOrigin >= height ||
			xDestination < 0 || xDestination >= width || yDestination < 0 || yDestination >= height ) {
			Debug.LogWarning( "MoveEntity: Coordinates out of bounds." );
			return (false, false);
		}

		AbstractEntity entity = MapTiles[xOrigin, yOrigin].GetEntityOccupation();
		if ( entity == null ) {
			Debug.LogWarning( "MoveEntity: No entity at the origin position." );
			return (false, false);
		}

		if ( MapTiles[xDestination, yDestination].GetEntityOccupation() != null ) {
			if ( MapTiles[xDestination, yDestination].GetEntityOccupation().GetPlayerRelation() == PlayerRelation.HOSTILE )
				return (false, true);
			else
				return (false, false);
		}
		else {
			return (true, false);
		}
	}

	public void MoveEntity( int xOrigin, int yOrigin, int xDestination, int yDestination )
	{
		AbstractEntity entity = MapTiles[xOrigin, yOrigin].GetEntityOccupation();
		MapTiles[xOrigin, yOrigin].SetEntityOccupation( null );
		MapTiles[xDestination, yDestination].SetEntityOccupation( entity );
    }

    public Coordinate GenerateSpawnPoint()
    {
        int mapWidth = MapTiles.GetLength(0);
        int mapHeight = MapTiles.GetLength(1);

        int minX = mapWidth / 2;
        int maxX = mapWidth;
        int minY = 0;
        int maxY = mapHeight / 2;

        var validBiomes = Biomes.GetBeachBiomes();

        int attempts = 500;

        for (int i = 0; i < attempts; i++)
        {
            int x = UnityEngine.Random.Range(minX, maxX);
            int y = UnityEngine.Random.Range(minY, maxY);

            Tile tile = MapTiles[x, y];

            if (validBiomes.Contains(tile.GetBiome())
                && (tile.GetNaturalFeature() == null || tile.GetNaturalFeature().GetSpawnCondition() is not RandomSpawnCondition)
                 && tile.GetStructureFeature() == null)
            {
                if (tile.GetEntityOccupation() == null)
                {
                    MapTiles[x, y].SetEntityOccupation(ScoutEntity.Scouts.NOVICE_SCOUTS);
                    return new Coordinate(x, y);
                }
            }
        }

        return new Coordinate(-1, -1);
    }

    public void PopulateMapWithEntities()
    {
        Dictionary<Coordinate, int> proximityData = PopulateDictionaryWithProximityData(MapTiles);
        Dictionary<Coordinate, int> categorizedData = CategorizeProximityData(proximityData);

        foreach (var data in categorizedData)
        {
            MapTiles[data.Key.GetX(), data.Key.GetY()].SetEntityOccupation(Entity.Entities.GetEnemyEntityBasedOnStrength(data.Value));
        }
    }

    private Dictionary<Coordinate, int> PopulateDictionaryWithProximityData(Tile[,] mapTiles)
    {
        Dictionary<Coordinate, int> dict = new();

        int width = mapTiles.GetLength(0);
        int height = mapTiles.GetLength(1);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Tile currentTile = mapTiles[x, y];

                bool hasStructure = currentTile.GetStructureFeature() != null;
                bool hasRandomSpawn = currentTile.GetNaturalFeature()?.GetSpawnCondition() is RandomSpawnCondition;

                if (hasStructure || hasRandomSpawn)
                {
                    int proximity = CalculateProximityToOcean(mapTiles, width, height, x, y);

                    dict[new Coordinate(x, y)] = proximity;
                }
            }
        }

        return dict;
    }

    //BFS search
    private int CalculateProximityToOcean(Tile[,] mapTiles, int width, int height, int xOrigin, int yOrigin)
    {
        List<ElevationType> oceans = new() { ElevationTypes.OCEAN, ElevationTypes.DEEP_OCEAN };

        int[] dx = { 0, 0, -1, 1 };
        int[] dy = { -1, 1, 0, 0 };

        Queue<(int x, int y, int distance)> queue = new();
        HashSet<(int, int)> visited = new();

        queue.Enqueue((xOrigin, yOrigin, 0));
        visited.Add((xOrigin, yOrigin));

        while (queue.Count > 0)
        {
            var (x, y, distance) = queue.Dequeue();


            if (oceans.Contains(ElevationTypes.GetElevationTypeFromBiome(mapTiles[x, y].GetBiome())))
            {
                return distance;
            }

            for (int i = 0; i < 4; i++)
            {
                int newX = x + dx[i];
                int newY = y + dy[i];

                if (newX >= 0 && newX < width && newY >= 0 && newY < height && !visited.Contains((newX, newY)))
                {
                    queue.Enqueue((newX, newY, distance + 1));
                    visited.Add((newX, newY));
                }
            }
        }

        return -1;
    }

    private Dictionary<Coordinate, int> CategorizeProximityData(Dictionary<Coordinate, int> proximityData)
    {
        if (proximityData.Count == 0)
            return proximityData;

        int minProximity = proximityData.Values.Min();
        int maxProximity = proximityData.Values.Max();

        double range = maxProximity - minProximity;
        double segmentSize = range / 6.0;

        Dictionary<Coordinate, int> categorizedData = new();

        foreach (var kvp in proximityData)
        {
            int proximity = kvp.Value;

            int category = (int)Math.Ceiling((proximity - minProximity + 1) / segmentSize);

            category = Math.Clamp(category, 1, 6);

            categorizedData[kvp.Key] = category;
        }

        return categorizedData;
    }

    public void RemoveScouts()
    {
        foreach(Tile tile in MapTiles)
        {
            if (tile.GetEntityOccupation() is AbstractEntity) tile.SetEntityOccupation(null);
        }
    }

    public void GenerateNewSpawnPoint()
    {
        SpawnPoint = GenerateSpawnPoint();
    }
}
