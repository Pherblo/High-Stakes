using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum TextPopMode
{
    FromCharacterCenter = 0,
    FromCustomPosition = 1
}

public class Typewriter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _dialogueText;
    // TODO: Probably unnecessary now, due to updating Typewriter's way of presenting texts to the pop animation.
    [SerializeField] private TextMeshProUGUI _textboxFillerText;        // Used to set text box sizes beforehand. This text component is usually invisible/transparent.
    [Header("Text Animation Settings")]
    [SerializeField] private float _timePerChar = 0.015f;
    [SerializeField] private float _animationTimePerChar = 0.1f;
    [SerializeField] private TextPopMode _popMode = TextPopMode.FromCharacterCenter;
    [SerializeField] private Vector3 _customTextPopStartPosition = Vector3.zero;

    // private char[] _characters;      // Used by old type animation.

    public void RunDialogue(string text)
    {
        StopAllCoroutines();
        StartCoroutine(AnimateTextPop(text));
    }

    // Old type animation.
    /*
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
    */

    private IEnumerator AnimateTextPop(string text)
    {
        // Update text and cache color.
        Color32 cachedColor = _dialogueText.color;
        _dialogueText.color = new Color(0, 0, 0, 0);
        _textboxFillerText.SetText(text);
        _dialogueText.SetText(text);
        yield return null;      // Wait for mesh to update.

        // Update geometry.
        _dialogueText.ForceMeshUpdate(true);
        yield return null;

        // Initialize local variables.
        TMP_TextInfo textInfo = _dialogueText.textInfo;
        List<Vector3> originalPositions = new();
        List<Vector3> centerPositions = new();

        // Modify vertices' positions.
        for (int charIndex = 0; charIndex < textInfo.characterCount; charIndex++)
        {
            // Initialize references.
            TMP_CharacterInfo charInfo = textInfo.characterInfo[charIndex];
            TMP_MeshInfo meshInfo = textInfo.meshInfo[charInfo.materialReferenceIndex];
            Vector3[] vertices = meshInfo.vertices;
            Vector3 centerPosition = Vector3.zero;
            for (int i = 0; i < 4; i++)
            {
                originalPositions.Add(vertices[charInfo.vertexIndex + i]);
                centerPosition += vertices[charInfo.vertexIndex + i];
            }
            centerPositions.Add(centerPosition / 4f);

            // Assign center positions to each character-corresponding vertices.
            for (int j = 0; j < 4; j++)
            {
                if (!charInfo.isVisible) break;
                vertices[charInfo.vertexIndex + j] = centerPositions[charIndex];
            }

            SetVertexColors(_dialogueText, textInfo, charIndex, cachedColor);
            UpdateTextMesh(meshInfo, charInfo, vertices);
        }

        // Start pop animation.
        for (int charIndex = 0; charIndex < textInfo.characterCount; charIndex++)
        {
            StartCoroutine(AnimateCharacterVertices(textInfo, charIndex, centerPositions, originalPositions));
            yield return new WaitForSeconds(_timePerChar);
        }
        yield return null;
    }

    private IEnumerator AnimateCharacterVertices(TMP_TextInfo textInfo, int charIndex, List<Vector3> startPositions, List<Vector3> endPositions)
    {
        // Start pop animation.
        TMP_CharacterInfo charInfo = textInfo.characterInfo[charIndex];
        TMP_MeshInfo meshInfo = textInfo.meshInfo[charInfo.materialReferenceIndex];
        Vector3[] vertices = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
        float timer = 0f;
        do
        {
            timer += Time.deltaTime;
            float lerpValue = timer / _animationTimePerChar;
            // Update vertices.
            for (int j = 0; j < 4; j++)
            {
                if (charInfo.isVisible)
                {
                    vertices[charInfo.vertexIndex + j] = Vector3.Lerp(startPositions[charIndex], endPositions[(charIndex * 4) + j], lerpValue);
                }
                else
                {
                    timer = _animationTimePerChar;
                    break;
                }
            }
            SetVertexColors(_dialogueText, textInfo, charIndex, new Color32(255, 255, 255, 255));
            UpdateTextMesh(meshInfo, charInfo, vertices);
            yield return null;
        } while (timer < _animationTimePerChar);
        yield return null;
    }

    private void UpdateTextMesh(TMP_MeshInfo meshInfo, TMP_CharacterInfo charInfo, Vector3[] vertices)
    {
        meshInfo.mesh.vertices = vertices;
        _dialogueText.UpdateGeometry(meshInfo.mesh, charInfo.materialReferenceIndex);
    }

    private void SetVertexColors(TextMeshProUGUI textComponent, TMP_TextInfo textInfo, int charIndex, Color32 newColor)
    {
        int materialIndex = textInfo.characterInfo[charIndex].materialReferenceIndex;
        Color32[] vertexColors = textInfo.meshInfo[materialIndex].colors32;
        for (int i = 0; i < vertexColors.Length; i++)
        {
            vertexColors[i] = newColor;
        }
        textComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }
}
