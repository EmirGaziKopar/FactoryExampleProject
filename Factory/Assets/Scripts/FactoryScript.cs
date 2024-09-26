using System.Collections;
using UnityEngine;

public class FactoryScript : MonoBehaviour
{
    public GameObject product1Prefab;
    public GameObject product2Prefab;
    public GameObject product3Prefab;
    
    public StockScript stockScript; // Stok scriptine referans
    public Transform productSpawnPoint; // �r�nlerin spawnlanaca�� nokta
    
    private bool isProducing = false;
    private bool stopProduction = false;
    private ProductType currentProductType = ProductType.None;

    public void ProduceProduct1()
    {
        if (stopProduction) stopProduction = false;
        currentProductType = ProductType.Product1;
        if (!isProducing)
        {
            StartCoroutine(ProductionLoop());
        }
    }

    public void ProduceProduct2()
    {
        if (stopProduction) stopProduction = false;
        currentProductType = ProductType.Product2;
        if (!isProducing)
        {
            StartCoroutine(ProductionLoop());
        }
    }

    public void ProduceProduct3()
    {
        if (stopProduction) stopProduction = false;
        currentProductType = ProductType.Product3;
        if (!isProducing)
        {
            StartCoroutine(ProductionLoop());
        }
    }

    public void StopProduction()
    {
        stopProduction = true;
    }

    IEnumerator ProductionLoop()
    {
        while (!stopProduction)
        {
            if (currentProductType != ProductType.None)
            {
                yield return StartCoroutine(ProduceProduct(currentProductType));
            }
            else
            {
                yield return null;
            }
        }
        
        isProducing = false;
    }

    private IEnumerator ProduceProduct(ProductType productType)
    {
        isProducing = true;

        // �retim maliyetlerini ve s�relerini kontrol edin
        switch (productType)
        {
            case ProductType.Product1:
                if (stockScript.goldAmount >= 1 && stockScript.diamondAmount >= 1)
                {
                    stockScript.goldAmount -= 1;
                    stockScript.diamondAmount -= 1;
                    stockScript.updateGoldAmount();
                    stockScript.updateDiamondAmount();
                    yield return new WaitForSeconds(2); // �retim s�resi
                    GameObject spawnedProduct = Instantiate(product1Prefab, productSpawnPoint.position, productSpawnPoint.rotation);
                    Product productController = spawnedProduct.GetComponent<Product>();
                }
                break;
            
            case ProductType.Product2:
                if (stockScript.goldAmount >= 2 && stockScript.diamondAmount >= 2)
                {
                    stockScript.goldAmount -= 2;
                    stockScript.diamondAmount -= 2;
                    stockScript.updateGoldAmount();
                    stockScript.updateDiamondAmount();
                    yield return new WaitForSeconds(3); // �retim s�resi
                    GameObject spawnedProduct = Instantiate(product2Prefab, productSpawnPoint.position, productSpawnPoint.rotation);
                    Product productController = spawnedProduct.GetComponent<Product>();
                }
                break;
            
            case ProductType.Product3:
                if (stockScript.goldAmount >= 2 && stockScript.diamondAmount >= 3)
                {
                    stockScript.goldAmount -= 2;
                    stockScript.diamondAmount -= 3;
                    stockScript.updateGoldAmount();
                    stockScript.updateDiamondAmount();
                    yield return new WaitForSeconds(4); // �retim s�resi
                    GameObject spawnedProduct = Instantiate(product3Prefab, productSpawnPoint.position, productSpawnPoint.rotation);
                    Product productController = spawnedProduct.GetComponent<Product>();
                }
                break;
        }

        isProducing = false;
    }
}

public enum ProductType
{
    None,
    Product1,
    Product2,
    Product3
}
