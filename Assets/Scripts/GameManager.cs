using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject ballPrefab;
    public GameObject playerPrefab;
    public GameObject bonusPrefab;
    public Text scoreText;
    public Text ballsText;
    public Text levelText;
    public Text highScoreText;

    public GameObject panelMenu;
    public GameObject panelPlay;
    public GameObject panelLevelCompleted;
    public GameObject panelGameOver;
    public GameObject canvas;

    public GameObject[] levels;
    public Bonus[] bonuses;

    public BallFactory BallFactory { get; private set; }
    public BonusFactory BonusFactory { get; private set; }

    public static GameManager Instance { get; private set; }

    public enum State
    {
        MENU,
        INIT,
        PLAY,
        LEVELCOMPLETED,
        LOADLEVEL,
        GAMEOVER
    }
    State _state;

    GameObject _currentBall;
    GameObject _currentLevel;
    bool _isSwitchingState;
    bool _levelCompleted;

    private int _score;

    public int Score
    {
        get { return _score; }
        set
        {
            _score = value;
            scoreText.text = "SCORE: " + _score;
        }
    }

    private int _level;

    public int Level
    {
        get { return _level; }
        set
        {
            _level = value;
            levelText.text = "LEVEL: " + _level;
        }
    }

    private int _lives;

    public int Lives
    {
        get { return _lives; }
        set
        {
            _lives = value;
            ballsText.text = "BALLS: " + _lives;
        }
    }


    public void PlayClicked()
    {
        SwitchState(State.INIT);
    }

    public void CreateBall()
    {
        if (BallFactory.CountBalls == 0)
        {
            GameManager.Instance.Lives--;
            if (Lives > 0)
            { 
                BallFactory.CreateBall(Vector3.zero);
            }
        }
    }

    void Start()
    {
        Instance = this;
        SwitchState(State.MENU);
    }

    private void Awake()
    {
        bonuses = new Bonus[2] { new MultiBall(2), new MultiBall(3) };
    }

    public void SwitchState(State newState, float delay = 0)
    {
        StartCoroutine(SwitchDelay(newState, delay));
    }

    IEnumerator SwitchDelay(State newState, float delay)
    {
        _isSwitchingState = true;
        yield return new WaitForSeconds(delay);

        EndState();
        _state = newState;
        BeginState(newState);
        _isSwitchingState = false;
    }

    void BeginState(State newState)
    {
        switch (newState)
        {
            case State.MENU:
                Cursor.visible = true;
                highScoreText.text = "HIGHSCORE: " + PlayerPrefs.GetInt("highscore");
                panelMenu.SetActive(true);
                break;
            case State.INIT:
                BallFactory = new BallFactory(ballPrefab);
                BallFactory.OnBallDestroyed += CreateBall;
                BonusFactory = new BonusFactory();
                Cursor.visible = false;
                panelPlay.SetActive(true);
                Score = 0;
                Level = 0;
                Lives = 3;
                if (_currentLevel != null)
                {
                    Destroy(_currentLevel);
                }
                Instantiate(playerPrefab);
                SwitchState(State.LOADLEVEL);
                break;
            case State.PLAY:
                break;
            case State.LEVELCOMPLETED:
                BallFactory.Dispose();
                BonusFactory.Dispose();
                Destroy(_currentLevel);
                Level++;
                panelLevelCompleted.SetActive(true);
                SwitchState(State.LOADLEVEL, 2f);
                break;
            case State.LOADLEVEL:
                if (Level >= levels.Length)
                    SwitchState(State.GAMEOVER);
                else
                {
                    BallFactory.CreateBall(Vector3.zero);
                    _currentLevel = Instantiate(levels[Level]);
                    SwitchState(State.PLAY);
                }
                break;
            case State.GAMEOVER:
                if (Score > PlayerPrefs.GetInt("higherscore"))
                {
                    PlayerPrefs.SetInt("higherscore", Score);
                }
                panelGameOver.SetActive(true);
                break;
        }
    }

    void Update()
    {
        switch (_state)
        {
            case State.MENU:
                break;
            case State.INIT:
                break;
            case State.PLAY:
                if (Lives <= 0)
                {
                    SwitchState(State.GAMEOVER);
                }

                if (_currentLevel != null && _currentLevel.transform.childCount == 0 && !_isSwitchingState)
                {
                    SwitchState(State.LEVELCOMPLETED);
                }
                break;
            case State.LEVELCOMPLETED:
                break;
            case State.LOADLEVEL:
                break;
            case State.GAMEOVER:
                if (Input.anyKeyDown)
                    SwitchState(State.MENU);
                break;
        }
    }

    void EndState()
    {
        switch (_state)
        {
            case State.MENU:
                panelMenu.SetActive(false);
                break;
            case State.INIT:
                break;
            case State.PLAY:
                break;
            case State.LEVELCOMPLETED:
                panelLevelCompleted.SetActive(false);
                break;
            case State.LOADLEVEL:
                break;
            case State.GAMEOVER:
                panelPlay.SetActive(false);
                panelGameOver.SetActive(false);
                break;
        }
    }
}
