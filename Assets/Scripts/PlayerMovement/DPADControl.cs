using UnityEngine;

public class DPADControl : MonoBehaviour
{
    [HideInInspector] public Vector2 inputVector; //  must be public

    public void UpPressDown() { inputVector.y = 1; }
    public void UpPressUp() { inputVector.y = 0; }

    public void DownPressDown() { inputVector.y = -1; }
    public void DownPressUp() { inputVector.y = 0; }

    public void LeftPressDown() { inputVector.x = -1; }
    public void LeftPressUp() { inputVector.x = 0; }

    public void RightPressDown() { inputVector.x = 1; }
    public void RightPressUp() { inputVector.x = 0; }
}
