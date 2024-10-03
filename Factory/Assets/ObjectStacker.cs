using UnityEngine;

public class ObjectStacker : MonoBehaviour
{
    public float yOffset = 1.0f;      // Her objenin aralýðýnda ne kadar yükseklik olacak
    public Vector3 rotation = new Vector3(0, 0, 0);  // Objelerin alacaðý rotasyon
    private GameObject lastObject;    // Bir önceki objeyi takip etmek için

    private void OnTriggerEnter(Collider other)
    {
        // Eðer bir obje içeri girerse ve lastObject null deðilse, yeni objeyi üstüne yerleþtir
        if (other.gameObject.CompareTag("Stack")) // Sadece "Stackable" tag'li objeleri dikkate al
        {
            if (lastObject == null)
            {
                // Ýlk obje ise, direk yerine yerleþtir ve rotasyonu ayarla
                lastObject = other.gameObject;
                lastObject.transform.rotation = Quaternion.Euler(rotation);
            }
            else
            {
                // Bir önceki objenin üzerine yerleþtir
                Vector3 newPosition = lastObject.transform.position;
                newPosition.y += yOffset;  // Y ekseni boyunca yükseklik ekle
                other.transform.position = newPosition;

                // Rotasyonu ayarla
                other.transform.rotation = Quaternion.Euler(rotation);

                // Yeni objeyi lastObject olarak ayarla
                lastObject = other.gameObject;
            }
        }
    }
}
