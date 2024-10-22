using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemController : MonoBehaviour
{
    public bool isActive;
    private bool isClicked = false;  // Týklanýp týklanmadýðýný kontrol eder
    private Color[] originalColors;  // Orijinal renkleri saklar
    private Renderer[] childRenderers;  // Tüm child'larýn Renderer'larý
    public GameObject mainObject;

    void Start()
    {
        // Tüm child'larýn Renderer'larýný al
        childRenderers = GetComponentsInChildren<Renderer>();

        // Orijinal renkleri kaydet
        originalColors = new Color[childRenderers.Length];
        for (int i = 0; i < childRenderers.Length; i++)
        {
            originalColors[i] = childRenderers[i].material.color;
        }

        // Baþlangýçta parent'ý kapalý yapalým
        mainObject.SetActive(false);
    }

    void OnMouseEnter()
    {
        // Eðer týklanmadýysa materyalleri yeþil yap
        if (!isClicked)
        {
            foreach (Renderer renderer in childRenderers)
            {
                renderer.material.color = Color.green; // Yeþil renge döndür
            }
            mainObject.SetActive(true); // Mouse üzerine gelince aktif et
        }
    }

    void OnMouseExit()
    {
        // Eðer týklanmadýysa materyalleri eski haline döndür ve parent'ý kapat
        if (!isClicked)
        {
            ResetMaterials();
            mainObject.SetActive(false); // Mouse ayrýldýðýnda kapat
        }
    }

    void OnMouseDown()
    {
        // Sol týklama yapýldýðýnda materyalleri eski haline döndür ve objeyi aktif tut
        isClicked = true;
        ResetMaterials();
        mainObject.SetActive(true); // Týklama sonrasý hep aktif kalsýn
        isActive = true; //Bunun amacý üretim UI'ýnýn aktifliðini baþlatmak
    }

    // Orijinal renkleri geri yükler
    private void ResetMaterials()
    {
        for (int i = 0; i < childRenderers.Length; i++)
        {
            childRenderers[i].material.color = originalColors[i];
        }
    }
}
