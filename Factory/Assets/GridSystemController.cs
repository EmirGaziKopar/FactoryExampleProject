using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemController : MonoBehaviour
{
    public bool isActive;
    private bool isClicked = false;  // T�klan�p t�klanmad���n� kontrol eder
    private Color[] originalColors;  // Orijinal renkleri saklar
    private Renderer[] childRenderers;  // T�m child'lar�n Renderer'lar�
    public GameObject mainObject;

    void Start()
    {
        // T�m child'lar�n Renderer'lar�n� al
        childRenderers = GetComponentsInChildren<Renderer>();

        // Orijinal renkleri kaydet
        originalColors = new Color[childRenderers.Length];
        for (int i = 0; i < childRenderers.Length; i++)
        {
            originalColors[i] = childRenderers[i].material.color;
        }

        // Ba�lang��ta parent'� kapal� yapal�m
        mainObject.SetActive(false);
    }

    void OnMouseEnter()
    {
        // E�er t�klanmad�ysa materyalleri ye�il yap
        if (!isClicked)
        {
            foreach (Renderer renderer in childRenderers)
            {
                renderer.material.color = Color.green; // Ye�il renge d�nd�r
            }
            mainObject.SetActive(true); // Mouse �zerine gelince aktif et
        }
    }

    void OnMouseExit()
    {
        // E�er t�klanmad�ysa materyalleri eski haline d�nd�r ve parent'� kapat
        if (!isClicked)
        {
            ResetMaterials();
            mainObject.SetActive(false); // Mouse ayr�ld���nda kapat
        }
    }

    void OnMouseDown()
    {
        // Sol t�klama yap�ld���nda materyalleri eski haline d�nd�r ve objeyi aktif tut
        isClicked = true;
        ResetMaterials();
        mainObject.SetActive(true); // T�klama sonras� hep aktif kals�n
        isActive = true; //Bunun amac� �retim UI'�n�n aktifli�ini ba�latmak
    }

    // Orijinal renkleri geri y�kler
    private void ResetMaterials()
    {
        for (int i = 0; i < childRenderers.Length; i++)
        {
            childRenderers[i].material.color = originalColors[i];
        }
    }
}
