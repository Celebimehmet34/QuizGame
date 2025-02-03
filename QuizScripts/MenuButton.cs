using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    public Button menuButton;
    public TMP_Text menuText;

    void Start()
    {
        UpdateMenuText();

        LanguageSelector.OnLanguageChanged += UpdateMenuText;

        menuButton.onClick.AddListener(() =>
        {
            SceneManager.LoadSceneAsync(0);
        });
    }

    private void UpdateMenuText()
    {
        string selectedLanguage = PlayerPrefs.GetString("Language", "Turkish");
        if (selectedLanguage == "Turkish")
        {
            menuText.text = "Menü";
        }
        else if (selectedLanguage == "English")
        {
            menuText.text = "Menu";
        }
    }

    private void OnDestroy()
    {
        LanguageSelector.OnLanguageChanged -= UpdateMenuText;
    }
}
