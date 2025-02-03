using System.IO;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using System.Collections;

using System.Collections.Generic;

public class QuizManager : MonoBehaviour
{
    public TMP_Text questionText;
    public TMP_Text optionAText;
    public TMP_Text optionBText;
    public TMP_Text optionCText;
    public TMP_Text optionDText;
    public TMP_Text resultText;

    public Button AButton;
    public Button BButton;
    public Button CButton;
    public Button DButton;

    public TMP_Text questionIndex;

    public Button MenuButton;
    public Button MenuButton2;
    public Button MenuButton3;
    public Button ResumeButton;


    public GameObject MenuPanel;
    public GameObject OptionsPanel;

    public Button HalfButton;
    public Button CallButton;
    public Button PeopleButton;
    public Button DoubleButton;

    public GameObject ResultPanel;


    private string[] questions;
    private string[][] options;
    private string[] correctAnswers;

    private List<int> availableIndexes;
    private string filePath2 = "";

    System.Random random = new System.Random();


    private int[] deletedOptions;
    private int randomQuestion = 0;
    private int currentQuestionIndex = 0;
    int randomIndex = 0;
    private int AnswerChance = 1;
    private string GetPersistentPath(string fileName)
    {
        return Path.Combine(Application.persistentDataPath, fileName);
    }


    void Start()
    {
        DoubleButton.onClick.AddListener(() =>
        {
            AnswerChance = 2;
            DoubleButton.interactable = false;
        });
        ResultPanel.SetActive(false);
        MenuPanel.SetActive(false);
        OptionsPanel.SetActive(false);
        if (!PlayerPrefs.HasKey("Language"))
        {
            PlayerPrefs.SetString("Language", "Turkish");
            PlayerPrefs.Save();
        }

        string selectedLanguage = PlayerPrefs.GetString("Language", "Turkish");
        string filePath = "";
        deletedOptions = new int[3];
        
        if (selectedLanguage == "Turkish")
        {
            filePath = "soru1";
            filePath2 = "indeks1.txt";
            Debug.Log("Seçilen dil türkçe");
        }
        else if (selectedLanguage == "English")
        {
            filePath = "question1";
            filePath2 = "index1.txt";
        }
        InitializeIndexFile();
        LoadQuestions(filePath);
        AButton.onClick.AddListener(() => CheckAnswer("A"));
        BButton.onClick.AddListener(() => CheckAnswer("B"));
        CButton.onClick.AddListener(() => CheckAnswer("C"));
        DButton.onClick.AddListener(() => CheckAnswer("D"));
        MenuButton.onClick.AddListener(() =>
        {
            MenuPanel.SetActive(true);
        });
        MenuButton2.onClick.AddListener(() =>
        {
            MenuPanel.SetActive(false);
        });
        MenuButton3.onClick.AddListener(() =>
        {
            MenuPanel.SetActive(false);
            OptionsPanel.SetActive(false);
        });
        ResumeButton.onClick.AddListener(() =>
        {
            MenuPanel.SetActive(false);
        });



        HalfButton.onClick.AddListener(UseHalfButton);



        DisplayQuestion();
    }

    private void InitializeIndexFile()
    {
        string indexPath = GetPersistentPath(filePath2);
        if (!File.Exists(indexPath))
        {
            availableIndexes = new List<int>();
            for (int i = 0; i < 70; i++)
            {
                availableIndexes.Add(i);
            }
            SaveNumbersToFile(filePath2);
        }
        else
        {
            LoadNumbersFromFile();
        }
    }

    public void LoadQuestions(string filePath)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(filePath);
        if (textAsset == null)
        {
            Debug.LogError($"Soru dosyasý bulunamadý: {filePath}");
            return;
        }

        try
        {
            string[] lines = textAsset.text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            int questionCount = lines.Length / 6;
            questions = new string[questionCount];
            options = new string[questionCount][];
            correctAnswers = new string[questionCount];

            for (int i = 0; i < lines.Length; i += 6)
            {
                int questionIndex = i / 6;
                questions[questionIndex] = lines[i].Trim();
                options[questionIndex] = new string[]
                {
                    lines[i + 1].Split(':')[1].Trim(),
                    lines[i + 2].Split(':')[1].Trim(),
                    lines[i + 3].Split(':')[1].Trim(),
                    lines[i + 4].Split(':')[1].Trim()
                };
                correctAnswers[questionIndex] = lines[i + 5].Split(':')[1].Trim();
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Soru yüklenirken hata oluþtu: {e.Message}");
        }
    }


