using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public class UnitAlignmentIndicator : MonoBehaviour {
    public GameObject AlliedMinimapIndicator;
    public GameObject EnemyMinimapIndicator;

    public Color AlliedColor = Color.blue;
    public Color EnemyColor = Color.red;

    Unit _unit;
    GameObject _child;
    Light _light;

    // Start is called before the first frame update
    void Start() {
        _unit = GetComponent<Unit>();
        _child = new GameObject("Alignment Light");
        if (_unit.Alignment == Alignment.Neutral) {
        } else {
            _light = _child.AddComponent<Light>();
            if (TryGetComponent<RichAI>(out RichAI ai)) {
                _light.range = ai.radius * 4;
            } else if (TryGetComponent<NavmeshCut>(out NavmeshCut nc)) {
                _light.range = nc.circleRadius * 2;
            } else {
                _light.range = 3;
            }
            _light.bounceIntensity = 0;
        }

        if (_unit.Alignment == Alignment.Friendly) {
            _light.color = AlliedColor;
            Instantiate(AlliedMinimapIndicator, transform).transform.localScale *= Mathf.Sqrt(_unit.ThreatLevel);
        } else if (_unit.Alignment == Alignment.Hostile) {
            _light.color = EnemyColor;
            Instantiate(EnemyMinimapIndicator, transform).transform.localScale *= Mathf.Sqrt(_unit.ThreatLevel);
        }
        _child.transform.SetParent(transform);
        _child.transform.localPosition = new Vector3(0f, 1f, 0f);
    }

    public void Destroy() {
        Destroy(_child);
    }
}
