using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameBoard : MonoBehaviour
{
    public static GameBoard Instance {get; private set;}

    [SerializeField] private Material[] appleSections;
    [SerializeField] private Material[] androidSections;
    [SerializeField] private Transform tilePrefab;

    private Board board;
    private Camera mainCamera;

    private void Awake() {

        if (Instance != null)
        {
            Debug.LogError("More than one GameBoard! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        board = new Board(3, 3, 10f);
        
        if (Application.platform == RuntimePlatform.Android)
        {
            board.CreateTiles(tilePrefab, androidSections);
        }
        else if(Application.platform == RuntimePlatform.IPhonePlayer)
        {
            board.CreateTiles(tilePrefab, appleSections);
        } 
        else
        {
            board.CreateTiles(tilePrefab, appleSections);
        }
    }

    private void Start() {
        mainCamera = Camera.main;
    }


    private void Update() {
        if (Touchscreen.current.primaryTouch.press.isPressed)
        {
            BoardPosition position = board.GetBoardPosition(MouseWorld.GetPosition());
            Debug.Log(position);
        }
    }
}
