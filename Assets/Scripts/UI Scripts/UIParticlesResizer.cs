using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIParticlesResizer : MonoBehaviour
{
    [SerializeField] private RectTransform _selfTransform;
    [SerializeField] private ParticleSystem _uiParticleSystem;
    [SerializeField] private float _xSizeMultiplier = 1f;
    [Header("Particle System Adjustments")]
    [SerializeField] private float _emissionPerScale = 15f;

    private ParticleSystem.ShapeModule _shapeModule;
    private ParticleSystem.EmissionModule _emissionModule;

    private void Awake()
    {
        _shapeModule = _uiParticleSystem.shape;
        _emissionModule = _uiParticleSystem.emission;
    }

    // Called using UnityEvents from UIAnimator.
    public void StartParticles()
    {
        _uiParticleSystem.Play();
    }

    // Called using UnityEvents from UIAnimator.
    public void StopParticles()
    {
        _uiParticleSystem.Stop();
    }

    // Called using UnityEvents from UIAnimator.
    public void UpdateScale()
    {
        _shapeModule.scale = new Vector3((_selfTransform.rect.size.x / 100f) * _xSizeMultiplier, _shapeModule.scale.y, _shapeModule.scale.z);
    }

    // Called using UnityEvents from UIAnimator.
    public void UpdateEmission()
    {
        _emissionModule.rateOverTime = _emissionPerScale * _shapeModule.scale.x;
    }
}
