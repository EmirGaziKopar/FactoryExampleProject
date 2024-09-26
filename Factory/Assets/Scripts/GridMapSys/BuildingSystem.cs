using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingSystem : MonoBehaviour
{
    [SerializeField] private Tilemap MainTilemap;
    [SerializeField] private TileBase whiteTile;
    
    public static BuildingSystem current;

    public GridLayout gridLayout;
    private Grid grid;

    public GameObject prefab1;
    public GameObject prefab2;
    public GameObject prefab3;
    public GameObject destroyModeText; // Yok etme modu i�in Text objesi

    private PlacableObject objectToPlace;
    private bool destroyMode = false;

    #region Unity methods
    private void Awake()
    {
        current = this;
        grid = gridLayout.gameObject.GetComponent<Grid>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            destroyMode = !destroyMode;
            destroyModeText.gameObject.SetActive(destroyMode); // Text objesini aktif/deaktif yap
        }

        if (destroyMode)
        {
            CheckForDestroy();
        }
        else if (objectToPlace)
        {
            MoveObjectToMouse();

            if (Input.GetKeyDown(KeyCode.R))
            {
                RotateObject();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (CanBePlaced(objectToPlace))
                {
                    objectToPlace.Place();
                    objectToPlace = null;
                }
                else
                {
                    Destroy(objectToPlace.gameObject);
                    objectToPlace = null;
                }
            }
        }
    }
    #endregion

    #region Utils
    public void GoldDrill(){
        ChangePrefab(prefab2);
    }

    public void DiamondDrill(){
        ChangePrefab(prefab1);
    }

    public void Conveyor(){
        ChangePrefab(prefab3);
    }


    public static Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            return raycastHit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }

    public Vector3 SnapCoordinateToGrid(Vector3 position)
    {
        Vector3Int cellPos = gridLayout.WorldToCell(position);
        position = grid.GetCellCenterWorld(cellPos);
        return position;
    }

    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        int counter = 0;

        foreach (var v in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(v.x, v.y, 0);
            array[counter] = tilemap.GetTile(pos);
            counter++;
        }

        return array;
    }

    #endregion

    #region Building Placement

    public void InitializeWithObject(GameObject prefab)
    {
        Vector3 position = SnapCoordinateToGrid(GetMouseWorldPosition());

        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        objectToPlace = obj.GetComponent<PlacableObject>();
        obj.AddComponent<ObjectDrag>();
    }

    public void ChangePrefab(GameObject newPrefab)
    {
        // E�er bir obje zaten se�iliyse, eski objeyi yok et
        if (objectToPlace != null)
        {
            Destroy(objectToPlace.gameObject);
            objectToPlace = null;
        }

        // Yeni objeyi ba�lat
        InitializeWithObject(newPrefab);
    }

    private bool CanBePlaced(PlacableObject placableObject)
    {
        BoundsInt area = new BoundsInt();
        area.position = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
        area.size = placableObject.Size;

        TileBase[] baseArray = GetTilesBlock(area, MainTilemap);

        foreach (var b in baseArray)
        {
            if (b == whiteTile)
            {
                return false;
            }
        }

        // Yeni kontrol: Yerle�im alan�nda ba�ka bir collider var m�?  
        Vector3 checkPosition = objectToPlace.transform.position;
        Vector3 checkSize = (Vector3)placableObject.Size / 2.0f;
        Collider[] colliders = Physics.OverlapBox(checkPosition, checkSize, Quaternion.identity);
        if (colliders.Length > 0)
        {
            return false;
        }

        return true;
    }

    private void MoveObjectToMouse()
    {
        Vector3 position = SnapCoordinateToGrid(GetMouseWorldPosition());
        objectToPlace.transform.position = position;
    }

    private void RotateObject()
    {
        objectToPlace.transform.Rotate(0, 90, 0);
    }

    private void CheckForDestroy()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("drill") || hit.collider.CompareTag("conveyor")|| hit.collider.CompareTag("diamondOre")|| hit.collider.CompareTag("goldOre"))
                {
                    Destroy(hit.collider.gameObject);
                }
            }
        }
    }

    #endregion
}
