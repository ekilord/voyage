using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MapHUDController : MonoBehaviour
{
    public TMP_Text BiomeName;
    public TMP_Text BiomeCoordinates;

    public TMP_Text NaturalFeatureName;
    public TMP_Text NaturalFeatureStats;

    public TMP_Text StructureFeatureName;
    public TMP_Text StructureFeatureStats;

    public TMP_Text EntityName;

    public GameObject FeatureInfo;
    public GameObject StructureInfo;
    public GameObject EntityInfo;

    public Button PillageButton;
    public TMP_Text PillageText;

    public Button MoveButton;
    public TMP_Text MoveText;

    public Button SetCampButton;
    public TMP_Text SetCampText;

    private Tile SelectedTile;
    private Tile OldSelectedTile;

    public static MapHUDController instance;

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

        SetBiomeTexts();
        SetNaturalFeatureTexts();
        SetStructureFeatureTexts();
        SetEntityTexts();
    }

    private void Update()
    {
        UpdateButtons();
    }

    public void SetSelectedTile(Tile tile)
    {
        OldSelectedTile = SelectedTile;
        SelectedTile = tile;
        SetBiomeTexts();
        SetNaturalFeatureTexts();
        SetStructureFeatureTexts();
        CheckForTraversal();
        ShowAvailableInfo();
        UpdateButtons();
        ScoutButtonsToggle();
    }

    public void SetBiomeTexts()
    {
        if (SelectedTile != null && BiomeName != null && BiomeCoordinates != null)
        {
            BiomeName.text = SelectedTile.GetBiome().GetName();
            BiomeCoordinates.text = $"X: {SelectedTile.GetCoordinate().GetX()}, Y: {SelectedTile.GetCoordinate().GetY()}";
            return;
        }

        BiomeName.text = "";
        BiomeCoordinates.text = "";
    }

    public void SetNaturalFeatureTexts()
    {
        if (NaturalFeatureName != null && NaturalFeatureStats != null)
        {
            if (SelectedTile != null && SelectedTile.GetNaturalFeature() != null)
            {
                LootTable? lt = SelectedTile.GetNaturalFeature().GetLootTable();

                NaturalFeatureName.text = SelectedTile.GetNaturalFeature().GetName();
                if (lt != null) NaturalFeatureStats.text = ConcatenateResources(lt);
                else NaturalFeatureStats.text = "";
                return;
            }
        }

        NaturalFeatureName.text = "";
        NaturalFeatureStats.text = "";
    }

    public void SetStructureFeatureTexts()
    {
        if (StructureFeatureName != null && StructureFeatureStats != null)
        {
            if (SelectedTile != null && SelectedTile.GetStructureFeature() != null)
            {
                LootTable? lt = SelectedTile.GetStructureFeature().GetLootTable();

                StructureFeatureName.text = SelectedTile.GetStructureFeature().GetName();
                if (lt != null) StructureFeatureStats.text = ConcatenateResources(lt);
                else StructureFeatureStats.text = "";
                return;
            }
        }

        StructureFeatureName.text = "";
        StructureFeatureStats.text = "";
    }

    public void SetEntityTexts()
    {
        if (EntityName != null)
        {
            if (MapController.instance.GetSelectedScout() == null)
            {
                if (SelectedTile != null && SelectedTile.GetEntityOccupation() != null)
                {
                    EntityName.text = SelectedTile.GetEntityOccupation().GetName();

                    return;
                }
            }
            else
            {
                Coordinate coords = MapController.instance.GetSelectedScout();
                EntityName.text = PlayerCharacter.GetMap().GetTiles()[coords.GetX(), coords.GetY()].GetEntityOccupation().GetName();

                return;
            }
        }

        EntityName.text = "";
    }

    public void CheckForTraversal()
    {
        SetEntityTexts();

        if (SelectedTile == OldSelectedTile)
        {
            MapController.instance.SetScoutDestination(null);
            SelectedTile = null;

            return;
        }

        AbstractEntity entity = SelectedTile.GetEntityOccupation();
        var selectedScout = MapController.instance.GetSelectedScout();

        if (entity != null)
        {
            if (entity is ScoutEntity)
            {
                MapController.instance.SetSelectedScout(selectedScout == null ? SelectedTile.GetCoordinate() : null);

                return;
            }
        }

        if (SelectedTile != null) MapController.instance.GoToDestination(SelectedTile.GetCoordinate().GetX(), SelectedTile.GetCoordinate().GetY());
    }
    private string ConcatenateResources(LootTable lt)
    {
        string resources = "";

        resources += lt.GetMainEntry().GetAmount() + " " + lt.GetMainEntry().GetResource().GetResourceType()[0];

        if (lt.GetAllSecondaryEntries() != null)
        {
            foreach (var entry in lt.GetAllSecondaryEntries())
            {
                resources += ", " + entry.GetAmount() + " " + entry.GetResource().GetResourceType()[0];
            }
        }

        return resources;
    }

    public void MoveScout()
    {
        MapController.instance.MoveScout();
        Coordinate scoutDestination = MapController.instance.GetScoutDestination();
        ScoutButtonsToggle();
    }

    public void ShowAvailableInfo()
    {
        if (SelectedTile != null)
        {
            if (SelectedTile.GetNaturalFeature() != null)
            {
                UIUtils.MoveGameObjectAbsolutelyHorizontallyEaseInOut(FeatureInfo, -12, 0.25f);
            }
            else
            {
                UIUtils.MoveGameObjectAbsolutelyHorizontallyEaseInOut(FeatureInfo, 300, 0.25f);
            }

            if (SelectedTile.GetStructureFeature() != null)
            {
                UIUtils.MoveGameObjectAbsolutelyHorizontallyEaseInOut(StructureInfo, -80, 0.25f);
            }
            else
            {
                UIUtils.MoveGameObjectAbsolutelyHorizontallyEaseInOut(StructureInfo, 300, 0.25f);
            }

            if (SelectedTile.GetEntityOccupation() != null || MapController.instance.GetSelectedScout() != null)
            {
                UIUtils.MoveGameObjectAbsolutelyHorizontallyEaseInOut(EntityInfo, -60, 0.25f);
            }
            else
            {
                UIUtils.MoveGameObjectAbsolutelyHorizontallyEaseInOut(EntityInfo, 300, 0.25f);
            }
        }
    }

    public void ScoutButtonsToggle()
    {
        Coordinate selectedScout = MapController.instance.GetSelectedScout();
        Coordinate scoutDestination = MapController.instance.GetScoutDestination();

        if (selectedScout != null)
        {
            if (scoutDestination != null)
            {
                MoveText.text = $"Move\n({MapController.instance.GetMovementCost()}h)";
                UIUtils.MoveGameObjectAbsolutelyVerticallyEaseInOut(MoveButton.gameObject, -350, 0.25f);
                UIUtils.MoveGameObjectAbsolutelyVerticallyEaseInOut(PillageButton.gameObject, 0, 0.25f);
                UIUtils.MoveGameObjectAbsolutelyVerticallyEaseInOut(SetCampButton.gameObject, 0, 0.25f);
            }
            else
            {
                UIUtils.MoveGameObjectAbsolutelyVerticallyEaseInOut(MoveButton.gameObject, 0, 0.25f);

                int resources = PlayerCharacter.GetMap().GetTiles()[selectedScout.GetX(), selectedScout.GetY()].GetTotalResources();

                if (resources > 0)
                {
                    PillageText.text = $"Pillage\n({Mathf.FloorToInt(resources / 10)}h)";
                    UIUtils.MoveGameObjectAbsolutelyVerticallyEaseInOut(PillageButton.gameObject, -350, 0.25f);
                }
                else
                    UIUtils.MoveGameObjectAbsolutelyVerticallyEaseInOut(PillageButton.gameObject, 0, 0.25f);

                if (MapController.instance.CanRepairBase())
                {
                    SetCampText.text = "Repair Camp\n(8h)";
                }
                else
                {
                    if (PlayerCharacter.GetCurrentBase().GetLocation() == null) SetCampText.text = "Make Camp";
                    else SetCampText.text = "Move Camp\n(16h)";
                }

                UIUtils.MoveGameObjectAbsolutelyVerticallyEaseInOut(SetCampButton.gameObject, -350, 0.25f);
            }

            return;
        }

        UIUtils.MoveGameObjectAbsolutelyVerticallyEaseInOut(PillageButton.gameObject, 0, 0.25f);
        UIUtils.MoveGameObjectAbsolutelyVerticallyEaseInOut(MoveButton.gameObject, 0, 0.25f);
        UIUtils.MoveGameObjectAbsolutelyVerticallyEaseInOut(SetCampButton.gameObject, 0, 0.25f);
    }

    public void UpdateButtons()
    {
        Coordinate selectedScout = MapController.instance.GetSelectedScout();

        if (selectedScout != null)
        {
            int resources = PlayerCharacter.GetMap().GetTiles()[selectedScout.GetX(), selectedScout.GetY()].GetTotalResources();

            if (resources > 0 && PlayerCharacter.GetClock().CanAddHours(Mathf.FloorToInt(resources / 10)))
            {
                PillageButton.enabled = true;
                PillageButton.gameObject.GetComponent<Image>().color = UIUtils.Colors.ColorFromInt(255, 255, 255);
            }
            else
            {
                PillageButton.enabled = false;
                PillageButton.gameObject.GetComponent<Image>().color = UIUtils.Colors.ColorFromInt(180, 180, 180);
            }

            Coordinate scoutDestination = MapController.instance.GetScoutDestination();
            int movementCost = MapController.instance.GetMovementCost();

            if (scoutDestination != null && movementCost > 0)
            {
                if (PlayerCharacter.GetClock().CanAddHours(movementCost))
                {
                    MoveButton.enabled = true;
                    MoveButton.gameObject.GetComponent<Image>().color = UIUtils.Colors.ColorFromInt(255, 255, 255);
                }
                else
                {
                    MoveButton.enabled = false;
                    MoveButton.gameObject.GetComponent<Image>().color = UIUtils.Colors.ColorFromInt(180, 180, 180);
                }
            }

            if (PlayerCharacter.GetCurrentBase().GetLocation() == null)
            {
                SetCampButton.enabled = true;
                SetCampButton.gameObject.GetComponent<Image>().color = UIUtils.Colors.ColorFromInt(255, 255, 255);
            }
            else
            {
                if (PlayerCharacter.GetClock().CanAddHours(16))
                {
                    SetCampButton.enabled = true;
                    SetCampButton.gameObject.GetComponent<Image>().color = UIUtils.Colors.ColorFromInt(255, 255, 255);
                }
                else
                {
                    SetCampButton.enabled = false;
                    SetCampButton.gameObject.GetComponent<Image>().color = UIUtils.Colors.ColorFromInt(180, 180, 180);
                }
            }
        }
    }

    public void Pillage()
    {
        if (MapController.instance.Pillage())
        {
            SetNaturalFeatureTexts();
            SetStructureFeatureTexts();
            ShowAvailableInfo();
            UpdateButtons();
            ScoutButtonsToggle();
        }
    }

    public void SetCamp()
    {
        if (MapController.instance.SetCamp())
        {
            SetNaturalFeatureTexts();
            SetStructureFeatureTexts();
            ShowAvailableInfo();
            UpdateButtons();
            ScoutButtonsToggle();
        }
    }
}
