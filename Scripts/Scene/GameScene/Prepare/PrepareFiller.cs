using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrepareFiller : MonoBehaviour
{
    public GameObject PerkTabButtonsHolder;
    public GameObject ResearchTabButtonsHolder;

    public GameObject PrepareButtonPrefab;

    private Vector2 startPosition = new(-575, 185);
    private Vector2 offsetX = new(230, 0);
    private Vector2[] positionsInGroup = new Vector2[] 
    {
        new(0, 0),
        new(0, -150),
        new(0, -300)
    };

    void Awake()
    {
        CreatePreparationButtons();
    }

    private void CreatePreparationButtons()
    {
        InstantiatePerkGroup(Experience.Perks.GetPerkGroups()["PP"], 0, PerkTabButtonsHolder, PreparationType.PERK);
        InstantiatePerkGroup(Experience.Perks.GetPerkGroups()["AP"], 1, PerkTabButtonsHolder, PreparationType.PERK);
        InstantiatePerkGroup(Experience.Perks.GetPerkGroups()["D"], 2, PerkTabButtonsHolder, PreparationType.PERK);
        InstantiatePerkGroup(Experience.Perks.GetPerkGroups()["ACC"], 3, PerkTabButtonsHolder, PreparationType.PERK);
        InstantiatePerkGroup(Experience.Perks.GetPerkGroups()["HP"], 4, PerkTabButtonsHolder, PreparationType.PERK);
        InstantiatePerkGroup(Experience.Perks.GetPerkGroups()["ACT"], 5, PerkTabButtonsHolder, PreparationType.PERK);

        InstantiateResearchGroup(Experience.Research.GetAllResearch(), 0, ResearchTabButtonsHolder, PreparationType.RESEARCH);
    }

    private void InstantiatePerkGroup(List<Effect> perks, int groupIndex, GameObject holder, PreparationType type)
    {
        Vector2 groupPosition = startPosition + groupIndex * offsetX;

        for (int k = 0; k < 3; k++)
        {
            Vector2 position = groupPosition + positionsInGroup[k];
            GameObject buttonInstance = Instantiate(PrepareButtonPrefab, new Vector3(0, 0), Quaternion.identity, holder.transform);
            buttonInstance.GetComponent<Button>().enabled = false;
            Effect perk = perks[k];
            buttonInstance.GetComponent<PrepareEntity>().SetName(perk.GetName());
            buttonInstance.GetComponent<PrepareEntity>().SetPreparationType(type);
            buttonInstance.transform.localPosition = position;

            TMP_Text buttonTitle = buttonInstance.transform.Find("ButtonTitle").GetComponent<TMP_Text>();
            TMP_Text buttonDesc = buttonInstance.transform.Find("ButtonDesc").GetComponent<TMP_Text>();
            Image background = buttonInstance.GetComponent<Image>();
            background.color = new(200 / 255f, 200 / 255f, 200 / 255f, 1f);

            buttonTitle.text = perk.GetName();
            buttonDesc.text = perk.GetDescription();
        }
    }

    private void InstantiateResearchGroup(List<string> researches, int groupIndex, GameObject holder, PreparationType type)
    {
        Vector2 groupPosition = startPosition + groupIndex * offsetX;
        int maxItemsPerRow = 6;

        for (int i = 0; i < researches.Count; i++)
        {
            int row = i / maxItemsPerRow;
            int column = i % maxItemsPerRow;

            Vector2 position = groupPosition + new Vector2(column * 230, -row * 150);

            GameObject buttonInstance = Instantiate(PrepareButtonPrefab, Vector3.zero, Quaternion.identity, holder.transform);
            buttonInstance.GetComponent<Button>().enabled = false;
            string research = researches[i];
            buttonInstance.GetComponent<PrepareEntity>().SetName(research);
            buttonInstance.GetComponent<PrepareEntity>().SetPreparationType(type);
            buttonInstance.transform.localPosition = position;

            TMP_Text buttonTitle = buttonInstance.transform.Find("ButtonTitle").GetComponent<TMP_Text>();
            TMP_Text buttonDesc = buttonInstance.transform.Find("ButtonDesc").GetComponent<TMP_Text>();
            Image background = buttonInstance.GetComponent<Image>();
            background.color = new Color(200 / 255f, 200 / 255f, 200 / 255f, 1f);

            buttonTitle.text = research;
            buttonDesc.text = string.Empty;
        }
    }

}
