using UnityEngine;
using System.Collections.Generic;

public class NPCDialogueTrigger : MonoBehaviour
{
    [Header("References")]
    public GameObject talkButton;
    public GameObject dialogueBox;
    public DialogueManager dialogueManager;
    public GameObject Dpad;
    public GameObject Hotbar;
    public PlayerInventoryManager playerInventory;
    public Transform player;

    [Header("NPC Info")]
    public string npcName;
    [TextArea(2, 5)] public List<string> dialogueSentences;

    [Header("Reward Tools")]
    public List<ToolItem> rewardTools;

    [Header("Destroy Settings")]
    public float destroyDistanceFromPlayer = 10f;

    private bool playerInRange = false;
    private bool rewardsGiven = false;
    private bool readyToDestroy = false;

    void Start()
    {
        if (talkButton != null) talkButton.SetActive(false);
        if (dialogueBox != null) dialogueBox.SetActive(false);

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
            Debug.Log("[NPC] Player was null, auto-assigned: " + (player != null));
        }
    }

    void Update()
    {
        // Destroy NPC only if rewards given and player far enough
        if (readyToDestroy && player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer > destroyDistanceFromPlayer)
            {
                Debug.Log("[NPC] Destroying NPC because player left area.");
                Destroy(gameObject);
            }
        }
    }

    public void TriggerDialogue()
    {
        Debug.Log("[NPC] TriggerDialogue() called. playerInRange = " + playerInRange);

        if (!playerInRange)
        {
            Debug.LogWarning("[NPC] Dialogue NOT triggered. Player NOT in range.");
            return;
        }

        // Show dialogue UI
        if (dialogueBox != null) dialogueBox.SetActive(true);

        // Hide gameplay UI
        if (talkButton != null) talkButton.SetActive(false);
        if (Dpad != null) Dpad.SetActive(false);
        if (Hotbar != null) Hotbar.SetActive(false);

        Debug.Log("[NPC] Starting dialogue for NPC: " + npcName);

        dialogueManager?.StartDialogue(npcName, dialogueSentences, this);
    }

    public void OnDialogueEnd()
    {
        Debug.Log("[NPC] Dialogue ended for NPC: " + npcName);

        // Hide dialogue UI
        if (dialogueBox != null) dialogueBox.SetActive(false);

        // Show gameplay UI again
        if (Dpad != null) Dpad.SetActive(true);
        if (Hotbar != null) Hotbar.SetActive(true);

        GiveRewards();
        readyToDestroy = true;
    }

    private void GiveRewards()
    {
        if (rewardsGiven || playerInventory == null)
        {
            Debug.Log("[NPC] Rewards NOT given. Either already given or inventory is null.");
            return;
        }

        foreach (ToolItem tool in rewardTools)
        {
            bool added = playerInventory.AddToolToHotbar(tool);
            if (!added)
                playerInventory.AddToolToInventory(tool);

            Debug.Log("[NPC] Reward given: " + tool.itemName);
        }

        rewardsGiven = true;
        Debug.Log("[NPC] All rewards delivered.");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("[NPC] OnTriggerEnter2D → " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("[NPC] Player ENTERED dialogue range.");
            playerInRange = true;

            if (talkButton != null)
                talkButton.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("[NPC] OnTriggerExit2D → " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("[NPC] Player EXITED dialogue range.");
            playerInRange = false;

            if (talkButton != null)
                talkButton.SetActive(false);
        }
    }
}
