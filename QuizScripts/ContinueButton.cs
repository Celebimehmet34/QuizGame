using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ContinueButton : MonoBehaviour
{
    public Button continueButton;
    public TMP_Text continueText;

    public GameObject ResultPanel; 

    void Start()
    {
        UpdateStartText();
        LanguageSelector.OnLanguageChanged += UpdateStartText;
        continueButton.onClick.AddListener(() =>
        {
            ResultPanel.SetActive(false);
        });
    }

    private void UpdateStartText()
    {
        string selectedLanguage = PlayerPrefs.GetString("Language", "Turkish");
        if (selectedLanguage == "Turkish")
        {
            continueText.text = "Devam et";
        }
        else if (selectedLanguage == "English")
        {
            continueText.text = "Continue";
        }
    }

    private void OnDestroy()
    {
        LanguageSelector.OnLanguageChanged -= UpdateStartText;
    }
}