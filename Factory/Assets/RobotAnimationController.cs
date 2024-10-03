using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAnimationController : MonoBehaviour
{
    public Animation robotArmAnimation;

    void Start()
    {
        // Animation bileþeni atanmamýþsa, component'i bul.
        if (robotArmAnimation == null)
        {
            robotArmAnimation = GetComponent<Animation>();
        }
    }

    void Update()
    {
        // Eðer bir tuþa basýlýrsa animasyonu tetikle
        if (Input.GetKeyDown(KeyCode.Space))
        {
            robotArmAnimation.Play("RobotArm");
        }
    }
}
