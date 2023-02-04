using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AttackCardUI : MonoBehaviour
{
    [SerializeField] private TMP_Text cardName;
    [SerializeField] private TMP_Text cardDescription;
    [SerializeField] private TMP_Text cardPower;

    [SerializeField] private Button button;

    [SerializeField] private Image selectedImage;

    private Card _card;
    private GameManager _gameManager;

    private void Start()
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnCardClick);
        DeselectCard();
    }

    public void Refresh(AttackCard card)
    {
        cardName.text = card.Name;
        cardDescription.text = card.Description;
        cardPower.text = card.damage.ToString();
    }

    public void Init( Card card, GameManager gameManager)
    {
        _gameManager = gameManager;
        _card = card;
        if (card is AttackCard attackCard)
        {
            Refresh(attackCard);
        }

        _card.OnCardSelected += SelectCard;
        _card.OnCardDeselected += DeselectCard;
    }

    public void OnCardClick()
    {
        if (!_card.IsSelected)
        {
            _gameManager.SelectCard(_card);
        }
        else
        {
            _gameManager.DeselectCard(_card);
        }
    }

    public void SelectCard()
    {
        selectedImage.gameObject.SetActive(true);
    }
    
    public void DeselectCard()
    {
        selectedImage.gameObject.SetActive(false);
    }
    
}
