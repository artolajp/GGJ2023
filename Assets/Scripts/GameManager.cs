using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private Unit _player1 = new Unit();

    [SerializeField] private PlayerUI player1UI;
    [SerializeField] private List<UnitUI> enemiesUI;
    
    [SerializeField] private List<AttackCardUI> cardUI;

    [SerializeField] private TMP_Text deckCount;
    [SerializeField] private TMP_Text discardCount;
    [SerializeField] private TMP_Text dayText;

    [SerializeField] private Button endTurnButton;
    
    private Card _selectedCard;

    [SerializeField] private LevelData _levelData;
    [SerializeField] private StageData _stageData;
    [SerializeField] private UnitData _playerData;

    [SerializeField] private GameObject _panelWin;
    [SerializeField] private GameObject _panelLoss;

    [SerializeField] private UnitData _frecuentEnemy;

    private Level _currentLevel;
    private int _currentLevelIndex;
    private int _currentDay;

    private void Start()
    {
        _player1 = _playerData.GetUnit();
        InitPlayer(_player1);
        
        _player1.OnHandChanged += RefreshHand;
        _player1.OnUnitDead += OnUnitDead;
        player1UI.Init(_player1, SelectUnit);
        endTurnButton.onClick.AddListener(EndTurn);
        _currentLevelIndex = 0;
        StartLevel();
    }

    private void StartLevel()
    {
        _currentLevel = _stageData.levels[_currentLevelIndex].GetLevel();
        RefreshEnemies();
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
        _player1.Draw(_player1.HandSize);
        _currentDay++;
        dayText.text = _currentDay.ToString();
        if(_currentLevel.Enemies.FindAll(unit => !unit.IsDead).Count<4)
        {   
            _currentLevel.Enemies.Add(_frecuentEnemy.GetUnit());
        }
        RefreshEnemies();
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
        if (_currentLevel.Enemies.All(enemy => enemy.IsDead))
        {
            _currentLevelIndex++;
            if (_currentLevelIndex < _stageData.levels.Count)
            {
                StartLevel();
            }
            else
            {
                EndGame();
            }
        }

        if (unit == _player1)
        {
            GameOver();
        }
    }

    private void EndGame()
    {
        _panelWin.SetActive(true);
    }

    private void GameOver()
    {
        _panelLoss.SetActive(true);

    }
}
