using UnityEngine;

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

            if (Input.GetButton("Jump"))
            {
                direction = Vector3.up * jumpForce;
                if(playerJump != null)
                {
                    playerJump.Play();
                }
            }
        }

        character.Move(direction * Time.deltaTime);

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Obstacle"))
        {
            GameManager.Instance.GameOver();
        }
        else if (other.gameObject.tag == "Scoring")
        {

            FindObjectOfType<GameManager>().IncreaseScore();
        }
    }
}
