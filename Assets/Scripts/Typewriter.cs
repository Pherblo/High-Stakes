using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public enum TextPopMode
{
    FromCharacterCenter = 0,
    FromCustomPosition = 1
}

public class Typewriter : MonoBehaviour
{
    // IMPORTANT: THIS SCRIPT ASSUMES THE TMPUGUI USES A MATERIAL THAT USES TEXTDISSOLVE SHADER.
    public Action OnTextGenerationFinished;

    [Header("Component References")]
    [SerializeField] private TextMeshProUGUI _dialogueText;
    // TODO: Probably unnecessary now, due to updating Typewriter's way of presenting texts to the pop animation.
    [SerializeField] private TextMeshProUGUI _textboxFillerText;        // Used to set text box sizes beforehand. This text component is usually invisible/transparent.
    [Header("Text Animation Settings")]
    [SerializeField] private float _timePerChar = 0.015f;
    [SerializeField] private float _animationTimePerChar = 0.1f;
    [SerializeField] private Vector2 _startingPositionOffset = Vector2.zero;        // Due to how coordinates work in the TMP's space, this vector's x and y values would be pretty big.
    [SerializeField, Min(-1f)] private float _startingSizeMultiplier = -1f;        // -1f = all vertices are in the center. 0f = verts stay the same. 1f makes the verts move away from center.
    [SerializeField] private Color32 _startColor;       // Not to be confused with the resulting color at the end of the text's animation.
    [Header("Pop Animation Lerp Curves")]
    [SerializeField] private AnimationCurve _xAxisLerpCurve;
    [SerializeField] private AnimationCurve _yAxisLerpCurve;
    [SerializeField] private AnimationCurve _startColorLerpCurve;

    private Color32 _originalColor = Color.white;

    // private char[] _characters;      // Used by old type animation.

    private void Awake()
    {
        _originalColor = _dialogueText.color;
    }

    public void RunDialogue(string text)
    {
        StopAllCoroutines();
        StartCoroutine(AnimateTextPop(text));
    }

    public void SetFillerText(string text)
    {
        _textboxFillerText.SetText(text);
    }

    public void ClearText()
    {
        StopAllCoroutines();
        _dialogueText.SetText("");
        //_textboxFillerText.SetText("");
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

    // IMPORTANT: THIS ASSUMES THE TMPUGUI USES A MATERIAL THAT USES TEXTDISSOLVE SHADER.
    private IEnumerator AnimateTextPop(string text)
    {
        // Update text and cache color.
        _dialogueText.color = new Color32(0, 0, 0, 0);

        // Set texts and wait for mesh to update. Yes this is separate from the geometry update. I have no fucking clue as to why but that is how it is.
        _textboxFillerText.SetText(text);
        _dialogueText.SetText(text);
        yield return null;

        // Update geometry.
        _dialogueText.ForceMeshUpdate(true);
        yield return null;

        // Initialize local variables.
        TMP_TextInfo textInfo = _dialogueText.textInfo;
        List<Vector3> originalPositions = new();
        List<Vector3> startPositions = new();

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

            // Apply offsets to start position then add to list.
            centerPosition /= 4f;
            for (int i = 0; i < 4; i++)
            {
                int vertexIndex = (charIndex * 4) + i;
                Vector3 newStartPosition = Vector3.LerpUnclamped(originalPositions[vertexIndex], centerPosition, -_startingSizeMultiplier);
                newStartPosition += (Vector3)_startingPositionOffset;
                startPositions.Add(newStartPosition);
            }

            // Assign center positions to each character-corresponding vertices.
            for (int j = 0; j < 4; j++)
            {
                if (!charInfo.isVisible) break;
                vertices[charInfo.vertexIndex + j] = startPositions[charIndex];
            }

            SetVertexColors(_dialogueText, textInfo, charInfo, charIndex, new Color32(0, 0, 0, 0));
            UpdateTextMesh(meshInfo, charInfo, vertices);
        }

        // Start pop animation.
        for (int charIndex = 0; charIndex < textInfo.characterCount; charIndex++)
        {
            StartCoroutine(AnimateCharacterVertices(textInfo, charIndex, startPositions, originalPositions, _originalColor));
            yield return new WaitForSeconds(_timePerChar);
        }
        OnTextGenerationFinished?.Invoke();
        yield return null;
    }

    private IEnumerator AnimateCharacterVertices(TMP_TextInfo textInfo, int charIndex, List<Vector3> startPositions, List<Vector3> endPositions, Color32 targetColor)
    {
        // Start pop animation.
        TMP_CharacterInfo charInfo = textInfo.characterInfo[charIndex];
        TMP_MeshInfo meshInfo = textInfo.meshInfo[charInfo.materialReferenceIndex];
        Vector3[] vertices = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
        float timer = 0f;
        do
        {
            timer += Time.deltaTime;
            float lerpValue = Mathf.Min(timer / _animationTimePerChar);
            // Update vertices.
            for (int j = 0; j < 4; j++)
            {
                if (charInfo.isVisible)
                {
                    int vertIndex = (charIndex * 4) + j;
                    //vertices[charInfo.vertexIndex + j] = Vector3.Lerp(startPositions[(charIndex * 4) + j], endPositions[(charIndex * 4) + j], lerpValue);

                    float xLerpValue = _xAxisLerpCurve.Evaluate(lerpValue);
                    float yLerpValue = _yAxisLerpCurve.Evaluate(lerpValue);
                    float newXPosition = Mathf.Lerp(startPositions[vertIndex].x, endPositions[vertIndex].x, xLerpValue);
                    float newYPosition = Mathf.Lerp(startPositions[vertIndex].y, endPositions[vertIndex].y, yLerpValue);
                    vertices[charInfo.vertexIndex + j] = new Vector3(newXPosition, newYPosition, endPositions[vertIndex].z);
                }
                else
                {
                    timer = _animationTimePerChar;
                    break;
                }
            }
            // Apply colors.
            float colorCurveValue = _startColorLerpCurve.Evaluate(lerpValue);
            Color32 lerpedColor = Color32.Lerp(_startColor, targetColor, colorCurveValue);
            SetVertexColors(_dialogueText, textInfo, charInfo, charIndex, lerpedColor);
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

    private void SetVertexColors(TextMeshProUGUI textComponent, TMP_TextInfo textInfo, TMP_CharacterInfo charInfo, int charIndex, Color32 newColor)
    {
        int materialIndex = charInfo.materialReferenceIndex;
        Color32[] vertexColors = textInfo.meshInfo[materialIndex].colors32;
        int vertexIndex = charInfo.vertexIndex;
        if (charInfo.isVisible)
        {
            for (int i = 0; i < 4; i++)
            {
                if (charInfo.isVisible) vertexColors[vertexIndex + i] = newColor;
            }
        }
        textComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }
}
