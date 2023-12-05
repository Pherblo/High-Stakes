using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class OnClick : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private UnityEvent whenClicked;

    public void OnPointerClick(PointerEventData eventData)
    {
            whenClicked.Invoke();
    }
}

