using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ProductMovement : MonoBehaviour
{
    public GameObject productPrefab;
    public Transform SpawnPoint;
    public Transform[] wayPoints; //üzerinde gezmemiz gereken noktalar 
    public float SpawnInterval = 5.0f;
    public float moveSpeed = 2.0f;
    public float rotationSpeed = 2.0f;


    //Animasyonlar için gerekenler 
    public Animation robotArmAnimation;
    public Animation secondAnimation;
    public float waitTimeFirs = 3.0f;
    public float waitTimeSecond = 3.0f;

    //Kaynak ve ürün yönetimi  
    public int totalResources = 100;
    public int resourceAmountNeeded = 5;
    public TextMeshProUGUI productCounterText;

    // içsel durum 
    private int totalProductsProduced = 0; //Üretilen Toplam ürün sayýsý 
    private bool isSpawning = true;
    private bool isProductMoving = false;

    private GameObject currentProduct; //Þu anda hareket edenn ürün 
    private int currentWayPointIndex = 0;
    private bool isWaiting = false;


    void Start()
    {
        UpdateProductCounter();
        StartCoroutine(SpawnProducts());
    }


    IEnumerator SpawnProducts()
    {
        //Burada bir iþlem var 

        while(isSpawning && totalResources >= resourceAmountNeeded)
        {

            if(!isProductMoving && totalResources >= resourceAmountNeeded)
            {
                totalResources -= resourceAmountNeeded;
                currentProduct = Instantiate(productPrefab, SpawnPoint.position, Quaternion.identity); //Burada artýk ürün ütetildi 

                totalProductsProduced++;

                UpdateProductCounter(); //Text updated

                isProductMoving = true;

                currentWayPointIndex = 0;

                //Ürünleri harekete baþlatacak fonksiyon burada çalýþtýrýlacak
                StartCoroutine(MoveProduct());


            }

            yield return new WaitForSeconds(SpawnInterval);
        }

        //Burada bir iþlem var 
    }


    IEnumerator MoveProduct()
    {
        while (currentWayPointIndex < wayPoints.Length)
        {
            if (!isWaiting)
            {
                Transform targetWayPoint = wayPoints[currentWayPointIndex];

                currentProduct.transform.position = Vector3.MoveTowards(currentProduct.transform.position, targetWayPoint.position, moveSpeed * Time.deltaTime);

                Vector3 lookDirection = targetWayPoint.position - currentProduct.transform.position;
                lookDirection.y = 0; //Y ekseninde dönmesin

                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                currentProduct.transform.rotation = Quaternion.Lerp(currentProduct.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                if(Vector3.Distance(currentProduct.transform.position, targetWayPoint.position) < 0.1f) //0.1 den daha yakýnsa 
                {
                    currentWayPointIndex++;

                    if(currentWayPointIndex == 5) //Bekleme noktasý 
                    {
                        yield return StartCoroutine(WaitAndPlayFirstAnimation());
                    }
                    if(currentWayPointIndex == 4)
                    {
                        yield return StartCoroutine(WaitAndPlaySecondAnimation());
                    }
                }

            }
            //Bir sonraki kareyi bekle 
            yield return null;
        }

        isProductMoving = false;
        
    }

    IEnumerator WaitAndPlayFirstAnimation()
    {
        isWaiting = true;

        robotArmAnimation.Play("RobotArm");

        yield return new WaitForSeconds(waitTimeFirs);

        isWaiting = false;

    }

    IEnumerator WaitAndPlaySecondAnimation()
    {
        isWaiting = true;
        secondAnimation.Play("Line06");

        yield return new WaitForSeconds(waitTimeSecond);

        isWaiting = false;

    }



    void UpdateProductCounter()
    {
        if (productCounterText.transform.gameObject.activeSelf)
        {
            productCounterText.text = "Üretilen ürün sayýsý: " + totalProductsProduced.ToString();
        }
    }
    // Update is called once per frame
}
