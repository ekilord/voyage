using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsController : MonoBehaviour
{
    public GameObject EscapeMenu;

    private bool Toggled = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        if (Toggled)
        {
            MoveMenuOutOfView();
            Toggled = false;
        }
        else
        {
            MoveMenuIntoView();
            Toggled = true;
        }
    }

    public void MoveMenuIntoView()
    {
        UIUtils.MoveGameObjectAbsolutelyVerticallyEaseInOut(EscapeMenu, -1080, 0.5f);
    }

    public void MoveMenuOutOfView()
    {
        UIUtils.MoveGameObjectAbsolutelyVerticallyEaseInOut(EscapeMenu, 0, 0.5f);
    }

    public void SaveExit()
    {
        SaveSystem.SavePlayerCharacter(int.Parse(PlayerCharacter.GetName().Split(" ")[1]));
        Application.Quit();
    }
}
