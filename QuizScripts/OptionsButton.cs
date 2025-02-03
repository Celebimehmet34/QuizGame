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
        // Ba�lang��ta dil se�imi yap�lm��sa metni g�ncelle
        UpdateOptionsText();

        // Dil de�i�ti�inde metni g�ncellemek i�in event'i dinle
        LanguageSelector.OnLanguageChanged += UpdateOptionsText;

        // Paneli ba�lang��ta gizle
        panel.SetActive(false);

        // Butona t�kland���nda paneli a�
        optionsButton.onClick.AddListener(() =>
        {
            panel.SetActive(true);
        });
    }

    // Dil de�i�imi oldu�unda metni g�ncelleyen metod
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

    // Script sonland���nda event dinlemesini kald�r
    private void OnDestroy()
    {
        LanguageSelector.OnLanguageChanged -= UpdateOptionsText;
    }
}

