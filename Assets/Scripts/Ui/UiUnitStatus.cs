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

     void Start() {
        // Unit Status Panel
        _statsPanel = transform.GetChild(0);
        _attack = GameObject.Find("AttackText").GetComponent<TextMeshProUGUI>();
        _attackSpeed = GameObject.Find("AttackSpeedText").GetComponent<TextMeshProUGUI>();
        _movementSpeed = GameObject.Find("MovementSpeedText").GetComponent<TextMeshProUGUI>();
    }

    public void ChangeUnitStatus(GameObject unit) {
        float attack = unit.GetComponent<Weapon>().AttackDamage;
        float attackSpeed = (float) (1.0 / unit.GetComponent<Weapon>().CooldownTime);
        float movementSpeed = unit.GetComponent<Unit>().MoveSpeed;
        
        _attack.text = attack.ToString("0.00");
        _attackSpeed.text = attackSpeed.ToString("0.00");
        _movementSpeed.text = movementSpeed.ToString("0.00");
    }

}
