using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameBoard : MonoBehaviour
{
    public static GameBoard Instance {get; private set;}

    [SerializeField] private float waitTime;
    [SerializeField] private Material[] appleSections;
    [SerializeField] private Material[] androidSections;
    [SerializeField] private Transform tilePrefab;
    [SerializeField] private LayerMask tileLayerMask;

    private Board board;
    private Camera mainCamera;
    private TileObject selectedTile;
    private bool isBusy;

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

        if (isBusy) return;

        if (Touchscreen.current.primaryTouch.press.isPressed)
        {
            BoardPosition position = board.GetBoardPosition(MouseWorld.GetPosition());
            //Debug.Log(position);
            //Debug.Log(board.isValidMove(position));

            if(!TryHandleTileSelection(position)) return;

            StartCoroutine(TileLerpAnimation(board.GetEmptyTilePosition()));
            board.MoveTiles(selectedTile);
            
        }
    }
    
    public bool TryHandleTileSelection(BoardPosition position)
    {
        Ray ray = Camera.main.ScreenPointToRay(Touchscreen.current.position.ReadValue());
        
        if(Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, tileLayerMask))
        {
            if(raycastHit.transform.TryGetComponent<TileObject>(out TileObject tile) && board.isValidMove(position))
            {
                Debug.Log("tile selected");
                SetSelectedTile(tile);
                return true;
            }
        }
        Debug.Log("Nothing found");
        return false;
    }

    public void SetSelectedTile(TileObject tile)
    {
        this.selectedTile = tile;
    }

    public IEnumerator TileLerpAnimation(BoardPosition emptyPosition)
    {
        isBusy = true;
        float elapsedTime = 0;
        Vector3 emptyWorldPosition = board.GetWorldPosition(emptyPosition);

        while (elapsedTime < waitTime)
        {
            selectedTile.transform.position = Vector3.Lerp(selectedTile.transform.position, emptyWorldPosition, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;
        
            // Yield here
            yield return null;
        }  
        // Make sure we got there
        selectedTile.transform.position = emptyWorldPosition;
        selectedTile = null;

        isBusy = false;

        yield return null;
    }
}
