using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class IndicatorController : MonoBehaviour
{
    public float targetAlpha = 0.1f;
    public float lerpSpeed = 5f;
    private Material objMaterial;
    private bool isMouseOver = false;
    private bool isClicked = false;
    private bool shouldStayTransparent = false; //�effafl�k korunacak m� ? 

    public GameObject UI; //Aktif pasif yap�lacak olan gameObje



    // Start is called before the first frame update
    void Start()
    {
        objMaterial = GetComponent<Renderer>().material;

        SetMaterialAlpha(0); //Bu Transparanl�k anlam�na geliyor (Tamamen �effaf)

        UI.SetActive(false);

    }


    void Update()
    {
        if(isMouseOver && !isClicked)
        {
            float alpha = Mathf.Lerp(objMaterial.color.a, targetAlpha, Time.deltaTime * lerpSpeed);
            SetMaterialAlpha(alpha);
        }

        if(isMouseOver && Input.GetMouseButtonDown(0)) //S�f�r left click
        {
            isClicked = true;
            shouldStayTransparent = true; //Ne zaman transparan kalmal�y�m ? 
            UI.SetActive(true); //Objeyi aktif hale getirdi�imizi g�sterir
        }

        if(isMouseOver == false && Input.GetMouseButtonDown(0) && !IsPointerOverUI())
        {
            isClicked = false;
            shouldStayTransparent = false;
            SetMaterialAlpha(0f); //Hemen �effaf hale gelecek
            UI.SetActive(false);
        }

        if (shouldStayTransparent)
        {
            float alpha = Mathf.Lerp(objMaterial.color.a, targetAlpha, Time.deltaTime * lerpSpeed);
            SetMaterialAlpha(alpha);
        }

    }

    private void OnMouseOver()
    {
        isMouseOver = true;
    }

    private void OnMouseExit()
    {
        isMouseOver = false;

        if (!isClicked) //Exit olduysak ve clicklemiyorsak
        {
            SetMaterialAlpha(0f);
        }
    }

    private void SetMaterialAlpha(float alpha)
    {
        Color color = objMaterial.color;
        color.a = alpha;
        objMaterial.color = color;
    }

    private bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    // Update is called once per frame
    
}
