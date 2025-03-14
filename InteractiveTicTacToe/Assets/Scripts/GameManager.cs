using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private const int Rows = 3;
    private const int Columns = 3;

    [SerializeField]
    private AudioClip xAudio;

    [SerializeField]
    private AudioClip oAudio;

    [SerializeField]
    private AudioClip winAudio;

    [SerializeField]
    private int eraseTime = 4;

    [SerializeField]
    private Tile[] tiles;

    private TileImage currentImageTurn;

    private float eraseTimer;
    
    private bool xStarted;
    private bool oStarted;

    private Queue<Tile> tileQueue;

    private bool isGameOver;

    public void RestartGame()
    {
        SceneManager.LoadScene("Main");
    }

    private void Start()
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            Tile tile = tiles[i];
            tile.TileIndex = i;
            tile.TileButton.onClick.AddListener(() => TileButtonClick(tile));
        }

        currentImageTurn = TileImage.X;
        eraseTimer = 0;
        xStarted = false;
        oStarted = false;
        tileQueue = new Queue<Tile>();
    }
    
    private void TileButtonClick(Tile tile)
    {
        tile.TileImage = currentImageTurn;
        if (!xStarted && currentImageTurn == TileImage.X)
        {
            xStarted = true;
        }
        if (!oStarted && currentImageTurn == TileImage.O)
        {
            oStarted = true;
        }

        tileQueue.Enqueue(tile);

        SwapTurn();
        isGameOver = CheckGameOver();

        var audioClip = isGameOver ? winAudio : (currentImageTurn == TileImage.X ? xAudio : oAudio);
        GetComponent<AudioSource>().PlayOneShot(audioClip);
    }

    private bool CheckGameOver()
    {
        List<Tile> winningTiles = new List<Tile>();
        for (int row = 0; row < Rows; row++)
        {
            var startIndex = row * Columns;
            
            if (tiles[startIndex].TileImage == TileImage.None) continue;

            if (tiles[startIndex].TileImage == tiles[startIndex + 1].TileImage 
                && tiles[startIndex].TileImage == tiles[startIndex + 2].TileImage)
            {
                winningTiles.AddRange(new Tile[] { tiles[startIndex], tiles[startIndex + 1], tiles[startIndex + 2] });
            }
        }

        for (int col = 0; col < Columns; col++)
        {
            if (tiles[col].TileImage == TileImage.None) continue;

            if (tiles[col].TileImage == tiles[col + Rows].TileImage
                && tiles[col].TileImage == tiles[col + 2 * Rows].TileImage)
            {
                winningTiles.AddRange(new Tile[] { tiles[col], tiles[col + Rows], tiles[col + 2 * Rows] });
            }
        }

        //Check diagonals
        if (tiles[0].TileImage != TileImage.None 
            && tiles[0].TileImage == tiles[4].TileImage
            && tiles[0].TileImage == tiles[8].TileImage)
        {
            winningTiles.AddRange(new Tile[] { tiles[0], tiles[4], tiles[8] });
        }

        if (tiles[2].TileImage != TileImage.None
            && tiles[2].TileImage == tiles[4].TileImage
            && tiles[2].TileImage == tiles[6].TileImage)
        {
            winningTiles.AddRange(new Tile[] { tiles[2], tiles[4], tiles[6] });
        }

        foreach (var winTile in winningTiles)
        {
            winTile.SetGameOverColor();
        }

        if(winningTiles.Count > 0)
        {
            foreach(var tile in tiles)
            {
                tile.SetInteractable(false);
            }
        }

        return winningTiles.Count > 0;
    }

    private void SwapTurn()
    {
        currentImageTurn = currentImageTurn == TileImage.X ? TileImage.O : TileImage.X;
    }

    private void Update()
    {
        if (isGameOver) return;

        if (oStarted && xStarted)
        {
            eraseTimer += Time.deltaTime;
        }

        if (eraseTimer > eraseTime)
        {
            eraseTimer = 0;
            if (tileQueue.Count > 0)
            {
                tileQueue.Dequeue().TileImage = TileImage.None;
            }
        }
    }
}
