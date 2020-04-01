using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UiPopUp : MonoBehaviour {
    // For fading
    private float _timeSinceBirth = 0;
    [SerializeField]
    private float _timeStartFade = 0;
    [SerializeField]
    private float _fadeSpeed = 0;
    [SerializeField]
    private float _alphaUpperBound = 0;
    private bool _hitUpperBound = false;

    // For setting up the box properly
    private TextMeshProUGUI _name = null;
    private TextMeshProUGUI _ramenCost = null;
    private TextMeshProUGUI _thymeCost = null;
    private TextMeshProUGUI _description = null;
    private TextMeshProUGUI _flavour = null;

    private Image _blackSurfaceBox = null;

    private RectTransform _rect;

    private float _boxWidth = 500;
    private float _baseHeight = 100;
    private float _heightPerLine = 30f;

    // Can't use start
    void Awake() {
        _name = transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        _thymeCost = transform.GetChild(1).GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        _ramenCost = transform.GetChild(2).GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        _description = transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>();
        _flavour = transform.GetChild(4).gameObject.GetComponent<TextMeshProUGUI>();

        _rect = GetComponent<RectTransform>();

        _boxWidth = _rect.sizeDelta.x;
        _baseHeight = _rect.sizeDelta.y;

        _blackSurfaceBox = GetComponent<Image>();
    }

    public void Configure(GameObject unit, GameObject parent) {
        float minX = _rect.offsetMin.x;
        float minY = _rect.offsetMin.y;
        float maxX = _rect.offsetMax.x;
        float maxY = _rect.offsetMax.y;
        
        _name.text = unit.GetComponent<Unit>().Name;
        
        UnitTraining ut = unit.GetComponent<UnitTraining>();
        
        _ramenCost.text = ut.GetUnitRamenCost().ToString("0");
        _thymeCost.text = ut.GetUnitThymeCost().ToString("0");
        _description.text = ut.GetUnitDescription();
        _flavour.text = ut.GetUnitFlavourText();

        transform.SetParent(parent.transform);

        _rect.localScale = new Vector3(1, 1, 1);
        _rect.offsetMin = new Vector2(minX, maxX);
        _rect.offsetMax = new Vector2(minY, maxY);

        float boxHeight = _baseHeight + GetStringLines(_description.text) + GetStringLines(_flavour.text) * _heightPerLine;
        _rect.sizeDelta = new Vector2(_boxWidth, boxHeight);
        _rect.anchoredPosition = new Vector3(-_boxWidth / 2, boxHeight / 2);
    }

    private int GetStringLines(string str) {
        Regex rx = new Regex("<br>");
        return rx.Matches(str).Count + str.Length / 40;
    }

    // Controls fading
    void Update() {
        if (_hitUpperBound) {
            return;
        }

        if (_timeSinceBirth > _timeStartFade) {
            float currentAlpha = _blackSurfaceBox.color.a;

            float newAlpha = currentAlpha - Time.deltaTime * _fadeSpeed;

            if (newAlpha < _alphaUpperBound) {
                newAlpha = _alphaUpperBound;
                _hitUpperBound = true;
            }

            _blackSurfaceBox.color = new Color(_blackSurfaceBox.color.r, _blackSurfaceBox.color.g, _blackSurfaceBox.color.b, newAlpha);
        } else {
            _timeSinceBirth += Time.deltaTime;
        }
    }
}
