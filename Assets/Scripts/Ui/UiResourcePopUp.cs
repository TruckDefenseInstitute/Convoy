using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiResourcePopUp : MonoBehaviour {
    // For fading
    private float _timeSinceBirth = 0;
    [SerializeField]
    private float _timeStartFade = 0;
    [SerializeField]
    private float _fadeSpeed = 0;
    [SerializeField]
    private float _alphaUpperBound = 0;
    private bool _hitUpperBound = false;

    private Image _blackSurfaceBox = null;
    private TextMeshProUGUI _name;
    private TextMeshProUGUI _description;
    private RectTransform _rect;

    public void Awake() {
        _name = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        _description = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        _rect = GetComponent<RectTransform>();
        _blackSurfaceBox = GetComponent<Image>();
    }
    
    public void Configure(GameObject parent, string name, string description) {
        float minX = _rect.offsetMin.x;
        float minY = _rect.offsetMin.y;
        float maxX = _rect.offsetMax.x;
        float maxY = _rect.offsetMax.y;

        transform.SetParent(parent.transform);

        _rect.localScale = new Vector3(1, 1, 1);
        _rect.offsetMin = new Vector2(minX, maxX);
        _rect.offsetMax = new Vector2(minY, maxY);

        _rect.anchoredPosition = new Vector2(0, maxY / 2);

        _name.text = name;
        _description.text = description;
    }

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
