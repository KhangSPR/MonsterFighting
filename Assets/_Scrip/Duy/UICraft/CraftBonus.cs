using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UI = UnityEngine.UI;

public class CraftBonus : MonoBehaviour {
    [SerializeField] UI.Button craftButton;
    [SerializeField] UI.Slider bonusSlider;
    [SerializeField] TextMeshProUGUI comboText;

    [SerializeField] CraftFX fx;

    [SerializeField] float bonusInterval;
    [SerializeField] float clickInterval;

    float lastClickTime;

    int _combo;
    int combo {
        get {
            return _combo;
        }
        set {
            if (_combo == value) return;
            _combo = value;
            comboText.text = "x"+_combo.ToString();
            if (_combo != 1)
                fx.BonusEffect();
        }
    }

    float _bonusTimer;
    float bonusTimer {
        get {
            return _bonusTimer;
        }
        set {
            if (_bonusTimer == value) return;
            _bonusTimer = value;
            if (_bonusTimer <= 0) {
                bonusActivated = false;
            }
        }
    }

    bool _bonusActivated;
    bool bonusActivated {
        get {
            return _bonusActivated;
        }
        set {
            _bonusActivated = value;
            if (_bonusActivated){
                bonusSlider.gameObject.SetActive(true);
            } else { 
                bonusSlider.gameObject.SetActive(false);
                bonusTimer = bonusInterval;
                combo = 1;
            }
        }
    }

    private void Awake(){
        fx = GetComponent<CraftFX>();
    }

    private void OnEnable(){
        craftButton.onClick.AddListener(() => Bonus());
        craftButton.onClick.AddListener(() => fx.CraftEffect());
    }

    private void OnDisable(){
        craftButton.onClick.RemoveAllListeners();
    }

    private void Start(){
        combo = 1;
        bonusActivated = false;
    }

    private void Update(){
        bonusTimer = Mathf.Clamp(bonusTimer,0,bonusInterval);
        if (bonusActivated){
            bonusTimer -= Time.deltaTime;
            bonusSlider.value = bonusTimer/bonusInterval;
        }
    }

    private void Bonus(){
        var interval = Time.time - lastClickTime;
        lastClickTime = Time.time;
        if (interval <= clickInterval){
            bonusActivated = true;
        }
        if (bonusActivated) {
            combo++;
        }
    }
}
