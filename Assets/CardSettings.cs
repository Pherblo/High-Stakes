using UnityEngine;

public class CardSettings : MonoBehaviour
{
    [SerializeField] private Animator[] cards;

    public void ChooseCard()
    {
        Invoke("DelayChooseCard", 1);
    }
    public void StopChoosingCard()
    {
        // Update each animator in all card objects to set the cardToChosen to false
        foreach (var card in cards)
        {
            card.SetBool("CardIsChosen", false);
        }
    }
    private void DelayChooseCard()
    {
        // Update each animator in all card objects to set the cardToChosen to true
        foreach (var card in cards)
        {
            card.SetBool("CardIsChosen", true);
        }
    }
}
