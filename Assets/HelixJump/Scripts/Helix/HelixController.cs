using UnityEngine;

public class HelixController : MonoBehaviour
{
    public GameObject helix; //Refrence for Rotation
    public BallController ball; //Refrence for to know wether is GameOvered or Not


    private Vector2 initialPos;
    public float sensivity = 3;
    private bool isSliding;
   
    void Update()
    {
        Controls();
    }
    public void Controls()
    {
        if (ball.isGameOver) return;  //if  Game over rest will not execute
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                initialPos = touch.position;
                isSliding = true;
            }
            else if (touch.phase == TouchPhase.Moved && isSliding)
            {
                RotatePlayer(touch.position.x - initialPos.x);
                initialPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isSliding = false;
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            initialPos = Input.mousePosition;
            isSliding = true;
        }
        else if (Input.GetMouseButton(0) && isSliding)
        {
            RotatePlayer(Input.mousePosition.x - initialPos.x);
            initialPos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isSliding = false;
        }
    }
    public void RotatePlayer(float distance)
    {
            float rotation = -distance * sensivity;
            helix.transform.Rotate(0, rotation, 0);
    }

}
