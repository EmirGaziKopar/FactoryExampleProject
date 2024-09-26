using UnityEngine;

public class InGameEventManager : MonoBehaviour
{
    public static InGameEventManager Instance;

    // Sonradan tekrar bakalım.
    public delegate void ProductStateDelegate(Product product);
    public event ProductStateDelegate OnProductArrived;
    public event ProductStateDelegate OnProductPickedUp;
    public event ProductStateDelegate OnProductDroppedOff;
    
    public delegate void RobotStateSetOnIdle(RobotController product);
    public event RobotStateSetOnIdle OnRobotStateSetOnIdle;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    
    public void ProductArrived(Product product)
    {
        OnProductArrived?.Invoke(product);
    }

    public void ProductPickedUp(Product product)
    {
        OnProductPickedUp?.Invoke(product);
    }
    
    public void ProductDroppedOff(Product product)
    {
        OnProductDroppedOff?.Invoke(product);
    }

    public void RobotSetOnIdle(RobotController product)
    {
        OnRobotStateSetOnIdle?.Invoke(product);
    }
}