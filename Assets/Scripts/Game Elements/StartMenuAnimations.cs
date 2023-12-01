using UnityEngine;

public class StartMenuAnimations : MonoBehaviour
{
    private Animator anim;

    // Is the settings card
    [SerializeField] private bool isToggledElsewhere;
    public bool IsToggledElsewhere => isToggledElsewhere;

    [Header("** Console Debug Logs **")]
    [SerializeField] private bool debug_WhichAnimationPlaying;

    private void Start()
    {
        // Get this objects animator
        anim = GetComponent<Animator>();
    }

    // Run one of the setting cards animations

    public void LeftSettings()
    {
        anim.SetTrigger("LeftSetting");

        // ** Debugs **
        if (debug_WhichAnimationPlaying) { Debug.Log("Playing : LeftSetting Animation"); }
    }
    public void MiddleSettings()
    {
        anim.SetTrigger("MiddleSetting");

        // ** Debugs **
        if (debug_WhichAnimationPlaying) { Debug.Log("Playing : LeftSetting Animation"); }
    }
    public void RightSettings()
    {
        anim.SetTrigger("RightSetting");

        // ** Debugs **
        if (debug_WhichAnimationPlaying) { Debug.Log("Playing : LeftSetting Animation"); }
    }
}
