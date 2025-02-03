using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuitButton : MonoBehaviour
{
    public TMP_Text quitText;

    void Start()
    {
        // Baþlangýçta metni güncelle
        UpdateQuitText();

        // Dil deðiþtiðinde metni güncellemek için event'i dinle
        LanguageSelector.OnLanguageChanged += UpdateQuitText;
    }

    // Quit butonundaki metni dile göre güncelleyen metod
    private void UpdateQuitText()
    {
        string selectedLanguage = PlayerPrefs.GetString("Language", "Turkish");

        if (selectedLanguage == "Turkish")
        {
            quitText.text = "Çýkýþ";
        }
        else if (selectedLanguage == "English")
        {
            quitText.text = "Quit";
        }
    }

    public void QuitGame()
    {

        Debug.Log("uygulama kapatýldý");
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
