using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAnimationController : MonoBehaviour
{
    public Animation robotArmAnimation;

    void Start()
    {
        // Animation bile�eni atanmam��sa, component'i bul.
        if (robotArmAnimation == null)
        {
            robotArmAnimation = GetComponent<Animation>();
        }
    }

    void Update()
    {
        // E�er bir tu�a bas�l�rsa animasyonu tetikle
        if (Input.GetKeyDown(KeyCode.Space))
        {
            robotArmAnimation.Play("RobotArm");
        }
    }
}
