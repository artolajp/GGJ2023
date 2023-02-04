using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private Unit _player1 = new Unit();
    private List<Unit> _enemies;

    [SerializeField] private UnitUI player1UI;
    [SerializeField] private List<UnitUI> enemiesUI;
    
    [SerializeField] private List<AttackCardUI> cardUI;

    [SerializeField] private TMP_Text deckCount;
    [SerializeField] private TMP_Text discardCount;

    [SerializeField] private Button endTurnButton;
    
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
        _enemies = new List<Unit>();
        var _enemy = new Unit();        
        InitPlayer(_enemy, 10, 3, "Enemy", "{{   }}\n{{----}}\n{{   }}", new List<Card>()
        {
            new AttackCard("Ataque basico", "ata", 1,1)
        });
        _enemies.Add(_enemy);
        _enemy = new Unit();
        InitPlayer(_enemy, 10, 3, "Enemy", "{{   }}\n{{----}}\n{{   }}", new List<Card>()
        {
            new AttackCard("Ataque basico", "ata", 1,1)
        });
        _enemies.Add(_enemy);

        player1UI.Init(_player1, SelectUnit);
        _player1.OnHandChanged += RefreshHand;
        _player1.OnUnitDead += OnUnitDead;
        
        RefreshEnemies();

        endTurnButton.onClick.AddListener(EndTurn);
        
        StartTurn();
    }

    private void RefreshEnemies()
    {
        foreach (var unitUI in enemiesUI)
        {
            unitUI.Clear();
        }

        for (int i = 0; i < _enemies.Count; i++)
        {
            enemiesUI[i].Init(_enemies[i], SelectUnit);
            _enemies[i].OnUnitDead += OnUnitDead;
        }
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
        EnemyTurn();
    }

    public void EnemyTurn()
    {
        StartTurn();
    }

    public void StartTurn()
    {
        _player1.CurrentResources = _player1.Resources;
        _player1.Draw(3);
    }
    
    public void RefreshHand(Unit unit){
        if (unit != _player1) return;
        
        for (int i = 0; i < cardUI.Count && i< unit.Hand.Count; i++)
        {
            cardUI[i].Clear();
            cardUI[i].Init(unit.Hand[i], OnCardClick);
        }
        for (int i = unit.Hand.Count; i < cardUI.Count; i++)
        {
            cardUI[i].Clear();
        }

        discardCount.text = _player1.Discard.Count.ToString();
        deckCount.text = _player1.Deck.Count.ToString();
    }

    public void OnUnitDead(Unit unit)
    {
        Debug.Log($"{unit.Name} dead");
        if (_enemies.All(enemy => enemy.IsDead))
        {
            Debug.Log($"En hora buena jugador 1");
        }

        if (unit == _player1)
        {
            Debug.Log($"Derrota!");

        }
    }
}
