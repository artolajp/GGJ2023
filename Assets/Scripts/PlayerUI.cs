using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text playerASCIIText;
    [SerializeField] private TMP_Text playerNameText;
    [SerializeField] private TMP_Text playerHealthText;
    [SerializeField] private TMP_Text playerResourcesText;
    
    public void SetPlayer(Unit unit)
    {
        Refresh(unit);
        unit.OnUnitChanged += Refresh;
    }

    private void Refresh(Unit unit)
    {
        playerASCIIText.text = unit.PlayerASCII;
        playerNameText.text = unit.PlayerName;
        playerHealthText.text = unit.PlayerHealth.ToString();
        playerResourcesText.text = unit.PlayerResource.ToString();
    }
}
