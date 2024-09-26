using System;
using UnityEngine;
using TMPro;

public class FinalProductCounter : MonoBehaviour
{
    public TextMeshProUGUI product1Text;
    public TextMeshProUGUI product2Text;
    public TextMeshProUGUI product3Text;

    public TextMeshProUGUI Delivery1Text;
    public TextMeshProUGUI Delivery2Text;
    public TextMeshProUGUI Delivery3Text;

    public int product1Count = 0;
    public int product2Count = 0;
    public int product3Count = 0;

    public int Delivery1Count = 0;
    public int Delivery2Count = 0;
    public int Delivery3Count = 0;

    private void OnEnable()
    {
        InGameEventManager.Instance.OnProductPickedUp += ProductPickedUp;
        InGameEventManager.Instance.OnProductDroppedOff += ProductDroppedOff;
    }

    private void OnDisable()
    {
        InGameEventManager.Instance.OnProductPickedUp -= ProductPickedUp;
        InGameEventManager.Instance.OnProductDroppedOff -= ProductDroppedOff;
    }

    private void ProductPickedUp(Product product)
    {
        switch (product.productType)
        {
            case ProductType.Product1:
                product1Count--;
                break;
            case ProductType.Product2:
                product2Count--;
                break;
            case ProductType.Product3:
                product3Count--;
                break;
        }

        UpdateProductTexts();
    }

    private void ProductDroppedOff(Product product)
    {
        switch (product.productType)
        {
            case ProductType.Product1:
                Delivery1Count++;
                break;

            case ProductType.Product2:
                Delivery2Count++;
                break;

            case ProductType.Product3:
                Delivery3Count++;
                break;
        }

        Destroy(product.gameObject);
        
        UpdateProductTexts();
    }

    private void Start()
    {
        UpdateProductTexts();
    }

    public void UpdateProductTexts()
    {
        product1Text.text = "Product 1: " + product1Count;
        product2Text.text = "Product 2: " + product2Count;
        product3Text.text = "Product 3: " + product3Count;
        Delivery1Text.text = "Delivery 1: " + Delivery1Count;
        Delivery2Text.text = "Delivery 2: " + Delivery2Count;
        Delivery3Text.text = "Delivery 3: " + Delivery3Count;
    }

    private void OnTriggerEnter(Collider other)
    {
        Product arrivedProduct = other.GetComponent<Product>();
        
        if (arrivedProduct)
        {
            InGameEventManager.Instance.ProductArrived(arrivedProduct);

            switch (arrivedProduct.productType)
            {
                case ProductType.Product1:
                    product1Count++;
                    break;
                case ProductType.Product2:
                    product2Count++;
                    break;
                case ProductType.Product3:
                    product3Count++;
                    break;
            }
            
            arrivedProduct.gameObject.SetActive(false);
        }
        
        UpdateProductTexts();
    }
}