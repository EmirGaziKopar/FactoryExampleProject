using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RobotManager : MonoBehaviour
{
    [Header("Robots")]
    [SerializeField] private GameObject robotPrefab;
    [SerializeField] private Transform[] spawnPoints;

    [Header("Factory")]
    [SerializeField] private Transform productPickupLocation;

    [Header("UI")] 
    [SerializeField] private TextMeshProUGUI _robotCountText;
    [SerializeField] private GameObject _robotInfoPanel;
    [SerializeField] private GameObject _robotInfoWindow;

    private List<RobotController> robots;
    private List<RobotController> availableRobots = new();
    private RobotController bestRobotForCurrentProduct;

    private List<Product> currentProductPool;

    private bool canSignalRobots;
    
    private void OnEnable()
    {
        InGameEventManager.Instance.OnProductArrived += ProductArrived;
        InGameEventManager.Instance.OnProductPickedUp += OnProductPickedUp;
        InGameEventManager.Instance.OnProductDroppedOff += OnProductDroppedOff;
        InGameEventManager.Instance.OnRobotStateSetOnIdle += OnRobotStateSetOnIdle;
    }

    private void OnDisable()
    {
        InGameEventManager.Instance.OnProductArrived -= ProductArrived;
        InGameEventManager.Instance.OnProductPickedUp -= OnProductPickedUp;
        InGameEventManager.Instance.OnProductDroppedOff -= OnProductDroppedOff;
        InGameEventManager.Instance.OnRobotStateSetOnIdle -= OnRobotStateSetOnIdle;
    }

    private void Start()
    {
        currentProductPool = new List<Product>();
        robots = new List<RobotController>();
        
        SpawnRobots();
        SetRobotInfoUI();
    }

    private void SpawnRobots()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            GameObject robot = Instantiate(robotPrefab, spawnPoint.position, spawnPoint.rotation);
            RobotController robotController = robot.GetComponent<RobotController>();

            if (robotController)
            {
                robotController.OnSpawned(spawnPoint, productPickupLocation);
                robots.Add(robotController);
            }
        }
    }

    private void SetRobotInfoUI()
    {
        for (var index = 0; index < robots.Count; index++)
        {
            var robot = robots[index];
            GameObject robotInfoWindow = Instantiate(_robotInfoWindow, Vector3.zero, Quaternion.identity,
                _robotInfoPanel.transform);
            RobotInfoWindow infoController = robotInfoWindow.GetComponent<RobotInfoWindow>();
            infoController.RobotController = robot;
            infoController.RobotName = $"Robot {index + 1}";
        }
        
        _robotCountText.text = $"Robot Count: {robots.Count}";
    }
    
    private void ProductArrived(Product product)
    {
        currentProductPool.Add(product);
        UpdateCanSignalRobotsState();
        StartCoroutine(SignalRobotsForPickup());
    }
    
    private void InitiateRobotForMovement(Product product)
    {
        GetAvailableRobots();
        FindBestRobotForProduct(product);
        SignalBestRobotForProduct(product);
    }
    
    private void OnProductPickedUp(Product product)
    {
        currentProductPool.Remove(product);
        UpdateCanSignalRobotsState();
    }
    
    private void OnProductDroppedOff(Product product)
    {
        UpdateCanSignalRobotsState();
    }
    
    private void GetAvailableRobots()
    {
        availableRobots.Clear();
        
        foreach (RobotController robot in robots)
        {
            if (!robot.IsWorking)
            {
                availableRobots.Add(robot);
            }
        }
    }

    private void FindBestRobotForProduct(Product product)
    {
        float distanceToProduct;
        float bestDistance = float.MaxValue;
        
        foreach (RobotController robot in availableRobots)
        {
            distanceToProduct  = Vector3.Distance(robot.transform.position, product.transform.position);
            
            if (bestDistance > distanceToProduct)
            {
                bestDistance = distanceToProduct;
                bestRobotForCurrentProduct = robot;
            }
        }
    }

    private void SignalBestRobotForProduct(Product product)
    {
        if (bestRobotForCurrentProduct)
        {
            bestRobotForCurrentProduct.InitializeRobotForPickup(product);
        }
        else
        {
            Debug.Log($"There is no best robot!");
        }
    }
    
    private void OnRobotStateSetOnIdle(RobotController robot)
    {
        UpdateCanSignalRobotsState();
        StartCoroutine(SignalRobotsForPickup());
    }

    private void UpdateCanSignalRobotsState()
    {
        canSignalRobots = currentProductPool.Count > 0;
    }
    
    private IEnumerator SignalRobotsForPickup()
    {
        while (canSignalRobots)
        {
            yield return new WaitForSeconds(Random.Range(1,2));

            foreach (var productForRobot in currentProductPool)
            {
                if (!productForRobot.isGoalForRobot)
                {
                    GetAvailableRobots();
                    if (availableRobots.Count > 0)
                    {
                        productForRobot.isGoalForRobot = true;
                        FindBestRobotForProduct(productForRobot);
                        SignalBestRobotForProduct(productForRobot);
                        break;
                    }
                }
                else
                {
                    
                    continue;
                }
                

                canSignalRobots = false;
            }
        }

        if (!canSignalRobots)
        {
            GetAvailableRobots();
            foreach (RobotController robot in availableRobots)
            {
                robot.SetState(RobotState.MovingToSleep);
            }
        }
    }
}
