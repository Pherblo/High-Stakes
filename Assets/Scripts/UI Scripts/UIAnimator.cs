using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimator : MonoBehaviour
{
    // This script is attached to UI components, with Image components, whose materials use the custom UI Fade shader.
    // All methods in this script is called via events.
    [Header("Component References")]
    [SerializeField] private Image _imageComponent;
    [Header("General Settings")]
    [SerializeField] private float _startingFadeValue = 0f;
    [Header("Fading Animation Settings")]
    [SerializeField] private float _fadeDuration = 0.5f;
    [SerializeField] private float _fadeInRotation = 0f;
    [SerializeField] private float _fadeOutRotation = 180f;

    private void Awake()
    {
        _imageComponent.material.SetFloat("_FadeAlphaClip", _startingFadeValue);
    }

    public void StartEnterFade()
    {
        StopAllCoroutines();
        StartCoroutine(StartFading(1f, 0f, 0f));
    }

    public void StartExitFade()
    {
        StopAllCoroutines();
        StartCoroutine(StartFading(1f, 0f, 180f));
    }

    private IEnumerator StartFading(float startValue, float endValue, float newRotation)
    {
        // Set fade direction.
        _imageComponent.material.SetFloat("_FadeDirectionRotation", newRotation);
        // Start fading.
        float timer = 0;
        do
        {
            float lerpProgress = timer / _fadeDuration;
            float lerpValue = Mathf.Lerp(startValue, endValue, lerpProgress);
            _imageComponent.material.SetFloat("_FadeAlphaClip", lerpValue);
            timer += Time.deltaTime;
            yield return null;
        } while (timer < _fadeDuration);

        yield return null;
    }
}
