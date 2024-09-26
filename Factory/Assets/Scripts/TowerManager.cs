using System;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public static TowerManager Instance;
    
    [SerializeField] private Transform productOneTower;
    [SerializeField] private Transform productTwoTower;
    [SerializeField] private Transform productThreeTower;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public Vector3 RetrieveProductTowerPosition(Product product)
    {
        switch (product.productType)
        {
            case ProductType.Product1: return productOneTower.position;
            case ProductType.Product2: return productTwoTower.position;
            case ProductType.Product3: return productThreeTower.position;
        }

        return Vector3.zero;
    }
}