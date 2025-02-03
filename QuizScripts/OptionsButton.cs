using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsButton : MonoBehaviour
{
    public Button optionsButton;
    public GameObject panel;
    public TMP_Text optionsText;

    void Start()
    {
        // Baþlangýçta dil seçimi yapýlmýþsa metni güncelle
        UpdateOptionsText();

        // Dil deðiþtiðinde metni güncellemek için event'i dinle
        LanguageSelector.OnLanguageChanged += UpdateOptionsText;

        // Paneli baþlangýçta gizle
        panel.SetActive(false);

        // Butona týklandýðýnda paneli aç
        optionsButton.onClick.AddListener(() =>
        {
            panel.SetActive(true);
        });
    }

    // Dil deðiþimi olduðunda metni güncelleyen metod
    private void UpdateOptionsText()
    {
        string selectedLanguage = PlayerPrefs.GetString("Language", "Turkish");
        if (selectedLanguage == "Turkish")
        {
            optionsText.text = "Ayarlar";
        }
        else if (selectedLanguage == "English")
        {
            optionsText.text = "Options";
        }
    }

    // Script sonlandýðýnda event dinlemesini kaldýr
    private void OnDestroy()
    {
        LanguageSelector.OnLanguageChanged -= UpdateOptionsText;
    }
}

