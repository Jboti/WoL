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
    private string apiUrl = "http://localhost:3000/api/v1";

    public Button ExitButton;
    public Button CreateCharacterButton;

    [SerializeField] public TMP_Text ErrorText;

    public Transform panelContainer;
    public GameObject characterPanelPrefab;

    void Start()
    {
        LoadCharacters();
        CreateCharacterButton.onClick.AddListener(LoadCharacterCreatingScene);
        ExitButton.onClick.AddListener(Exit);
    }

    #region loadCharacters

    public void LoadCharacters()
    {
        string username = PlayerPrefs.GetString("name", "");
        string id = PlayerPrefs.GetString("id", "");

        if (!string.IsNullOrEmpty(username))
            StartCoroutine(LoadCharactersRequest(username,id));
    }

    [System.Serializable]
    public class LoadCharactersData
    {
        public string id;
    }
   

    [System.Serializable]
    public class CharacterInfo
    {
        public string characterName;
        public string description;
    }

    [System.Serializable]
    public class CharacterData
    {
        public int id;
        public string name;
        public int lvl;
        public int attack_power;
        public CharacterInfo character;
    }

    [System.Serializable]
    public class CharacterList
    {
        public List<CharacterData> characters;
    }

    [System.Serializable]
    public class ResponseWrapper
    {
        public string error;
    }

    private IEnumerator LoadCharactersRequest(string username,string id)
    {
        string url = apiUrl + "/user-characters";


        LoadCharactersData data = new LoadCharactersData();
        data.id = id;

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
            string responseText = request.downloadHandler.text;
            

            
            string wrappedJson = "{\"characters\":" + request.downloadHandler.text + "}";
            UnityEngine.Debug.Log(wrappedJson);
            CharacterList characterList = JsonUtility.FromJson<CharacterList>(wrappedJson);

            if (characterList != null && characterList.characters != null && characterList.characters.Count > 0)
            {
                DisplayCharacters(characterList.characters);
            }
            else
            {
                ErrorText.text = "Nincsenek cigányaid!";
            }

        }
        else
        {

            var responseWrapper = JsonUtility.FromJson<ResponseWrapper>(request.downloadHandler.text);
            ErrorText.text = responseWrapper.error;

        }
    }


    private void DisplayCharacters(List<CharacterData> characters)
    {
       
        foreach (Transform child in panelContainer)
        {
            Destroy(child.gameObject);
        }

      
        foreach (var character in characters)
        {
            GameObject panel = Instantiate(characterPanelPrefab, panelContainer);
            CharacterPanel panelScript = panel.GetComponent<CharacterPanel>();
           
            if (panelScript != null)
            {
                panelScript.SetCharacterData(character.name, character.lvl, character.attack_power);
                Sprite characterSprite = LoadCharacterImage(character.character.characterName);
                if (characterSprite != null)
                {
                    panelScript.SetCharacterImage(characterSprite);
                }

                panelScript.selectCharacterButton.onClick.AddListener(() => SelectCharacterToPlay(character.id));
            }
        }

        StartCoroutine(RefreshLayout());
    }

    private void SelectCharacterToPlay(int charid)
    {
        UnityEngine.Debug.Log("kiválasztott karakter id: "+ charid);
    }

    private Sprite LoadCharacterImage(string characterName)
    {
       
        Texture2D texture = Resources.Load<Texture2D>("CharacterImages/" + characterName);

        if (texture != null)
        {
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
        else
        {
            UnityEngine.Debug.LogWarning($"Kép nem található: {characterName}.jpg");
            return null;
        }
    }

    private IEnumerator RefreshLayout()
    {
        yield return new WaitForEndOfFrame(); 
        LayoutRebuilder.ForceRebuildLayoutImmediate(panelContainer.GetComponent<RectTransform>());
    }

    #endregion

    public void LoadCharacterCreatingScene()
    {
        SceneManager.LoadScene("CreateCharacterScene");
    }

    public void Exit()
    {
        Application.Quit();
    }


}
