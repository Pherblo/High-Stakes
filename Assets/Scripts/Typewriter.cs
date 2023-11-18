using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.XR;
using System.Globalization;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class Typewriter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _dialogueText;
    [SerializeField] private TextMeshProUGUI _textboxFillerText;        // Used to set text box sizes beforehand. This text component is usually invisible/transparent.
    //[SerializeField] private CanvasRenderer _canvasRenderer;

    [SerializeField] private float _timePerChar = 0.1f;

    public float animationDuration = 1.0f;

    private void Start()
    {
        // Start the animation coroutine
        //StartCoroutine(MoveVerticesToCenter());
    }

    private IEnumerator MoveVerticesToCenter()
    {
        _dialogueText.ForceMeshUpdate();
        TMP_TextInfo textInfo = _dialogueText.textInfo;

        for (int charIndex = 0; charIndex < textInfo.characterCount; charIndex++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[charIndex];

            if (!charInfo.isVisible)
                continue;

            TMP_MeshInfo meshInfo = textInfo.meshInfo[charInfo.materialReferenceIndex];
            Vector3[] vertices = meshInfo.vertices;

            List<Vector3> originalPositions = new List<Vector3>();
            Vector3 centerPosition = Vector3.zero;

            // Record original positions and calculate center position
            for (int i = 0; i < 4; i++)
            {
                originalPositions.Add(vertices[charInfo.vertexIndex + i]);
                centerPosition += vertices[charInfo.vertexIndex + i];
            }
            centerPosition /= 4;

            float timer = 0f;

            while (timer < animationDuration)
            {
                timer += Time.deltaTime;

                // Lerp each vertex towards the center position
                for (int i = 0; i < 4; i++)
                {
                    vertices[charInfo.vertexIndex + i] = Vector3.Lerp(originalPositions[i], centerPosition, timer / animationDuration);
                }

                // Update the mesh vertices
                meshInfo.mesh.vertices = vertices;

                // Update the text mesh geometry
                _dialogueText.UpdateGeometry(meshInfo.mesh, charInfo.materialReferenceIndex);

                yield return null;
            }
        }
    }

    //private char[] _characters;

    public void RunDialogue(string text)
    {
        StopAllCoroutines();
        //_textboxFillerText.text = text;
        //_dialogueText.text = text;
        StartCoroutine(AnimateTextPop(text));
    }

    /*private IEnumerator TypeAnimation(string text)
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
    }*/

    private IEnumerator AnimateTextPop(string text)
    {
        // Initial set up.
        _textboxFillerText.SetText(text);
        _dialogueText.SetText(text);
        _dialogueText.ForceMeshUpdate(true);
        yield return null;
        //_dialogueText.ForceMeshUpdate();

        // Initialize local variables.
        TMP_TextInfo textInfo = _dialogueText.textInfo;
        //TMP_MeshInfo[] meshInfos = textInfo.CopyMeshInfoVertexData();
        List<Vector3> originalPositions = new();
        List<Vector3> centerPositions = new();

        // Move vertices to their local centers.
        TMP_MeshInfo cachedMeshInfo = textInfo.meshInfo[0];
        TMP_CharacterInfo cachedCharInfo = textInfo.characterInfo[0];
        Vector3[] cachedVertices = new Vector3[0];
        for (int charIndex = 0; charIndex < textInfo.characterCount; charIndex++)
        {
            // Get original positions.
            // Get center positions.
            // Assign center positions to each character-corresponding vertices.
            TMP_CharacterInfo charInfo = textInfo.characterInfo[charIndex];
            TMP_MeshInfo meshInfo = textInfo.meshInfo[charInfo.materialReferenceIndex];
            Vector3[] vertices = meshInfo.vertices;
            Vector3 centerPosition = Vector3.zero;
            for (int i = 0; i < 4; i++)
            {
                originalPositions.Add(vertices[charInfo.vertexIndex + i]);
                centerPosition += vertices[charInfo.vertexIndex + i];
            }
            //Vector3 centerPosition = vertices.Aggregate((aggregateVector, nextVector) => aggregateVector + nextVector) / 4;
            centerPositions.Add(centerPosition / 4f);

            // Update vertices.
            for (int j = 0; j < 4; j++)
            {
                if (!charInfo.isVisible) break;
                vertices[charInfo.vertexIndex + j] = centerPositions[charIndex];
            }
            cachedMeshInfo = meshInfo;
            cachedCharInfo = charInfo;
            cachedVertices = vertices;
            UpdateTextMesh(meshInfo, charInfo, vertices);
        }
        UpdateTextMesh(cachedMeshInfo, cachedCharInfo, cachedVertices);
        yield return null;
        // Update mesh.
        /*for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
            _dialogueText.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
            yield return null;
        }*/
        // Start pop animation.
        for (int charIndex = 0; charIndex < textInfo.characterCount; charIndex++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[charIndex];
            TMP_MeshInfo meshInfo = textInfo.meshInfo[charInfo.materialReferenceIndex];
            Vector3[] vertices = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
            float timer = 0f;
            do
            {
                timer += Time.deltaTime;
                float lerpValue = timer / _timePerChar;
                // Update vertices.
                for (int j = 0; j < 4; j++)
                {
                    if (charInfo.isVisible)
                    {
                        vertices[charInfo.vertexIndex + j] = Vector3.Lerp(centerPositions[charIndex], originalPositions[(charIndex * 4) + j], lerpValue);
                    }
                    else
                    {
                        timer = _timePerChar;
                        break;
                        vertices[charInfo.vertexIndex + j] = originalPositions[(charIndex * 4) + j];
                        timer = _timePerChar;
                    }
                }
                // Update mesh.
                /*for (int i = 0; i < textInfo.meshInfo.Length; i++)
                {
                    textInfo.meshInfo[i].mesh.vertices = vertices;
                    _dialogueText.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
                    yield return null;
                }*/
                /*
                meshInfo.mesh.vertices = vertices;
                _dialogueText.UpdateGeometry(meshInfo.mesh, charInfo.materialReferenceIndex);*/
                UpdateTextMesh(meshInfo, charInfo, vertices);
                yield return null;
            } while (timer < _timePerChar);
        }
        // Update mesh.
        //UpdateTextMesh(meshInfo, charInfo, vertices);
        yield return null;
        /*for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
            _dialogueText.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
            yield return null;
        }*/
    }

    private void UpdateTextMesh(TMP_MeshInfo meshInfo, TMP_CharacterInfo charInfo, Vector3[] vertices)
    {
        meshInfo.mesh.vertices = vertices;
        _dialogueText.UpdateGeometry(meshInfo.mesh, charInfo.materialReferenceIndex);
    }



    private IEnumerator NewTypeAnimation(string text)
    {
        _dialogueText.ForceMeshUpdate();
        _textboxFillerText.SetText("");
        _dialogueText.SetText("");
        yield return null;
        _dialogueText.ForceMeshUpdate();
        _textboxFillerText.SetText(text);
        _dialogueText.SetText(text);
        yield return null;
        _dialogueText.ForceMeshUpdate();
        TMP_TextInfo textInfo = _dialogueText.textInfo;
        /*
        for (charIndex = 0; charIndex < textInfo.characterCount; charIndex++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[charIndex];

            if (!charInfo.isVisible) continue;
        }*/
        List<Vector3> originalPositions = new();
        List<Vector3> centerPositions = new();

        // Zero out all verts.
        for (int charIndex = 0; charIndex < textInfo.characterCount; charIndex++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[charIndex];

            // Skip space/empty characters.
            //if (!charInfo.isVisible) continue;

            // Start pop-in animation for current text.
            TMP_MeshInfo meshInfo = textInfo.meshInfo[0];
            Vector3[] vertices = textInfo.CopyMeshInfoVertexData()[charInfo.materialReferenceIndex].vertices;

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
            centerPositions.Add(centerPosition);
            
            // Set all verts to the center.
            
            for (int i = 0; i < 4; i++)
            {
                vertices[charInfo.vertexIndex + i] = centerPositions[charIndex];
            }

            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                var meshInfo2 = textInfo.meshInfo[i];
                meshInfo2.mesh.vertices = meshInfo2.vertices;
                //_dialogueText.UpdateGeometry(meshInfo2.mesh, 0);
                //yield return null;
                //_dialogueText.ForceMeshUpdate(true, true);
            }
        }
        yield return null;

        // Animation
        for (int charIndex = 0; charIndex < textInfo.characterCount; charIndex++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[charIndex];

            // Skip space/empty characters.
            //if (!charInfo.isVisible) continue;

            // Start pop-in animation for current text.
            TMP_MeshInfo meshInfo = textInfo.meshInfo[0];
            Vector3[] vertices = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;

            // Do pop-in animation on vertices.
            float timer = 0;
            do
            {
                //_dialogueText.ForceMeshUpdate();
                //_dialogueText.ForceMeshUpdate();
                timer += Time.deltaTime;
                // Move each vertex.
                for (int i = 0; i < 4; i++)
                {
                    if (charInfo.isVisible)
                        vertices[charInfo.vertexIndex + i] = Vector3.Lerp(centerPositions[charIndex], originalPositions[(charIndex * 4) + i], Mathf.Clamp01(timer / _timePerChar));
                    else
                    {
                        vertices[charInfo.vertexIndex + i] = Vector3.Lerp(originalPositions[0], originalPositions[0], 1f);
                        //timer = _timePerChar + 1f;
                    }
                    //vertices[charInfo.vertexIndex + i] = originalPositions[(charIndex * 4) + i];
                    if ((timer / _timePerChar) == 0) print("IS ZERO");
                    meshInfo.mesh.vertices = vertices;
                }
                /*for (int j = 0; j < textInfo.meshInfo.Length; j++)
                {
                    var meshInfo2 = textInfo.meshInfo[1];
                    meshInfo2.mesh.vertices = meshInfo2.vertices;
                    _dialogueText.UpdateGeometry(meshInfo2.mesh, 1);
                }*/
                for (int i = 0; i < textInfo.meshInfo.Length; i++)
                {
                    textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                    _dialogueText.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
                    yield return null;
                }
                yield return null;
            } while (timer <= _timePerChar);
            yield return null;
        }
        yield return null;
    }
}
