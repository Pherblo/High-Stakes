using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class OnClickSwitch : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private UnityEvent whenClicked;
    [SerializeField] private UnityEvent otherWhenClicked;

    [SerializeField] private bool runOtherEvent;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (runOtherEvent)
        {
            otherWhenClicked.Invoke();
            runOtherEvent = false;
        }
        else
        {
            whenClicked.Invoke();
            runOtherEvent = true;
        }
    }
}

