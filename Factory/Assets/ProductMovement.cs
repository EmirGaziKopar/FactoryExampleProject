using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ProductMovement : MonoBehaviour
{
    public GameObject productPrefab;
    public Transform SpawnPoint;
    public Transform[] wayPoints; //�zerinde gezmemiz gereken noktalar 
    public float SpawnInterval = 5.0f;
    public float moveSpeed = 2.0f;
    public float rotationSpeed = 2.0f;


    //Animasyonlar i�in gerekenler 
    public Animation robotArmAnimation;
    public Animation secondAnimation;
    public float waitTimeFirs = 3.0f;
    public float waitTimeSecond = 3.0f;

    //Kaynak ve �r�n y�netimi  
    public int totalResources = 100;
    public int resourceAmountNeeded = 5;
    public TextMeshProUGUI productCounterText;

    // i�sel durum 
    private int totalProductsProduced = 0; //�retilen Toplam �r�n say�s� 
    private bool isSpawning = true;
    private bool isProductMoving = false;

    private GameObject currentProduct; //�u anda hareket edenn �r�n 
    private int currentWayPointIndex = 0;
    private bool isWaiting = false;


    void Start()
    {
        UpdateProductCounter();
        StartCoroutine(SpawnProducts());
    }


    IEnumerator SpawnProducts()
    {
        //Burada bir i�lem var 

        while(isSpawning && totalResources >= resourceAmountNeeded)
        {

            if(!isProductMoving && totalResources >= resourceAmountNeeded)
            {
                totalResources -= resourceAmountNeeded;
                currentProduct = Instantiate(productPrefab, SpawnPoint.position, Quaternion.identity); //Burada art�k �r�n �tetildi 

                totalProductsProduced++;

                UpdateProductCounter(); //Text updated

                isProductMoving = true;

                currentWayPointIndex = 0;

                //�r�nleri harekete ba�latacak fonksiyon burada �al��t�r�lacak
                StartCoroutine(MoveProduct());


            }

            yield return new WaitForSeconds(SpawnInterval);
        }

        //Burada bir i�lem var 
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
                lookDirection.y = 0; //Y ekseninde d�nmesin

                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                currentProduct.transform.rotation = Quaternion.Lerp(currentProduct.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                if(Vector3.Distance(currentProduct.transform.position, targetWayPoint.position) < 0.1f) //0.1 den daha yak�nsa 
                {
                    currentWayPointIndex++;

                    if(currentWayPointIndex == 5) //Bekleme noktas� 
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
            productCounterText.text = "�retilen �r�n say�s�: " + totalProductsProduced.ToString();
        }
    }
    // Update is called once per frame
}
