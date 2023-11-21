using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardSwipeAnimator : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public void OnBeginDrag(PointerEventData eventData)
    {
        print("ON BEGIN DRAG HAPPENED");
    }

    public void OnDrag(PointerEventData eventData)
    {
        //
        print("ON DRAG HAPPENED");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        print("ON END DRAG HAPPENED");
    }

    public void TestEvents()
    {
        print("Event fired");
    }
}
