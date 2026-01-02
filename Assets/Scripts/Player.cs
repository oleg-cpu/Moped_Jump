using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    private CharacterController character;
    private Vector3 direction;
    private AudioSource playerJump;
    public float gravity = 9.81f * 2f;
    public float jumpForce = 5f;

    private void Awake()
    {

        character = GetComponent<CharacterController>();
        playerJump = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {

        direction = Vector3.zero;
    }

    private void Update()
    {

        direction += Vector3.down * gravity * Time.deltaTime;

        if (character.isGrounded)
        {

            direction = Vector3.down;

            if (CheckJumpInput())
            {
                direction = Vector3.up * jumpForce;
                if (playerJump != null)
                {
                    playerJump.Play();
                }
            }
        }

        character.Move(direction * Time.deltaTime);

    }


    private bool CheckJumpInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                {
                    return false;
                }
                return true;
            }
        }
        else if (Input.GetButtonDown("Jump"))
        {
            return true;
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Obstacle"))
        {
            GameManager.Instance.GameOver();
        }
        else if (other.CompareTag("Scoring"))
        {
            GameManager.Instance.IncreaseScore();
        }
    }
}
