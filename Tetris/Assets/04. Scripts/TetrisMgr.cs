/////////////////////////////////////////////////////////////////////////////////////////////////////////
// TetrisMgr.cs
/////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/////////////////////////////////////////////////////////////////////////////////////////////////////////
// Tetris Manager Class
//
// This class manages tetris panel and UI. 
//
// Changes
// - 191029 Namhun Kim
//   Create class.
// - 191030 Namhun Kim
//   Linking with UI
/////////////////////////////////////////////////////////////////////////////////////////////////////////
public class TetrisMgr : MonoBehaviour
{
    private static TetrisMgr instance = null; // instance.
    public static TetrisMgr Instance
    {
        get
        {
            return instance;
        }
    }

    private Transform blockCtrlMgr;

    private bool[,] tetrisPanel = new bool[10, 22];
    public bool[,] TetrisPanel
    {
        get
        {
            return tetrisPanel;
        }
    }

    [SerializeField]private int level = 1; // level
    public int Level
    {
        get
        {
            return level;
        }
    }

    private bool isGameOver; // game over variable
    public bool IsGameOver
    {
        get
        {
            return isGameOver;
        }
        set
        {
            isGameOver = value;
        }
    }

    private int score; // score
    private int combo; // combo

    [SerializeField] private Text scorePanel; // score ui
    [SerializeField] private Text levelPanel; // level ui
    [SerializeField] private Text comboPanel; // combo ui
    [SerializeField] private GameObject gameOverPanel; // game over ui

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // private void Initialize()
    //
    // This method initialize member variables.
    //
    // @param    void
    // @return   void
    //
    // Changes
    // 191029 Namhun Kim
    // - Create method.
    // 191030 Namhun Kim
    // - Add display initialize method.
    private void Initialize()
    {
        blockCtrlMgr = GameObject.Find("BlockCtrlMgr").GetComponent<Transform>();
        if (blockCtrlMgr == null)
        {
            Debug.Log("TetrisMgr.cs");
            Debug.Log("void Initialize()");
            Debug.Log("Can't find BlockCtrlMgr game object.");
        }

        score = 0;
        combo = 0;

        isGameOver = false;
        gameOverPanel.SetActive(false);

        DisplayScore();
        DisplayLevel();
        DisplayCombo();
    }

    // public void CheckLineFull()
    //
    // This method checks fully line.
    // And clear line.
    //
    // @param    void
    // @return   void
    //
    // Changes
    // 191029 Namhun Kim
    // - Create method
    // 191030 Namhun Kim
    // - Add combo, score, multiplier system
    public void CheckLineFull()
    {
        bool _isFull = true;
        bool _isComboSuccess = false;
        int _multiplier = 0;

        for(int j = 0; j < 20; ++j)
        {
            for(int i = 0; i < 10; ++i)
            {
                if (!tetrisPanel[i, j])
                {
                    _isFull = false;
                    break;
                }
            }

            if (_isFull)
            {
                ClearLine(j--);
                _isComboSuccess = true;
                ++_multiplier;
            }
            else
                _isFull = true;
        }

        if (_isComboSuccess)
        {
            ++combo;
        }
        else
        {
            combo = 0;
        }

        CalculateScore(_multiplier);
        DisplayCombo();
    }

    // private void ClearLine(int _line)
    //
    // This method clears tetris panel line.
    // After clear line, pull down all blocks once.
    //
    // @param    int line   Line that want to delete.
    // @return   void
    //
    // Changes
    // 191029 Namhun Kim
    // - Create class.
    private void ClearLine(int _line)
    {
        for(int i = blockCtrlMgr.childCount - 1; i >= 0; --i)
        {
            Transform _tr = blockCtrlMgr.GetChild(i);

            if (_tr.position.y > _line - 0.5f && _tr.position.y < _line + 0.5f)
            {
                Destroy(_tr.gameObject);
            }
            else if(_tr.position.y >= _line + 0.5f)
            {
                _tr.position += Vector3.down;
            }
        }

        for (int j = _line; j < 20; ++j)
        {
            for(int i = 0; i < 10; ++i)
            {
                tetrisPanel[i, j] = tetrisPanel[i, j + 1];
            }
        }

    }

    // private void CalculateScore(int multiplier)
    //
    // This method calculates score with multiplier and combo.
    //
    // @param    int multiplier   count that clear line at one time.
    // @return   void
    //
    // Changes
    // 191030 Namhun Kim
    // - Create class.
    private void CalculateScore(int _multiplier)
    {
        score += 10 * _multiplier * (combo + 1);

        CalculateLevel();
        
        DisplayScore();
        DisplayLevel();
    }

    // private void CalculateLevel()
    //
    // This method calculates level with score.
    //
    // @param    void
    // @return   void
    //
    // Changes
    // 191030 Namhun Kim
    // - Create class.
    private void CalculateLevel()
    {
        while(score > 500 * level * level)
        {
            if (level > 20) break;

            ++level;
        }
    }

    // private void DisplayScore()
    //
    // This method write score on text ui.
    //
    // @param    void
    // @return   void
    //
    // Changes
    // 191030 Namhun Kim
    // - Create class.
    private void DisplayScore()
    {
        scorePanel.text = "Score : " + score;
    }

    // private void DisplayLevel()
    //
    // This method write level on text ui.
    //
    // @param    void
    // @return   void
    //
    // Changes
    // 191030 Namhun Kim
    // - Create class.
    private void DisplayLevel()
    {
        levelPanel.text = "Level : " + level;
    }

    // private void DisplayCombo()
    //
    // This method write combo on text ui.
    //
    // @param    void
    // @return   void
    //
    // Changes
    // 191030 Namhun Kim
    // - Create class.
    private void DisplayCombo()
    {
        comboPanel.text = "Combo : " + combo;
    }

    // public void GameOver()
    //
    // This method displays game over ui.
    //
    // @param    void
    // @return   void
    //
    // Changes
    // 191030 Namhun Kim
    // - Create class.
    public void GameOver()
    {
        gameOverPanel.SetActive(true);
    }
}
