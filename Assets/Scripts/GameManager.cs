using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Diagnostics.CodeAnalysis;
using System.Collections;
using Unity.VectorGraphics;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using Unity.Hierarchy;
public class GameManager : MonoBehaviour
{
    public GameObject[] gameObjects;
    [SerializeField] private GameObject[] number0;
    private int target;
    public bool isCanvasReady = false;
    public int score;
    private int highScore=0;
    private int playerScore;
    private int timerStart = 0;
    private float spawnRate = 0.75f;
    private float ySpawnPoint = 60.0f;
    private float zSpawnPoint = 43.09f;
    private float elapsedTime = 0;
    public bool isGameActive = false;
    public float[] xSpawnPoints = new float[4];
    private float gateSpeed = 0.125f;
    private float displacementX = 5f;

    public Player playerScript;

    [SerializeField] private RectTransform topBar;
    [SerializeField] private TextMeshPro targetText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI finalScore;
    [SerializeField] private TextMeshProUGUI targetTextStationary;
    [SerializeField] private TextMeshPro scoreText;
    [SerializeField] private TextMeshProUGUI plusSign;
    [SerializeField] private TextMeshProUGUI minusSign;
    [SerializeField] private TextMeshProUGUI multiplySign;
    [SerializeField] private TextMeshProUGUI moduloSign;
    [SerializeField] private TextMeshProUGUI winningText;
    [SerializeField] private TextMeshProUGUI losingText;

    [SerializeField] private GameObject dungeonGate;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject volumeObject;

    [SerializeField] private Button startGameButton;
    [SerializeField] private Button restartGameButton;

    [SerializeField] private AudioSource menuMusic;
    [SerializeField] private AudioSource gameMusic;
    [SerializeField] private AudioMixer masterMixer;
    [SerializeField] private AudioSource sfxMusic;
    [SerializeField] private AudioClip victorySfx;
    [SerializeField] private AudioClip losingSfx;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Canvas.ForceUpdateCanvases();
        UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(topBar);
        xSpawnPoints[0] = plusSign.transform.position.x+displacementX;
        xSpawnPoints[1] = minusSign.transform.position.x;
        xSpawnPoints[2] = multiplySign.transform.position.x;
        xSpawnPoints[3] = moduloSign.transform.position.x-displacementX;
        highScore = PlayerPrefs.GetInt("SavedHighScore", 0);
    }
    // Update is called once per frame
    void Update()
    {
        UpdateScore();
        if (isGameActive)
        {
            elapsedTime += Time.deltaTime;
        }
        if (elapsedTime > timerStart)
        {
            ySpawnPoint -= gateSpeed;
            DungeonGateMovement(); //Abstraction
        }
    }

    public void isGameOver()
    {
        if (!isGameActive && playerScript.dungeonCollided)
        {
            StopCoroutine("SpawnNumbers");
            topBar.gameObject.SetActive(false);
            player.gameObject.SetActive(false);
            dungeonGate.gameObject.SetActive(false);
            menuMusic.gameObject.SetActive(true);
            gameMusic.gameObject.SetActive(false);
            playerScore = 500 - Mathf.Abs(target - score);
            if(playerScore == 500)
            {
                winningText.gameObject.SetActive(true);
                sfxMusic.PlayOneShot(victorySfx);
            }
            else
            {
                losingText.gameObject.SetActive(true);
                sfxMusic.PlayOneShot(losingSfx);
            }
            if (playerScore > highScore)
            {
                highScore = playerScore;
                SaveHighScore();
            }
                restartGameButton.gameObject.SetActive(true);
            finalScore.text = "Score: " + playerScore.ToString();
            highScoreText.text = "High Score: " + highScore.ToString();
            highScoreText.gameObject.SetActive(true);
            finalScore.gameObject.SetActive(true);
        }
    }
    private void SaveHighScore()
    {
        PlayerPrefs.SetInt("SavedHighScore", highScore);
        PlayerPrefs.Save();
    }
    private void DungeonGateMovement()
    {
        if (!playerScript.dungeonCollided)
        {
        dungeonGate.transform.Translate(0, -gateSpeed, 0);
        }
        if (dungeonGate.transform.position.y < 35)
        {
            targetTextStationary.gameObject.SetActive(false);
        }
        if (dungeonGate.transform.position.y < 0)
        {
            StopCoroutine("SpawnNumbers");
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void UpdateScore()
    {
        scoreText.text = score.ToString();
    }
    private Vector3 SpawnPosition(int range)
    {
        int ind = Random.Range(0, range);
        if (elapsedTime < timerStart)
        {
            Vector3 posi = new Vector3(xSpawnPoints[ind], ySpawnPoint, zSpawnPoint);
            return posi;
        }
        else
        {
            Vector3 posi = new Vector3(xSpawnPoints[ind], ySpawnPoint, zSpawnPoint);
            return posi;
        }
    }
    IEnumerator SpawnNumbers()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnRate);
            int index = Random.Range(0, gameObjects.Length);
            if(index == 0)
            { 
                Instantiate(gameObjects[index], SpawnPosition(3), gameObjects[index].transform.rotation);
            }
            else
                Instantiate(gameObjects[index], SpawnPosition(4), gameObjects[index].transform.rotation);
        }
    }
    public void StartGame()
    {
        isGameActive = true;
        volumeObject.gameObject.SetActive(false);
        topBar.gameObject.SetActive(true);
        timerStart = Random.Range(60,121);
        startGameButton.gameObject.SetActive(false);
        player.gameObject.SetActive(true);
        target = Random.Range(-500, 501);
        targetTextStationary.gameObject.SetActive(true);
        targetText.text = "Target: " + target.ToString();
        targetTextStationary.text = "Target: " + target.ToString();
        score = 0;
        menuMusic.gameObject.SetActive(false);
        gameMusic.gameObject.SetActive(true);
        StartCoroutine("SpawnNumbers");
    }

    public void AudioController(float volume)
    {
        float decibalVolume = Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20;
        masterMixer.SetFloat("MasterVolume",decibalVolume);
    }
}
