using UnityEngine;
using TMPro;

public class LanguageSelector : MonoBehaviour
{
    public TMP_Dropdown languageDropdown;


    // Dil deðiþikliði olayý
    public delegate void LanguageChanged();
    public static event LanguageChanged OnLanguageChanged;

    void Start()
    {
        string selectedLanguage = PlayerPrefs.GetString("Language", "Turkish");
        languageDropdown.value = selectedLanguage == "Turkish" ? 1 : 0;


        languageDropdown.onValueChanged.AddListener(delegate { ChangeLanguage(languageDropdown.value); });
    }

    public void ChangeLanguage(int index)
    {
        string newLanguage = index == 0 ? "English" : "Turkish";
        PlayerPrefs.SetString("Language", newLanguage);
        PlayerPrefs.Save();

        Debug.Log("Dil deðiþtirildi: " + newLanguage);

        OnLanguageChanged?.Invoke();
    }
}
