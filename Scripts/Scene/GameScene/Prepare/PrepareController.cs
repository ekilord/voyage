using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PrepareController : MonoBehaviour
{
    public GameObject PerkTab;
    public GameObject ResearchTab;

    public GameObject PerkTabButtonsHolder;
    public GameObject ResearchTabButtonsHolder;

    public TMP_Text PerkPointsText;
    public TMP_Text CurrentResearchText;

    private GameObject InUseCategory;

    public static PrepareController instance;

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

    private void Start()
    {
        UpdateEverything();
    }

    public void TogglePerkTab()
    {
        ToggleTab(PerkTab);
    }

    public void ToggleResearchTab()
    {
        ToggleTab(ResearchTab);
    }

    private void ToggleTab(GameObject tab)
    {
        if (InUseCategory == null)
        {
            MoveTabIn(tab);
        }
        else
        {
            if (InUseCategory != tab)
            {
                MoveCurrentTabOut();
                MoveTabIn(tab);
            }
        }
    }

    private void MoveTabIn(GameObject tab)
    {
        InUseCategory = tab;

        tab.transform.localPosition = new Vector2(-960, -75);
        UIUtils.MoveGameObjectAbsolutelyHorizontallyEaseInOut(tab, 800, 1f);
    }

    private void MoveCurrentTabOut()
    {
        UIUtils.MoveGameObjectAbsolutelyHorizontallyEaseInOut(InUseCategory, 2720, 1f);
    }

    public void UpdatePerkAvailabilities()
    {
        if (PerkTabButtonsHolder == null) return;

        PrepareEntity[] preparations = PerkTabButtonsHolder.GetComponentsInChildren<PrepareEntity>();

        foreach (PrepareEntity preparation in preparations)
        {
            Image background = preparation.transform.GetComponent<Image>();

            if (background != null)
            {
                List<string> available = PlayerCharacter.GetExperience().GetAvailablePerkNames();
                List<string> owned = PlayerCharacter.GetExperience().GetAllAcquiredPerks();

                if (available.Contains(preparation.GetName()))
                {
                    if (PlayerCharacter.GetExperience().GetPerkPoints() > 0)
                    {
                        preparation.GetComponent<Button>().enabled = true;
                        background.color = new(221 / 255f, 255 / 255f, 221 / 255f, 1f);
                    }
                    else
                    {
                        preparation.GetComponent<Button>().enabled = false;
                        background.color = new(255 / 255f, 221 / 255f, 221 / 255f, 1f);
                    }
                }
                else
                {
                    if (owned.Contains(preparation.GetName()))
                    {
                        preparation.GetComponent<Button>().enabled = false;
                        background.color = new(221 / 255f, 221 / 255f, 255 / 255f, 1f);
                    }

                }
            }
        }
    }

    public void UpdateResearchAvailabilities()
    {
        if (ResearchTabButtonsHolder == null) return;

        PrepareEntity[] preparations = ResearchTabButtonsHolder.GetComponentsInChildren<PrepareEntity>();

        foreach (PrepareEntity preparation in preparations)
        {
            Image background = preparation.transform.GetComponent<Image>();

            if (background != null)
            {
                List<string> available = PlayerCharacter.GetExperience().GetAvailableResearchNames();
                List<string> owned = PlayerCharacter.GetExperience().GetAllAcquiredResearches();

                if (available.Contains(preparation.GetName()))
                {
                    if (PlayerCharacter.GetExperience().GetCurrentResearch() == null)
                    {
                        preparation.GetComponent<Button>().enabled = true;
                        background.color = new(221 / 255f, 255 / 255f, 221 / 255f, 1f);
                    }
                    else
                    {
                        if (PlayerCharacter.GetExperience().GetCurrentResearch() == preparation.GetName())
                        {
                            preparation.GetComponent<Button>().enabled = false;
                            background.color = new(221 / 255f, 221 / 255f, 255 / 255f, 1f);
                        }
                        else
                        {
                            preparation.GetComponent<Button>().enabled = true;
                            background.color = new(255 / 255f, 221 / 255f, 221 / 255f, 1f);
                        }
                    }
                }
                else
                {
                    if (owned.Contains(preparation.GetName()))
                    {
                        preparation.GetComponent<Button>().enabled = false;
                        background.color = new(221 / 255f, 221 / 255f, 255 / 255f, 1f);
                    }

                }
            }
        }
    }

    public void UpdatePerkPoints()
    {
        PerkPointsText.text = $"Perk Points:\n{PlayerCharacter.GetExperience().GetPerkPoints()}";
    }

    public void UpdateCurrentResearch()
    {
        CurrentResearchText.text = $"Current Research:\n{PlayerCharacter.GetExperience().GetCurrentResearch()}";
    }

    public void UpdateEverything()
    {
        UpdatePerkAvailabilities();
        UpdateResearchAvailabilities();
        UpdatePerkPoints();
        UpdateCurrentResearch();
    }

    public void TryToAcquirePreparation(string name, PreparationType preparationType)
    {
        if (PlayerCharacter.TryToAcquirePreparation(name, preparationType))
        {
           UpdateEverything();
        }
    }

    public void GoButton()
    {
        PlayerCharacter.StartRun();
        StartCoroutine(DelayedSceneSwitch("GameScene"));
    }

    public void SaveExitButton()
    {
        SaveSystem.SavePlayerCharacter(int.Parse(PlayerCharacter.GetName().Split(" ")[1]));
        Application.Quit();
    }

    private IEnumerator DelayedSceneSwitch(string sceneName)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncOperation.isDone)
        {
            yield return null;
        }
    }
}
