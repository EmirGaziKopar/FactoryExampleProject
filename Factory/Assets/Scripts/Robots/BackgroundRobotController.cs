using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class BackgroundRobotController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject[] _carryObjects;
    [SerializeField] private Transform[] _moveTargets;

    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int CarryMove = Animator.StringToHash("CarryMove");
    private static readonly int Walking = Animator.StringToHash("Walking");
    
    // Eğer robotun elinde taşıması için farklı objeler kullanmak istersen bu satırları sil:
    [SerializeField] private GameObject _carryCube;
    
    private RobotState _currentState;
    private GameObject _currentCarryObject;
    private Transform _currentMoveTarget;

    private List<Transform> _moveTargetList;

    public bool isWorking;

    private NavMeshAgent _navMeshAgent;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        GenerateNewMoveTargetList();
    }

    private void Start()
    {
        SetState(RobotState.Idle);
    }
    
    private void Update()
    {
        if (!isWorking)
        {
            if (_currentState != RobotState.Sleeping)
            { 
                SetState(RobotState.Sleeping);
            }
        }
        else
        {
            if (_currentState == RobotState.Sleeping || _currentState == RobotState.Idle)
            {
                SetState(RobotState.Idle);
            }
        }
    }

    private void SetAnimation(int animationName)
    {
        _animator.SetTrigger(animationName);
    }
    public void SetState(RobotState newState)
    {
        _currentState = newState;
        OnStateChanged();
    }

    private void OnStateChanged()
    {
        switch (_currentState)
        {
            case RobotState.Sleeping:
                SetAnimation(Idle);
                _currentCarryObject = null;
                if(_carryCube.activeInHierarchy) _carryCube.SetActive(false);
                _currentMoveTarget = null;
                if (_navMeshAgent && _navMeshAgent.enabled)
                {
                    _navMeshAgent.isStopped = true;
                    _navMeshAgent.ResetPath();
                }
                break;

            case RobotState.MovingToPickup:
                SetAnimation(Walking);
                break;

            case RobotState.Picking:
                _currentMoveTarget = null;
                EnableCarryObject();
                SetState(RobotState.MovingToDrop);
                break;

            case RobotState.MovingToDrop:
                SetAnimation(CarryMove);
                SetNewDestination();
                break;

            case RobotState.Dropping:
                _currentMoveTarget = null;
                EnableCarryObject(false);
                SetAnimation(Idle);
                break;

            case RobotState.Idle:
                SetNewDestination();
                SetState(RobotState.MovingToPickup);
                isWorking = true;
                break;

            case RobotState.MovingToSleep:
                SetAnimation(Walking);
                break;
        }
    }

    //TODO: Gideceği yere varınca burası çalışacak
    private void EnableCarryObject(bool state = true)
    {
        // alıyorum
        if (!_currentCarryObject)
        {
            if (_carryObjects.Length > 0)
            {
                int randomIndex = Random.Range(0, _carryObjects.Length);
                _currentCarryObject = _carryObjects[randomIndex];
                return;
            }
            // Eğer robotun elinde taşıması için farklı objeler kullanmak istersen else bloğunu komple sil

            if (_carryCube)
            {
                Material material = _carryCube.gameObject.GetComponent<Renderer>().material;
                material.color =  new Color(Random.value, Random.value, Random.value);
                _currentCarryObject = _carryCube;
                _currentCarryObject.SetActive(state);
                return;
            }
        }

        // bırakıyorum, baska objeler gelirse burasi degisecek
        _currentCarryObject.SetActive(state);
        _currentCarryObject = null;
        
        SetState(RobotState.Idle);
    }

    private void SetNewDestination()
    {
        if (FindMoveTarget())
        {
            MoveToPoint(_currentMoveTarget);
        }
    }
    
    //TODO: Şimdilik rastgele çalışıyor. Belki ileride belli bir algoritmaya göre çalışabilir.
    private Transform FindMoveTarget()
    {
        if (_currentMoveTarget == null || _moveTargetList.Count <= 0)
        {
            if (_moveTargetList.Count <= 0)
            {
                GenerateNewMoveTargetList();
            }

            for (int i = 0; i < _moveTargetList.Count; i++)
            {
                float distance = Vector3.Distance(transform.position, _moveTargetList[i].position);

                if (distance > 15f)
                {
                    _currentMoveTarget = _moveTargetList[i];
                    _moveTargetList.RemoveAt(i);
                    break;
                }
            }
        }

        return _currentMoveTarget;
    }

    
    private GameObject _lastCollidedObject;
    //TODO: Varış noktasına geldiği yerler
    private void OnTriggerEnter(Collider other)
    {
        if (_lastCollidedObject != other.gameObject)
        {
            _lastCollidedObject = other.gameObject;

            if (_currentState != RobotState.Sleeping || _currentState != RobotState.Idle)
            {
                if (_lastCollidedObject == _currentMoveTarget.gameObject)
                {
                    if (other.CompareTag("BackgroundRobotInteraction"))
                    {
                        if (_currentCarryObject)
                        {
                            SetState(RobotState.Dropping);
                        }
                        else
                        {
                            SetState(RobotState.Picking);
                        }
                    }
                }
            }
        }
    }

    private void MoveToPoint(Transform target)
    {
        _navMeshAgent.SetDestination(target.position);
    }
    
    private void GenerateNewMoveTargetList()
    {
        _moveTargetList = new List<Transform>();

        if (_moveTargets.Length > 0)
        {
            _moveTargetList.AddRange(_moveTargets);
            Shuffle(_moveTargetList); 
        }
        else
        {
            Debug.LogWarning("Robotun gidecegi hicbir yer yok. Inspector panelinden gidecegi yerlerin atanmasi gerekli.");
        }

    }
    public static void Shuffle<T>(IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = Random.Range(i, count);
            (ts[i], ts[r]) = (ts[r], ts[i]);
        }
    }
}