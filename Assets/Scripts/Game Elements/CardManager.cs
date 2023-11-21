using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    // This script handles game logic. This was previously done via events but it caused some headaches. Therefore, everything will be aggregated to this script for convenience's sake.
    [Header("Scene References")]
    [SerializeField] private Deck _deck;
    [SerializeField] private CardDisplay _cardDisplay;
}
