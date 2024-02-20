using UnityEngine;

public class IKControl : MonoBehaviour
{
    protected Animator animator;

    public bool ikActive = false;
    public Transform rightHandObj = null;
    public Transform lookObj = null;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnAnimatorIK()
    {
        if (animator)
        {
            // If IK is active and the right hand object has been assigned
            if (ikActive && rightHandObj != null)
            {
                // Set the right hand target position and rotation directly from your landmark data
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandObj.position);
                animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandObj.rotation);
            }
            
            // If the look object has been assigned
            if (lookObj != null)
            {
                animator.SetLookAtWeight(1);
                animator.SetLookAtPosition(lookObj.position);
            }
        }
    }
}
