using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private readonly Unit _player1 = new Unit();
    private readonly Unit _enemy = new Unit();
    private readonly Unit _enemy2 = new Unit();

    [SerializeField] private PlayerUI player1UI;
    [SerializeField] private PlayerUI enemy1UI;
    [SerializeField] private PlayerUI enemy2UI;
    
    [SerializeField] private AttackCardUI cardUI;
    [SerializeField] private AttackCardUI cardUI2;

    private Card _selectedCard;


    private void Start()
    {
        InitPlayer(_player1, 10, 3, "You", "[[[[   ]]]]\n[[[[><><]]]]\n[[[[   ]]]]", new List<Card>()
        {
            new AttackCard("Ataque basico", "ata", 3),
            new AttackCard("Super Ataque", "ataKKK", 7)
        });
        InitPlayer(_enemy, 10, 3, "Enemy", "{{   }}\n{{----}}\n{{   }}", new List<Card>() {new AttackCard("Ataque basico", "ata", 1)});
        InitPlayer(_enemy2, 10, 3, "Enemy", "{{   }}\n{{----}}\n{{   }}", new List<Card>() {new AttackCard("Ataque basico", "ata", 1)});
        player1UI.SetPlayer(_player1);
        cardUI.Init(_player1.Cards[0], this);
        cardUI2.Init(_player1.Cards[1], this);
        enemy1UI.SetPlayer(_enemy);
        enemy2UI.SetPlayer(_enemy2);
        
    }

    private void InitPlayer(Unit unit, int startHealth, int startResource, string name, string ascii, List<Card> cards)
    {
        unit.PlayerHealth = startHealth;
        unit.PlayerResource = startResource;
        unit.PlayerName = name;
        unit.PlayerASCII = ascii;
        unit.Cards = cards;
    }

    public void SelectCard(Card card)
    {
        if(_selectedCard != null) _selectedCard.IsSelected = false;
        card.IsSelected = true;
        _selectedCard = card;
    }

    public void UseCard(Card card)
    {
        _selectedCard.Use(_player1, _enemy);
    }

    public void DeselectCard(Card card)
    {
        if (_selectedCard == card)
        {
            _selectedCard = null;
        }

        card.IsSelected = false;
    }
}
