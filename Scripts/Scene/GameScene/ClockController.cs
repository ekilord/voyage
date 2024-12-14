using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClockController : MonoBehaviour
{
    public GameObject Clock;

    public TMP_Text ClockText;
    public TMP_Text SkipTimeText;
    public GameObject SkipTimeButton;

    public static ClockController instance;

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

        ClockText.text = PlayerCharacter.GetClock().TimeToString();
        SkipTimeText.text = "Skip Time\n(1h)";
    }

    public void SkipTime(int hour)
    {
        if (PlayerCharacter.GetClock().AddHours(hour))
        {
            PlayerCharacter.GetBase().PassTime(hour);
            UpdateClock();
        }
        else
        {
            if (PlayerCharacter.GetClock().IsDusk())
            {
                PlayerCharacter.GetClock().ProgressToDusk();
                UpdateClock();
                SkipTimeText.text = "Progress to\nNight";
            }
            else
            {
                if (PlayerCharacter.GetClock().IsNight())
                {
                    PlayerCharacter.GetClock().ProgressToNight();
                    UpdateClock();
                    BeginNightAssault();
                }
            }
        }
    }

    private void BeginNightAssault()
    {
        float delay = 2f;

        MapController.instance.NightMovement();

        Entity entity = (Entity)( EnemyAI.ChooseNightTimeEnemy() ?? null );

        if ( entity != null ) {
            BattleController.instance.CreateBatte(entity, delay + 0.75f );
            ContainerController.instance.GoToBattle( delay );
            HideClock();
        }
        else {
			ProgressToMorning( 2f );
		}
        
    }

    public void ProgressToMorning(float delay)
    {
        PlayerCharacter.GetClock().ProgressToNextMorning();
        UpdateClock();
        StatsController.instance.UpdateEverything();
        SkipTimeText.text = "Skip Time\n(1h)";
        StartCoroutine(ShowClockWithDelay(delay));
    }

    public void PassAnHour()
    {
        SkipTime(1);
    }

    public void UpdateClock()
    {
        ClockText.text = PlayerCharacter.GetClock().TimeToString();
    }

    public void HideClock()
    {
        Clock.SetActive(false);
    }

    public void ShowClock()
    {
        Clock.SetActive(true);
    }

    public IEnumerator ShowClockWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        ShowClock();
    }
}
