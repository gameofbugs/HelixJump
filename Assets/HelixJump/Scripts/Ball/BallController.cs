using UnityEngine;

public class BallController : MonoBehaviour
{
    private Rigidbody ball;

    [Header("Movement Settings")]
    public float speed=2;
    public float jumpHeight = 1.5f;

    [HideInInspector] public bool isGameOver = false;
    private bool isMovingDown = true;
    private Vector3 ballInitialPos;
    public AudioManger AudioManger;
    public HelixSingleStack helix;

    private void Start()
    {
        ball = GetComponent<Rigidbody>();
        ballInitialPos = transform.position; // ensure it has initial position
    }

    private void Update()
    {
        if (isGameOver) return;
        if (speed==2||speed<3)
        {
            speed += helix.GetDangerElementsNo() / 4;
        }
        else
        {
            speed = 3;
        }
      
        float moveStep = speed * Time.deltaTime;

        if (isMovingDown)
        {
            transform.Translate(Vector3.down * moveStep);
        }
        else if (transform.position.y < ballInitialPos.y + jumpHeight)
        {
            transform.Translate(Vector3.up * moveStep);
        }
        else
        {
            isMovingDown = true;
            ballInitialPos = transform.position;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isGameOver) return;

        switch (collision.gameObject.tag)
        {
            case "Safe":
                isMovingDown = false;
                ballInitialPos = transform.position;
                AudioManger.SFX(AudioManger.ball);
                break;

            case "Danger":
                isGameOver = true;
                AudioManger.SFX(AudioManger.ball);
                GameManager.Instance.GameOver();
                ball.linearVelocity = Vector3.zero; // better than linearVelocity
                break;
        }
    }

}
    /*using UnityEngine;

public class BallController : MonoBehaviour
{
    private Rigidbody ball;
    public float speed = 5f;
    public float jumpHeight = 1.5f;

    private Vector3 ballInitialPos;
    [HideInInspector] public bool isGameOver = false;
    private bool isMovingDown = true;

    void Start()
    {
        ball = GetComponent<Rigidbody>();
        ball.useGravity = false;
    }
    void Update()
    {
        if (isGameOver) return; //if  Game over rest will not execute

        if (isMovingDown)
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
        }
        else if (transform.position.y < ballInitialPos.y + jumpHeight)
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
        else
        {
            isMovingDown = true;
            ballInitialPos = transform.position;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (isGameOver) return;  //if  Game over rest will not execute
        if (collision.gameObject.CompareTag("Safe"))
        {
            isMovingDown = false;
            ballInitialPos = transform.position;
        }
        else if (collision.gameObject.CompareTag("Danger"))
        {
            isGameOver = true;
            GameManager.Instance.GameOver();
            ball.linearVelocity = Vector2.zero;
        }
    }
}
*/

