using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("General")]
    public GameObject parent;

    [Header("Main Menu")]
    public Button startButton;
    public Button optionsButton;
    public Button exitButton;

    public void MoveScreenToLeft()
    {
        UIUtils.MoveGameObjectRelativelyHorizontallyEaseInOut(parent, -1920f, 1f);
    }

    public void MoveScreenToRight()
    {
        UIUtils.MoveGameObjectRelativelyHorizontallyEaseInOut(parent, 1920f, 1f);
    }

    public void SelectSaveFile(int slot)
    {
        SaveSystem.LoadPlayerCharacter(slot);

        if (PlayerCharacter.IsInGame())
        {
            StartCoroutine(DelayedSceneSwitch("GameScene"));
        }
        else
        {
            StartCoroutine(DelayedSceneSwitch("PrepareScene"));
        }
    }

    private IEnumerator DelayedSceneSwitch(string sceneName)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncOperation.isDone)
        {
            yield return null;
        }
    }

    public void SelectSave1()
    {
        SelectSaveFile(1);
    }

    public void SelectSave2()
    {
        SelectSaveFile(2);
    }

    public void SelectSave3()
    {
        SelectSaveFile(3);
    }
}
