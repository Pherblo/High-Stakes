using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class Typewriter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _dialogueText;
    [SerializeField] private TextMeshProUGUI _textboxFillerText;        // Used to set text box sizes beforehand. This text component is usually invisible/transparent.
    [SerializeField] private CanvasRenderer _canvasRenderer;

    [SerializeField] private float _timePerChar = 0.1f;

    private char[] _characters;

    public void RunDialogue(string text)
    {
        StopAllCoroutines();
        _textboxFillerText.text = text;
        _dialogueText.text = text;
        _dialogueText.ForceMeshUpdate();
        TMP_TextInfo textInfo = _dialogueText.textInfo;
        int charIndex;
        _dialogueText.ForceMeshUpdate();

        // Zero out all verts.
        /*for (charIndex = 0; charIndex < textInfo.characterCount; charIndex++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[charIndex];

            // Skip space/empty characters.
            //if (!charInfo.isVisible) continue;

            // Start pop-in animation for current text.
            TMP_MeshInfo meshInfo = textInfo.meshInfo[0];
            Vector3[] vertices = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
            //Vector3[] originalPositions = (Vector3[])vertices.Clone();
            List<Vector3> originalPositions = new();
            for (int i = 0; i < 4; i++)
            {
                originalPositions.Add(vertices[charInfo.vertexIndex + i]);
            }

            // Add all vertices and divide by their total count.
            Vector3 centerPosition = Vector3.zero;// = vertices.Aggregate((aggregateVector, nextVector) => aggregateVector + nextVector) / vertices.Length;
            for (int i = 0; i < 4; i++)
            {
                centerPosition += vertices[charInfo.vertexIndex + i];
            }
            centerPosition /= 4;

            // Set all verts to the center.
            for (int i = 0; i < 4; i++)
            {
                vertices[charInfo.vertexIndex + i] = centerPosition;
            }

            for (int i = 0; i < 4; i++)
            {
                vertices[charInfo.vertexIndex + i] = centerPosition;
                //meshInfo.mesh.vertices = vertices;
            }
            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                var meshInfo2 = textInfo.meshInfo[i];
                meshInfo2.mesh.vertices = meshInfo2.vertices;
                _dialogueText.UpdateGeometry(meshInfo2.mesh, i);
            };
        }*/
        StartCoroutine(NewTypeAnimation(text));
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
        _dialogueText.text = text;
        _dialogueText.ForceMeshUpdate();
        _dialogueText.text = text;
        TMP_TextInfo textInfo = _dialogueText.textInfo;
        int charIndex;
        yield return null;
        /*
        for (charIndex = 0; charIndex < textInfo.characterCount; charIndex++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[charIndex];

            if (!charInfo.isVisible) continue;
        }*/
        List<Vector3> originalPositions = new();
        List<Vector3> centerPositions = new();

        // Zero out all verts.

        for (charIndex = 0; charIndex < textInfo.characterCount; charIndex++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[charIndex];

            // Skip space/empty characters.
            //if (!charInfo.isVisible) continue;

            // Start pop-in animation for current text.
            TMP_MeshInfo meshInfo = textInfo.meshInfo[0];
            Vector3[] vertices = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;

            for (int i = 0; i < 4; i++)
            {
                originalPositions.Add(vertices[charInfo.vertexIndex + i]);
            }

            // Add all vertices and divide by their total count.
            Vector3 centerPosition = Vector3.zero;// = vertices.Aggregate((aggregateVector, nextVector) => aggregateVector + nextVector) / vertices.Length;
            for (int i = 0; i < 4; i++)
            {
                centerPosition += vertices[charInfo.vertexIndex + i];
            }
            centerPosition /= 4;

            // Set all verts to the center.
            for (int i = 0; i < 4; i++)
            {
                vertices[charInfo.vertexIndex + i] = centerPosition;
            }

            for (int i = 0; i < 4; i++)
            {
                vertices[charInfo.vertexIndex + i] = centerPosition;
                //meshInfo.mesh.vertices = vertices;
            }
            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                var meshInfo2 = textInfo.meshInfo[i];
                meshInfo2.mesh.vertices = meshInfo2.vertices;
                _dialogueText.UpdateGeometry(meshInfo2.mesh, i);
                //yield return null;
            }
        }
        
        /*for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            var meshInfo2 = textInfo.meshInfo[i];
            meshInfo2.mesh.vertices = meshInfo2.vertices;
            _dialogueText.UpdateGeometry(meshInfo2.mesh, i);
        }
        yield return null;
        _dialogueText.ForceMeshUpdate();*/

        // Animation
        for (charIndex = 0; charIndex < textInfo.characterCount; charIndex++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[charIndex];

            // Skip space/empty characters.
            if (!charInfo.isVisible) continue;

            // Start pop-in animation for current text.
            TMP_MeshInfo meshInfo = textInfo.meshInfo[0];
            Vector3[] vertices = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
            //Vector3[] originalPositions = (Vector3[])vertices.Clone();
            //List<Vector3> originalPositions = new();
            /*for (int i = 0; i < 4; i++)
            {
                originalPositions.Add(vertices[charInfo.vertexIndex + i]);
            }*/

            // Add all vertices and divide by their total count.
            Vector3 centerPosition = Vector3.zero;// = vertices.Aggregate((aggregateVector, nextVector) => aggregateVector + nextVector) / vertices.Length;
            for (int i = 0; i < 4; i++)
            {
                centerPosition += vertices[charInfo.vertexIndex + i];
            }
            centerPosition /= 4;

            // Set all verts to the center.
            for (int i = 0; i < 4; i++)
            {
                vertices[charInfo.vertexIndex + i] = centerPosition;
            }

            // Do pop-in animation on vertices.
            float timer = 0;
            while (timer <= _timePerChar)
            {
                // Move each vertex.
                for (int i = 0; i < 4; i++)
                {
                    vertices[charInfo.vertexIndex + i] = Vector3.Lerp(centerPosition, originalPositions[i], timer / _timePerChar);

                    //meshInfo.mesh.vertices = vertices;
                }
                for (int j = 0; j < textInfo.meshInfo.Length; j++)
                {
                    var meshInfo2 = textInfo.meshInfo[j];
                    meshInfo2.mesh.vertices = meshInfo2.vertices;
                    _dialogueText.UpdateGeometry(meshInfo2.mesh, j);
                }
                timer += Time.deltaTime;
                yield return null;
            }
            yield return null;
        }
        yield return null;
    }
}
