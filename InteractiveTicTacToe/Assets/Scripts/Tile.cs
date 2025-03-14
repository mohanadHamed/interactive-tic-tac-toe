using NUnit.Framework.Constraints;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public Button TileButton => GetComponent<Button>();

    public int TileIndex { get; set; }

    private TileImage _tileImage;

    [SerializeField]
    private Sprite xSprite;
    [SerializeField]
    private Sprite oSprite;
    [SerializeField]
    private Sprite noneSprite;

    [SerializeField]
    private Color noneColor;
    [SerializeField]
    private Color xColor;
    [SerializeField]
    private Color oColor;

    [SerializeField]
    private Color gameOverColor;

    private Dictionary<TileImage, Sprite> spriteMap;
    private Dictionary<TileImage, Color> colorMap;

    public TileImage TileImage {  get { return _tileImage; } 
        set 
        {
            _tileImage = value;
            var image = GetComponent<Image>();
            image.sprite = spriteMap[_tileImage];
            image.color = colorMap[_tileImage];
            image.type = Image.Type.Simple;
            image.preserveAspect = true;
            TileButton.interactable = _tileImage == TileImage.None;
        } }

    public void SetGameOverColor()
    {
        var image = GetComponent<Image>();
        image.color = gameOverColor;
    }

    public void SetInteractable(bool value)
    {
       TileButton.interactable = value;
    }

    private void Start()
    {
        spriteMap = new() { { TileImage.None, noneSprite }, { TileImage.X, xSprite }, { TileImage.O, oSprite } };
        colorMap = new() { { TileImage.None, noneColor}, { TileImage.X, xColor}, { TileImage.O, oColor } };
        TileImage = TileImage.None;
    }
}

public enum TileImage
{
    None,
    X,
    O
}