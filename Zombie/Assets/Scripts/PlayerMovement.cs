using UnityEditor;
using UnityEditor.Build.Player;
using UnityEngine;
using UnityEngine.UIElements;



public class PlayerMovement : MonoBehaviour
{
    private static readonly int hashMove = Animator.StringToHash("Move");

    private Rigidbody rb;
    private PlayerInput input;
    private Animator playerAnimator;

    public float moveSpeed = 5f;
    public float rotateSpeed = 180f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        input = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        //È¸Àü
        //input.Roate * rotateSpeed *Time.deltaTime
        var distance = input.Move * moveSpeed * Time.deltaTime;
     
        var rotation = Quaternion.Euler(0f, input.Roate * rotateSpeed * Time.deltaTime, 0f);

        rb.MoveRotation(rb.rotation * rotation);
        rb.MovePosition(transform.position + distance * transform.forward);

        //playerAnimator.SetFloat(10, input.Move);
        playerAnimator.SetFloat(hashMove, input.Move);
    }
}
