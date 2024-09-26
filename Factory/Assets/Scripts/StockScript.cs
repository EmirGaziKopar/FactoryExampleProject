using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StockScript : MonoBehaviour
{

    public float diamondAmount;
    public float goldAmount;
    public TextMeshPro goldAmountText;
    public TextMeshPro diamondAmountText;



    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "diamondOre")
        {
            diamondAmount++;
            updateDiamondAmount();
            Destroy(other.gameObject);

        }
        else if (other.tag == "goldOre")
        {
            goldAmount++;
            updateGoldAmount();
            Destroy(other.gameObject);

        }
    }

    public void updateGoldAmount()
    {
        goldAmountText.text = "Gold: " + goldAmount.ToString();
    }
    public void updateDiamondAmount()
    {
        diamondAmountText.text = "Diamond: " + diamondAmount.ToString();
    }
}
