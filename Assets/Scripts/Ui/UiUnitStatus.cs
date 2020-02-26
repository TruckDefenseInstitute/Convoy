using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiUnitStatus : MonoBehaviour {

    // Stats panel
    private Transform _statsPanel;
    private TextMeshProUGUI _attack;
    private TextMeshProUGUI _attackSpeed;
    private TextMeshProUGUI _movementSpeed;

    // Skills panel not yet implemented

    private void Start() {
        // Unit Status Panel
        _statsPanel = transform.GetChild(0);
        _attack = _statsPanel.GetChild(0).GetComponent<TextMeshProUGUI>();
        _attackSpeed = _statsPanel.GetChild(1).GetComponent<TextMeshProUGUI>();
        _movementSpeed = _statsPanel.GetChild(2).GetComponent<TextMeshProUGUI>();
    }

    public void ChangeUnitStatus(GameObject unit) {
        _attack.text = unit.GetComponent<Weapon>().AttackDamage.ToString();
        _attackSpeed.text = (1.0 / unit.GetComponent<Weapon>().CooldownTime).ToString();
        _movementSpeed.text = unit.GetComponent<Unit>().MoveSpeed.ToString();
    }

}
