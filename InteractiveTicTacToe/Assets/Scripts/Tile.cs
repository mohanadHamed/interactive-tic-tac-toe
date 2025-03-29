using NUnit.Framework.Constraints;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public Button TileButton => GetComponent<Button>();

    public int TileIndex { get; set; }

    public float RemainingTime { get; set; }

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

    [SerializeField]
    private Slider slider;

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
            RemainingTime = GameManager.Instance.EraseTime;
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

    private void Update()
    {
        slider.value = RemainingTime / GameManager.Instance.EraseTime;

        slider.gameObject.SetActive(!GameManager.Instance.IsGameOver && _tileImage != TileImage.None && GameManager.Instance.BothPlayersStarted);

        if (slider.gameObject.activeSelf && _tileImage == GameManager.Instance.CurrentImageTurn)
        {
            RemainingTime -= Time.deltaTime;
        }
    }
}

public enum TileImage
{
    None,
    X,
    O
}