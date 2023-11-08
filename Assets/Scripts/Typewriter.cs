using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Typewriter : MonoBehaviour
{
    public TMP_Text dialogue;
    public ContentSizeFitter fitter;

    public float timePerChar = 0.1f;

    private char[] characters;

    void Start()
    {
        StartCoroutine(TypeAnimation(dialogue.text));
    }

    public void RunDialogue(string text)
    {
        StopAllCoroutines();

        StartCoroutine(TypeAnimation(text));
    }

    private IEnumerator TypeAnimation(string text)
    {
        fitter.enabled = true;
        dialogue.text = text;

        characters = text.ToCharArray();

        yield return new WaitForSeconds(0);

        fitter.enabled = false;
        dialogue.text = "";

        for (int i = 0; i < characters.Length; i++)
        {
            dialogue.text += characters[i];
            if(characters[i] != ' ')
            yield return new WaitForSeconds(timePerChar);
        }
    }
}
