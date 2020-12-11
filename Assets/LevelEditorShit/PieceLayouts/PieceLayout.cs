using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PieceLayout : ScriptableObject
{
    [System.Serializable]
    public struct PieceAndPos
    {
        public GameObject Piece;
        public Vector2 Location;
    }

    public PieceAndPos[] LayoutsPieces1;
    public int colour1;
    public int direction1;

    public PieceAndPos[] LayoutsPieces2;
    public int colour2;
    public int direction2;
}
