using UnityEngine;
using System;

public class PlacableObject : MonoBehaviour
{
    public bool Placed { get; private set; }
    public Vector3Int Size { get; private set; }

    private Vector3[] Vertices;
    private Collider objectCollider;
    private RotationScript rotationScript;
    private OreSpawner oreSpawner;
    private BoxCollider conveyorBeltCollider;
    private BoxCollider pusherLeftCollider;
    private BoxCollider pusherRightCollider;
    private BoxCollider wareHouseCollider;

    private void GetColliderVertexPositionsLocal()
    {
        BoxCollider b = gameObject.GetComponent<BoxCollider>();
        Vertices = new Vector3[4];
        Vertices[0] = b.center + new Vector3(-b.size.x, -b.size.y, -b.size.z) * 0.5f;
        Vertices[1] = b.center + new Vector3(b.size.x, -b.size.y, -b.size.z) * 0.5f;
        Vertices[2] = b.center + new Vector3(b.size.x, -b.size.y, b.size.z) * 0.5f;
        Vertices[3] = b.center + new Vector3(-b.size.x, -b.size.y, b.size.z) * 0.5f;
    }

    private void CalculateSizeInCells()
    {
        Vector3Int[] vertices = new Vector3Int[Vertices.Length];

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 worldPos = transform.TransformPoint(Vertices[i]);
            vertices[i] = BuildingSystem.current.gridLayout.WorldToCell(worldPos);
        }

        Size = new Vector3Int(Math.Abs((vertices[0] - vertices[1]).x),
            Math.Abs((vertices[0] - vertices[3]).y), 1);
    }

    public Vector3 GetStartPosition()
    {
        return transform.TransformPoint(Vertices[0]);
    }

    private void Start()
    {
        GetColliderVertexPositionsLocal();
        CalculateSizeInCells();

        objectCollider = GetComponent<BoxCollider>();
        if (objectCollider != null)
        {
            objectCollider.enabled = false; // Box Collider'� ba�lang��ta devre d��� b�rak
        }

        // Conveyor_Belt'in collider'�n� kapat
        Transform conveyorBeltTransform = transform.Find("Conveyor_Kit Variant/MadeUp/Conveyor_Belt");
        if (conveyorBeltTransform != null)
        {
            conveyorBeltCollider = conveyorBeltTransform.GetComponent<BoxCollider>();
            if (conveyorBeltCollider != null)
            {
                conveyorBeltCollider.enabled = false;
            }
        }

        // Pusher'lar�n collider'lar�n� kapat
        Transform pusherLeftTransform = transform.Find("pusher_left");
        if (pusherLeftTransform != null)
        {
            pusherLeftCollider = pusherLeftTransform.GetComponent<BoxCollider>();
            if (pusherLeftCollider != null)
            {
                pusherLeftCollider.enabled = false;
            }
        }

        Transform pusherRightTransform = transform.Find("pusher_right");
        if (pusherRightTransform != null)
        {
            pusherRightCollider = pusherRightTransform.GetComponent<BoxCollider>();
            if (pusherRightCollider != null)
            {
                pusherRightCollider.enabled = false;
            }
        }

        // WareHouse Variant'�n collider'�n� kapat
        Transform wareHouseTransform = transform.Find("WareHouse Variant");
        if (wareHouseTransform != null)
        {
            wareHouseCollider = wareHouseTransform.GetComponent<BoxCollider>();
            if (wareHouseCollider != null)
            {
                //wareHouseCollider.enabled = false;
            }
        }

        // RotationScript'i kapat
        rotationScript = GetComponentInChildren<RotationScript>();
        if (rotationScript != null)
        {
            rotationScript.enabled = false;
        }

        // OreSpawner'� kapat
        oreSpawner = GetComponentInChildren<OreSpawner>();
        if (oreSpawner != null)
        {
            oreSpawner.enabled = false;
        }
    }

    public virtual void Place()
    {
        ObjectDrag drag = gameObject.GetComponent<ObjectDrag>();
        Destroy(drag);

        if (objectCollider != null)
        {
            objectCollider.enabled = true; // Yerle�tirildi�inde Box Collider'� etkinle�tir
        }

        if (conveyorBeltCollider != null)
        {
            conveyorBeltCollider.enabled = true; // Yerle�tirildi�inde Conveyor_Belt'in Box Collider'�n� etkinle�tir
        }

        if (pusherLeftCollider != null)
        {
            pusherLeftCollider.enabled = true; // Yerle�tirildi�inde pusher_left'in Box Collider'�n� etkinle�tir
        }

        if (pusherRightCollider != null)
        {
            pusherRightCollider.enabled = true; // Yerle�tirildi�inde pusher_right'�n Box Collider'�n� etkinle�tir
        }

        if (wareHouseCollider != null)
        {
            wareHouseCollider.enabled = true; // Yerle�tirildi�inde WareHouse Variant'�n Box Collider'�n� etkinle�tir
        }

        if (rotationScript != null)
        {
            rotationScript.enabled = true; // Yerle�tirildi�inde RotationScript'i etkinle�tir
        }

        if (oreSpawner != null)
        {
            oreSpawner.enabled = true; // Yerle�tirildi�inde OreSpawner'� etkinle�tir
        }

        Placed = true;
    }
}
