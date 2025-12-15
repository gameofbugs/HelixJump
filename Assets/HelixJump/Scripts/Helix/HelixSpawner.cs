using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelixSpawner : MonoBehaviour
{
    public GameObject stack;   //refrence of the slice
    private List<GameObject> generatedStack = new List<GameObject>();  //list of genereted of slice(10) in start

    public int gapAmount = 2;  //gap between slice
    public  int lowestStack; //lowest stack number
    public GameObject ball;   //refrence of the ball wether the ball cleared the privious slice or not
    public HelixSingleStack helix;  //refrence for helix script whre it is attached to prefab used to get prefab components like animator ,methods,bool
    public AudioManger AudioManger;


    private Camera mainCamera; 
    private bool isMoving;
    private Vector3 cameraTargetPos;  
    private float cameraMoveStartTime;
    public float cameraMoveDuration = 0.2f;
    void Start()
    {
        mainCamera = Camera.main;
        SpawnStacks();
    }
    void Update()
    {
        CheckClearedStack();

        if (isMoving && mainCamera)
        {
            float time = (Time.time - cameraMoveStartTime) / cameraMoveDuration;
            if (time < 1f)
            {
                mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, cameraTargetPos, time);
            }
            else
            {
                mainCamera.transform.position = cameraTargetPos;
                isMoving = false;
            }
        }

    }

    public void SpawnStacks()      //spawn first 10 stacks or slice
    {
        ball.SetActive(true);
        for (int i = 0; i < 10; i++)
        {
            Vector3 spawnPos = new Vector3(0, -i * gapAmount, 0);
            if (i == 0)
            {
                helix.isFirstStack = true;
                helix.ReadyStack();
            }
            else
            {
                helix.isFirstStack = false;
                helix.ReadyStack();
            }

            GameObject stackGroup = Instantiate(stack, spawnPos, Quaternion.identity);
            generatedStack.Add(stackGroup);
            stackGroup.transform.SetParent(transform);
            isMoving = false;
        }
        lowestStack = -9 * gapAmount;
    }

    public void CheckClearedStack()    //check wether the ball crossed the previous stack
    {
        for (int i = generatedStack.Count - 1; i >= 0; i--)
        {
            if (generatedStack[i] && ball.transform.position.y < generatedStack[i].transform.position.y)
            {
                GameManager.Instance.AddScore(true);
                RemoveStack(i);
                AudioManger.SFX(AudioManger.stackDestroy);
                break;
            }
        }
    }
    public void RemoveStack(int index)   //remove the cleared stack using paramater or reffrence
    {
        GameObject remove = generatedStack[index];
        generatedStack.RemoveAt(index);
        if (remove)
        {
            StartCoroutine(DestroyAnimationsTime(remove));
           
            lowestStack -= gapAmount;
            helix.ReadyStack();
            GameObject newStack = Instantiate(stack, new(0, lowestStack, 0), Quaternion.identity);
            newStack.transform.SetParent(transform);
            generatedStack.Add(newStack);
            if (mainCamera)
            {
                cameraTargetPos = new(mainCamera.transform.position.x, mainCamera.transform.position.y - gapAmount, -4f);
                isMoving = true;
                cameraMoveStartTime = Time.time;
            }
        }
    }
    IEnumerator DestroyAnimationsTime(GameObject game)  //Coroutine used to wait game /executin for completing animations
    {
        HelixSingleStack removingStack=game.GetComponent<HelixSingleStack>();

        if(removingStack&&removingStack.animator)
        {
            removingStack.animator.SetBool("IsDestroyed", true);
        }
        yield return new WaitForSeconds(2.6f);
        Destroy(game);
    }
}
