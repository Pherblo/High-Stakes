using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Typewriter : MonoBehaviour
{
    [SerializeField] private TMP_Text _dialogue;
    [SerializeField] private TMP_Text _visualFillerText;        // Used to set text box sizes beforehand. This text component is usually invisible/transparent.
    [SerializeField] private ContentSizeFitter _fitter;

    [SerializeField] private float _timePerChar = 0.1f;

    private char[] _characters;

    public void RunDialogue(string text)
    {
        StopAllCoroutines();

        StartCoroutine(TypeAnimation(text));
    }

    private IEnumerator TypeAnimation(string text)
    {
        _fitter.enabled = true;
        _dialogue.text = text;
        _visualFillerText.text = text;

        _characters = text.ToCharArray();

        yield return new WaitForSeconds(0);

        _fitter.enabled = false;
        _dialogue.text = "";

        for (int i = 0; i < _characters.Length; i++)
        {
            _dialogue.text += _characters[i];
            if(_characters[i] != ' ')
            yield return new WaitForSeconds(_timePerChar);
        }
    }
}
