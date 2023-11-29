using UnityEngine;

public class StartMenuAnimations : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        // Get this objects animator
        anim = GetComponent<Animator>();
    }

    // Run one of the setting cards animations
    public void LeftSettings()
    {
        anim.SetTrigger("LeftSetting");
    }
    public void MiddleSettings()
    {
        anim.SetTrigger("MiddleSetting");
    }
    public void RightSettings()
    {
        anim.SetTrigger("RightSetting");
    }
}
