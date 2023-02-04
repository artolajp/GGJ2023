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

    [SerializeField] private Button button;

    private Unit _unit;
    private GameManager _gameManager;

    public Action<Unit> OnUnitClick;

    public void Init(Unit unit, Action<Unit> onUnitClick)
    {
        _unit = unit;

        Refresh(unit);
        unit.OnUnitChanged += Refresh;
        button.onClick.AddListener(OnClick);
        OnUnitClick = onUnitClick;
    }

    private void Refresh(Unit unit)
    {
        playerASCIIText.text = unit.ASCII;
        playerNameText.text = unit.Name;
        playerHealthText.text = unit.Health.ToString();
        playerResourcesText.text = unit.CurrentResources.ToString();
    }

    private void OnClick()
    {
        OnUnitClick?.Invoke(_unit);
    }
}
