using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Typewriter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _dialogueText;
    [SerializeField] private TextMeshProUGUI _textboxFillerText;        // Used to set text box sizes beforehand. This text component is usually invisible/transparent.

    [SerializeField] private float _timePerChar = 0.1f;

    private char[] _characters;

    public void RunDialogue(string text)
    {
        StopAllCoroutines();

        StartCoroutine(TypeAnimation(text));
    }

    private IEnumerator TypeAnimation(string text)
    {
        _textboxFillerText.text = text;
        _characters = text.ToCharArray();

        _dialogueText.text = "";

        for (int i = 0; i < _characters.Length; i++)
        {
            _dialogueText.text += _characters[i];
            if (_characters[i] != ' ')
                yield return new WaitForSeconds(_timePerChar);
        }
    }

    private IEnumerator NewTypeAnimation(string text)
    {
        _textboxFillerText.text = text;
        _characters = text.ToCharArray();

        _dialogueText.text = "";

        for (int i = 0; i < _characters.Length; i++)
        {
            _dialogueText.text += _characters[i];
            if (_characters[i] != ' ')
                yield return new WaitForSeconds(_timePerChar);
        }
    }
}
