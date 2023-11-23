using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardBase : MonoBehaviour
{
    public abstract CardEvent GetCard();

    public abstract bool CheckRequirements();
}
