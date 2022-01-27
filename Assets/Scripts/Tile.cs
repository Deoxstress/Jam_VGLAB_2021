using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color baseColor, offsetColor;
    [SerializeField] private SpriteRenderer _renderer;

    // Start is called before the first frame update
    public void InitTile(bool isOffset)
    {
        if (isOffset)
            _renderer.color = offsetColor;
        else
            _renderer.color = baseColor;
    }
}
