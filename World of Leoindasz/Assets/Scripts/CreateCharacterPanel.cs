using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateCharacterPanel : MonoBehaviour
{
    public TMP_Text characterNameText;
    public TMP_Text descriptionText;
    public Image characterImage;
    public Button selectCharacterButton;


    public void SetCharacterData(string characterName, string description)
    {


        characterNameText.text = characterName;
        descriptionText.text = description;
    }

    public void SetCharacterImage(Sprite sprite)
    {
        if (characterImage != null)
        {
            characterImage.sprite = sprite;
        }
    }


}