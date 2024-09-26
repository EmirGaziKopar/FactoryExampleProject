// Son ürün bölmesi ürün aldığı zaman bu mesajı robotları kontrol eden üst sisteme bildirir.

// Tüm robotları kontrol eden bir üst sistem **
// Bu sistem "fabrika paletine ürün geldi" bilgisine sahip olacak. Mesaj geldiğinde: **
// Eğer ürün geldi ise -> Bünyede bulunan robotlardan güncel ürüne en uygun ve en yakın olan robotu saptayacak.
// Eğer saptadıysa -> Robota çalışması için haber verecek.
// Tüm robotlar doluysa ve meşgulse bir bekleme mekanizması hazırla. (opsiyonel)

// 1. Bekleme, uyuma
// 1.1. Robota palete ürün geldiği bilgisi verilir. Robot yönetim sistemi en yakın ve en uygun robota 1.2. adımı iletir.
// 1.2. Şu anda objeyi almaya giden robot var mı? -> yoksa 2. adıma git., varsa 1-2 saniye bekle, dolan.
// 2. Obje almaya gidiyor**
// 3. Objeyi alma**
// 4. Objeyi bırakmaya gidiyor**
// 5. Objeyi bıraktı** -> Çalışıyor , ürün alma sırasında
// 6. İş var mı kontrolü -> iş varsa 2. adıma geri dön, iş yoksa 7. duruma dön
// 7. Uyumaya gitme

// Robotun durum bilgisi:
// 1. Çalışıyor - Uyuyor
// 2. Çalışıyor ise -> Ürünü almaya gidiyor, Ürünü bırakmaya gidiyor, Ürün alma sırasında
// 3. Çalışmıyor ise -> Uyumaya gider, Komut bekler

using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public enum RobotState
{
    Sleeping,
    MovingToPickup,
    Picking,
    MovingToDrop,
    Dropping,
    Idle,
    MovingToSleep
}

public class RobotController : MonoBehaviour
{
    [Header("Product Views")]
    [SerializeField] private GameObject productOneCarryView;
    [SerializeField] private GameObject productTwoCarryView;
    [SerializeField] private GameObject productThreeCarryView;
    
    private Transform productPickupLocation;
    
    private GameObject currentProductView;
    private GameObject currentTower;
    private RobotState currentState;
    private Transform sleepPoint;
  
    private bool isWorking;
    public bool IsWorking => isWorking;
    public RobotState CurrentState => currentState;

    private Product goalProduct;
    private Product currentProduct;
    private NavMeshAgent agent;

    public void OnSpawned(Transform spawnPoint, Transform pickupLocation)
    {
        sleepPoint = spawnPoint;
        productPickupLocation = pickupLocation;
    }
    
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        SetState(RobotState.Sleeping);
    }

    public void SetState(RobotState newState)
    {
        currentState = newState;
        OnStateChanged();
    }

    public void InitializeRobotForPickup(Product product)
    {
        isWorking = true;
        goalProduct = product;

        if (currentState == RobotState.Sleeping || currentState == RobotState.Idle)
        {
            SetState(RobotState.MovingToPickup);
        }
    }

    private void OnStateChanged()
    {
        switch (currentState)
        {
            case RobotState.Sleeping:
                isWorking = false;
                break;

            case RobotState.MovingToPickup:
                GoTowardsProduct();
                break;

            case RobotState.Picking:
                PickUpProduct();
                break;

            case RobotState.MovingToDrop:
                GoTowardsTower();
                break;

            case RobotState.Dropping:
                DropOffProduct();
                break;

            case RobotState.Idle:
                isWorking = false;
                SetIdleBehaviour();
                InGameEventManager.Instance.RobotSetOnIdle(this);
                break;
            
            case RobotState.MovingToSleep:
                GoToSleepPosition();
                SetState(RobotState.Sleeping);
                break;
        }
    }

    private void GoTowardsProduct()
    {
        agent.SetDestination(productPickupLocation.position);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Product"))
        {
            if (!currentProduct)
            {
                SetState(RobotState.Picking);
                return;
            }
        }
        
        if (other.CompareTag("tower"))
        {
            if (currentProduct)
            {
                SetState(RobotState.Dropping);
                return;
            }
        }

        if (other.CompareTag("Spawn"))
        {
            SetState(RobotState.Sleeping);
        }
    }

    private void PickUpProduct()
    {
        if (goalProduct)
        {
            currentProduct = goalProduct;
            goalProduct = null;
        }

        if (currentProduct)
        {
            switch (currentProduct.productType)
            {
                case ProductType.Product1:
                    currentProductView = productOneCarryView;
                    break;
                case ProductType.Product2:
                    currentProductView = productTwoCarryView;
                    break;
                case ProductType.Product3:
                    currentProductView = productThreeCarryView;
                    break;
            }
            
            currentProductView.SetActive(true);

        }
        
        InGameEventManager.Instance.ProductPickedUp(currentProduct);
        SetState(RobotState.MovingToDrop);
    }

    private void DropOffProduct()
    {
        InGameEventManager.Instance.ProductDroppedOff(currentProduct);
        currentProductView.SetActive(false);
        currentProduct = null;
        SetState(RobotState.Idle);
    }
    
    private void GoTowardsTower()
    {
        Vector3 towerPosition = TowerManager.Instance.RetrieveProductTowerPosition(currentProduct);
        agent.SetDestination(towerPosition);
    }

    private void GoToSleepPosition()
    {
        agent.SetDestination(sleepPoint.position);
    }

    public void SetIdleBehaviour()
    {
        // TODO: Temporary, get a better random position.
        float randXValue = Random.Range(-20.0f, 20.0f);
        float randZValue = Random.Range(-20.0f, 20.0f);
        Vector3 randomPoint = new 
            Vector3(transform.position.x + randXValue, transform.position.y, transform.position.z + randZValue);
        
        agent.SetDestination(randomPoint);
    }
}

// public Transform[] towers; // Array to store the towers (assign in Inspector)
// public Transform productSpawnPoint;
// public Transform productWaitPoint;
// public Camera cam;
// public UnityEngine.AI.NavMeshAgent agent;
// public finalProductCounter productCounter; 
// public GameObject product1;
// public GameObject product2;
// public GameObject product3;
// public bool workOn = false;
// public RobotManager robotMan; 
// private GameObject currentProduct;
//
// // Start is called before the first frame update
// void Start()
// {
//     agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
// }
//
// void Update()
// {
//     if (productCounter.product1Count > 0 || productCounter.product2Count > 0 || productCounter.product3Count > 0)
//     {
//         if(currentProduct == null && workOn)
//         {
//             GoTakeProduct();
//         }
//     }
// }
//
// private void GoTakeProduct(Product product)
// {
//     agent.SetDestination(product.transform.position);
// }
//
// void GoWaitProduct()
// {
//     // Command the agent to move to the product spawn point
//     agent.SetDestination(productWaitPoint.position);
//     workOn = false;
//     robotMan.whichRobot = !robotMan.whichRobot;
// }
//
//

//
