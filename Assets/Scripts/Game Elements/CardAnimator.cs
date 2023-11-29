using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public enum CardPos
{
    left,
    right,
    middle
}
public class CardAnimator : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    // In order for drag events to work on this 3D game object, the camera its assigned to must have a Graphic Raycaster component.

    public Action OnCardDrawFinished;
    public Action OnCardDiscard;

    [Header("Scene References")]
    [SerializeField] private Camera _cardCamera;
    [Header("Component References")]
    [SerializeField] private Animator _animator;
    [SerializeField] private UIAnimator _uiAnimator;        // UI Animator that masks the card.
    [SerializeField] private Image _cardArt;
    [SerializeField] private TextMeshProUGUI _characterName;
    [SerializeField] private TextMeshProUGUI _characterTitle;
    [Header("Draw Animation Settings (Swipe animation must have a duration of 1)")]
    [SerializeField] private float _swipeAnimationDuration = 1f;        // Assumes that the swipe animation has a duration of 1.
    [Header("Swipe Animation Settings")]
    [SerializeField] private float _maxXOffset = 5f;
    [SerializeField] private float _maxRotationOffset = 20f;
    [SerializeField] private Vector3 _normalizedRotationAxis;     // Must be normalized.
    [Header("Snapback Animation Settings")]
    [SerializeField] private float _snapbackDuration = 0.2f;
    [Header("Exit Animation Settings")]
    [SerializeField] private float _exitSpeed = 20f;
    [SerializeField] private float _exitDuration = 2f;
    [SerializeField] private float _rotationResetDuration = 0.25f;
    [SerializeField] private float _exitRotationSpeed = 180f;
    [Header("Variable for seeom which dialogue is hovered: ")]
    [SerializeField] public CardPos currentPos = CardPos.middle;


    // References to be passed onto the CardDisplay, done by CardManager.
    public Image CardArt => _cardArt;
    public TextMeshProUGUI CharacterName => _characterName;
    public TextMeshProUGUI CharacterTitle => _characterTitle;

    private bool _isInteractable = true;
    private Vector3 _originalPosition;
    private Quaternion _originalRotation;
    private Vector3 _cachedDragStartPosition;
    private Vector3 _cachedNewPosition;

    private float _lerpValue = 0f;      // -1 = card is on the left. 1 = card is on the right. 0 = card is on the center.

    private void Awake()
    {
        _originalPosition = transform.position;
        _originalRotation = transform.rotation;
        _isInteractable = false;
        _animator.speed = 1f / _swipeAnimationDuration;
    }

    private void Update()
    {
        print(currentPos);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_isInteractable)
        {
            StopAllCoroutines();
            // Cache mouse's starting world position.
            _cachedDragStartPosition = GetMousePosition(eventData.position);
            _cachedDragStartPosition.Scale(Vector3.right);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_isInteractable)
        {
            MoveCard(eventData.position);
            RotateCard(eventData.position);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_isInteractable)
        {
            //StartCoroutine(SnapbackCard());
            float dragLength = (GetMousePosition(eventData.position) - _cachedDragStartPosition).magnitude;
            if (dragLength >= _maxXOffset)
            {
                StartCoroutine(StartExitAnimation());
            }
            else StartCoroutine(SnapbackCard());
        }
    }

    private void MoveCard(Vector3 eventDataPosition)
    {
        // Calculate mouse's world position.
        Vector3 pointerPosition = GetMousePosition(eventDataPosition);
        Vector3 dragDirection = pointerPosition - _cachedDragStartPosition;
        dragDirection.Scale(Vector3.right);

        Vector3 targetPosition = _originalPosition + dragDirection.normalized * _maxXOffset;

        _cachedNewPosition = _originalPosition + (dragDirection);

        float lerpValue = GetLerpValue();
        transform.position = Vector3.Lerp(_originalPosition, targetPosition, lerpValue);
        _cachedNewPosition = transform.position;

        //see if card is leaning left or right
        if (targetPosition.x > _originalPosition.x)
        {
            currentPos = CardPos.right;
        }
        else if (targetPosition.x < _originalPosition.x)
        {
            currentPos = CardPos.left;
        }
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
        currentPos = CardPos.middle;
        do
        {
            timer += Time.deltaTime;

            transform.position = Vector3.Lerp(currentPosition, _originalPosition, timer / _snapbackDuration);
            transform.rotation = Quaternion.Lerp(currentRotation, _originalRotation, timer / _snapbackDuration);

            _cachedNewPosition = transform.position;

            yield return null;
        } while (timer < _snapbackDuration);

        yield return null;
    }

    private IEnumerator StartExitAnimation()
    {
        OnCardDiscard?.Invoke();
        float signedDirection = Mathf.Sign(_cachedNewPosition.x - _originalPosition.x);
        //_isInteractable = false;
        StartCoroutine(StartExitRotationZ());

        float timer = 0f;
        do
        {
            timer += Time.deltaTime;

            // Modify position.
            transform.position += new Vector3(_exitSpeed * signedDirection * Time.deltaTime, 0f, 0f);
            _cachedNewPosition = transform.position;

            // Modify rotation.
            //transform.rotation *= Quaternion.Euler(0f, _exitRotationSpeed * signedDirection * Time.deltaTime, 0f);
            transform.rotation *= Quaternion.AngleAxis(_exitRotationSpeed * signedDirection * Time.deltaTime, Vector3.up);
            yield return null;
        } while (timer < _exitDuration);

        yield return null;
    }

    private IEnumerator StartExitRotationZ()
    {
        float currentZ = transform.rotation.eulerAngles.z;
        float targetZ = currentZ > 180f ? 360f : 0f;
        //print(currentZ + " " + targetZ);
        float timer = 0f;
        do
        {
            timer += Time.deltaTime;

            // Modify rotation
            float newZRotation = Mathf.Lerp(currentZ, targetZ, timer / _rotationResetDuration);
            //print("new z rot " + newZRotation + " timer " + timer);
            //Quaternion newRotation = Quaternion.AngleAxis(newZRotation, Vector3.forward);
            //transform.rotation *= newRotation;
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, newZRotation);

            yield return null;
        } while (timer < _rotationResetDuration);
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

    public void AnimateCardDraw()
    {
        StopAllCoroutines();
        _animator.enabled = true;
        _animator.SetTrigger("DrawCard");
        _isInteractable = false;
        _uiAnimator.SetStartingFadeClip();
    }

    public void AnimateCardDiscard()
    {
        //_animator.SetTrigger("DiscardCard");
    }

    // Called via animation events.
    public void SendDrawFinishedEvent()
    {
        _animator.enabled = false;
        _isInteractable = true;
        OnCardDrawFinished?.Invoke();
    }

    public void RevealCard()
    {
        _uiAnimator.StartEnterFade();
    }
}
