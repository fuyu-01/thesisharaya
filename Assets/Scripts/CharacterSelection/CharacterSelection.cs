using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

[System.Serializable]
public class CharacterOption
{
    public string genderName;
    public Sprite Icon;
    public Sprite Preview;
    public GameObject prefab;
}

public class CharacterSelection : MonoBehaviour
{
    [Header("UI References")]
    public Image Icon;
    public Image Preview;
    public TMP_InputField nameInput;

    [Header("Database")]
    public CharacterDatabase characterDatabase;

    private int currentIndex = 0;

    void Start()
    {
        if (characterDatabase == null || characterDatabase.characters.Length == 0)
        {
            Debug.LogError("Character Database is empty or not assigned!");
            return;
        }

        UpdateUI();
    }

    public void ToggleGender()
    {
        currentIndex++;
        if (currentIndex >= characterDatabase.characters.Length)
            currentIndex = 0;

        UpdateUI();
    }

    private void UpdateUI()
    {
        CharacterOption option = characterDatabase.characters[currentIndex];
        Icon.sprite = option.Icon;
        Preview.sprite = option.Preview;
    }

    public void ConfirmSelection()
    {
        string playerName = nameInput.text.Trim();

        // Stop if the name field is empty
        if (string.IsNullOrEmpty(playerName))
        {
            Debug.LogWarning("Player name is empty! Please enter a name before continuing.");
            return; // Do not proceed
        }

        // Save the chosen character and name
        CharacterOption chosen = characterDatabase.characters[currentIndex];
        PlayerPrefs.SetString("SelectedCharacter", chosen.genderName);
        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.Save();

        // Proceed to Prologue
        SceneManager.LoadScene("PrologueScene");
    }
}
