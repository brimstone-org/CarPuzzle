using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{

    public static GameLogic instance { get; private set; }

    public GameObject winPanel;
    public GameObject losePanel;

    [SerializeField]
    private float stepDuration;

    public float StepDuration
    {
        get { return stepDuration; }
        set
        { //stepDuration = value; 
            StartCoroutine(LerpDuration(value));
        }
    }

    public delegate void StepEvent();

    public event StepEvent stepEvent;

    public delegate void StartGameEvent();

    public event StartGameEvent startGameEvent;

    public delegate void WinGameEvent();

    public event WinGameEvent winGameEvent;

    public delegate void ResetGameEvent();

    public event ResetGameEvent resetGameEvent;

    public float stepTimeCounter = 0;

    public bool playing { get; private set; }

    public CarScript[] cars;

    public BaseTile[,] tiles;

    public Objective[] objectives;

    public bool usedInHelpScreen = false;

    void Awake()
    {
        Application.targetFrameRate = 60;
        Time.timeScale = 1;
        instance = this;
        stepTimeCounter = 0;
        playing = false;
    }

    void Start()
    {
        winGameEvent += WinLogic;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!playing)
            return;

        stepTimeCounter += Time.fixedDeltaTime;

        if (stepTimeCounter >= stepDuration)
        {
            stepEvent();
            //Debug.Log ("Step Event Triggered");
            stepTimeCounter = 0;
        }

    }

    public void StartPlaying()
    {
        stepTimeCounter = 0;
        playing = true;
        Debug.Log("GAME LOGIC ON GAMEPLAY START");
        startGameEvent();
    }

    public void GameLost(float time)
    {
        //Debug.Log("GAME OVER");
        //playing = false;
        //losePanel.SetActive(true);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        StartCoroutine(Lose(time));
    }

    public void CheckGameState()
    {
        Debug.Log("GameStateCheck");
        bool finished = true;

        for (int i = 0; i < cars.Length; i++)
        {
            if (cars[i] == null)
                continue;
            if (cars[i].DestinationReached == false || cars[i].remainingObjectives > 0)
                finished = false;
			
        }

        if (finished)
        {
            playing = false;
            Debug.Log("FINISHED");
            Win();
        }
        bool lost = false;
        for (int i = 0; i < cars.Length; i++)
        {
            if (cars[i] == null)
                continue;
            if (cars[i].DestinationReached == true && cars[i].remainingObjectives > 0)
            {
                lost = true;
                Debug.Log("LOSING");
            }
        }
        if (lost)
            GameLost(1);
        //StartCoroutine(Lose(2));

    }


    void Win()
    {

        if (usedInHelpScreen)
            winGameEvent -= WinLogic;

        winGameEvent();

    }

    void OnDisable()
    {
        winGameEvent -= WinLogic;
    }

    void WinLogic()
    {
        winPanel.SetActive(true);
        Time.timeScale = 1;

        int levelCategory = PlayerPrefs.GetInt("Difficulty");
        int level = PlayerPrefs.GetInt("Level");
        if (PlayerPrefs.GetInt(levelCategory + "Skipped" + level) == 1)
            PlayerPrefs.SetInt(levelCategory + "Skipped" + level, 0);
        PlayerPrefs.SetInt(levelCategory + "Completed" + PlayerPrefs.GetInt("Level"), 1);

        int maxLevel = PlayerPrefs.GetInt(levelCategory + "maxCompleted");
        if (level > maxLevel)
        {
            maxLevel = level;
            PlayerPrefs.SetInt(levelCategory + "maxCompleted", maxLevel);
            //if (OneSignalManager.instance != null)
            //{
            //    OneSignalManager.instance.SendTag("maxLevelInPack" + levelCategory, maxLevel.ToString());
            //}
        }

        if (GPGSManager.instance != null)
            GPGSManager.instance.CheckPackCompletedAchievement(levelCategory, PlayerPrefs.GetInt("LevelsIn" + levelCategory));

        if (AdsManager.Instance != null)
        {
            AdsManager.Instance.UpdateAds();
        }

        if (Tedrasoft_Rate.RatePlugin.instance != null & (AdsManager.Instance.LevelsCount % AdsManager.Instance.ShowRate != 0 || AdsManager.Instance.AdsDisabled == true))
        {
            Tedrasoft_Rate.RatePlugin.instance.UpdateRate();
        }
    }

    IEnumerator Lose(float seconds)
    {
        Time.timeScale = 1;
        //Debug.Log("GAME OVER");
        playing = false;
        yield return new WaitForSeconds(seconds);
        losePanel.SetActive(true);
        if (AdsManager.Instance != null)
        {
            AdsManager.Instance.UpdateAds();
        }
    }

    float nextDuration;

    IEnumerator LerpDuration(float duration)
    {
        Time.timeScale = duration;
        yield return null;
    }

    public void CarWait(int steps, GameObject car)
    {
        StartCoroutine(WaitForEntry(steps, car));
    }

    IEnumerator WaitForEntry(int steps, GameObject car)
    {
        car.SetActive(false);
        yield return new WaitForSeconds(steps * StepDuration);
        car.SetActive(true);
    }

    public void Reset()
    {
        for (int i = 0; i < cars.Length; i++)
        {
            if (cars[i] == null)
                continue;
            cars[i].gameObject.SetActive(true);
        }
        //StepDuration = 1;
        Time.timeScale = 1;
        Debug.Log("RESET");
        losePanel.SetActive(false);
        resetGameEvent();
    }

}