    void DisplayQuestion()
    {
        if(availableIndexes.Count == 0) {
            LoadNumbersFromFile();
        }
        randomIndex = UnityEngine.Random.Range(0, availableIndexes.Count);
        randomQuestion = availableIndexes[randomIndex];
        deletedOptions[0] = 5;
        deletedOptions[1] = 5;
        deletedOptions[2] = 5;
        RepairButtons();
        availableIndexes.RemoveAt(randomIndex);
        SaveNumbersToFile(filePath2);
        questionIndex.text = (currentQuestionIndex+1).ToString() + " / 10";
        questionText.text = questions[randomQuestion];
        optionAText.text = options[randomQuestion][0];
        optionBText.text = options[randomQuestion][1];
        optionCText.text = options[randomQuestion][2];
        optionDText.text = options[randomQuestion][3];
        AnswerChance = 1;
    }


    public void CheckAnswer(string selectedAnswer)
    {
        if (AnswerChance == 1)
        {
            if (selectedAnswer == correctAnswers[randomQuestion])
            {
                resultText.text = "Doðru!";
                for (int i = 0; i < 3; i++)
                {
                    deletedOptions[i] = 5; // Tüm seçenekleri sýfýrla
                }
                StartCoroutine(WaitAndProceed());
            }
            else
            {
                resultText.text = "Yanlýþ!";
                ResultPanel.SetActive(true);

                int index = 0;
                while (index < deletedOptions.Length && deletedOptions[index] != 5)
                {
                    index++;
                }
                if (index < deletedOptions.Length)
                {
                    int optionIndex = GetOptionIndex(selectedAnswer);
                    deletedOptions[index] = optionIndex; 
                    DisableOption(optionIndex);
                }
            }
        }
        else
        {
            if (selectedAnswer == correctAnswers[randomQuestion])
            {
                resultText.text = "Doðru!";
                for (int i = 0; i < 3; i++)
                {
                    deletedOptions[i] = 5;
                }
                StartCoroutine(WaitAndProceed());
            }
            else
            {
                resultText.text = "Yanlýþ! Bir hakkýnýz daha var.";

                int index = 0;
                while (index < deletedOptions.Length && deletedOptions[index] != 5)
                {
                    index++;
                }
                if (index < deletedOptions.Length)
                {
                    int optionIndex = GetOptionIndex(selectedAnswer);
                    deletedOptions[index] = optionIndex;
                    DisableOption(optionIndex);
                }
                AnswerChance--;
            }
        }
    }
    private IEnumerator WaitAndProceed()
    {
        yield return new WaitForSeconds(0.5f);
        resultText.text = "";
        NextQuestion();
    }

    public void NextQuestion()
    {
        currentQuestionIndex++;
        loadQuestions();
        if (currentQuestionIndex < 10)
        {
            DisplayQuestion();
        }
        else
        {
            resultText.text = "Quiz Bitti!";
        }
    }


