using UnityEngine;

public class ClickableObjects : MonoBehaviour
{
    public GameObject drillPrefab; // Spawnlanacak drill prefab�
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Sol mouse tu�u t�klamas�
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform) // T�klanan obje bu obje mi?
                {
                    SpawnDrill(gameObject.transform.position);
                    Destroy(gameObject); // Objenin kendisini yok et
                }
            }
        }
    }

    void SpawnDrill(Vector3 position)
    {
        // Drill prefab�n� spawnla
        Instantiate(drillPrefab, position, Quaternion.identity);
    }
  
}
