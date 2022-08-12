﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreboardView : BaseView
{
    [SerializeField] private Button closeButton;
    [SerializeField] private PlayerScoreboardCard cardPrefab;
    private VerticalLayoutGroup layoutGroup;
    private readonly Dictionary<string,PlayerScoreboardCard> playerCards = new Dictionary<string, PlayerScoreboardCard>();

    public override void Init()
    {
        base.Init();
        layoutGroup = GetComponentInChildren<VerticalLayoutGroup>();    
        closeButton.onClick.AddListener(() =>
        {
            Show(false);
            ViewManager.Instance.Show<PausedView>(true);
        });
    }
    public void AddPlayerCards(List<PlayerScoreboardCardData> cardsData)
    {
        foreach (var cardData in cardsData)
        {
            AddPlayerCard(cardData);
        }
    }
       
    private void AddPlayerCard(PlayerScoreboardCardData cardData)
    {
        if (playerCards.ContainsKey(cardData.playerName))
            return;
        PlayerScoreboardCard playerScoreboardCard = Instantiate(cardPrefab);
        playerScoreboardCard.transform.SetParent(layoutGroup.transform, false);
        playerScoreboardCard.UpdateCard(cardData);   
        playerCards.Add(cardData.playerName, playerScoreboardCard);
    }

    public void RemovePlayerCard(string cardTag)
    {
        if (playerCards.ContainsKey(cardTag))
        {
            playerCards.TryGetValue(cardTag, out PlayerScoreboardCard playerScoreboardCard);
            playerScoreboardCard.gameObject.SetActive(false); //TODO: Pooling
            playerCards.Remove(cardTag);
        }
    }

    public void RefreshPlayerCard(PlayerScoreboardCardData cardData)
    {
        if (playerCards.TryGetValue(cardData.playerName, out PlayerScoreboardCard card))
            card.UpdateCard(cardData);
    }
}
