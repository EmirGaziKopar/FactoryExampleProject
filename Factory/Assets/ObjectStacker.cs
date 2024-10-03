using UnityEngine;

public class ObjectStacker : MonoBehaviour
{
    public float yOffset = 1.0f;      // Her objenin aral���nda ne kadar y�kseklik olacak
    public Vector3 rotation = new Vector3(0, 0, 0);  // Objelerin alaca�� rotasyon
    private GameObject lastObject;    // Bir �nceki objeyi takip etmek i�in

    private void OnTriggerEnter(Collider other)
    {
        // E�er bir obje i�eri girerse ve lastObject null de�ilse, yeni objeyi �st�ne yerle�tir
        if (other.gameObject.CompareTag("Stack")) // Sadece "Stackable" tag'li objeleri dikkate al
        {
            if (lastObject == null)
            {
                // �lk obje ise, direk yerine yerle�tir ve rotasyonu ayarla
                lastObject = other.gameObject;
                lastObject.transform.rotation = Quaternion.Euler(rotation);
            }
            else
            {
                // Bir �nceki objenin �zerine yerle�tir
                Vector3 newPosition = lastObject.transform.position;
                newPosition.y += yOffset;  // Y ekseni boyunca y�kseklik ekle
                other.transform.position = newPosition;

                // Rotasyonu ayarla
                other.transform.rotation = Quaternion.Euler(rotation);

                // Yeni objeyi lastObject olarak ayarla
                lastObject = other.gameObject;
            }
        }
    }
}
