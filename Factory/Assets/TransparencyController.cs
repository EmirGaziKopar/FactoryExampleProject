using UnityEngine;
using UnityEngine.EventSystems; // UI sistemini kontrol etmek için ekleniyor

public class TransparencyController : MonoBehaviour
{
    public float targetAlpha = 0.5f; // Fare objenin üzerinde iken ulaþýlacak transparanlýk deðeri
    public float lerpSpeed = 5f;     // Geçiþ hýzý
    private Material objMaterial;
    private bool isMouseOver = false; // Fare objenin üzerinde mi deðil mi kontrolü
    private bool isClicked = false;   // Fare týklanmýþ mý kontrolü
    private bool shouldStayTransparent = false; // Þeffaflýk korunacak mý?

    public GameObject targetObject; // Aktif/Pasif yapýlacak olan GameObject

    void Start()
    {
        objMaterial = GetComponent<Renderer>().material;

        // Baþlangýç rengini tamamen þeffaf yap
        SetMaterialAlpha(0f);

        // Baþlangýçta targetObject'i pasif yap
        targetObject.SetActive(false);
    }

    void Update()
    {
        // Fare objenin üzerine gelindiðinde þeffaflýk geçiþi yap
        if (isMouseOver && !isClicked)
        {
            float alpha = Mathf.Lerp(objMaterial.color.a, targetAlpha, Time.deltaTime * lerpSpeed);
            SetMaterialAlpha(alpha);
        }

        // Fare objeye týklandýðýnda þeffaflýk korunsun ve targetObject aktif hale getirilsin
        if (isMouseOver && Input.GetMouseButtonDown(0))
        {
            isClicked = true;
            shouldStayTransparent = true; // Týklama yapýldýðýnda transparanlýk korunacak
            targetObject.SetActive(true); // Objeyi aktif hale getir
        }

        // Fare objenin dýþýna týklandýðýnda transparanlýðý sýfýrla ve targetObject'i kapat
        if (!isMouseOver && Input.GetMouseButtonDown(0) && !IsPointerOverUI())
        {
            isClicked = false;
            shouldStayTransparent = false; // Baþka yere týklanýnca transparanlýk sýfýrlanacak
            SetMaterialAlpha(0f); // Hemen þeffaf hale getir
            targetObject.SetActive(false); // TargetObject'i devre dýþý býrak
        }

        // Eðer týklama yapýlmýþ ve þeffaflýk korunuyorsa þeffaflýk sabit kalýr
        if (shouldStayTransparent)
        {
            float alpha = Mathf.Lerp(objMaterial.color.a, targetAlpha, Time.deltaTime * lerpSpeed);
            SetMaterialAlpha(alpha);
        }
    }

    // Fare objenin üzerine geldiðinde çalýþýr
    void OnMouseOver()
    {
        isMouseOver = true;
    }

    // Fare objeden çýktýðýnda çalýþýr
    void OnMouseExit()
    {
        isMouseOver = false;

        // Eðer týklama yapýlmadýysa transparanlýðý sýfýrla, eðer yapýldýysa sabit kalsýn
        if (!isClicked)
        {
            SetMaterialAlpha(0f); // Fareyi çekince eðer týklanmadýysa þeffaflýk sýfýrlanacak
        }
    }

    // Materyalin alfa (þeffaflýk) deðerini ayarlayan yardýmcý fonksiyon
    private void SetMaterialAlpha(float alpha)
    {
        Color color = objMaterial.color;
        color.a = alpha;
        objMaterial.color = color;
    }

    // Fare týklamasýnýn UI üzerinde olup olmadýðýný kontrol eden yardýmcý fonksiyon
    private bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
