using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCtrlMgr : MonoBehaviour
{
    [SerializeField] private WaitForSeconds waitSec;

    private GameObject block;

    // Start is called before the first frame update
    void Start()
    {
        waitSec = new WaitForSeconds(2.0f);

    }

    // Update is called once per frame
    void Update()
    {
        InputKey();
    }

    private void InputKey()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.Log("left");
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log("right");
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Debug.Log("Up");
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Debug.Log("Down");
        }
    }

    IEnumerator PullDownBlock()
    {
        yield return waitSec;
    }
}
