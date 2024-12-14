using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class BattleController : MonoBehaviour
{
    public TMP_Text PlayerName;
    public TMP_Text PlayerHealth;
    public TMP_Text PlayerActions;
    public TMP_Text PlayerStats;

    public TMP_Text EnemyName;
    public TMP_Text EnemyHealth;
    public TMP_Text EnemyActions;
    public TMP_Text EnemyStats;

    public Image TurnCounterBackground;
    public TMP_Text TurnCounter;

    public Button EndTurnButton;

    public GameObject CardPrefab;

    public GameObject PlayerCardSlots;
    public GameObject EnemyCardSlots;

    public GameObject EndingCard;
    public TMP_Text EndingText;

    private BattleManager BattleManager;

    public static BattleController instance;

    private bool HasEnded;

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

    public void CreateBatte(Entity entity, float delay)
    {
        HasEnded = false;
        PlayerCardSlots.SetActive(true);
        EnemyCardSlots.SetActive(true);
        DeleteAllChildren(PlayerCardSlots);
        DeleteAllChildren(EnemyCardSlots);

        EndTurnButton.enabled = true;

        BattleManager = new(entity);

        PlayerName.text = BattleManager.GetPlayerEntity().GetName();
        EnemyName.text = BattleManager.GetEnemyEntity().GetName();

        PrepareTurn(delay + 3f);
        StartCoroutine(DelayedGameStart(delay));
    }

    public void DeleteAllChildren(GameObject parentObject)
    {
        foreach (Transform child in parentObject.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void PrepareTurn(float? delay)
    {
        if (BattleManager.IsPlayerTurn())
        {
            SwitchToPlayerTurn();
        }
        else
        {
            SwitchToEnemyTurn();
            if (delay != null)
            {
                StartCoroutine(DelayedEnemyTurn((float)delay));
            }
        }
        UpdateStats(BattleManager.GetPlayerEntity(), BattleManager.GetEnemyEntity());
    }

    private IEnumerator DelayedEnemyTurn(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        EnemyTurn();
    }

    private IEnumerator DelayedGameStart(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        DealCardsOnStart();
    }

    public void DealCardsOnStart()
    {
        (List<int> PlayerHand, List<int> EnemyHand) = BattleManager.StartBattle();
        CreateCards(PlayerHand, Turns.PLAYER);
        CreateCards(EnemyHand, Turns.ENEMY);
    }

    private float CreateCards(List<int> indexes, Turns turn)
    {
        float allDelay = 0f;

        CombatantEntity entity = turn == Turns.PLAYER ? BattleManager.GetPlayerEntity() : BattleManager.GetEnemyEntity();

        int cardCount = entity.GetHand().Length;
        float delay = 0.5f;
        int count = 0;

        foreach (int index in indexes)
        {
            float length = 1f;
            Card card = entity.GetCardFromHand(index);

            if (turn == Turns.PLAYER)
            {
                GameObject cardEntity = CreateCardEntity(GetHolderForCurrentEntity(turn), index, card, 1075, false);
                UIUtils.MoveGameObjectAbsolutelyHorizontallyEaseInOut(cardEntity, ((index + 1) * 50) + (index * 140), length, count * delay);
            }
            else
            {
                GameObject cardEntity = CreateCardEntity(GetHolderForCurrentEntity(turn), index, card, -1225, true);
                UIUtils.MoveGameObjectAbsolutelyHorizontallyEaseInOut(cardEntity, ((cardCount - index) * 50) + ((cardCount - index - 1) * 140) - 1000, length, count * delay);
            }

            allDelay += count * delay + length;

            count++;
        }

        return allDelay;
    }

    private GameObject GetHolderForCurrentEntity(Turns turn)
    {
        return turn == Turns.PLAYER ? PlayerCardSlots : EnemyCardSlots;
    }

    private GameObject CreateCardEntity(GameObject holder, int index, Card card, int offset, bool shouldHide)
    {
        GameObject cardEntity = Instantiate(CardPrefab, new(0, 0), Quaternion.identity);
        cardEntity.transform.SetParent(holder.transform, false);
        cardEntity.transform.localPosition = new Vector2(offset, 125);

        GameObject cardTitleObject = cardEntity.transform.Find("CardTitle").gameObject;
        GameObject cardDescriptionObject = cardEntity.transform.Find("CardDescription").gameObject;
        GameObject cardCostObject = cardEntity.transform.Find("CardCost").gameObject;

        TextMeshProUGUI cardTitle = cardTitleObject.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI cardDescription = cardDescriptionObject.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI cardCost = cardCostObject.GetComponent<TextMeshProUGUI>();

        GameObject cardTitleBackgroundObject = cardEntity.transform.Find("TitleBackground").gameObject;
        GameObject cardCostBackgroundObject = cardEntity.transform.Find("CostBackground").gameObject;

        Image cardTitleBackground = cardTitleBackgroundObject.GetComponent<Image>();
        Image cardCostBackground = cardCostBackgroundObject.GetComponent<Image>();

        if (shouldHide)
        {
            cardTitleObject.SetActive(false);
            cardDescriptionObject.SetActive(false);
            cardCostObject.SetActive(false);
            cardTitleBackgroundObject.SetActive(false);
            cardCostBackgroundObject.SetActive(false);
        }

        CardEntity cardCardEntity = cardEntity.GetComponent<CardEntity>();
        cardCardEntity.SetIndex(index);
        cardCardEntity.SetCard(card);

        cardTitle.text = card.GetName();
        cardDescription.text = card.GetDescription();
        cardCost.text = card.GetCost().ToString();

        Color color;

        switch (card.GetCardType())
        {
            case CardType.DAMAGE:
                color = UIUtils.Colors.ColorFromInt(255, 223, 223);
                break;
            case CardType.SUPPORT:
                color = UIUtils.Colors.ColorFromInt(223, 255, 223);
                break;
            case CardType.UTILITY:
                color = UIUtils.Colors.ColorFromInt(223, 223, 255);
                break;
            default:
                color = UIUtils.Colors.ColorFromInt(223, 223, 223);
                break;
        }

        cardTitleBackground.color = color;
        cardCostBackground.color = color;

        return cardEntity;
    }

    public GameObject FindCardByIndex(int targetIndex, Turns turn)
    {
        if (turn == Turns.PLAYER)
        {
            foreach (Transform child in PlayerCardSlots.transform)
            {
                CardEntity cardEntity = child.GetComponent<CardEntity>();
                if (cardEntity != null && cardEntity.GetIndex() == targetIndex)
                {
                    return child.gameObject;
                }
            }
        }
        else
        {
            foreach (Transform child in EnemyCardSlots.transform)
            {
                CardEntity cardEntity = child.GetComponent<CardEntity>();
                if (cardEntity != null && cardEntity.GetIndex() == targetIndex)
                {
                    return child.gameObject;
                }
            }
        }

        return null;
    }

    public bool PlayCard(int index)
    {
        (bool canPlay, bool successful, bool lethal) = BattleManager.PlayCard(index);
        if (successful) UpdateStats(BattleManager.GetPlayerEntity(), BattleManager.GetEnemyEntity());
        if (lethal) StartCoroutine(DelayedEndCombat());
        return (canPlay);
    }

    public IEnumerator DelayedEndCombat()
    {
        HasEnded = true;
        PlayerCardSlots.SetActive(false);
        EnemyCardSlots.SetActive(false);

        EndTurnButton.enabled = false;

        yield return new WaitForSeconds(0.75f);

        EndCombat();
    }

    private void EndCombat()
    {
        if (BattleManager.IsPlayerTurn())
        {
            if (BattleManager.GetEnemyEntity().GetName() == Entity.Entities.FEROCIOUS_BEAR.GetName())
            {
                string text = "The End!";

                EndingText.text = text;
                UIUtils.MoveGameObjectRelativelyVerticallyEaseInOut(EndingCard, 1080, 1f);

                SaveSystem.DeleteSlot(int.Parse(PlayerCharacter.GetName().Split(" ")[1]));
                StartCoroutine(DelayedEndGame());
            }
            else
            {
                string text = "You Won!";

                EndingText.text = text;
                PlayerCharacter.GetExperience().AddWonFight(1);

                UIUtils.MoveGameObjectRelativelyVerticallyEaseInOut(EndingCard, 1080, 1f);

                if (PlayerCharacter.GetClock().IsNight())
                {
                    ClockController.instance.ProgressToMorning(3f);
                }

                ContainerController.instance.EndBattle(3f);
                UIUtils.MoveGameObjectRelativelyVerticallyEaseInOut(EndingCard, 0, 0.1f, 4f);
            }
        }
        else
        {
            string text = "You Lost...";

            EndingText.text = text;
            PlayerCharacter.EndRun();
            
            UIUtils.MoveGameObjectRelativelyVerticallyEaseInOut(EndingCard, 1080, 1f);

            StartCoroutine(DelayedSceneSwitch());
        }
        
    }

    private IEnumerator DelayedSceneSwitch()
    {
        yield return new WaitForSeconds(3f);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("PrepareScene");
        while(!asyncOperation.isDone)
        {
            yield return null;
        }
    }

    private IEnumerator DelayedEndGame()
    {
        yield return new WaitForSeconds(3f);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("MainMenuScene");
        while (!asyncOperation.isDone)
        {
            yield return null;
        }
    }

    private void EnemyAnimateCardPlay(int index, float length, float delay)
    {
        GameObject playedCard = FindCardByIndex(index, Turns.ENEMY);
        if (playedCard != null)
        {

            StartCoroutine(DelayedReveal(playedCard, delay));
            UIUtils.MoveGameObjectRelativelyVerticallyEaseInOut(playedCard, 200, length, delay);
            StartCoroutine(DelayedDelete(playedCard, delay + length + 1f));
        }
    }

    private IEnumerator DelayedDelete(GameObject gameObject, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    private IEnumerator DelayedReveal(GameObject gameObject, float delay)
    {
        yield return new WaitForSeconds(delay);

        GameObject cardTitleObject = gameObject.transform.Find("CardTitle").gameObject;
        GameObject cardDescriptionObject = gameObject.transform.Find("CardDescription").gameObject;
        GameObject cardCostObject = gameObject.transform.Find("CardCost").gameObject;

        GameObject cardTitleBackgroundObject = gameObject.transform.Find("TitleBackground").gameObject;
        GameObject cardCostBackgroundObject = gameObject.transform.Find("CostBackground").gameObject;

        cardTitleObject.SetActive(true);
        cardDescriptionObject.SetActive(true);
        cardCostObject.SetActive(true);
        cardTitleBackgroundObject.SetActive(true);
        cardCostBackgroundObject.SetActive(true);
    }

    private void SwitchTurn()
    {
        if (BattleManager.IsPlayerTurn())
        {
            SwitchToPlayerTurn();
        }
        else
        {
            SwitchToEnemyTurn();
        }

        UpdateStats(BattleManager.GetPlayerEntity(), BattleManager.GetEnemyEntity());
    }

    private void SwitchToPlayerTurn()
    {
        EndTurnButton.GetComponent<Image>().color = new Color(1f, 1, 1f);
        EndTurnButton.enabled = true;
        TurnCounterBackground.color = new Color(215f / 255f, 255f / 255f, 205f / 255f);
        TurnCounter.text = "Player Turn";
    }

    private void SwitchToEnemyTurn()
    {
        EndTurnButton.GetComponent<Image>().color = new Color(179f / 255f, 179f / 255f, 179f / 255f);
        EndTurnButton.enabled = false;
        TurnCounterBackground.color = new Color(255f / 255f, 215f / 255f, 205f / 255f);
        TurnCounter.text = "Enemy Turn";
    }

    public void EndTurnButtonPressed()
    {
        if (BattleManager.IsEnemyTurn())
            return;

        List<int> indexes = BattleManager.ChangeTurn();
        SwitchTurn();
        float delay = CreateCards(indexes, Turns.ENEMY);

        StartCoroutine(WaitForEnemyTurn(delay));
    }

    private void EnemyTurn()
    {
        int index = BattleManager.EnemyMakeDecision();
        float length = 1f;
        float count = 0;
        float totalDelay = 0f;

        do
        {
            PlayCard(index);
            EnemyAnimateCardPlay(index, length, count);
            UpdateStats(BattleManager.GetPlayerEntity(), BattleManager.GetEnemyEntity());
            count += 1f;
            totalDelay += count;
            index = BattleManager.EnemyMakeDecision();
        } while (index >= 0 && !HasEnded);

        if  (!HasEnded) StartCoroutine(WaitForCardDrawingAndAnimations(totalDelay + length));
        
    }

    private IEnumerator WaitForEnemyTurn(float delay)
    {
        yield return new WaitForSeconds(delay);
        EnemyTurn();
    }

    private IEnumerator WaitForCardDrawingAndAnimations(float delay)
    {
        yield return new WaitForSeconds(delay);
        List<int> indexes = BattleManager.ChangeTurn();
        SwitchTurn();
        CreateCards(indexes, Turns.PLAYER);
    }

    private void UpdateStats(CombatantEntity player, CombatantEntity enemy)
    {
        SetHealth(PlayerHealth, player.GetRemainingHealth());
        SetActions(PlayerActions, player.GetRemainingActions());
        SetStats(PlayerStats, player);


        SetHealth(EnemyHealth, enemy.GetRemainingHealth());
        SetActions(EnemyActions, enemy.GetRemainingActions());
        SetStats(EnemyStats, enemy);
    }

    private void SetHealth(TMP_Text text, int amount)
    {
        text.text = "Health: " + amount;
    }

    private void SetActions(TMP_Text text, int amount)
    {
        text.text = "Actions: " + amount;
    }

    private void SetStats(TMP_Text text, CombatantEntity entity)
    {
        text.text = $"PP:\t{entity.GetPhysicalPowerValue():F2},\r\n" +
                    $"AP:\t{entity.GetArcanePowerValue():F2},\r\n" +
                    $"D:\t{entity.GetDefenseValue():F2},\r\n" +
                    $"I:\t{entity.GetInitiativeValue():F2},\r\n" +
                    $"AC:\t{entity.GetAccuracyValue():F2},\r\n" +
                    $"M:\t{entity.GetMoraleValue():F2}";

    }
}
