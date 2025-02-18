using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using System.Text;
using System.Diagnostics;

public class CharacterSelectUI : MonoBehaviour
{
    public Button ExitButton;

    void Start()
    {
        LoadCharacters();

        ExitButton.onClick.AddListener(Exit);
    }

    public void LoadCharacters()
    {
        Console.WriteLine("load");
    }

    public void Exit()
    {
        Application.Quit();
    }


}
