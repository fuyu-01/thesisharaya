using UnityEngine;
using Cinemachine;

public class CharacterSpawner : MonoBehaviour
{
    public CharacterDatabase characterDatabase;
    public Transform spawnPoint;
    public DPADControl uiDPad; // Assign DPad UI in the Inspector

    private void Start()
    {
        SpawnCharacter();
    }

    private void SpawnCharacter()
    {
        string selectedGender = PlayerPrefs.GetString("SelectedCharacter", "Male");
        string playerName = PlayerPrefs.GetString("PlayerName", "Player");

        CharacterOption option = characterDatabase.GetCharacterByName(selectedGender);
        if (option == null || option.prefab == null)
        {
            Debug.LogError("Character or prefab missing for selected gender!");
            return;
        }

        GameObject player = Instantiate(option.prefab, spawnPoint.position, spawnPoint.rotation);
        player.name = playerName;

        //  Assign the DPAD reference
        playerMovement movement = player.GetComponent<playerMovement>();
        if (movement != null && uiDPad != null)
        {
            movement.dPad = uiDPad;
            Debug.Log("DPad connected to spawned player.");
        }

        //  Assign Cinemachine follow
        CinemachineVirtualCamera vcam = FindObjectOfType<CinemachineVirtualCamera>();
        if (vcam != null)
        {
            vcam.Follow = player.transform;
            vcam.LookAt = player.transform;
        }
    }
}
