using UnityEngine;

public class HoverArea : MonoBehaviour
{
    [SerializeField] private bool isOptionA;
    [SerializeField] private bool aIsHovering;
    [SerializeField] private bool bIsHovering;

    public bool AIsHovering => aIsHovering;
    public bool BIsHovering => bIsHovering;

    public void EnterHovering()
    {
        if (isOptionA)
        {
            aIsHovering = true;
        }
        else
        {
            bIsHovering = true;
        }
    }

    public void ExitHovering()
    {
        if (isOptionA)
        {
            aIsHovering = false;
        }
        else
        {
            bIsHovering = false;
        }
    }

}

