using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.GridLayoutGroup;

public class ResourceProductionController : MonoBehaviour
{
    public TextMeshProUGUI copper;
    public TextMeshProUGUI plastic;
    public TextMeshProUGUI steel;
    public Color defaultColor = Color.white; // Varsayýlan buton rengi
    public Color selectedColor = Color.green; // Seçili buton rengi
    public float timer = 5f; // Üretim için zamanlayýcý (dýþarýdan ayarlanabilir)

    public Button[] buttons; // Panel altýndaki butonlar
    private int selectedButtonIndex = -1; // Seçili butonun index'i
    private float productionTimer = 0f; // Zamanlayýcý için sayaç

    public int plasticCount = 0;
    public int copperCount = 0;
    public int steelCount = 0;

    void Start()
    {
        buttons = new Button[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            buttons[i] = transform.GetChild(i).GetComponent<Button>();
        }

        ResetButtonColors();
    }

    void Update()
    {
        // Eðer bir buton seçildiyse üretim için zamanlayýcýyý baþlat
        if (selectedButtonIndex != -1)
        {
            productionTimer += Time.deltaTime;
            if (productionTimer >= timer)
            {
                // Zamanlayýcý dolduðunda ilgili kaynaðý artýr
                switch (selectedButtonIndex)
                {
                    case 0:
                        copperCount++;
                        copper.text = " " + copperCount.ToString();
                        Debug.Log("Copper üretiliyor: " + copperCount);
                        break;
                    case 1:
                        plasticCount++;
                        plastic.text = " " + plasticCount.ToString();
                        Debug.Log("Plastic üretiliyor: " + plasticCount);
                        break;
                    case 2:
                        steelCount++;
                        steel.text = " " + steelCount.ToString();
                        Debug.Log("Steel üretiliyor : " + steelCount);
                        break;
                }
                productionTimer = 0f; // Sayaç sýfýrlanýr ve üretim devam eder
            }
        }
    }

    public void OnButtonClick(int index)
    {
        // Buton týklandýðýnda ilgili buton seçilir ve diðerleri sýfýrlanýr
        selectedButtonIndex = index;
        ResetButtonColors();
        SetButtonColor(buttons[selectedButtonIndex], selectedColor); // Seçili butonu yeþil yap
    }

    void ResetButtonColors()
    {
        // Tüm butonlarýn renklerini varsayýlan rengine döndür
        foreach (Button btn in buttons)
        {
            SetButtonColor(btn, defaultColor);
        }
    }

    void SetButtonColor(Button button, Color color)
    {
        // Button'un tüm durumlarýnýn rengini deðiþtir
        ColorBlock cb = button.colors;
        cb.normalColor = color;
        cb.highlightedColor = color;
        cb.pressedColor = color;
        cb.selectedColor = color;
        button.colors = cb;
    }
}