    public void UseHalfButton()
    {
        string correctAnswer = correctAnswers[randomQuestion];
        int correctIndex = GetOptionIndex(correctAnswer);
        System.Random rand = new System.Random();

        // Aktif olan þýklarý bul
        List<int> activeOptions = new List<int>();
        for (int i = 0; i < 4; i++)
        {
            if (!IsOptionDeleted(i))
            {
                activeOptions.Add(i);
            }
        }

        // Aktif þýklardan yanlýþ olanlarý bul
        List<int> activeWrongOptions = new List<int>();
        foreach (int option in activeOptions)
        {
            if (option != correctIndex)
            {
                activeWrongOptions.Add(option);
            }
        }

        // Aktif yanlýþ þýk sayýsýna göre iþlem yap
        if (activeWrongOptions.Count == 1)
        {
            // Sadece bir yanlýþ þýk kalmýþsa onu ele
            DisableOption(activeWrongOptions[0]);

            // deletedOptions dizisine ekle
            int index = 0;
            while (index < deletedOptions.Length && deletedOptions[index] != 5)
            {
                index++;
            }
            if (index < deletedOptions.Length)
            {
                deletedOptions[index] = activeWrongOptions[0];
            }
        }
        else if (activeWrongOptions.Count == 2)
        {
            // Ýki yanlýþ þýk kalmýþsa ikisini de ele
            foreach (int option in activeWrongOptions)
            {
                DisableOption(option);

                // deletedOptions dizisine ekle
                int index = 0;
                while (index < deletedOptions.Length && deletedOptions[index] != 5)
                {
                    index++;
                }
                if (index < deletedOptions.Length)
                {
                    deletedOptions[index] = option;
                }
            }
        }
        else
        {
            // Ýkiden fazla yanlýþ þýk varsa rastgele ikisini ele
            int[] optionsToHide = new int[2];

            // Ýlk yanlýþ þýkký seç
            int randomIndex = rand.Next(0, activeWrongOptions.Count);
            optionsToHide[0] = activeWrongOptions[randomIndex];

            // Ýkinci yanlýþ þýkký seç
            activeWrongOptions.RemoveAt(randomIndex);
            randomIndex = rand.Next(0, activeWrongOptions.Count);
            optionsToHide[1] = activeWrongOptions[randomIndex];

            // Seçilen þýklarý disable et ve deletedOptions'a ekle
            foreach (int option in optionsToHide)
            {
                DisableOption(option);

                int index = 0;
                while (index < deletedOptions.Length && deletedOptions[index] != 5)
                {
                    index++;
                }
                if (index < deletedOptions.Length)
                {
                    deletedOptions[index] = option;
                }
            }
        }

        HalfButton.interactable = false;
    }

    private bool IsOptionDeleted(int optionIndex)
    {
        for (int i = 0; i < deletedOptions.Length; i++)
        {
            if (deletedOptions[i] == optionIndex)
                return true;
        }
        return false;
    }




    private void DisableOption(int optionIndex)
    {
        TMP_Text buttonText = null;
        Button buttonToDisable = null;
        Image buttonImage = null;

        switch (optionIndex)
        {
            case 0:
                buttonText = optionAText;
                buttonToDisable = AButton;
                buttonImage = AButton.GetComponent<Image>();
                break;
            case 1:
                buttonText = optionBText;
                buttonToDisable = BButton;
                buttonImage = BButton.GetComponent<Image>();
                break;
            case 2:
                buttonText = optionCText;
                buttonToDisable = CButton;
                buttonImage = CButton.GetComponent<Image>();
                break;
            case 3:
                buttonText = optionDText;
                buttonToDisable = DButton;
                buttonImage = DButton.GetComponent<Image>();
                break;
        }

        buttonToDisable.interactable = false;

        if (buttonImage != null)
        {
            Color color = buttonImage.color;
            color.a = 0.5f;
            buttonImage.color = color;
        }

        if (buttonText != null)
        {
            Color textColor = buttonText.color;
            textColor.a = 0.5f;
            buttonText.color = textColor;
        }
    }

    private int GetOptionIndex(string correctAnswer)
    {
        switch (correctAnswer)
        {
            case "A":
                return 0;
            case "B":
                return 1;
            case "C":
                return 2;
            case "D":
                return 3;
            default:
                return -1;
        }
    }


