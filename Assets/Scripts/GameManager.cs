using System;
using System.Linq;
using Card;
using Player;
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
    private PlayerMovement playerMovement;
    
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
        playerMovement = player.GetComponent<PlayerMovement>();
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
        endWavePage.SetActive(false);
        playerMovement.joystick.gameObject.SetActive(true);
        Choose3RandomCard();
        Time.timeScale = 1;
    }

    public void OpenEndWavePage()
    {
        Time.timeScale = 0;
        playerMovement.joystick.gameObject.SetActive(false);
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
}