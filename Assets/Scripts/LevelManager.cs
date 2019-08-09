using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private const float time_Before_Start = 3.0f;

    private static LevelManager instance;

    public static LevelManager Instance{ get { return instance; } }

    public GameObject pauseMenu, joystick, endMenu;
    public Button pauseBtn, boostBtn;
    public Transform respawnPoint;
    public Text timerText;
    private GameObject player;

    private float startTime;
    private float levelDuration;
    public float silverTime;
    public float goldTime;


    private void Start()
    {
        instance = this;
        pauseMenu.SetActive(false);
        endMenu.SetActive(false);
        startTime = Time.time;
        player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = respawnPoint.position;
    }

    private void Update()
    {
        if (Time.time - startTime < time_Before_Start)
        {
            return;
        }
        Motor.play = true;
        if (player.transform.position.y < -10.0f)
        {
            Death();
        }

        levelDuration = Time.time - (startTime + time_Before_Start);
        string minutes = ((int)levelDuration / 60).ToString("00");
        string seconds = (levelDuration % 60).ToString("00.00");

        timerText.text = minutes + ":" + seconds;
    }

    public void TogglePauseMenu()
    {
        joystick.GetComponent<VirtualJoystick>().enabled = pauseMenu.activeSelf;
        boostBtn.interactable = pauseMenu.activeSelf;
        pauseBtn.interactable = pauseMenu.activeSelf;
        Time.timeScale = (!pauseMenu.activeSelf) ? 0 : 1;
        pauseMenu.SetActive(!pauseMenu.activeSelf);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void Victory()
    {
        foreach (Transform t in endMenu.transform.parent)
        {
            t.gameObject.SetActive(false);
        }

        Time.timeScale = 0;
        string minutes = ((int)levelDuration / 60).ToString("00");
        string seconds = (levelDuration % 60).ToString("00.00");
        endMenu.SetActive(true);
        endMenu.transform.GetChild(2).GetComponent<Text>().text = minutes + ":" + seconds;

        if (levelDuration < goldTime)
        {
            GameManager.Instance.currency += 50;
            endMenu.transform.GetChild(2).GetComponent<Text>().color = new Color((float)254 / 255, (float)211 / 255, (float)62 / 255);
        }
        else if (levelDuration < silverTime)
        {
            GameManager.Instance.currency += 25;
            endMenu.transform.GetChild(2).GetComponent<Text>().color = Color.gray;
        }
        else
        {
            GameManager.Instance.currency += 10;
            endMenu.transform.GetChild(2).GetComponent<Text>().color = new Color((float)112 / 255, (float)86 / 255, (float)2 / 255);
        }
        GameManager.Instance.Save();

        string saveString = "";
        LevelData level = new LevelData(SceneManager.GetActiveScene().buildIndex);
        saveString += (level.BestTime > levelDuration || level.BestTime == 0.0f) ? levelDuration.ToString() : level.BestTime.ToString();
        saveString += "&";
        saveString += silverTime.ToString();
        saveString += "&";
        saveString += goldTime.ToString();
        PlayerPrefs.SetString(SceneManager.GetActiveScene().buildIndex.ToString(), saveString);
    }

    public void Death()
    {
        player.transform.position = respawnPoint.position;
        Rigidbody rigid = player.GetComponent<Rigidbody>();
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }
}
