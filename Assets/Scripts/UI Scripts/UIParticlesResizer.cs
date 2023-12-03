using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIParticlesResizer : MonoBehaviour
{
    [SerializeField] private RectTransform _selfTransform;
    [SerializeField] private ParticleSystem _uiParticleSystem;
    [SerializeField] private float _xSizeMultiplier = 1f;

    private void Update()
    {
        ParticleSystem.ShapeModule shape = _uiParticleSystem.shape;
        shape.scale = new Vector3((_selfTransform.rect.size.x / 100f) * _xSizeMultiplier, shape.scale.y, shape.scale.z);
    }
}
