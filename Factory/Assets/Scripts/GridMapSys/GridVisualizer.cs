using UnityEngine;
using UnityEngine.Tilemaps;

public class GridVisualizer : MonoBehaviour
{
    //public Grid grid;
    //public Tilemap tilemap;
    //private LineRenderer lineRenderer;

    //public Color gridColor = Color.white;
    //public float lineWidth = 0.1f; // Çizgi kalýnlýðýný artýrdým

    //private void Start()
    //{
    //    lineRenderer = gameObject.AddComponent<LineRenderer>();
    //    lineRenderer.startWidth = lineWidth;
    //    lineRenderer.endWidth = lineWidth;
    //    lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
    //    lineRenderer.startColor = gridColor;
    //    lineRenderer.endColor = gridColor;
    //    lineRenderer.positionCount = 0;
    //    lineRenderer.useWorldSpace = true; // Dünya uzayýnda çizim yapacak þekilde ayarla

    //    DrawGrid();
    //}

    //private void DrawGrid()
    //{
    //    BoundsInt bounds = tilemap.cellBounds;
    //    Vector3Int cellPosition;

    //    for (int x = bounds.xMin; x <= bounds.xMax; x++)
    //    {
    //        for (int y = bounds.yMin; y <= bounds.yMax; y++)
    //        {
    //            for (int z = bounds.zMin; z <= bounds.zMax; z++)
    //            {
    //                cellPosition = new Vector3Int(x, y, z);
    //                if (tilemap.HasTile(cellPosition))
    //                {
    //                    DrawCell(cellPosition);
    //                }
    //            }
    //        }
    //    }
    //}

    //private void DrawCell(Vector3Int cellPosition)
    //{
    //    Vector3 cellWorldPosition = grid.CellToWorld(cellPosition);
    //    Vector3[] cellVertices = new Vector3[5];

    //    cellVertices[0] = cellWorldPosition;
    //    cellVertices[1] = cellWorldPosition + new Vector3(grid.cellSize.x, 0, 0);
    //    cellVertices[2] = cellWorldPosition + new Vector3(grid.cellSize.x, grid.cellSize.y, 0);
    //    cellVertices[3] = cellWorldPosition + new Vector3(0, grid.cellSize.y, 0);
    //    cellVertices[4] = cellWorldPosition;

    //    int currentCount = lineRenderer.positionCount;
    //    lineRenderer.positionCount = currentCount + cellVertices.Length;
    //    lineRenderer.SetPositions(cellVertices);
    //}

    //private void OnDrawGizmos()
    //{
    //    if (grid != null && tilemap != null)
    //    {
    //        BoundsInt bounds = tilemap.cellBounds;
    //        Vector3Int cellPosition;

    //        for (int x = bounds.xMin; x <= bounds.xMax; x++)
    //        {
    //            for (int y = bounds.yMin; y <= bounds.yMax; y++)
    //            {
    //                for (int z = bounds.zMin; z <= bounds.zMax; z++)
    //                {
    //                    cellPosition = new Vector3Int(x, y, z);
    //                    if (tilemap.HasTile(cellPosition))
    //                    {
    //                        Vector3 cellWorldPosition = grid.CellToWorld(cellPosition);
    //                        Gizmos.color = Color.red;
    //                        Gizmos.DrawWireCube(cellWorldPosition + new Vector3(grid.cellSize.x, grid.cellSize.y, grid.cellSize.z) / 2, new Vector3(grid.cellSize.x, grid.cellSize.y, grid.cellSize.z));
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}
}
