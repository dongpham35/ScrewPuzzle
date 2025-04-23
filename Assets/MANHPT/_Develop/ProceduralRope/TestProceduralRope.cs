using System;
using UnityEngine;

[RequireComponent(typeof(ProceduralRope))]
public class TestProceduralRope : MonoBehaviour
{
    [SerializeField] private ProceduralRope _rope;

    private void OnValidate()
    {
        if (_rope == null) _rope = GetComponent<ProceduralRope>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _rope.TogglePinTail();
        }
    }
}