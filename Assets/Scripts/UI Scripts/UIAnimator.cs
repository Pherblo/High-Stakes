using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimator : MonoBehaviour
{
    // This script is attached to UI components, with Image components, whose materials use the custom UI Fade shader.
    // All methods in this script is called via events.
    [Header("Component References")]
    [SerializeField] private Animator _animator;

    public void StartEnterFade()
    {
        _animator.SetTrigger("FadeIn");
    }

    public void StartExitFade()
    {
        _animator.SetTrigger("FadeOut");
    }
}
