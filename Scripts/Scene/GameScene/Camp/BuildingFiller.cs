using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingFiller : MonoBehaviour
{
    public GameObject GatheringButtonsHolder;
    public GameObject CombatButtonsHolder;
    public GameObject UtilityButtonsHolder;

    public GameObject BuildingButtonPrefab;

    private Vector2 startPosition = new(-450, 230);
    private Vector2 offsetX = new(225, 0);
    private Vector2[] positionsInGroup = new Vector2[] 
    {
        new(0, 0),
        new(0, -230),
        new(0, -460)
    };

    void Start()
    {
        CreateBuildingButtons();
    }

    private void CreateBuildingButtons()
    {
        InstantiateGroup(Building.Buildings.WOOD, 0, GatheringButtonsHolder);
        InstantiateGroup(Building.Buildings.STONE, 1, GatheringButtonsHolder);
        InstantiateGroup(Building.Buildings.IRON, 2, GatheringButtonsHolder);
        InstantiateGroup(Building.Buildings.AETHER, 3, GatheringButtonsHolder);
        InstantiateGroup(Building.Buildings.GOLD, 4, GatheringButtonsHolder);

        InstantiateGroup(Building.Buildings.ATTACK, 1, CombatButtonsHolder);
        InstantiateGroup(Building.Buildings.ARCANE, 2, CombatButtonsHolder);
        InstantiateGroup(Building.Buildings.DEFENSE, 3, CombatButtonsHolder);

        InstantiateGroup(Building.Buildings.INITIATIVE, 1, UtilityButtonsHolder);
        InstantiateGroup(Building.Buildings.MORALE, 2, UtilityButtonsHolder);
        InstantiateGroup(Building.Buildings.HP, 3, UtilityButtonsHolder);
    }

    private void InstantiateGroup(Building building, int groupIndex, GameObject holder)
    {
        Vector2 groupPosition = startPosition + groupIndex * offsetX;

        for (int k = 0; k < 3; k++)
        {
            Vector2 position = groupPosition + positionsInGroup[k];
            GameObject buttonInstance = Instantiate(BuildingButtonPrefab, new Vector3(0, 0), Quaternion.identity, holder.transform);
            buttonInstance.GetComponent<Button>().enabled = false;
            BuildingVariant variant = building.GetVariants()[k];
            buttonInstance.GetComponent<VariantEntity>().SetVariant(variant);
            buttonInstance.transform.localPosition = position;

            TMP_Text buttonTitle = buttonInstance.transform.Find("ButtonTitle").GetComponent<TMP_Text>();
            TMP_Text buttonDesc = buttonInstance.transform.Find("ButtonDesc").GetComponent<TMP_Text>();
            TMP_Text costText = buttonInstance.transform.Find("CostImage/CostText").GetComponent<TMP_Text>();
            Image background = buttonInstance.GetComponent<Image>();
            background.color = new(200 / 255f, 200 / 255f, 200 / 255f, 1f);
            Image smallBackground = buttonInstance.transform.Find("CostImage").GetComponent<Image>();
            smallBackground.color = new(204 / 255f, 183 / 255f, 170 / 255f, 1f);

            buttonTitle.text = variant.GetName();
            buttonDesc.text = variant.GetDescription();
            costText.text = variant.GetCost().ToString();
        }
    }
}
