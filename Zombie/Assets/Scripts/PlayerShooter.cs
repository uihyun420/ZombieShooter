using System.Diagnostics.Eventing.Reader;
using System.Security.Policy;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public static readonly int IdReload = Animator.StringToHash("Reload");
    public Gun gun;

    private Vector3 gunInitPosition;
    private Quaternion gunInitRotation;

    private Rigidbody gunRb;
    private Collider gunCollider;

    private PlayerInput input;
    private Animator animator;

    public Transform gunPivot;
    public Transform leftHandMount;
    public Transform rightHandMount;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();

        gunRb = gun.GetComponent<Rigidbody>();
        gunCollider = gun.GetComponent<Collider>();

        gunInitPosition = gun.transform.localPosition;
        gunInitRotation = gun.transform.localRotation;
    }

    private void Update()
    {
        if (input.Fire)
        {
            gun.Fire();
        }
        else if (input.Reload)
        {
            if (gun.Reload())
            {
                animator.SetTrigger(IdReload);
            }
        }
    }

    private void OnEnable()
    {
        gunRb.isKinematic = true;
        gunCollider.enabled = false;

        gunInitPosition = gun.transform.localPosition;
        gunInitRotation = gun.transform.localRotation;
    }

    private void OnDisable()
    {
        gunRb.linearVelocity = Vector3.zero;
        gunRb.angularVelocity = Vector3.zero;

        gunRb.isKinematic = false;
        gunCollider.enabled = true;

        //gunInitPosition = gun.transform.localPosition;
        //gunInitRotation = gun.transform.localRotation;
    }

    private void OnAnimatorIK(int layerIndex)
    {
        gunPivot.position = animator.GetIKHintPosition(AvatarIKHint.RightElbow); // 총이 먼저 포지션 잡고 이후 왼손 오른손

        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);

        animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandMount.position);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandMount.rotation);

        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);

        animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandMount.position);
        animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandMount.rotation);
    }
}
