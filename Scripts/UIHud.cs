using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIHud : MonoBehaviour {

    static Slider _healthSlider;
    static Image _fill;
    static Color _normal = Color.green, _mid = Color.yellow, _low = Color.red;

    void Awake()
    {
        GetComponentInChildren<Slider>().fillRect.GetComponent<Image>().color = _normal;
    }

    public static void ChangeHealthSlider(float value, string name)
    {
        _healthSlider = GameObject.Find(name).GetComponentInChildren<Slider>();
        _healthSlider.value = value;
        _fill = _healthSlider.fillRect.GetComponent<Image>();
        if (_healthSlider.value <= 0.7f)
            _fill.color = _mid;
        if (_healthSlider.value <= 0.35f)
            _fill.color = _low;
        if (_healthSlider.value > 0.7f)
            _fill.color = _normal;
        
    }
}
