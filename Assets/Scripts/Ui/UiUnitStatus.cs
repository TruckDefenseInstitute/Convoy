using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiUnitStatus : MonoBehaviour {

    // Stats
    private Transform _stats;
    private TextMeshProUGUI _attack;
    private TextMeshProUGUI _attackSpeed;
    private TextMeshProUGUI _movementSpeed;

    // Description
    private Transform _description;
    private TextMeshProUGUI _name;

     void Start() {
        // Unit Status 
        _stats = transform.GetChild(0);
        _attack = _stats.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        _attackSpeed = _stats.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
        _movementSpeed = _stats.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>();

        // Description 
        _description = transform.GetChild(1);
        _name = _description.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void ChangeUnitStatus(GameObject unit) {
        float attack = unit.GetComponent<Weapon>().AttackDamage;
        float attackSpeed = (float) (1.0 / unit.GetComponent<Weapon>().CooldownTime);
        float movementSpeed = unit.GetComponent<Unit>().MoveSpeed;
        string name = unit.GetComponent<Unit>().Name;

        _attack.text = attack.ToString("0");
        _attackSpeed.text = attackSpeed.ToString("0.00");
        _movementSpeed.text = movementSpeed.ToString("0.0");
        _name.text = name;
    }

}
