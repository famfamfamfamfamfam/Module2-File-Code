using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GeneralUI : MonoBehaviour, IOrderOfRunningStart
{
    [SerializeField]
    TextMeshProUGUI validAttack;
    [SerializeField]
    Button dash;
    [SerializeField]
    Slider SEBar;

    public void Init()
    {
        GameManager.instance.HasChanged += TextForValidAttack;
        GameManager.instance.HasChanged += SetSEBarMaxValue;
        GameManager.instance.HasChanged += UpdateSEBarValue;

        dash.onClick.AddListener(CommunicateManager.instance.DashChar().Dash);
        validAttack.text = $"{GameManager.instance.weaponAmount}";
        SEBar.maxValue = GameManager.instance.thresold;
        SEBar.value = GameManager.instance.specialEnergy;
    }

    void TextForValidAttack(string s)
    {
        if (s == "weaponAmount")
            validAttack.text = $"{GameManager.instance.weaponAmount}";
    }

    void SetSEBarMaxValue(string s)
    {
        if (s == "thresold")
            SEBar.maxValue = GameManager.instance.thresold;
    }

    void UpdateSEBarValue(string s)
    {
        if (s == "specialEnergy")
            SEBar.value = GameManager.instance.specialEnergy;
    }

    private void OnDisable()
    {
        GameManager.instance.HasChanged -= TextForValidAttack;
        GameManager.instance.HasChanged -= SetSEBarMaxValue;
        GameManager.instance.HasChanged -= UpdateSEBarValue;
    }

    void OnDestroy()
    {
        dash.onClick.RemoveAllListeners();
    }
}
