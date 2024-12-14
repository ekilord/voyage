using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadChecker : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "BattleScene")
        {
            StartCoroutine(DelayedAnimationStart());
        }
    }

    private IEnumerator DelayedAnimationStart()
    {
        yield return new WaitForSeconds(1f);
        StartAnimations();
    }

    private void StartAnimations()
    {
        gameObject.GetComponent<BattleController>().DealCardsOnStart();
    }
}
