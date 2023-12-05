using System.Collections;
using UnityEngine;

public class SceneFadeAnimations : MonoBehaviour
{
    [SerializeField] private GameObject sceneFadeBlackImage;
    [SerializeField] private float startDelay = 2;

    private Animator animator;

    // Grabbing animator from scene fade image
    private void Start()
    {
        animator = sceneFadeBlackImage.GetComponent<Animator>();
    }

    // Makes it accessible and adds a delay if wanted
    public void Fade(bool fadeIn)
    {
        if (fadeIn)
        {
            Invoke("FadeIn", startDelay);
        }
        else
        {
            Invoke("FadeOut", startDelay);
        }
    }

    // Turns on the scene fade black image then runs fade in
    private void FadeIn()
    {
        if (sceneFadeBlackImage != null && animator != null)
        {
            sceneFadeBlackImage.SetActive(true);

            animator.SetTrigger("Fade In");
        }
    }

    // Turns on the scene fade black image then runs fade out
    private void FadeOut()
    {
        if (sceneFadeBlackImage != null && animator != null)
        {
            sceneFadeBlackImage.SetActive(true);

            animator.SetTrigger("Fade Out");
        }
    }
}
