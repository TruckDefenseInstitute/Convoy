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

    private GameObject _thyme;
    private GameObject _ramen;

    private GameObject _name = null;
    private GameObject _description = null;
    private GameObject _flavour = null;
    private GameObject _unitStatus = null;

    private Image _blackSurfaceBox = null;
    private RectTransform _rect;

    private float _fontHeight = 90.735f;     // Get from the font itself.
    


    // Can't use start
    void Awake() {
        _name = transform.GetChild(0).gameObject;
        _thyme = transform.GetChild(1).gameObject;
        _ramen = transform.GetChild(2).gameObject;
        _description = transform.GetChild(3).gameObject;
        _flavour = transform.GetChild(4).gameObject;
        _unitStatus = transform.GetChild(5).gameObject;

        _rect = GetComponent<RectTransform>();

        _blackSurfaceBox = GetComponent<Image>();
    }

    public void ConfigureUnitPopUp(GameObject parent, GameObject unit) {
        UnitTraining ut = unit.GetComponent<UnitTraining>();
        
        _thyme.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = ut.GetUnitThymeCost().ToString("0");
        _ramen.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = ut.GetUnitRamenCost().ToString("0");
        
        _name.GetComponent<TextMeshProUGUI>().text = unit.GetComponent<Unit>().Name;
        _description.GetComponent<TextMeshProUGUI>().text = ut.GetUnitDescription();
        _flavour.GetComponent<TextMeshProUGUI>().text = ut.GetUnitFlavourText();
        _unitStatus.GetComponent<UiUnitStatusPopUp>().Configure(unit);

        Configure(parent);
    }

    public void ConfigureResourcePopUp(GameObject parent, string name, string description, string flavour) {
        _thyme.SetActive(false);
        _ramen.SetActive(false);
        _unitStatus.SetActive(false);

        _name.GetComponent<TextMeshProUGUI>().text = name;
        _description.GetComponent<TextMeshProUGUI>().text = description;
        _flavour.GetComponent<TextMeshProUGUI>().text = flavour;

        Configure(parent);
    }

    public void ConfigureHealthPopUp(GameObject parent, string name, string description) {
        _thyme.SetActive(false);
        _ramen.SetActive(false);
        _unitStatus.SetActive(false);

        _name.GetComponent<TextMeshProUGUI>().text = name;
        _description.GetComponent<TextMeshProUGUI>().text = description;
        _flavour.GetComponent<TextMeshProUGUI>().text = "";

        Configure(parent);
    }

    private void Configure(GameObject parent) {
        float minX = _rect.offsetMin.x;
        float minY = _rect.offsetMin.y;
        float maxX = _rect.offsetMax.x;
        float maxY = _rect.offsetMax.y;
        
        transform.SetParent(parent.transform);

        _rect.localScale = new Vector3(1, 1, 1);
        _rect.offsetMin = new Vector2(minX, maxX);
        _rect.offsetMax = new Vector2(minY, maxY);

        
        // Get the size of the inside elements.
        float boxHeight = 40; // Flat 40 for spacing
        
        TextMeshProUGUI nameText = _name.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI descriptionText = _description.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI flavourText = _flavour.GetComponent<TextMeshProUGUI>();
        
        nameText.ForceMeshUpdate();
        descriptionText.ForceMeshUpdate();
        flavourText.ForceMeshUpdate();

        _flavour.GetComponent<RectTransform>().anchoredPosition = 
                new Vector2(_flavour.GetComponent<RectTransform>().anchoredPosition.x, 
                        _flavour.GetComponent<RectTransform>().anchoredPosition.y -
                        (descriptionText.textInfo.lineCount - 1) * 
                        descriptionText.textInfo.lineInfo[0].lineHeight -
                        10);    // A flat 10 for spacing
        
        boxHeight += nameText.textInfo.lineCount * nameText.textInfo.lineInfo[0].lineHeight + 
                descriptionText.textInfo.lineCount * descriptionText.textInfo.lineInfo[0].lineHeight +
                flavourText.textInfo.lineCount * flavourText.textInfo.lineInfo[0].lineHeight;

        _rect.sizeDelta = new Vector2(_rect.sizeDelta.x, boxHeight);
        _rect.anchoredPosition = new Vector2(_rect.anchoredPosition.x, boxHeight / 2);
      
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

            Color nextColor = new Color(_blackSurfaceBox.color.r, _blackSurfaceBox.color.g, _blackSurfaceBox.color.b, newAlpha);
            _blackSurfaceBox.color = nextColor;
            _unitStatus.GetComponent<UiUnitStatusPopUp>().ChangeColor(nextColor);
        } else {
            _timeSinceBirth += Time.deltaTime;
        }
    }
}
