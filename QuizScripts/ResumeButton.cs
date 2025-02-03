using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResumeButton : MonoBehaviour
{
    public TMP_Text quitText;

    void Start()
    {
        UpdateQuitText();
        LanguageSelector.OnLanguageChanged += UpdateQuitText;
    }

    private void UpdateQuitText()
    {
        string selectedLanguage = PlayerPrefs.GetString("Language", "Turkish");

        if (selectedLanguage == "Turkish")
        {
            quitText.text = "Devam";
        }
        else if (selectedLanguage == "English")
        {
            quitText.text = "Resume";
        }
    }

    private void OnDestroy()
    {
        LanguageSelector.OnLanguageChanged -= UpdateQuitText;
    }
}