    public void RepairButtons()
    {
        TMP_Text buttonText = null;
        Button buttonToDisable = null;
        Image buttonImage = null;
        buttonText = optionAText;
        buttonToDisable = AButton;
        buttonImage = AButton.GetComponent<Image>();

        buttonToDisable.interactable = true;

        if (buttonImage != null)
        {
            Color color = buttonImage.color;
            color.a = 1f;
            buttonImage.color = color;
        }

        if (buttonText != null)
        {
            Color textColor = buttonText.color;
            textColor.a = 1f;
            buttonText.color = textColor;
        }

        buttonText = optionBText;
        buttonToDisable = BButton;
        buttonImage = BButton.GetComponent<Image>();

        buttonToDisable.interactable = true;

        if (buttonImage != null)
        {
            Color color = buttonImage.color;
            color.a = 1f;
            buttonImage.color = color;
        }

        if (buttonText != null)
        {
            Color textColor = buttonText.color;
            textColor.a = 1f;
            buttonText.color = textColor;
        }

        buttonText = optionCText;
        buttonToDisable = CButton;
        buttonImage = CButton.GetComponent<Image>();

        buttonToDisable.interactable = true;

        if (buttonImage != null)
        {
            Color color = buttonImage.color;
            color.a = 1f;
            buttonImage.color = color;
        }

        if (buttonText != null)
        {
            Color textColor = buttonText.color;
            textColor.a = 1f;
            buttonText.color = textColor;
        }

        buttonText = optionDText;
        buttonToDisable = DButton;
        buttonImage = DButton.GetComponent<Image>();

        buttonToDisable.interactable = true;

        if (buttonImage != null)
        {
            Color color = buttonImage.color;
            color.a = 1f;
            buttonImage.color = color;
        }

        if (buttonText != null)
        {
            Color textColor = buttonText.color;
            textColor.a = 1f;
            buttonText.color = textColor;
        }

    }


    public void loadQuestions()
    {
        string filePath = "";
        string selectedLanguage = PlayerPrefs.GetString("Language", "Turkish");
        if(selectedLanguage == "Turkish")
        {
            if (currentQuestionIndex <= 1)
            {

                filePath = "soru1";
                filePath2 = "indeks1.txt";
            }
            else if (currentQuestionIndex <= 3)
            {
                filePath = "soru2";
                filePath2 = "indeks2.txt";
            }
            else if (currentQuestionIndex <= 5)
            {
                filePath = "soru3";
                filePath2 = "indeks3.txt";
            }
            else if (currentQuestionIndex <= 7)
            {
                filePath = "soru4";
                filePath2 = "indeks4.txt";
            }
            else if (currentQuestionIndex <= 9)
            {
                filePath = "soru5";
                filePath2 = "indeks5.txt";
            }
        }
        else if(selectedLanguage == "English")
        {
            if (currentQuestionIndex <= 1)
            {

                filePath = "question1";
                filePath2 = "index1.txt";
            }
            else if (currentQuestionIndex <= 3)
            {
                filePath = "question2";
                filePath2 = "index2.txt";
            }
            else if (currentQuestionIndex <= 5)
            {
                filePath = "question3";
                filePath2 = "index3.txt";
            }
            else if (currentQuestionIndex <= 7)
            {
                filePath = "question4";
                filePath2 = "index4.txt";
            }
            else if (currentQuestionIndex <= 9)
            {
                filePath = "question5";
                filePath2 = "index5.txt";
            }
        }
        

        LoadQuestions(filePath);
        LoadNumbersFromFile();
    }

    private void LoadNumbersFromFile()
    {

        string filePath = GetPersistentPath(filePath2);
        availableIndexes = new List<int>();

        try
        {
            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (int.TryParse(line, out int number))
                        {
                            availableIndexes.Add(number);
                            Debug.Log("eklenen index :" + number.ToString());
                            Debug.Log("dosyanýn adres :" +filePath);
                        }
                    }
                    
                }
                if(availableIndexes.Count <= 1)
                {
                    for (int i = 0; i < 70; i++)
                    {
                        availableIndexes.Add(i);
                    }
                    SaveNumbersToFile(filePath2);
                }
            }
            else
            {
                
                for (int i = 0; i < 70; i++)
                {
                    availableIndexes.Add(i);
                }
                SaveNumbersToFile(filePath2);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Index dosyasý okuma hatasý: {e.Message}");
            // Hata durumunda yeni liste oluþtur
            availableIndexes = new List<int>();
            for (int i = 0; i < 10; i++)
            {
                availableIndexes.Add(i);
            }
        }
    }

    private void SaveNumbersToFile(string fileName)
    {
        string filePath = GetPersistentPath(fileName);
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath, false))
            {
                foreach (int number in availableIndexes)
                {
                    writer.WriteLine(number);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Index dosyasý yazma hatasý: {e.Message}");
        }
    }
    
}
