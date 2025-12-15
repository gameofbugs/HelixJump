using System.Collections.Generic;
using UnityEngine;

public class HelixSingleStack : MonoBehaviour
{
    public List<GameObject> Stack = new List<GameObject>(); //list of 8 parts of circle
    public Material danger;
    public Material safe;
    public bool isFirstStack = false;
    public Animator animator;
    public void ReadyStack()
    {
        foreach (GameObject g in Stack)
        {
            g.SetActive(true);
            g.GetComponent<Renderer>().material = safe;
            g.tag = "Safe";
        }
        int emptyStack = Random.Range(0, Stack.Count);
        Stack[emptyStack].SetActive(false);
        Stack[emptyStack].tag = "Untagged";
        if (isFirstStack)   //if it is first stack All matrial is Safe
        {
            Stack[emptyStack].SetActive(false);
            return;
        }
        else                    //else mix of safe and danger
        {
            int level = GetDangerElementsNo();

            HashSet<int> usedIndices = new HashSet<int> { emptyStack };
            for (int i = 0; i < level; i++)
            {
                int dangerIndex;
                do
                {
                    dangerIndex = Random.Range(0, Stack.Count);
                } while (usedIndices.Contains(dangerIndex));
                usedIndices.Add(dangerIndex);
                Stack[dangerIndex].GetComponent<Renderer>().material = danger;
                Stack[dangerIndex].tag = "Danger";
            }
        }
    }
    public int GetDangerElementsNo()
    {
        int score = GameManager.Instance.helixScore / 5;
        int dangerCount = score /8;
        int dangerNo;
        if (dangerCount == 0 || dangerCount == 1)
        {
            dangerNo = 1;
        }
        else if (dangerCount == 2)
        {
            dangerNo = 2;
        }
        else if (dangerCount == 3)
        {
            dangerNo = 3;
        }
        else
        {
            dangerNo = 4;
        }
        return dangerNo;
    }
}
        