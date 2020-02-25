using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public class UnitAlignmentIndicator : MonoBehaviour {

    Unit _unit;
    GameObject _child;
    Light _light;

    // Start is called before the first frame update
    void Start() {
        _unit = GetComponent<Unit>();
        _child = new GameObject("Alignment Light");
        _light = _child.AddComponent<Light>();
        _light.color = _unit.Alignment == Alignment.Friendly ? Color.blue : Color.red;
        if (TryGetComponent<RichAI>(out RichAI ai)) {
            _light.range = ai.radius * 4;
        } else if (TryGetComponent<NavmeshCut>(out NavmeshCut nc)) {
            _light.range = nc.circleRadius * 2;
        } else {
            _light.range = 3;
        }
        _light.bounceIntensity = 0;
        _child.transform.SetParent(transform);
        _child.transform.localPosition = new Vector3(0f, 1f, 0f);
    }

    public void Destroy() {
        Destroy(_child);
    }
}
