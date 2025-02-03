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
        // Baþlangýçta dil seçimi yapýlmýþsa metni güncelle
        UpdateStartText();

        // Dil deðiþtiðinde metni güncellemek için event'i dinle
        LanguageSelector.OnLanguageChanged += UpdateStartText;

        startButton.onClick.AddListener(StartGame);
    }

    public void StartGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    // Dil deðiþimi olduðunda metni güncelleyen metod
    private void UpdateStartText()
    {
        string selectedLanguage = PlayerPrefs.GetString("Language", "Turkish");
        if (selectedLanguage == "Turkish")
        {
            startText.text = "Baþla";
        }
        else if (selectedLanguage == "English")
        {
            startText.text = "Start";
        }
    }

    // Script sonlandýðýnda event dinlemesini kaldýr
    private void OnDestroy()
    {
        LanguageSelector.OnLanguageChanged -= UpdateStartText;
    }
}
