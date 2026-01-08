using UnityEngine;

public class Animation : MonoBehaviour
{
    public static Animation Instance;   // Makes it easy to call from other scripts
    public Animator animator;                 // Assign your Animator here

    private void Awake()
    {
        Instance = this; // Store reference to this script
    }

    // Call this function from other scripts when player uses a tool
    public void PlayAnimation(string action)
    {
        switch (action)
        {
            case "Hoe":
                animator.SetTrigger("Plow");
                break;

            case "WateringCan":
                animator.SetTrigger("Water");
                break;

            case "Harvest":
                animator.SetTrigger("Harvest");
                break;

            default:
                Debug.LogWarning("Unknown animation action: " + action);
                break;
        }
    }
}
