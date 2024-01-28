using System;
using System.Linq;
using Card;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject endWavePage;
    public CardSO[] cards;
    public GameObject[] cardsUI;
    public GameObject[] iconsUI;
    public GameObject gameOverUI;
    public GameObject youWonUI;

    private WeightedList<CardSO> weightedCardList;
    private CardSO[] chosenCards = new CardSO[3];
    
    public static GameManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Game Manager in the scene.");
        }
        instance = this;
    }

    private void Start()
    {
        weightedCardList = new WeightedList<CardSO>();
        foreach (var card in cards)
        {
            weightedCardList.Add(card, card.weight);
        }

        Choose3RandomCard();
    }

    public void OpenGameOverScreen()
    {
        Time.timeScale = 0;
        gameOverUI.SetActive(true);
    }
    
    public void OpenYouWonScreen()
    {
        Time.timeScale = 0;
        youWonUI.SetActive(true);
    }

    public void ChooseCard(int index)
    {
        var _player = player.GetComponent<Player.Player>();
        foreach (var perk in chosenCards[index].perks)
        {
            perk.Upgrade(_player);
        }
        _player.UpdateStats();
        Time.timeScale = 1;
        endWavePage.SetActive(false);
        Choose3RandomCard();
    }

    public void OpenEndWavePage()
    {
        Time.timeScale = 0;
        endWavePage.SetActive(true);
    }

    private void Choose3RandomCard()
    {
        for (int i = 0; i < 3; i++)
        {
            var card = weightedCardList.Next();
            chosenCards[i] = card;
            cardsUI[i].transform.Find("Title").GetComponent<TextMeshProUGUI>().text = card.title;
            cardsUI[i].transform.Find("Description").GetComponent<TextMeshProUGUI>().text = card.description;
            iconsUI[i].GetComponent<UnityEngine.UI.Image>().sprite = card.icon;
        }
    }

    // private void CheckAllSpawners()
    // {
    //     var notFinishedSpawners = spawners.Where(s => s.finishedAllWaves == false).ToList();
    //     if (notFinishedSpawners.Count <= 0) { return; } // level passed
    //
    //     bool readyAllSpawners = !spawners.Any(s => s.waves.ElementAtOrDefault(s.currentWaveIndex)?.enemiesLeft > 0);
    //     if (readyAllSpawners)
    //     {
    //         foreach (var spawner in spawners)
    //         {
    //             spawner.readyForNextWave = true;
    //         }
    //     }
    //     else
    //     {
    //         foreach (var spawner in spawners)
    //         {
    //             spawner.readyForNextWave = false;
    //         }
    //     }
    // }
}