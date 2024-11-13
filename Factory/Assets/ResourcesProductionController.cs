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
    public Color defaultColor = Color.white; // Varsay�lan buton rengi
    public Color selectedColor = Color.green; // Se�ili buton rengi
    public float timer = 5f; // �retim i�in zamanlay�c� (d��ar�dan ayarlanabilir)

    public Button[] buttons; // Panel alt�ndaki butonlar
    private int selectedButtonIndex = -1; // Se�ili butonun index'i
    private float productionTimer = 0f; // Zamanlay�c� i�in saya�

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
        // E�er bir buton se�ildiyse �retim i�in zamanlay�c�y� ba�lat
        if (selectedButtonIndex != -1)
        {
            productionTimer += Time.deltaTime;
            if (productionTimer >= timer)
            {
                // Zamanlay�c� doldu�unda ilgili kayna�� art�r
                switch (selectedButtonIndex)
                {
                    case 0:
                        copperCount++;
                        copper.text = " " + copperCount.ToString();
                        Debug.Log("Copper �retiliyor: " + copperCount);
                        break;
                    case 1:
                        plasticCount++;
                        plastic.text = " " + plasticCount.ToString();
                        Debug.Log("Plastic �retiliyor: " + plasticCount);
                        break;
                    case 2:
                        steelCount++;
                        steel.text = " " + steelCount.ToString();
                        Debug.Log("Steel �retiliyor : " + steelCount);
                        break;
                }
                productionTimer = 0f; // Saya� s�f�rlan�r ve �retim devam eder
            }
        }
    }

    public void OnButtonClick(int index)
    {
        // Buton t�kland���nda ilgili buton se�ilir ve di�erleri s�f�rlan�r
        selectedButtonIndex = index;
        ResetButtonColors();
        SetButtonColor(buttons[selectedButtonIndex], selectedColor); // Se�ili butonu ye�il yap
    }

    void ResetButtonColors()
    {
        // T�m butonlar�n renklerini varsay�lan rengine d�nd�r
        foreach (Button btn in buttons)
        {
            SetButtonColor(btn, defaultColor);
        }
    }

    void SetButtonColor(Button button, Color color)
    {
        // Button'un t�m durumlar�n�n rengini de�i�tir
        ColorBlock cb = button.colors;
        cb.normalColor = color;
        cb.highlightedColor = color;
        cb.pressedColor = color;
        cb.selectedColor = color;
        button.colors = cb;
    }
}
