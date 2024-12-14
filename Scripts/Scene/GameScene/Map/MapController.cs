using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ElevationType;

public class MapController : MonoBehaviour
{
    public GameObject MapTilePrefab;
    public GameObject MapTilesHolder;

    public GameObject MapFeaturesPrefab;
    public GameObject MapFeaturesHolder;

    public GameObject MapEntityPrefab;
    public GameObject MapEntitiesHolder;

    public GameObject PathfindingHolder;

    private GameObject[,] MapTileEntities;
    private GameObject[,] MapFeatureEntities;
    private GameObject[,] MapEntities;
    private GameObject[,] Pathfinding;

    private HashSet<Coordinate> ScoutPositions;

    private Coordinate SelectedScout;

    private int MovementCost;
    private Coordinate ScoutDestination;

    public static MapController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        LoadScene();
    }

    public void LoadScene()
    {
        int width = PlayerCharacter.GetMap().GetTiles().GetLength(0);
        int height = PlayerCharacter.GetMap().GetTiles().GetLength(1);

        MapTileEntities = new GameObject[width, height];
        MapFeatureEntities = new GameObject[width, height];
        MapEntities = new GameObject[width, height];
        ScoutPositions = new HashSet<Coordinate>();
        Pathfinding = new GameObject[width, height];
        DisplayTiles();
        ShowExploredTiles();
        SetSelectedScout(null);
    }

    private void Update()
    {
        ShowExploredTiles();
    }

    public void ResetTiles()
    {
        DisplayTiles();
    }

    public Coordinate GetSelectedScout()
    {
        return SelectedScout;
    }

    public Coordinate GetScoutDestination()
    {
        return ScoutDestination;
    }

    public int GetMovementCost()
    {
        return MovementCost;
    }

    public void SetSelectedScout(Coordinate coords)
    {
        if (coords == null)
            HideAllPath();
        SelectedScout = coords;
        SetScoutDestination(null);
    }

    public void SetScoutDestination(Coordinate coords)
    {
        if (coords == null)
            HideAllPath();
        ScoutDestination = coords;
    }

    private void DisplayTiles()
    {
        int width = PlayerCharacter.GetMap().GetTiles().GetLength(0);
        int height = PlayerCharacter.GetMap().GetTiles().GetLength(1);

        Vector2 availableSpace = MapTilesHolder.GetComponent<RectTransform>().rect.size;

        float tileSize = Mathf.Min(availableSpace.x / width, availableSpace.y / height);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float xPos = (x - width / 2.0f + 0.5f) * tileSize;
                float yPos = (y - height / 2.0f + 0.5f) * tileSize;

                GameObject newTileEntity = Instantiate(MapTilePrefab, MapTilesHolder.transform, false);

                RectTransform tileRectTransform = newTileEntity.GetComponent<RectTransform>();
                if (tileRectTransform != null)
                {
                    tileRectTransform.sizeDelta = new Vector2(tileSize, tileSize);
                    tileRectTransform.anchoredPosition = new Vector2(xPos, yPos);
                }

                if (newTileEntity.TryGetComponent<TileEntity>(out var tileEntity))
                {
                    tileEntity.SetTile(PlayerCharacter.GetMap().GetTiles()[x, y]);
                }

                Biome currentBiome = PlayerCharacter.GetMap().GetTiles()[x, y].GetBiome();

                if (newTileEntity.TryGetComponent<Image>(out var renderer))
                {
                    renderer.color = currentBiome.GetColor();
                }

                MapTileEntities[x, y] = newTileEntity;

                if (tileEntity.GetTile().GetStructureFeature() != null)
                {
                    GameObject newFeatureEntity = Instantiate(MapFeaturesPrefab, MapFeaturesHolder.transform, false);

                    tileRectTransform = newFeatureEntity.GetComponent<RectTransform>();
                    if (tileRectTransform != null)
                    {
                        tileRectTransform.anchoredPosition = new Vector2(xPos, yPos);
                    }

                    newFeatureEntity.transform.localScale = new Vector3(tileSize, tileSize, 1);
                    if (newFeatureEntity.TryGetComponent<TextMeshProUGUI>(out var tmp))
                    {
                        tmp.text = tileEntity.GetTile().GetStructureFeature().GetName()[0].ToString();

                        tmp.fontSize = tileSize / 10;
                        tmp.enableAutoSizing = false;
                        tmp.raycastTarget = false;
                        tmp.color = new Color(0.8f, 0.8f, 0.8f);
                    }

                    MapFeatureEntities[x, y] = newFeatureEntity;
                }
                else if (tileEntity.GetTile().GetNaturalFeature() != null)
                {
                    GameObject newFeatureEntity = Instantiate(MapFeaturesPrefab, MapFeaturesHolder.transform, false);

                    tileRectTransform = newFeatureEntity.GetComponent<RectTransform>();
                    if (tileRectTransform != null)
                    {
                        tileRectTransform.anchoredPosition = new Vector2(xPos, yPos);
                    }

                    newFeatureEntity.transform.localScale = new Vector3(tileSize, tileSize, 1);
                    if (newFeatureEntity.TryGetComponent<TextMeshProUGUI>(out var tmp))
                    {
                        tmp.text = tileEntity.GetTile().GetNaturalFeature().GetName()[0].ToString();

                        tmp.fontSize = tileSize / 10;
                        tmp.enableAutoSizing = false;
                        tmp.raycastTarget = false;
                        tmp.color = new Color(0.8f, 0.8f, 0.8f);
                    }

                    MapFeatureEntities[x, y] = newFeatureEntity;
                }

                GameObject newEntity = Instantiate(MapEntityPrefab, MapEntitiesHolder.transform, false);

                tileRectTransform = newEntity.GetComponent<RectTransform>();
                if (tileRectTransform != null)
                {
                    tileRectTransform.sizeDelta = new Vector2(tileSize, tileSize);
                    tileRectTransform.anchoredPosition = new Vector2(xPos, yPos);
                }

                AbstractEntity entity = PlayerCharacter.GetMap().GetTiles()[x, y].GetEntityOccupation();
                if (entity is ScoutEntity)
                    ScoutPositions.Add(new Coordinate(x, y));

                if (entity != null)
                {
                    if (newEntity.TryGetComponent<Image>(out var renderer2))
                    {
                        renderer2.color = new Color(entity.GetColor().r, entity.GetColor().g, entity.GetColor().b, 0.35f);
                    }
                }
                else
                {
                    if (newEntity.TryGetComponent<Image>(out var renderer2))
                    {
                        renderer2.color = new Color(1f, 1f, 1f, 0.0f);
                    }
                    newEntity.SetActive(false);
                }

                MapEntities[x, y] = newEntity;

                GameObject newPathfindEntity = Instantiate(MapFeaturesPrefab, PathfindingHolder.transform, false);

                tileRectTransform = newPathfindEntity.GetComponent<RectTransform>();
                if (tileRectTransform != null)
                {
                    tileRectTransform.anchoredPosition = new Vector2(xPos, yPos);
                }

                newPathfindEntity.transform.localScale = new Vector3(tileSize, tileSize, 1);
                if (newPathfindEntity.TryGetComponent<TextMeshProUGUI>(out var tmp2))
                {
                    tmp2.text = "¤";
                    tmp2.fontSize = tileSize / 10;
                    tmp2.enableAutoSizing = false;
                    tmp2.raycastTarget = false;
                    Color col = UIUtils.Colors.Gold;
                    tmp2.color = new Color(col.r, col.g, col.b, 0.8f);
                }

                Pathfinding[x, y] = newPathfindEntity;
                newPathfindEntity.SetActive(false);

                TurnOffTile( x, y );
            }
        }
    }

    private void TurnOffTile(int x, int y)
    {
        if (MapTileEntities[x, y] != null)
        {
            MapTileEntities[x, y].SetActive(false);
        }
        if (MapFeatureEntities[x, y] != null)
        {
            MapFeatureEntities[x, y].SetActive(false);
        }
        if (MapEntities[x, y] != null)
        {
            MapEntities[x, y].SetActive(false);
        }
    }

    private void TurnOnTile(int x, int y)
    {
        if (MapTileEntities[x, y] != null)
        {
            MapTileEntities[x, y].SetActive(true);
        }
        if (MapFeatureEntities[x, y] != null)
        {
            MapFeatureEntities[x, y].SetActive(true);
        }
        if (MapEntities[x, y] != null)
        {
            MapEntities[x, y].SetActive(true);
        }
    }

    public void ShowExploredTiles()
    {
        int width = PlayerCharacter.GetMap().GetTiles().GetLength(0);
        int height = PlayerCharacter.GetMap().GetTiles().GetLength(1);

        Map map = PlayerCharacter.GetMap();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (map.GetTiles()[x, y].IsExplored())
                {
                    TurnOnTile(x, y);
                }
            }
        }
    }

    public void GoToDestination(int xDestination, int yDestination)
    {
        Coordinate origin = GetSelectedScout();

        if (GetSelectedScout() != null && PlayerCharacter.GetMap().GetTiles()[origin.GetX(), origin.GetY()].GetEntityOccupation() is ScoutEntity)
        {
            if (IsTraversable(PlayerCharacter.GetMap().GetTiles()[xDestination, yDestination]))
            {
                int path = GetCurrentPath(origin, new Coordinate(xDestination, yDestination));
                if (path > 0)
                {
                    ScoutEntity scout = (ScoutEntity)PlayerCharacter.GetMap().GetTiles()[GetSelectedScout().GetX(), GetSelectedScout().GetY()].GetEntityOccupation();
                    float time = path * scout.GetMovementSpeed();
                    int roundedTime = Mathf.RoundToInt(time) - 1;
                    if (PlayerCharacter.GetClock().CanAddHours(roundedTime))
                    {
                        MovementCost = roundedTime;
                        SetScoutDestination(new Coordinate(xDestination, yDestination));
                    }
                }
                else SetScoutDestination(null);
            }
        }
    }

    public void MoveScout()
    {
        if (GetSelectedScout() != null && GetScoutDestination() != null)
        {
            Coordinate origin = GetSelectedScout();
            Coordinate destination = GetScoutDestination();

            ScoutEntity scoutEntity = (ScoutEntity)PlayerCharacter.GetMap().GetTiles()[origin.GetX(), origin.GetY()].GetEntityOccupation();
            Entity enemyEntity = (Entity)PlayerCharacter.GetMap().GetTiles()[destination.GetX(), destination.GetY()].GetEntityOccupation();

            GameObject originObject = MapEntities[origin.GetX(), origin.GetY()];
            GameObject destinationObject = MapEntities[destination.GetX(), destination.GetY()];

            if (originObject != null)
            {
                if (destinationObject != null)
                {
                    var (isEmpty, startCombat) = PlayerCharacter.GetMap().ShouldMoveEntity(origin.GetX(), origin.GetY(), destination.GetX(), destination.GetY());

                    ScoutPositions.Remove(origin);
                    ScoutPositions.Add(destination);
                    SetSelectedScout(destination);
                    SetScoutDestination(null);

                    ClockController.instance.SkipTime(MovementCost);
                    ClockController.instance.UpdateClock();
                    PlayerCharacter.GetExperience().AddMovement(MovementCost);

                    if (originObject.TryGetComponent<Image>(out var renderer))
                    {
                        renderer.color = new Color(1f, 1f, 1f, 0f);
                    }
                    originObject.SetActive(false);

                    if (destinationObject.TryGetComponent<Image>(out var renderer2))
                    {
                        renderer2.color = new Color(scoutEntity.GetColor().r, scoutEntity.GetColor().g, scoutEntity.GetColor().b, 0.35f);
                    }
                    destinationObject.SetActive(true);

                    PlayerCharacter.GetMap().MoveEntity(origin.GetX(), origin.GetY(), destination.GetX(), destination.GetY());
                    PlayerCharacter.GetMap().ExploreTile(destination);
                    ShowExploredTiles();
                    HideAllPath();

                    if (!isEmpty && startCombat)
                    {
                        float delay = 2f;

                        BattleController.instance.CreateBatte(enemyEntity, delay + 0.75f);
                        ContainerController.instance.GoToBattle(delay);
                        ClockController.instance.HideClock();
                    }
                }

                return;
            }
        }
    }

    public void ForceMoveScout(Coordinate origin, Coordinate destination)
    {
        ScoutEntity scoutEntity = (ScoutEntity)PlayerCharacter.GetMap().GetTiles()[origin.GetX(), origin.GetY()].GetEntityOccupation();

        GameObject originObject = MapEntities[origin.GetX(), origin.GetY()];
        GameObject destinationObject = MapEntities[destination.GetX(), destination.GetY()];

        if (originObject != null)
        {
            if (destinationObject != null)
            {
                ScoutPositions.Remove(origin);
                ScoutPositions.Add(destination);
                SetSelectedScout(destination);

                if (originObject.TryGetComponent<Image>(out var renderer))
                {
                    renderer.color = new Color(1f, 1f, 1f, 0f);
                }
                originObject.SetActive(false);

                if (destinationObject.TryGetComponent<Image>(out var renderer2))
                {
                    renderer2.color = new Color(scoutEntity.GetColor().r, scoutEntity.GetColor().g, scoutEntity.GetColor().b, 0.35f);
                }
                destinationObject.SetActive(true);

                PlayerCharacter.GetMap().MoveEntity(origin.GetX(), origin.GetY(), destination.GetX(), destination.GetY());
                PlayerCharacter.GetMap().ExploreTile(destination);
                ShowExploredTiles();
            }

            return;
        }
    }

    private int GetCurrentPath(Coordinate start, Coordinate end)
    {
        HideAllPath();
        List<Coordinate> path = FindShortestPath(start, end);
        foreach (Coordinate coord in path)
        {
            Pathfinding[coord.GetX(), coord.GetY()].SetActive(true);
        }
        return path.Count;
    }

    private void HideAllPath()
    {
        foreach (GameObject element in Pathfinding)
        {
            element.SetActive(false);
        }
    }

    public List<Coordinate> FindShortestPath(Coordinate start, Coordinate end)
    {
        Tile[,] tiles = PlayerCharacter.GetMap().GetTiles();
        int width = tiles.GetLength(0);
        int height = tiles.GetLength(1);

        int[] dx = { 0, 0, 1, -1 };
        int[] dy = { 1, -1, 0, 0 };

        Queue<Coordinate> queue = new();
        Dictionary<Coordinate, Coordinate> cameFrom = new();

        queue.Enqueue(start);
        cameFrom[start] = null;

        while (queue.Count > 0)
        {
            Coordinate current = queue.Dequeue();

            if (current.Equals(end))
            {
                List<Coordinate> path = new();
                Coordinate step = end;
                while (step != null)
                {
                    path.Add(step);
                    step = cameFrom[step];
                }
                path.Reverse();
                return path;
            }

            for (int i = 0; i < 4; i++)
            {
                int nx = current.GetX() + dx[i];
                int ny = current.GetY() + dy[i];

                if (nx < 0 || nx >= width || ny < 0 || ny >= height)
                    continue;

                Coordinate neighbor = new(nx, ny);
                Tile neighborTile = tiles[nx, ny];

                if (cameFrom.ContainsKey(neighbor))
                    continue;

                if (!IsTraversable(neighborTile))
                    continue;

                queue.Enqueue(neighbor);
                cameFrom[neighbor] = current;
            }
        }

        return new List<Coordinate>();
    }

    private bool IsTraversable(Tile tile)
    {
        if (!tile.IsExplored() || ElevationTypes.GetElevationTypeFromBiome(tile.GetBiome()).GetElevation() < ElevationTypes.BEACH.GetElevation() || (tile.GetEntityOccupation() != null && tile.GetEntityOccupation() is ScoutEntity) || tile.GetEntityOccupation() is BaseEntity)
            return false;
        return true;
    }

    public bool Pillage()
    {
        if (GetSelectedScout() != null)
        {
            Coordinate selectedScout = GetSelectedScout();

            Tile tileToPillage = PlayerCharacter.GetMap().GetTiles()[selectedScout.GetX(), selectedScout.GetY()];

            int resources = tileToPillage.GetTotalResources();

            if (PlayerCharacter.GetClock().AddHours(Mathf.FloorToInt(resources / 10)))
            {
                PlayerCharacter.GetInventory().AddContents(tileToPillage.CollectResourcesFromTile());
                ClockController.instance.UpdateClock();
                StatsController.instance.UpdateEverything();

                return true;
            }
        }

        return false;
    }

    public void UpdateCampOnMap(Coordinate? oldLocation = null)
    {
        Coordinate coords = PlayerCharacter.GetCurrentBase().GetLocation();

        BaseEntity entity = (BaseEntity)PlayerCharacter.GetMap().GetTiles()[coords.GetX(), coords.GetY()].GetEntityOccupation();

        if (entity != null)
        {
            if (MapEntities[coords.GetX(), coords.GetY()].TryGetComponent<Image>(out var renderer2))
            {
                renderer2.color = new Color(entity.GetColor().r, entity.GetColor().g, entity.GetColor().b, 0.35f);
            }
            MapEntities[coords.GetX(), coords.GetY()].SetActive(true);
        }

        if (oldLocation != null)
        {
            if (MapEntities[oldLocation.GetX(), oldLocation.GetY()].TryGetComponent<Image>(out var renderer2))
            {
                renderer2.color = new Color(1f, 1f, 1f, 1f);
            }
            MapEntities[oldLocation.GetX(), oldLocation.GetY()].SetActive(false);
        }
    }

    public bool SetCamp()
    {
        if (CanRepairBase())
        {
            return RepairBase();
        }
        else
        {
            if (PlayerCharacter.GetCurrentBase().GetLocation() == null)
            {
                Coordinate location = GetSelectedScout();

                if (SwapScout())
                {
                    PlayerCharacter.GetMap().GetTiles()[location.GetX(), location.GetY()].SetEntityOccupation(PlayerCharacter.GetCurrentBase());
                    PlayerCharacter.GetCurrentBase().SetLocation(new Coordinate(location.GetX(), location.GetY()));
                    UpdateCampOnMap();

                    return true;
                }
            }
            else
            {
                if (PlayerCharacter.GetClock().CanAddHours(16))
                {
                    Coordinate oldLocation = PlayerCharacter.GetCurrentBase().GetLocation();
                    Coordinate location = GetSelectedScout();

                    if (SwapScout())
                    {
                        PlayerCharacter.GetMap().MoveEntity(oldLocation.GetX(), oldLocation.GetY(), location.GetX(), location.GetY());
                        PlayerCharacter.GetCurrentBase().SetLocation(new Coordinate(location.GetX(), location.GetY()));
                        UpdateCampOnMap(oldLocation);

                        PlayerCharacter.GetClock().AddHours(16);

                        ClockController.instance.UpdateClock();
                        StatsController.instance.UpdateEverything();

                        return true;
                    }
                }
            }
        }

        return false;
    }

    public bool SwapScout()
    {
        if (PlayerCharacter.GetMap().GetTiles()[GetSelectedScout().GetX() + 1, GetSelectedScout().GetY()].GetEntityOccupation() == null && IsTraversable(PlayerCharacter.GetMap().GetTiles()[GetSelectedScout().GetX() + 1, GetSelectedScout().GetY()]))
        {
            ForceMoveScout(GetSelectedScout(), new Coordinate(GetSelectedScout().GetX() + 1, GetSelectedScout().GetY()));
            return true;
        }
        else if (PlayerCharacter.GetMap().GetTiles()[GetSelectedScout().GetX(), GetSelectedScout().GetY() + 1].GetEntityOccupation() == null && IsTraversable(PlayerCharacter.GetMap().GetTiles()[GetSelectedScout().GetX(), GetSelectedScout().GetY() + 1]))
        {
            ForceMoveScout(GetSelectedScout(), new Coordinate(GetSelectedScout().GetX(), GetSelectedScout().GetY() + 1));
            return true;
        }
        else if (PlayerCharacter.GetMap().GetTiles()[GetSelectedScout().GetX() - 1, GetSelectedScout().GetY()].GetEntityOccupation() == null && IsTraversable(PlayerCharacter.GetMap().GetTiles()[GetSelectedScout().GetX() - 1, GetSelectedScout().GetY()]))
        {
            ForceMoveScout(GetSelectedScout(), new Coordinate(GetSelectedScout().GetX() - 1, GetSelectedScout().GetY()));
            return true;
        }
        else if (PlayerCharacter.GetMap().GetTiles()[GetSelectedScout().GetX(), GetSelectedScout().GetY() - 1].GetEntityOccupation() == null && IsTraversable(PlayerCharacter.GetMap().GetTiles()[GetSelectedScout().GetX(), GetSelectedScout().GetY() - 1]))
        {
            ForceMoveScout(GetSelectedScout(), new Coordinate(GetSelectedScout().GetX(), GetSelectedScout().GetY() - 1));
            return true;
        }

        return false;
    }

    public bool RepairBase()
    {
        if (GetSelectedScout() != null && GetScoutDestination() == null)
        {
            Tile baseTile = null;

            if (PlayerCharacter.GetMap().GetTiles()[GetSelectedScout().GetX() + 1, GetSelectedScout().GetY()].GetEntityOccupation() is BaseEntity)
            {
                baseTile = PlayerCharacter.GetMap().GetTiles()[GetSelectedScout().GetX() + 1, GetSelectedScout().GetY()];
            }
            else if (PlayerCharacter.GetMap().GetTiles()[GetSelectedScout().GetX(), GetSelectedScout().GetY() + 1].GetEntityOccupation() is BaseEntity)
            {
                baseTile = PlayerCharacter.GetMap().GetTiles()[GetSelectedScout().GetX(), GetSelectedScout().GetY() + 1];
            }
            else if (PlayerCharacter.GetMap().GetTiles()[GetSelectedScout().GetX() - 1, GetSelectedScout().GetY()].GetEntityOccupation() is BaseEntity)
            {
                baseTile = PlayerCharacter.GetMap().GetTiles()[GetSelectedScout().GetX() - 1, GetSelectedScout().GetY()];
            }
            else if (PlayerCharacter.GetMap().GetTiles()[GetSelectedScout().GetX(), GetSelectedScout().GetY() - 1].GetEntityOccupation() is BaseEntity)
            {
                baseTile = PlayerCharacter.GetMap().GetTiles()[GetSelectedScout().GetX(), GetSelectedScout().GetY() - 1];
            }

            if (baseTile != null && ((BaseEntity)baseTile.GetEntityOccupation()).IsOld())
            {
                if (PlayerCharacter.GetClock().AddHours(8))
                {
                    RemoveEntity(PlayerCharacter.GetOldBase().GetLocation());
                    RemoveEntity(PlayerCharacter.GetCurrentBase().GetLocation());
                    PlayerCharacter.RepairBase();
                    AddEntity(PlayerCharacter.GetCurrentBase().GetLocation());
                    ClockController.instance.UpdateClock();

                    return true;
                }
            }
        }

        return false;
    }

    public bool CanRepairBase()
    {
        if (GetSelectedScout() != null && GetScoutDestination() == null)
        {
            Tile baseTile = null;

            if (PlayerCharacter.GetMap().GetTiles()[GetSelectedScout().GetX() + 1, GetSelectedScout().GetY()].GetEntityOccupation() is BaseEntity)
            {
                baseTile = PlayerCharacter.GetMap().GetTiles()[GetSelectedScout().GetX() + 1, GetSelectedScout().GetY()];
            }
            else if (PlayerCharacter.GetMap().GetTiles()[GetSelectedScout().GetX(), GetSelectedScout().GetY() + 1].GetEntityOccupation() is BaseEntity)
            {
                baseTile = PlayerCharacter.GetMap().GetTiles()[GetSelectedScout().GetX(), GetSelectedScout().GetY() + 1];
            }
            else if (PlayerCharacter.GetMap().GetTiles()[GetSelectedScout().GetX() - 1, GetSelectedScout().GetY()].GetEntityOccupation() is BaseEntity)
            {
                baseTile = PlayerCharacter.GetMap().GetTiles()[GetSelectedScout().GetX() - 1, GetSelectedScout().GetY()];
            }
            else if (PlayerCharacter.GetMap().GetTiles()[GetSelectedScout().GetX(), GetSelectedScout().GetY() - 1].GetEntityOccupation() is BaseEntity)
            {
                baseTile = PlayerCharacter.GetMap().GetTiles()[GetSelectedScout().GetX(), GetSelectedScout().GetY() - 1];
            }

            return baseTile != null && ((BaseEntity)baseTile.GetEntityOccupation()).IsOld();
        }

        return false;
    }

    public void NightMovement()
    {
        int width = PlayerCharacter.GetMap().GetTiles().GetLength(0);
        int height = PlayerCharacter.GetMap().GetTiles().GetLength(1);

        for (int i = 0; i < height; i++)
        {
            for (int k = 0; k < width; k++)
            {
                Tile tile = PlayerCharacter.GetMap().GetTiles()[k, i];

                if (tile != null && tile.GetEntityOccupation() != null && tile.GetEntityOccupation() is not ScoutEntity && tile.GetEntityOccupation() is not BaseEntity)
                {
                    MoveEntityRandomly(tile.GetCoordinate());
                }
            }
        }
    }

    private bool IsFreeTile(Tile tile)
    {
        if (ElevationTypes.GetElevationTypeFromBiome(tile.GetBiome()).GetElevation() < ElevationTypes.BEACH.GetElevation() || tile.GetEntityOccupation() != null)
            return false;
        return true;
    }

    private void MoveEntityRandomly(Coordinate origin)
    {
        int radius = 2;
        int width = PlayerCharacter.GetMap().GetTiles().GetLength(0);
        int height = PlayerCharacter.GetMap().GetTiles().GetLength(1);

        List<Coordinate> validCoordinates = new();

        for (int dx = -radius; dx <= radius; dx++)
        {
            for (int dy = -radius; dy <= radius; dy++)
            {
                int targetX = origin.GetX() + dx;
                int targetY = origin.GetY() + dy;

                if (targetX >= 0 && targetX < width && targetY >= 0 && targetY < height && (dx != 0 || dy != 0))
                {
                    Tile targetTile = PlayerCharacter.GetMap().GetTiles()[targetX, targetY];

                    if (targetTile != null && targetTile.GetEntityOccupation() == null && IsFreeTile(targetTile))
                    {
                        validCoordinates.Add(new Coordinate(targetX, targetY));
                    }
                }
            }
        }

        if (validCoordinates.Count > 0)
        {
            Coordinate randomDestination = validCoordinates[Random.Range(0, validCoordinates.Count)];
            MoveEntity(origin, randomDestination);
        }
    }


    public void MoveEntity(Coordinate origin, Coordinate destination)
    {
        GameObject originObject = MapEntities[origin.GetX(), origin.GetY()];
        GameObject destinationObject = MapEntities[destination.GetX(), destination.GetY()];

        AbstractEntity entity = PlayerCharacter.GetMap().GetTiles()[origin.GetX(), origin.GetY()].GetEntityOccupation();

        if (originObject != null && destinationObject != null && entity != null)
        {
            if (originObject.TryGetComponent<Image>(out var renderer))
            {
                renderer.color = new Color(1f, 1f, 1f, 0f);
            }
            originObject.SetActive(false);

            if (PlayerCharacter.GetMap().GetTiles()[origin.GetX(), origin.GetY()].IsExplored())
            {
                if (destinationObject.TryGetComponent<Image>(out var renderer2))
                {
                    renderer2.color = new Color(entity.GetColor().r, entity.GetColor().g, entity.GetColor().b, 0.35f);
                }
                destinationObject.SetActive(true);
            }

            PlayerCharacter.GetMap().MoveEntity(origin.GetX(), origin.GetY(), destination.GetX(), destination.GetY());

            return;
        }
    }

    public void RemoveEntity(Coordinate origin)
    {
        if (origin == null) return;
        GameObject originObject = MapEntities[origin.GetX(), origin.GetY()];

        if (originObject != null)
        {
            if (originObject.TryGetComponent<Image>(out var renderer))
            {
                renderer.color = new Color(1f, 1f, 1f, 0f);
            }
            originObject.SetActive(false);
        }
    }

    public void AddEntity(Coordinate origin)
    {
        GameObject originObject = MapEntities[origin.GetX(), origin.GetY()];
        AbstractEntity entity = PlayerCharacter.GetMap().GetTiles()[origin.GetX(), origin.GetY()].GetEntityOccupation();

        if (originObject != null && entity != null)
        {
            if (originObject.TryGetComponent<Image>(out var renderer))
            {
                renderer.color = new Color(entity.GetColor().r, entity.GetColor().g, entity.GetColor().b, entity.GetColor().a);
            }
            originObject.SetActive(true);
        }
    }

}
