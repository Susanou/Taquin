using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameBoard : MonoBehaviour
{
    public static GameBoard Instance {get; private set;}

    [SerializeField] private Sprite[] appleSections;
    [SerializeField] private Sprite[] androidSections;
    [SerializeField] private Transform tilePrefab;
    [SerializeField] private Transform gameCanvas;

    private Board board;

    private void Awake() {

        if (Instance != null)
        {
            Debug.LogError("More than one GameBoard! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        board = new Board(3, 3, 100f);
        
        if (Application.platform == RuntimePlatform.Android)
        {
            board.CreateTiles(tilePrefab, androidSections, gameCanvas);
        }
        else if(Application.platform == RuntimePlatform.IPhonePlayer)
        {
            board.CreateTiles(tilePrefab, appleSections, gameCanvas);
        } 
        else
        {
            board.CreateTiles(tilePrefab, appleSections, gameCanvas);
        }
    }
}
