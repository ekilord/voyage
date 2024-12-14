using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ContainerController : MonoBehaviour
{
    public GameObject GeneralContainer;

    public Button LeftButton;
    public Button RightButton;
    public Button UpButton;
    public Button DownButton;

    private ContainerNavigation.Panel CurrentPanel;

    public static ContainerController instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if ( PlayerCharacter.GetName() == null )
            PlayerCharacter.CreatePlayer( "test" );

        GeneralContainer.transform.localPosition = new Vector3(0f, 0f);
        CurrentPanel = ContainerNavigation.Panel.Main;
    }

    private void Start()
    {
        StatsController.instance.UpdateEverything();
    }

    public void GoToBattle(float delay)
    {
        UIUtils.MoveGameObjectRelativelyHorizontallyEaseInOut(GeneralContainer, 10000, delay);
        HideAvailableButtons();
    }

    public void EndBattle(float delay)
    {
        UIUtils.MoveGameObjectRelativelyHorizontallyEaseInOut(GeneralContainer, -10000, 1f, delay);
        StartCoroutine(HideButtons(1f + delay));
        MapController.instance.SetScoutDestination(null);
    }

    public void GoLeft()
    {
        GoTo(ContainerNavigation.Direction.LEFT);
    }

    public void GoRight()
    {
        GoTo(ContainerNavigation.Direction.RIGHT);
    }

    public void GoUp()
    {
        GoTo(ContainerNavigation.Direction.UP);
    }

    public void GoDown()
    {
        GoTo(ContainerNavigation.Direction.DOWN);
    }

    private void GoTo(ContainerNavigation.Direction direction)
    {
        ContainerNavigation.NAVIGATION_RULES.TryGetValue(CurrentPanel, out var rules);

        if (rules != null && rules.ContainsKey(direction))
        {
            rules.TryGetValue(direction, out var goal);
            CurrentPanel = goal;

            ContainerNavigation.NAVIGATION_RULES.TryGetValue(CurrentPanel, out var newRules);

            (int DiffX, int DiffY) = TranslateDirection(direction);

            float seconds = 0.75f;

            foreach (var value in newRules.Keys)
            {
                EnableDirection(value);
            }

            var difference = ContainerNavigation.ALL_DIRECTIONS.Except(newRules.Keys).ToList();
            foreach (var value in difference)
            {
                DisableDirection(value);
            }

            if (DiffX != 0)
            {
                UIUtils.MoveGameObjectRelativelyHorizontallyEaseInOut(GeneralContainer, DiffX, seconds);
                StartCoroutine(HideButtons(seconds));
            }
            else if (DiffY != 0)
            {
                UIUtils.MoveGameObjectRelativelyVerticallyEaseInOut(GeneralContainer, DiffY, seconds);
                StartCoroutine(HideButtons(seconds));
            }

            CampController.instance.UpdateAvailabilities();
        }
    }

    private (int DiffX, int DiffY) TranslateDirection(ContainerNavigation.Direction direction)
    {
        return direction switch
        {
            ContainerNavigation.Direction.LEFT => (-1920, 0),
            ContainerNavigation.Direction.RIGHT => (1920, 0),
            ContainerNavigation.Direction.UP => (0, 1080),
            ContainerNavigation.Direction.DOWN => (0, -1080),
            _ => (0, 0),
        };
    }

    private void EnableDirection(ContainerNavigation.Direction direction)
    {
        switch (direction)
        {
            case ContainerNavigation.Direction.LEFT:
                LeftButton.enabled = true;
                LeftButton.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                break;
            case ContainerNavigation.Direction.RIGHT:
                RightButton.enabled = true;
                RightButton.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                break;
            case ContainerNavigation.Direction.UP:
                UpButton.enabled = true;
                UpButton.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                break;
            case ContainerNavigation.Direction.DOWN:
                DownButton.enabled = true;
                DownButton.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                break;
        }
    }

    private void DisableDirection(ContainerNavigation.Direction direction)
    {
        switch (direction)
        {
            case ContainerNavigation.Direction.LEFT:
                LeftButton.enabled = false;
                LeftButton.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                break;
            case ContainerNavigation.Direction.RIGHT:
                RightButton.enabled = false;
                RightButton.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                break;
            case ContainerNavigation.Direction.UP:
                UpButton.enabled = false;
                UpButton.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                break;
            case ContainerNavigation.Direction.DOWN:
                DownButton.enabled = false;
                DownButton.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                break;
        }
    }

    IEnumerator HideButtons(float seconds)
    {
        HideAvailableButtons();

        yield return new WaitForSeconds(seconds);

        ShowAvailableButtons();
        ClockController.instance.ShowClock();
    }

    private void HideAvailableButtons()
    {
        if (LeftButton.enabled)
        {
            LeftButton.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }

        if (RightButton.enabled)
        {
            RightButton.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }

        if (UpButton.enabled)
        {
            UpButton.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }

        if (DownButton.enabled)
        {
            DownButton.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }
    }

    private void ShowAvailableButtons()
    {
        if (LeftButton.enabled)
        {
            LeftButton.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }

        if (RightButton.enabled)
        {
            RightButton.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }

        if (UpButton.enabled)
        {
            UpButton.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }

        if (DownButton.enabled)
        {
            DownButton.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
    }
}
