using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIAnimator : MonoBehaviour
{
    // Used to start/stop particle effects.
    public UnityEvent OnFadeEnterFinished;
    public UnityEvent OnFadeExitStart;

    // This script is attached to UI components, with Image components, whose materials use the custom UI Fade shader.
    // All methods in this script is called via events.
    [Header("Component References")]
    [SerializeField] private Image _imageComponent;
    [Header("Material Settings and Property Names")]
    [SerializeField] private Material _materialInstance;
    [SerializeField] private string _fadeAlphaName = "materialPropertyName";
    [SerializeField] private string _fadeRotationName = "materialPropertyName";
    [Header("General Fade Settings")]
    [SerializeField] private float _startingFadeValue = 0f;
    [SerializeField, Range(0f, 1f)] private float _lerpStartValue = 0f;
    [SerializeField, Range(0f, 1f)] private float _lerpEndValue = 1f;
    [Header("Fading Animation Settings")]
    [SerializeField] private float _fadeDuration = 0.5f;
    [SerializeField] private float _fadeInRotation = 0f;
    [SerializeField] private float _fadeOutRotation = 180f;

    public float FadeDuration => _fadeDuration;

    private bool _firstFadeOut = true;      // Do not fade out the first time.

    private void Awake()
    {
        _imageComponent.material = new Material(_materialInstance);
        SetStartingFadeClip();
    }

    public void StartEnterFade()
    {
        StopAllCoroutines();
        StartCoroutine(StartFading(_lerpStartValue, _lerpEndValue, _fadeInRotation, true));
    }

    public void StartExitFade()
    {
        StopAllCoroutines();
        OnFadeExitStart?.Invoke();
        StartCoroutine(StartFading(0f, 1f, _fadeOutRotation, false));
        /*
        if (_firstFadeOut)
        {
            _firstFadeOut = false;
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(StartFading(0f, 1f, _fadeOutRotation, false));
        }*/
    }

    public void SetStartingFadeClip()
    {
        _imageComponent.materialForRendering.SetFloat(_fadeAlphaName, _startingFadeValue);
    }

    private IEnumerator StartFading(float startValue, float endValue, float newRotation, bool isFadeStart)
    {
        // Set fade direction.
        _imageComponent.materialForRendering.SetFloat(_fadeRotationName, newRotation);
        // Start fading.
        float timer = 0;
        do
        {
            float lerpProgress = timer / _fadeDuration;
            float lerpValue = Mathf.Lerp(startValue, endValue, lerpProgress);
            _imageComponent.materialForRendering.SetFloat(_fadeAlphaName, lerpValue);
            timer += Time.deltaTime;
            yield return null;
        } while (timer < _fadeDuration);

        if (isFadeStart) OnFadeEnterFinished?.Invoke();

        yield return null;
    }
}
