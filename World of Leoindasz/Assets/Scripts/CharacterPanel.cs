using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPanel : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text levelText;
    public Image characterImage;
    public TMP_Text attackPowerText;
    public Button selectCharacterButton;

    public void SetCharacterData(string name, int level, int attackPower)
    {
        nameText.text = name;
        levelText.text = "Level: " + level;
        attackPowerText.text = "Attack: " + attackPower;
    }

    public void SetCharacterImage(Sprite sprite)
    {
        if (characterImage != null)
        {
            characterImage.sprite = sprite;
        }
    }
}
