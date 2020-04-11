using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiUnitStatusPopUp : MonoBehaviour {

    private TextMeshProUGUI _attack;
    private TextMeshProUGUI _attackSpeed;
    private TextMeshProUGUI _movementSpeed;

    private Image _blackSurfaceBox = null;

    void Awake() {
        _attack = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        _attackSpeed = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        _movementSpeed = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        _blackSurfaceBox = GetComponent<Image>();
    }

    public void Configure(GameObject unit) {
        float attack = unit.GetComponent<Weapon>().AttackDamage;
        float attackSpeed = (float) (1.0 / unit.GetComponent<Weapon>().CooldownTime);
        float movementSpeed = unit.GetComponent<Unit>().MoveSpeed;
        string name = unit.GetComponent<Unit>().Name;

        _attack.text = "ATK: " + attack.ToString("0.00");
        _attackSpeed.text = "ASPD: " + attackSpeed.ToString("0.00");
        _movementSpeed.text = "MS: " + movementSpeed.ToString("0.00");
    }

    public void ChangeColor(Color color) {
        _blackSurfaceBox.color = color;
    }

}
