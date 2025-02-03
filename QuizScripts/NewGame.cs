using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewGame : MonoBehaviour
{
    public Button startButton;
    public TMP_Text startText;

    void Start()
    {
        UpdateStartText();

        LanguageSelector.OnLanguageChanged += UpdateStartText;

        startButton.onClick.AddListener(StartGame);
    }

    public void StartGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    private void UpdateStartText()
    {
        string selectedLanguage = PlayerPrefs.GetString("Language", "Turkish");
        if (selectedLanguage == "Turkish")
        {
            startText.text = "Yeni oyun";
        }
        else if (selectedLanguage == "English")
        {
            startText.text = "New game";
        }
    }


    private void OnDestroy()
    {
        LanguageSelector.OnLanguageChanged -= UpdateStartText;
    }
}
