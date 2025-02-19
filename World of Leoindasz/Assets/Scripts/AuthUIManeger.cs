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

public class AuthUIManager : MonoBehaviour
{
    private string apiUrl = "http://localhost:3000/api/v1";

    public GameObject loginPanel;
    public GameObject registerPanel;
    public GameObject loadingLayer;

    //Login
    [SerializeField] public TMP_InputField LoginUsernameField;
    [SerializeField] public TMP_InputField LoginPasswordField;
    public Toggle RememberMeToggle;
    [SerializeField] public TMP_Text LoginErrorText;

    public Button LoginButton;
    public Button GoToRegisterButton;

    //Register
    [SerializeField] public TMP_InputField RegisterUsernameField;
    [SerializeField] public TMP_InputField RegisterPasswordField;
    [SerializeField] public TMP_InputField RegisterConfirmPasswordField;
    [SerializeField] public TMP_Text RegisterErrorText;

    public Button RegisterButton;
    public Button GoToLoginButton;
    
    public Button ExitButton;

    
    void Start()
    {
        ShowLoginPanel();
        RememberMeToggle.isOn = false;

        if (PlayerPrefs.HasKey("RememberMe") && PlayerPrefs.GetInt("RememberMe") == 1)
        {
            LoginUsernameField.text = PlayerPrefs.GetString("Username");
            LoginPasswordField.text = PlayerPrefs.GetString("Password");
            RememberMeToggle.isOn = true;
        }

        GoToRegisterButton.onClick.AddListener(ShowRegisterPanel);
        GoToLoginButton.onClick.AddListener(ShowLoginPanel);
        LoginButton.onClick.AddListener(Login);
        RegisterButton.onClick.AddListener(Register);
        ExitButton.onClick.AddListener(Exit);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void ShowLoginPanel()
    {
        loginPanel.SetActive(true);
        registerPanel.SetActive(false);
        loadingLayer.SetActive(false);
        RegisterUsernameField.text = "";
        RegisterPasswordField.text = "";
        RegisterConfirmPasswordField.text = "";
        RegisterErrorText.text = "";
    }

    public void ShowRegisterPanel()
    {
        loginPanel.SetActive(false);
        registerPanel.SetActive(true);
        loadingLayer.SetActive(false);
        LoginUsernameField.text = "";
        LoginPasswordField.text = "";
        LoginErrorText.text = "";
    }

    [System.Serializable]
    public class ResponseWrapper
    {
        public string error;
        public string id;
        public string username;
    }

    #region login

    [System.Serializable]
    public class LoginData
    {
        public string username;
        public string password;
    }

    public void Login()
    {
        string username = LoginUsernameField.text;
        string password = LoginPasswordField.text;
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            LoginErrorText.text = "Üres mezõk!";
        else
        {
            StartCoroutine(LoginRequest(username, password));
            loadingLayer.SetActive(true);
            SetButtonsInteractable(false);

            if (RememberMeToggle.isOn)
            {
                PlayerPrefs.SetInt("RememberMe", 1);
                PlayerPrefs.SetString("Username", username);
                PlayerPrefs.SetString("Password", password);
            }
            else
            {
                PlayerPrefs.SetInt("RememberMe", 0);
                PlayerPrefs.DeleteKey("Username");
                PlayerPrefs.DeleteKey("Password");
            }
            PlayerPrefs.Save();
        }



    }

    private IEnumerator LoginRequest(string username, string password)
    {
        string url = apiUrl + "/login";


        LoginData data = new LoginData();
        data.username = username;
        data.password = password;

        string jsonData = JsonUtility.ToJson(data);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);

        UnityWebRequest request = new UnityWebRequest(url, "POST")
        {
            uploadHandler = new UploadHandlerRaw(bodyRaw),
            downloadHandler = new DownloadHandlerBuffer()
        };
        request.SetRequestHeader("x-api-key", "nagyonerosapikulcs");
        request.SetRequestHeader("Content-Type", "application/json");


        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            var responseWrapper = JsonUtility.FromJson<ResponseWrapper>(request.downloadHandler.text);

            PlayerPrefs.SetString("id", responseWrapper.id);
            PlayerPrefs.SetString("name", responseWrapper.username);
            PlayerPrefs.Save();

            loadingLayer.SetActive(false);
            SetButtonsInteractable(true);

            SceneManager.LoadScene("CharacterSelectionScene");

        }
        else
        {
            var responseWrapper = JsonUtility.FromJson<ResponseWrapper>(request.downloadHandler.text);
            LoginErrorText.text = responseWrapper.error;
            loadingLayer.SetActive(false);
            SetButtonsInteractable(true);
        }
    }

    #endregion

    #region register

    [System.Serializable]
    public class RegisterData
    {
        public string username;
        public string password;
    }

    public void Register()
    {
        

        string username = RegisterUsernameField.text;
        string password = RegisterPasswordField.text;
        string confirmPassword = RegisterConfirmPasswordField.text;
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
            RegisterErrorText.text = "Üres mezõk!";
        else if (password != confirmPassword)
            RegisterErrorText.text = "A két jelszó nem egyezik!";
        else
        { 
            StartCoroutine(RegisterRequest(username, password));
            loadingLayer.SetActive(true);
            SetButtonsInteractable(false);
        }

    }

    private IEnumerator RegisterRequest(string username, string password)
    {
        string url = apiUrl + "/register";


        LoginData data = new LoginData();
        data.username = username;
        data.password = password;

        string jsonData = JsonUtility.ToJson(data);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);

        UnityWebRequest request = new UnityWebRequest(url, "POST")
        {
            uploadHandler = new UploadHandlerRaw(bodyRaw),
            downloadHandler = new DownloadHandlerBuffer()
        };
        request.SetRequestHeader("x-api-key", "nagyonerosapikulcs");
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            loadingLayer.SetActive(false);
            SetButtonsInteractable(true);
            ShowLoginPanel();
        }
        else
        {
            var responseWrapper = JsonUtility.FromJson<ResponseWrapper>(request.downloadHandler.text);
            RegisterErrorText.text = responseWrapper.error;
            loadingLayer.SetActive(false);
            SetButtonsInteractable(true);
        }
    }

    #endregion

    private void SetButtonsInteractable(bool interactable)
    {
        LoginButton.interactable = interactable;
        GoToRegisterButton.interactable = interactable;
        RegisterButton.interactable = interactable;
        GoToLoginButton.interactable = interactable;
    }
}
