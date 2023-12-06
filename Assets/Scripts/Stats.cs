using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    [Header("Stat values")]
    [SerializeField] private float _currentValue = 50;
    [SerializeField] private float _maxValue = 100;

    [Header("Display settings")]
    public float minY = 0.8f;
    public float maxY = 2.8f;
    public float dangerThreshold = 75;

    public AnimationCurve potionTopSize;
    public float potionTopSizeMultiplier;

    [Header("Stat Highlight Settings")]
    [SerializeField] private float _glowDuration = 0.2f;
    [SerializeField] private Color32 _positiveColor;    // Color when value increases.
    [SerializeField] private Color32 _negativeColor;    // Color when value decreases.

    [Header("Prefab References")]
    [SerializeField] private Material _statsHighlightMaterial;      // Creates an instance to use, so that modifications to this material don't permanently change the main one.
    [SerializeField] private SpriteRenderer _spriteRenderer;
    public GameObject potionMask;
    public Transform potionTop;
    public ParticleSystem bubblePassive;
    public ParticleSystem bubbleBoil;
    public ParticleSystem bubbleBurst;
    public ParticleSystem overflow;
    public ParticleSystem shatter;

    [Space]
    public UnityEvent OnDeath;
    public UnityEvent OnShatter;
    public UnityEvent OnOverflow;

    private void Awake()
    {
        _spriteRenderer.material = new Material(_statsHighlightMaterial);

        // Initialize material values.
        _spriteRenderer.material.SetFloat("_EdgeAlpha", 0f);
    }

    private void Start()
    {
        updateDisplay();
    }

    //returns the current value of stat
    public float getValue()
    {
        return _currentValue;
    }

    //takes a change value and adds it to current value, values should be entered as negative or positive.
    public void changeValue(float change)
    {
        _currentValue += change;

        Mathf.Clamp(_currentValue, 0, _maxValue);

        updateDisplay(); //set the display to show current value
        bubbleBurst.Play();
        ClearGlow();
    }

    //check if player has died, and play corresponding particle animations. if dead, call OnDeath() for other scripts to listen to
    private void checkForDeath()
    {
        if(_currentValue >= _maxValue)
        {
            OnOverflow.Invoke();
            overflow.Play();
        } else if (_currentValue <= 0)
        {
            OnShatter.Invoke();
            shatter.Play();
        } else
        {
            return;
        }

        OnDeath.Invoke();
    }

    //updates image fill amount to match current value.
    private void updateDisplay()
    {
        if(_currentValue >= dangerThreshold)
        {
            bubbleBoil.Play();
        }
        else
            bubbleBoil.Stop();

        Vector3 pos = potionMask.transform.localPosition;
        pos.y = (_currentValue / _maxValue) * (maxY - minY) + minY;

        var scale = potionTopSize.Evaluate(_currentValue / _maxValue) * potionTopSizeMultiplier;
        potionTop.localScale = new Vector2(scale, scale);

        potionMask.transform.localPosition = pos;

        checkForDeath();
    }

    public void Glow(bool isPositive)
    {
        StopAllCoroutines();
        //gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        if (isPositive) StartCoroutine(StartHighlightColor(_positiveColor));
        else StartCoroutine(StartHighlightColor(_negativeColor));

        StartCoroutine(ShowHighlightAlpha());
    }

    public void ClearGlow()
    {
        StopAllCoroutines();
        //gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        StartCoroutine(StopHighlight());
    }

    private IEnumerator StartHighlightColor(Color32 targetColor)
    {
        Material currentMaterial = _spriteRenderer.material;
        Color32 startColor = currentMaterial.GetColor("_EdgeColor");

        // If alpha is 0, set color instantly. If it isn't 0, that means it was already highlighting previously.
        if (currentMaterial.GetFloat("_EdgeAlpha") == 0f)
        {
            currentMaterial.SetColor("_EdgeColor", targetColor);
        }
        else
        {
            //
            float timer = 0f;
            do
            {
                Color32 newColor = Color32.Lerp(startColor, targetColor, timer / _glowDuration);
                currentMaterial.SetColor("_EdgeColor", newColor);
                timer += Time.deltaTime;
                yield return null;
            } while (timer <= _glowDuration);
        }

        yield return null;
    }

    private IEnumerator ShowHighlightAlpha()
    {
        Material currentMaterial = _spriteRenderer.material;

        float currentAlpha = currentMaterial.GetFloat("_EdgeAlpha");
        float timer = 0f;
        do
        {
            float newAlpha = Mathf.Lerp(currentAlpha, 1f, timer / _glowDuration);
            currentMaterial.SetFloat("_EdgeAlpha", newAlpha);
            timer += Time.deltaTime;
            yield return null;
        } while (timer <= _glowDuration);

        yield return null;
    }

    private IEnumerator StopHighlight()
    {
        Material currentMaterial = _spriteRenderer.material;

        float currentAlpha = currentMaterial.GetFloat("_EdgeAlpha");
        float timer = 0f;
        do
        {
            float newAlpha = Mathf.Lerp(currentAlpha, 0f, timer / _glowDuration);
            currentMaterial.SetFloat("_EdgeAlpha", newAlpha);
            timer += Time.deltaTime;
            yield return null;
        } while (timer <= _glowDuration);

        yield return null;
    }
}
