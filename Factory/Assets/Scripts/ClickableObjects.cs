using UnityEngine;

public class ClickableObjects : MonoBehaviour
{
    public GameObject drillPrefab; // Spawnlanacak drill prefabý
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Sol mouse tuþu týklamasý
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform) // Týklanan obje bu obje mi?
                {
                    SpawnDrill(gameObject.transform.position);
                    Destroy(gameObject); // Objenin kendisini yok et
                }
            }
        }
    }

    void SpawnDrill(Vector3 position)
    {
        // Drill prefabýný spawnla
        Instantiate(drillPrefab, position, Quaternion.identity);
    }
  
}
