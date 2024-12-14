using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class StatsController : MonoBehaviour
{
    public TMP_Text PlayerNameText;
    public TMP_Text PlayerMaxHPText;
    public TMP_Text PlayerMaxActionsText;
    public TMP_Text PlayerStatsText;
    public TMP_Text PlayerResourcesText;
    public TMP_Text PlayerIncomeText;
    public TMP_Text DateText;

    public static StatsController instance;

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

    public void UpdateEverything()
    {
        UpdatePlayerIncome();
        UpdatePlayerResources();
        UpdatePlayerStats();
        UpdateDate();
    }

    public void UpdatePlayerResources()
    {
        PlayerResourcesText.text = $"GOLD:\t\t{PlayerCharacter.GetInventory().GetGold():F2},\r\n" +
                                   $"WOOD:\t\t{PlayerCharacter.GetInventory().GetWood():F2},\r\n" +
                                   $"STONE:\t\t{PlayerCharacter.GetInventory().GetStone():F2},\r\n" +
                                   $"IRON:\t\t{PlayerCharacter.GetInventory().GetIron():F2},\r\n" +
                                   $"AETHER:\t{PlayerCharacter.GetInventory().GetAether():F2}\r\n";
    }

    public void UpdatePlayerStats()
    {
        PlayerNameText.text = $"{PlayerCharacter.GetName()}";

        PlayerMaxHPText.text = $"Max Health: {PlayerCharacter.GetHealthValue()}";
        PlayerMaxActionsText.text = $"Max Actions: {PlayerCharacter.GetActionsValue()}";

        PlayerStatsText.text = $"PP:\t{PlayerCharacter.GetPhysicalPowerValue():F2},\r\n" +
                               $"AP:\t{PlayerCharacter.GetArcanePowerValue():F2},\r\n" +
                               $"D:\t{PlayerCharacter.GetDefenseValue():F2},\r\n" +
                               $"I:\t{PlayerCharacter.GetInitiativeValue():F2},\r\n" +
                               $"AC:\t{PlayerCharacter.GetAccuracyValue():F2},\r\n" +
                               $"M:\t{PlayerCharacter.GetMoraleValue():F2}";
    }

    public void UpdatePlayerIncome()
    {
        PlayerIncomeText.text = $"GOLD:\t\t+{PlayerCharacter.GetIncome().GetGold():F2},\r\n" +
                                   $"WOOD:\t\t+{PlayerCharacter.GetIncome().GetWood():F2},\r\n" +
                                   $"STONE:\t\t+{PlayerCharacter.GetIncome().GetStone():F2},\r\n" +
                                   $"IRON:\t\t+{PlayerCharacter.GetIncome().GetIron():F2},\r\n" +
                                   $"AETHER:\t+{PlayerCharacter.GetIncome().GetAether():F2}\r\n";
    }

    public void UpdateDate()
    {
        DateText.text = $"{PlayerCharacter.GetClock().DateToString()}";
    }
}
