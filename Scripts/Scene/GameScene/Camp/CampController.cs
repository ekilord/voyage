using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CampController : MonoBehaviour
{
    public GameObject GatheringCategory;
    public GameObject CombatCategory;
    public GameObject UtilityCategory;

    public GameObject GatheringButtonsHolder;
    public GameObject CombatButtonsHolder;
    public GameObject UtilityButtonsHolder;

    private GameObject InUseCategory;

    public static CampController instance;

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

    public void ToggleGatheringCategory()
    {
        ToggleCategory(GatheringCategory);
    }

    public void ToggleCombatCategory()
    {
        ToggleCategory(CombatCategory);
    }

    public void ToggleUtilityCategory()
    {
        ToggleCategory(UtilityCategory);
    }

    private void ToggleCategory(GameObject category)
    {
        if (InUseCategory == null)
        {
            MoveCategoryIn(category);
        }
        else
        {
            if (InUseCategory != category)
            {
                MoveCurrentCategoryOut();
                MoveCategoryIn(category);
            }
        }
    }

    private void MoveCategoryIn(GameObject category)
    {
        InUseCategory = category;

        category.transform.localPosition = new Vector2(810, 1080);
        UIUtils.MoveGameObjectAbsolutelyVerticallyEaseInOut(category, 0, 1f);
    }

    private void MoveCurrentCategoryOut()
    {
        UIUtils.MoveGameObjectAbsolutelyVerticallyEaseInOut(InUseCategory, -1080, 1f);
    }

    public void UpdateAvailabilities()
    {
        GameObject[] holders = { GatheringButtonsHolder, CombatButtonsHolder, UtilityButtonsHolder };

        foreach (GameObject holder in holders)
        {
            if (holder == null) continue;

            VariantEntity[] variants = holder.GetComponentsInChildren<VariantEntity>();

            foreach (VariantEntity variant in variants)
            {
                Image background = variant.transform.GetComponent<Image>();
                Image smallBackground = variant.transform.Find("CostImage").GetComponent<Image>();

                if (background != null && smallBackground != null)
                {
                    List<BuildingVariant> available = PlayerCharacter.GetBase().GetPossibleUpgrades();
                    List<BuildingVariant> owned = PlayerCharacter.GetBase().GetOwnedBuildings();
                    List<BuildingVariant> building = PlayerCharacter.GetBase().GetBuildingsInTimetable();

                    if (building.Contains(variant.GetVariant()))
                    {
                        variant.GetComponent<Button>().enabled = false;
                        background.color = new(255 / 255f, 235 / 255f, 170 / 255f, 1f);
                        smallBackground.color = new(255 / 255f, 240 / 255f, 180 / 255f, 1f);
                    }
                    else if (available.Contains(variant.GetVariant()))
                    {
                        if (variant.GetVariant().GetCost().IsEnough(PlayerCharacter.GetInventory()))
                        {
                            variant.GetComponent<Button>().enabled = true;
                            background.color = new(221 / 255f, 255 / 255f, 221 / 255f, 1f);
                            smallBackground.color = new(233 / 255f, 212 / 255f, 199 / 255f, 1f);
                        }
                        else
                        {
                            variant.GetComponent<Button>().enabled = false;
                            background.color = new(255 / 255f, 221 / 255f, 221 / 255f, 1f);
                            smallBackground.color = new(233 / 255f, 212 / 255f, 199 / 255f, 1f);
                        }
                    }
                    else
                    {
                        if (owned.Contains(variant.GetVariant()))
                        {
                            variant.GetComponent<Button>().enabled = false;
                            background.color = new(221 / 255f, 221 / 255f, 255 / 255f, 1f);
                            smallBackground.color = new(233 / 255f, 212 / 255f, 199 / 255f, 1f);
                        }
                    }
                }
            }
        }
    }

    public void TryToAcquireBuilding(BuildingVariant variant)
    {
        if (PlayerCharacter.TryToAcquireBuilding(variant))
        {
            UpdateAvailabilities();
        }
    }
}
