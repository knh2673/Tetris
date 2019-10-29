using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCtrlMgr : MonoBehaviour
{
    [SerializeField] private GameObject[] prefebs = new GameObject[7];

    [SerializeField] private WaitForSeconds waitSec;

    private GameObject block;
    private bool[,] tetrisPanel;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();

        CreateBlock();
        
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

    private void Initialize()
    {
        waitSec = new WaitForSeconds(2.0f);

        tetrisPanel = TetrisMgr.Instance.TetrisPanel;

        tetrisPanel[0, 0] = true;
    }

    private void CreateBlock()
    {
        block = Instantiate<GameObject>(prefebs[Random.Range(0,7)]);

        block.transform.position = new Vector3(-0.01f, 19.0f, 5.0f);
        
        for(int i=0; i < block.transform.childCount; ++i)
        {
            Transform _tr = block.transform.GetChild(i);

            tetrisPanel[(int)_tr.position.z, (int)_tr.position.y] = true;
        }
    }

    IEnumerator PullDownBlock()
    {
        yield return waitSec;
    }
}
