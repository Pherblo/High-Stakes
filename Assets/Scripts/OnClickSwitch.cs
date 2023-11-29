using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

// When object is clicked toggle between two Unity Events to invoke
public class OnClickSwitch : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private UnityEvent whenClicked;
    [SerializeField] private UnityEvent otherWhenClicked;

    [SerializeField] private bool runOtherEvent;

    // On object click
    public void OnPointerClick(PointerEventData eventData)
    {
        // If runOtherEvent is true run the other event and switch
        if (runOtherEvent)
        {
            otherWhenClicked.Invoke();
            runOtherEvent = false;
        }
        // Run whenClicked event
        else
        {
            whenClicked.Invoke();
            runOtherEvent = true;
        }
    }
}

