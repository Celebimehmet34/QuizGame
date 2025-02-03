using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuitButton : MonoBehaviour
{
    public TMP_Text quitText;

    void Start()
    {
        // Ba�lang��ta metni g�ncelle
        UpdateQuitText();

        // Dil de�i�ti�inde metni g�ncellemek i�in event'i dinle
        LanguageSelector.OnLanguageChanged += UpdateQuitText;
    }

    // Quit butonundaki metni dile g�re g�ncelleyen metod
    private void UpdateQuitText()
    {
        string selectedLanguage = PlayerPrefs.GetString("Language", "Turkish");

        if (selectedLanguage == "Turkish")
        {
            quitText.text = "��k��";
        }
        else if (selectedLanguage == "English")
        {
            quitText.text = "Quit";
        }
    }

    public void QuitGame()
    {

        Debug.Log("uygulama kapat�ld�");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    private void OnDestroy()
    {
        LanguageSelector.OnLanguageChanged -= UpdateQuitText;
    }
}
