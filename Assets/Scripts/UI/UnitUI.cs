using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitUI : MonoBehaviour
{
    [SerializeField] private TMP_Text playerASCIIText;
    [SerializeField] private TMP_Text playerNameText;
    [SerializeField] private TMP_Text playerHealthText;
    [SerializeField] private TMP_Text playerResourcesText;
    [SerializeField] private TMP_Text nextCardText;

    [SerializeField] private Button button;

    private Unit _unit;
    private GameManager _gameManager;
    private bool _isEnemy;

    public Action<Unit> OnUnitClick;

    public void Init(Unit unit, Action<Unit> onUnitClick, bool isEnemy = false)
    {
        _unit = unit;
        _isEnemy = isEnemy;
        Refresh(unit);
        unit.OnUnitChanged += Refresh;
        unit.OnUnitDead += OnUnitDead;
        button.onClick.AddListener(OnClick);
        OnUnitClick = onUnitClick;
    }

    private void Refresh(Unit unit)
    {
        playerASCIIText.text = unit.ASCII;
        playerNameText.text = unit.Name;
        playerHealthText.text = unit.Health.ToString();
        playerResourcesText.text = unit.CurrentResources.ToString();
        gameObject.SetActive(true);
        nextCardText.text = _isEnemy ? unit.GetNextCardAvailable()?.Name : "";
    }
    
    private void OnUnitDead(Unit unit)
    {
        Clear();
    }

    public void Clear()
    {        
        gameObject.SetActive(false);
        if (_unit != null)
        {
            _unit.OnUnitChanged -= Refresh;
            _unit.OnUnitDead -= OnUnitDead;
            _unit = null;
        }
        
    }

    private void OnClick()
    {
        OnUnitClick?.Invoke(_unit);
    }
}
