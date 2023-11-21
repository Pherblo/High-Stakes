using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardSwipeAnimator : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [Header("Scene References")]
    [SerializeField] private Camera _cardCamera;
    [Header("Swipe Animation Settings")]
    [SerializeField] private float _maxXOffset = 5;
    [SerializeField] private float _maxRotationOffset = 20f;
    [SerializeField] private Vector3 _normalizedRotationAxis;     // Must be normalized.
    [Header("Swipe Boundary Settings")]
    [SerializeField] private float _swipeXBoundsSize = 10f;

    private Vector3 _originalPosition;
    private Vector3 _cachedDragStartPosition = Vector3.zero;

    private void Awake()
    {
        _originalPosition = transform.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Calculate mouse's world position.
        float distanceFromCamera = (_originalPosition - _cardCamera.transform.position).magnitude;
        Vector3 pointerPosition = _cardCamera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, distanceFromCamera));
        _cachedDragStartPosition = pointerPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //
        print("ON DRAG HAPPENED");
        MoveCard(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        StartCoroutine(SnapbackCard());
    }

    public void TestEvents()
    {
        print("Event fired");
    }

    private void MoveCard(Vector3 eventDataPosition)
    {
        // Calculate mouse's world position.
        float distanceFromCamera = (_originalPosition - _cardCamera.transform.position).magnitude;
        Vector3 pointerPosition = _cardCamera.ScreenToWorldPoint(new Vector3(eventDataPosition.x, eventDataPosition.y, distanceFromCamera));

        // Constrain card movement.
        Vector3 dragDirection = pointerPosition - _cachedDragStartPosition;
        var newPosition = _originalPosition + Vector3.ClampMagnitude(dragDirection, _maxXOffset);
        transform.position = newPosition;
    }

    private void RotateCard()
    {
        //
    }

    private IEnumerator SnapbackCard()
    {
        transform.position = _originalPosition;
        yield return null;
    }
}
