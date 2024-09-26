using UnityEngine;
using UnityEngine.EventSystems; // UI sistemini kontrol etmek i�in ekleniyor

public class TransparencyController : MonoBehaviour
{
    public float targetAlpha = 0.5f; // Fare objenin �zerinde iken ula��lacak transparanl�k de�eri
    public float lerpSpeed = 5f;     // Ge�i� h�z�
    private Material objMaterial;
    private bool isMouseOver = false; // Fare objenin �zerinde mi de�il mi kontrol�
    private bool isClicked = false;   // Fare t�klanm�� m� kontrol�
    private bool shouldStayTransparent = false; // �effafl�k korunacak m�?

    public GameObject targetObject; // Aktif/Pasif yap�lacak olan GameObject

    void Start()
    {
        objMaterial = GetComponent<Renderer>().material;

        // Ba�lang�� rengini tamamen �effaf yap
        SetMaterialAlpha(0f);

        // Ba�lang��ta targetObject'i pasif yap
        targetObject.SetActive(false);
    }

    void Update()
    {
        // Fare objenin �zerine gelindi�inde �effafl�k ge�i�i yap
        if (isMouseOver && !isClicked)
        {
            float alpha = Mathf.Lerp(objMaterial.color.a, targetAlpha, Time.deltaTime * lerpSpeed);
            SetMaterialAlpha(alpha);
        }

        // Fare objeye t�kland���nda �effafl�k korunsun ve targetObject aktif hale getirilsin
        if (isMouseOver && Input.GetMouseButtonDown(0))
        {
            isClicked = true;
            shouldStayTransparent = true; // T�klama yap�ld���nda transparanl�k korunacak
            targetObject.SetActive(true); // Objeyi aktif hale getir
        }

        // Fare objenin d���na t�kland���nda transparanl��� s�f�rla ve targetObject'i kapat
        if (!isMouseOver && Input.GetMouseButtonDown(0) && !IsPointerOverUI())
        {
            isClicked = false;
            shouldStayTransparent = false; // Ba�ka yere t�klan�nca transparanl�k s�f�rlanacak
            SetMaterialAlpha(0f); // Hemen �effaf hale getir
            targetObject.SetActive(false); // TargetObject'i devre d��� b�rak
        }

        // E�er t�klama yap�lm�� ve �effafl�k korunuyorsa �effafl�k sabit kal�r
        if (shouldStayTransparent)
        {
            float alpha = Mathf.Lerp(objMaterial.color.a, targetAlpha, Time.deltaTime * lerpSpeed);
            SetMaterialAlpha(alpha);
        }
    }

    // Fare objenin �zerine geldi�inde �al���r
    void OnMouseOver()
    {
        isMouseOver = true;
    }

    // Fare objeden ��kt���nda �al���r
    void OnMouseExit()
    {
        isMouseOver = false;

        // E�er t�klama yap�lmad�ysa transparanl��� s�f�rla, e�er yap�ld�ysa sabit kals�n
        if (!isClicked)
        {
            SetMaterialAlpha(0f); // Fareyi �ekince e�er t�klanmad�ysa �effafl�k s�f�rlanacak
        }
    }

    // Materyalin alfa (�effafl�k) de�erini ayarlayan yard�mc� fonksiyon
    private void SetMaterialAlpha(float alpha)
    {
        Color color = objMaterial.color;
        color.a = alpha;
        objMaterial.color = color;
    }

    // Fare t�klamas�n�n UI �zerinde olup olmad���n� kontrol eden yard�mc� fonksiyon
    private bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
