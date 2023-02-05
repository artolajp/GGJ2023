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
    [SerializeField] private TMP_Text cardCost;

    [SerializeField] private Button button;

    [SerializeField] private Image selectedImage;

    private Card _card;
    private Action<Card> OnCardClick;
    private int _resources;

    private void Start()
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(ClickCard);
        DeselectCard();
    }

    public void Refresh(AttackCard card)
    {
        cardName.text = card.Name;
        cardDescription.text = "root@casihacker:~#" + card.Description;
        cardPower.text = card.damage.ToString();
        cardCost.text = card.Cost.ToString();
        cardCost.color = _resources < card.Cost ? Color.red : Color.white;
        cardPower.color = card.Cost > 0 ? Color.white : Color.green;
    }

    public void Init( Card card, int resources,  Action<Card> onCardClick)
    {
        _card = card;
        OnCardClick = onCardClick;
        _resources = resources;

        if (card is AttackCard attackCard)
        {
            Refresh(attackCard);
        }

        _card.OnCardSelected += SelectCard;
        _card.OnCardDeselected += DeselectCard;
        gameObject.SetActive(true);

    }

    public void ClickCard()
    {
        OnCardClick?.Invoke(_card);
    }

    public void SelectCard()
    {
        selectedImage.gameObject.SetActive(true);
    }
    
    public void DeselectCard()
    {
        selectedImage.gameObject.SetActive(false);
    }

    public void Clear()
    {
        if (_card != null)
        {
            _card.OnCardSelected -= SelectCard;
            _card.OnCardDeselected -= DeselectCard;
            _card = null;
        }
        
        OnCardClick = null;
        DeselectCard();
        gameObject.SetActive(false);
    }
}
