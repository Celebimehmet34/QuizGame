using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    public Button startButton;
    public TMP_Text startText;

    void Start()
    {
        // Ba�lang��ta dil se�imi yap�lm��sa metni g�ncelle
        UpdateStartText();

        // Dil de�i�ti�inde metni g�ncellemek i�in event'i dinle
        LanguageSelector.OnLanguageChanged += UpdateStartText;

        startButton.onClick.AddListener(StartGame);
    }

    public void StartGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    // Dil de�i�imi oldu�unda metni g�ncelleyen metod
    private void UpdateStartText()
    {
        string selectedLanguage = PlayerPrefs.GetString("Language", "Turkish");
        if (selectedLanguage == "Turkish")
        {
            startText.text = "Ba�la";
        }
        else if (selectedLanguage == "English")
        {
            startText.text = "Start";
        }
    }

    // Script sonland���nda event dinlemesini kald�r
    private void OnDestroy()
    {
        LanguageSelector.OnLanguageChanged -= UpdateStartText;
    }
}
