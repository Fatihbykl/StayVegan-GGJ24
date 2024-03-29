﻿using System;
using System.Linq;
using Card;
using Cysharp.Threading.Tasks;
using Player;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject endWavePage;
    public CardSO[] cards;
    public GameObject[] cardsUI, iconsUI, outlines;
    public GameObject gameOverUI;
    public GameObject youWonUI;
    public TextMeshProUGUI waveText;

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
        Time.timeScale = 1;
        playerMovement = player.GetComponent<PlayerMovement>();
        weightedCardList = new WeightedList<CardSO>();
        foreach (var card in cards)
        {
            weightedCardList.Add(card, card.weight);
        }
        AnimateWaveText();
        Choose3RandomCard();
    }

    public void OpenGameOverScreen()
    {
        Time.timeScale = 0;
        gameOverUI.SetActive(true);
        CloseEndWavePage();
    }
    
    public void OpenYouWonScreen()
    {
        Time.timeScale = 0;
        youWonUI.SetActive(true);
        CloseEndWavePage();
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
        AnimateWaveText();
    }

    public void OpenEndWavePage()
    {
        Time.timeScale = 0;
        playerMovement.joystick.gameObject.SetActive(false);
        endWavePage.SetActive(true);
    }
    
    public void CloseEndWavePage()
    {
        endWavePage.SetActive(false);
    }

    private void Choose3RandomCard()
    {
        for (int i = 0; i < 3; i++)
        {
            var card = weightedCardList.Next();
            chosenCards[i] = card;
            outlines[i].GetComponent<UIOutline>().color = card.GetOutlineColor();
            cardsUI[i].transform.Find("Title").GetComponent<TextMeshProUGUI>().text = card.title;
            cardsUI[i].transform.Find("Rarity").GetComponent<TextMeshProUGUI>().text = card.type.ToString();
            iconsUI[i].GetComponent<UnityEngine.UI.Image>().sprite = card.icon;
        }
    }

    private async void AnimateWaveText()
    {
        waveText.gameObject.SetActive(true);
        waveText.GetComponent<Animator>().SetTrigger("Play");
        await UniTask.WaitForSeconds(3f);
        waveText.gameObject.SetActive(false);
    }
}