using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

// When object is clicked toggle between two Unity Events to invoke
public class OnClickSwitch : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private UnityEvent whenClicked;
    [SerializeField] private UnityEvent otherWhenClicked;

    [SerializeField] private bool runOtherEvent;

    [Header("** Console Debug Logs **")]
    [SerializeField] private bool debug_Clicking;
    [SerializeField] private bool debug_EventInvoked;

    // On object click
    public void OnPointerClick(PointerEventData eventData)
    {
        // ** Debugs **
        if (debug_Clicking) { Debug.Log(gameObject.name + ": was clicked"); }

        // If runOtherEvent is true run the other event and switch
        if (runOtherEvent)
        {
            otherWhenClicked.Invoke();
            runOtherEvent = false;

            // ** Debugs **
            if (debug_EventInvoked) { Debug.Log("Running OnClickSwitch 'Other When Clicked()' event."); }
        }
        // Run whenClicked event
        else
        {
            whenClicked.Invoke();
            runOtherEvent = true;

            // ** Debugs **
            if (debug_EventInvoked) { Debug.Log("Running OnClickSwitch 'When Clicked()' event."); }
        }
    }
}

