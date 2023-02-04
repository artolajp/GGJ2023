using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private readonly Unit _player1 = new Unit();
    private readonly Unit _enemy = new Unit();
    private readonly Unit _enemy2 = new Unit();

    [SerializeField] private UnitUI player1UI;
    [SerializeField] private UnitUI enemy1UI;
    [SerializeField] private UnitUI enemy2UI;
    
    [SerializeField] private AttackCardUI cardUI;
    [SerializeField] private AttackCardUI cardUI2;

    [SerializeField] private TMP_Text deckCount;
    [SerializeField] private TMP_Text discardCount;

    private Card _selectedCard;


    private void Start()
    {
        InitPlayer(_player1, 10, 3, "You", "[[[[   ]]]]\n[[[[><><]]]]\n[[[[   ]]]]", new List<Card>()
        {
            new AttackCard("Ataque basico", "ata", 3,1),
            new AttackCard("Super Ataque", "ataKKK", 7,2),
            new AttackCard("Ataque basico", "ata", 3,1),
            new AttackCard("Super Ataque MAXXXIMO", "aaaaaaaaaaaaaaaaaaaa", 10,3)
        });
        InitPlayer(_enemy, 10, 3, "Enemy", "{{   }}\n{{----}}\n{{   }}", new List<Card>()
        {
            new AttackCard("Ataque basico", "ata", 1,1)
        });
        InitPlayer(_enemy2, 10, 3, "Enemy", "{{   }}\n{{----}}\n{{   }}", new List<Card>()
        {
            new AttackCard("Ataque basico", "ata", 1,1)
        });
        player1UI.Init(_player1, SelectUnit);
        _player1.OnUnitChanged += RefreshPlayerUI;
        _player1.Draw(2);
        cardUI.Init(_player1.Hand[0], OnCardClick);
        cardUI2.Init(_player1.Hand[1], OnCardClick);
        enemy1UI.Init(_enemy, SelectUnit);
        enemy2UI.Init(_enemy2, SelectUnit);
        
    }

    private void InitPlayer(Unit unit, int startHealth, int startResource, string unitName, string ascii, List<Card> cards)
    {
        unit.Health = startHealth;
        unit.Resources = startResource;
        unit.CurrentResources = startResource;
        unit.Name = unitName;
        unit.ASCII = ascii;
        unit.Cards = cards;
        unit.ShuffleAll();
        unit.OnCardUsed += OnCardUsed;
    }

    private void OnCardClick(Card card)
    {
        if (!card.IsSelected)
        {
            SelectCard(card);
        }
        else
        {
            DeselectCard(card);
        }
    }
    
    public void SelectCard(Card card)
    {
        if (_player1.CurrentResources < card.Cost) return;

        if (_selectedCard != null)
        {
            _selectedCard.IsSelected = false;
        }
        
        card.IsSelected = true;
        _selectedCard = card;
    }

    public void DeselectCard(Card card)
    {
        if (_selectedCard == card)
        {
            _selectedCard = null;
        }

        card.IsSelected = false;
    }

    public void SelectUnit(Unit unit)
    {
        if (_selectedCard == null) return;
        _player1.UseCard(unit, _selectedCard);
    }

    public void OnCardUsed(Card card)
    {
        DeselectCard(_selectedCard);
    }

    public void EndTurn()
    {
        _player1.DiscardHand();
    }

    public void RefreshPlayerUI(Unit unit)
    {
        if (unit != _player1) return;
        
        discardCount.text = _player1.Discard.Count.ToString();
        deckCount.text = _player1.Deck.Count.ToString();
    }
}
