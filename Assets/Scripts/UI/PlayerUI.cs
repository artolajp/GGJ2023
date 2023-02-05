using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text playerResourcesText;

    [SerializeField] private Button button;
    [SerializeField] private Slider _slider;

    private Unit _unit;
    private GameManager _gameManager;

    public Action<Unit> OnUnitClick;

    public void Init(Unit unit, Action<Unit> onUnitClick)
    {
        _unit = unit;
        Refresh(unit);
        unit.OnUnitChanged += Refresh;
        unit.OnUnitDead += OnUnitDead;
        button.onClick.AddListener(OnClick);
        OnUnitClick = onUnitClick;
    }

    private void Refresh(Unit unit)
    {
        playerResourcesText.text = unit.CurrentResources.ToString();
        gameObject.SetActive(true);
        _slider.maxValue = unit.Health;
        _slider.value = unit.CurrentHealth;
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
