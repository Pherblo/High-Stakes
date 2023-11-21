using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardSwipeAnimator : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [Header("Scene References")]
    [SerializeField] private Camera _cardCamera;
    [Header("Swipe Settings")]
    [SerializeField] private float _maxXOffset = 5;
    [SerializeField] private float _maxRotationOffset = 20f;
    [SerializeField] private Vector3 _normalizedRotationAxis;     // Must be normalized.
    [Header("Snapback Animation Settings")]
    [SerializeField] private float _snapbackDuration = 0.2f;

    private Vector3 _originalPosition;
    private Quaternion _originalRotation;
    private Vector3 _cachedDragStartPosition;
    //private Vector3 _cachedDragDirection;
    private Vector3 _cachedNewPosition;

    private float _lerpValue = 0f;      // -1 = card is on the left. 1 = card is on the right. 0 = card is on the center.

    private void Awake()
    {
        _originalPosition = transform.position;
        _originalRotation = transform.rotation;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        StopAllCoroutines();
        // Cache mouse's starting world position.
        _cachedDragStartPosition = GetMousePosition(eventData.position);
        _cachedDragStartPosition.Scale(Vector3.right);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //
        print("ON DRAG HAPPENED");
        MoveCard(eventData.position);
        RotateCard(eventData.position);
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
        Vector3 pointerPosition = GetMousePosition(eventDataPosition);
        Vector3 dragDirection = pointerPosition - _cachedDragStartPosition;
        dragDirection.Scale(Vector3.right);

        Vector3 targetPosition = _originalPosition + dragDirection.normalized * _maxXOffset;

        _cachedNewPosition = _originalPosition + (dragDirection);
        //_cachedDragDirection = dragDirection;

        float lerpValue = GetLerpValue();
        transform.position = Vector3.Lerp(_originalPosition, targetPosition, lerpValue);
    }

    private void RotateCard(Vector3 eventDataPosition)
    {
        // Calculate mouse's world position.
        Vector3 pointerPosition = GetMousePosition(eventDataPosition);

        // Create rotation.
        //Vector3 maxPosition = _originalPosition + (dragDirection.normalized * _maxXOffset);
        float currentMagnitude = (_cachedNewPosition - _originalPosition).magnitude;
        float lerpValue = currentMagnitude / _maxXOffset;

        float direction = Mathf.Sign(pointerPosition.x - _cachedDragStartPosition.x) * -1f;
        Quaternion targetRotation = Quaternion.AngleAxis(_maxRotationOffset * direction, _normalizedRotationAxis);
        Quaternion newRotation = Quaternion.Lerp(_originalRotation, targetRotation, lerpValue);
        transform.rotation = newRotation;
    }

    private IEnumerator SnapbackCard()
    {
        Vector3 currentPosition = transform.position;
        Quaternion currentRotation = transform.rotation;
        float timer = 0f;

        do
        {
            timer += Time.deltaTime;

            transform.position = Vector3.Lerp(currentPosition, _originalPosition, timer / _snapbackDuration);
            transform.rotation = Quaternion.Lerp(currentRotation, _originalRotation, timer / _snapbackDuration);

            yield return null;
        } while (timer < _snapbackDuration);

        yield return null;
    }

    private float GetLerpValue()
    {
        float currentMagnitude = (_cachedNewPosition - _originalPosition).magnitude;
        float lerpValue = currentMagnitude / _maxXOffset;
        return lerpValue;
    }

    private Vector3 GetMousePosition(Vector3 eventDataPosition)
    {
        float distanceFromCamera = (_originalPosition - _cardCamera.transform.position).magnitude;
        Vector3 pointerPosition = _cardCamera.ScreenToWorldPoint(new Vector3(eventDataPosition.x, eventDataPosition.y, distanceFromCamera));
        return pointerPosition;
    }
}
