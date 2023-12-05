using UnityEngine;
using UnityEngine.Events;

public class CallStartEvents : MonoBehaviour
{
    [SerializeField] private UnityEvent startingEvent;

    // Start is called before the first frame update
    void Start()
    {
        startingEvent.Invoke();
    }
}
