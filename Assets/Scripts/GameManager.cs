using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private Unit _player1 = new Unit();

    [SerializeField] private UnitUI player1UI;
    [SerializeField] private List<UnitUI> enemiesUI;
    
    [SerializeField] private List<AttackCardUI> cardUI;

    [SerializeField] private TMP_Text deckCount;
    [SerializeField] private TMP_Text discardCount;

    [SerializeField] private Button endTurnButton;
    
    private Card _selectedCard;

    [SerializeField] private LevelData _levelData;
    [SerializeField] private UnitData _playerData;

    private Level _currentLevel;

    private void Start()
    {
        _player1 = _playerData.GetUnit();
        InitPlayer(_player1);

        _currentLevel = _levelData.GetLevel();
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

        for (int i = 0; i < _currentLevel.Enemies.Count; i++)
        {
            enemiesUI[i].Init(_currentLevel.Enemies[i], SelectUnit, true);
            _currentLevel.Enemies[i].OnUnitDead += OnUnitDead;
        }
    }

    private void InitPlayer(Unit unit)
    {
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
        foreach (Unit enemy in _currentLevel.Enemies)
        {
            if(enemy.IsDead) continue;
            enemy.UseFirstCardAvailable(_player1);
            enemy.CurrentResources += 2;
        }
        
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
            cardUI[i].Init(unit.Hand[i], _player1.CurrentResources ,OnCardClick);
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
        if (_currentLevel.Enemies.All(enemy => enemy.IsDead))
        {
            Debug.Log($"En hora buena jugador 1");
        }

        if (unit == _player1)
        {
            Debug.Log($"Derrota!");
        }
    }
    
    
}
