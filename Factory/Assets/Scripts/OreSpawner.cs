using System.Collections;
using UnityEngine;

public class OreSpawner : MonoBehaviour
{
    public GameObject currentOrePrefab; // Dinamik olarak atanacak prefab
    public float spawnInterval = 3f; // Spawn aral��� (saniye)
    public RotationScript rotationScript; // Matkab�n rotation scripti

    private void Start()
    {
        StartCoroutine(SpawnOre());
    }

    IEnumerator SpawnOre()
    {
        while (true)
        {
            // Belirli bir s�re bekle
            yield return new WaitForSeconds(spawnInterval);

            // Matkap �al���rken ore spawnla
            if (rotationScript != null && rotationScript.isActiveAndEnabled)
            {
                if (currentOrePrefab != null)
                {
                    Instantiate(currentOrePrefab, transform.position, transform.rotation);
                }
            }
        }
    }

    public void SetOrePrefab(GameObject newOrePrefab)
    {
        currentOrePrefab = newOrePrefab;
    }
}
