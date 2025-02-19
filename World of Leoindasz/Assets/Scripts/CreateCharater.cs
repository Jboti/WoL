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

public class CreateCharater : MonoBehaviour
{
    private string apiUrl = "http://localhost:3000/api/v1";

    public GameObject creationPanel;
    [SerializeField] public TMP_InputField CharacterNameField;
    [SerializeField] public TMP_Text SelectedClassText;
    [SerializeField] public TMP_Text ErrorText;
    public Button CreateCharacterButton;

    public Button ExitButton;
    public Button BackToSelectionButton;

    public Transform cardContainer;
    public GameObject createCharacterPanelPrefab;

    public ScrollRect scrollView;

    private string selectedClass = "";
    private int selectedId = -1;


    void Start()
    {
        HideCreationPanel();
        ShowOptions();

        BackToSelectionButton.onClick.AddListener(BackToSelection);
        ExitButton.onClick.AddListener(Exit);
        CreateCharacterButton.onClick.AddListener(CreateNewCharacter);
    }

    #region ListCharacters

    public void ShowOptions()
    {
        StartCoroutine(CharactersRequest());
    }

    [System.Serializable]
    public class Character
    {
        public int id;
        public string characterName;
        public string description;
    }

    [System.Serializable]
    public class CharacterList
    {
        public List<Character> characters;
    }

    private IEnumerator CharactersRequest()
    {
        string url = apiUrl + "/get-characters";

        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("x-api-key", "nagyonerosapikulcs");
        request.SetRequestHeader("Content-Type", "application/json");


        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {

            string responseText = request.downloadHandler.text;

            List<Character> characters = JsonUtility.FromJson<CharacterList>("{\"characters\":" + responseText + "}").characters;

            if (characters != null && characters.Count > 0)
            {
                DisplayCharacterCards(characters);
            }
            else
            {
                UnityEngine.Debug.Log("Nincsenek karakterek.");
            }
        }
        else
        {
            UnityEngine.Debug.LogError("Hiba történt a karakterek lekérésekor.");
        }
    }

    private void DisplayCharacterCards(List<Character> characters)
    {
        
        foreach (Transform child in cardContainer)
        {
            Destroy(child.gameObject);
        }


        foreach (var character in characters)
        {
            GameObject card = Instantiate(createCharacterPanelPrefab, cardContainer);
            CreateCharacterPanel panelScript = card.GetComponent<CreateCharacterPanel>();

            if (panelScript != null)
            {
                panelScript.SetCharacterData(character.characterName, character.description);
                
                Sprite characterSprite = LoadCharacterImage(character.characterName);
                if (characterSprite != null)
                {
                    panelScript.SetCharacterImage(characterSprite);
                }

                panelScript.selectCharacterButton.onClick.AddListener(() => SelectCharacter(character.characterName, character.id));
            }
        }

        StartCoroutine(RefreshLayout());

    }

    private void SelectCharacter(string characterName, int charid)
    {
        selectedClass = characterName;
        selectedId = charid;

        ShowCreationPanel();
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
        LayoutRebuilder.ForceRebuildLayoutImmediate(cardContainer.GetComponent<RectTransform>());
    }

    #endregion

    #region createNewCharacter


    [System.Serializable]
    public class newCharacter
    {
        public string id;
        public string character_id;
        public string name;
    }

    public void CreateNewCharacter()
    {
        string id = PlayerPrefs.GetString("id", "");
        string cid = selectedId.ToString();
        string characterName = CharacterNameField.text;
        if (string.IsNullOrEmpty(characterName))
            ErrorText.text = "Üres mezõ!";
        else
            StartCoroutine(SendCharacterDataToServer(id,cid, characterName));
    }

    private IEnumerator SendCharacterDataToServer(string id, string cid, string name)
    {
        string url = apiUrl + "/create-new-character";


        newCharacter data = new newCharacter();
        data.id = id;
        data.character_id = cid;
        data.name = name;
       

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
            UnityEngine.Debug.Log("Karakter sikeresen létrehozva!");

            BackToSelection();
        }
        else
        {
            ErrorText.text = "Hiba a létrehozás során!";
            UnityEngine.Debug.LogError("Hiba történt a karakter létrehozásakor: " + request.error);
        }
    }

    #endregion

    public void HideCreationPanel()
    {
        creationPanel.SetActive(false);
        scrollView.gameObject.SetActive(true);
    }

    public void ShowCreationPanel()
    {
        SelectedClassText.text = "Választott kaszt: " + selectedClass;
        creationPanel.SetActive(true);
        scrollView.gameObject.SetActive(false);
    }

    public void BackToSelection()
    {
        SceneManager.LoadScene("CharacterSelectionScene");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
