using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackButton : MonoBehaviour
{
    public Button backButton;
    public GameObject panel;

    void Start()
    {
        backButton.onClick.AddListener(() =>
        {
            panel.SetActive(false);
        });
    }

}
