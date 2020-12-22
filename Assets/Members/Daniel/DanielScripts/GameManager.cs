using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Levels))]
public class GameManager : MonoBehaviour
{
    //Public Variables
    public GameObject playerPrefab;
    public GameObject[] enemyPrefab;
    public GameObject pauseText;
    public GameObject textBox;
    public GameObject holidayCard;
    public Effect[] hearts;
    public AudioManager audioManager;

    private GameObject player;
    private GameObject enemies;
    //private List<bool> enemyStatus;
    private int enemyCount = 0;
    private Levels levelData;
    private bool paused = false;
    private bool coroutineActive = false;
    private int stageNum = 0;
    private int health;

    // Parameters
    private int MAX_HEALTH = 9;
    private int MAX_STAGE = 2;

    // Start is called before the first frame update
    void Start()
    {
        player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        audioManager.Play("bgm");
        levelData = GetComponent<Levels>();

        pauseText.SetActive(false);
        holidayCard.SetActive(false);
        NextLevel();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !coroutineActive && enemyCount != 0)
        {
            if (paused)
                StartAllObjects();
            else
                StopAllObjects();
        }
        else if (Input.GetKeyDown(KeyCode.A))
            StartCoroutine(DisplayBox(2.5f, "Level 1"));
        else if (Input.GetKeyDown(KeyCode.B))
            SetHealth(Random.Range(1, 9));
        else if (Input.GetKeyDown(KeyCode.C))
            GameOver(false);
        else if (Input.GetKeyDown(KeyCode.D))
            GameOver(true);
        else if (Input.GetKeyDown(KeyCode.E))
            NextLevel();
        else if (Input.GetKeyDown(KeyCode.F))
            audioManager.Play("clap");
        else if (Input.GetKeyDown(KeyCode.G))
            audioManager.Play("zombieDeath");
        else if (Input.GetKeyDown(KeyCode.H))
            audioManager.Play("zombieHit");
        else if (Input.GetKeyDown(KeyCode.I))
            audioManager.Play("playerHit");
    }

    public void SetHealth(int hp)
    {
        if (hp == health)
            return;
        else if (hp > MAX_HEALTH)
            hp = MAX_HEALTH;

        for (int i = 0; i < MAX_HEALTH; i++)
            if (i > hp)
                hearts[i].ChangeVisibility(false);
            else
                hearts[i].ChangeVisibility(true);
    }

    public void EnemyKilled()
    {
        enemyCount--;
        if (enemyCount <= 0)
            NextLevel();
    }

    private void NextLevel()
    {
        stageNum++;
        if (stageNum > MAX_STAGE)
            GameOver(true);
        else
            StartCoroutine(StartLevel(stageNum));
    }

    private IEnumerator StartLevel(int level)
    {
        audioManager.Play("bell");
        audioManager.Play("playerSpawn");
        StartCoroutine(DisplayBox(2.5f, "Level " + level));
        yield return new WaitForSeconds(3.25f);
        GenerateEnemies(stageNum);
    }

    private void StartAllObjects()
    {
        player.SetActive(true);
        enemies.SetActive(false);
        paused = false;
        pauseText.SetActive(false);
    }

    private void StopAllObjects()
    {
        player.SetActive(false);
        enemies.SetActive(false);
        paused = true;
        pauseText.SetActive(true);
    }

    private void GenerateEnemies(int level)
    {
        enemies = new GameObject("Enemies");
        List<(int, Vector2)> data = levelData.GetLevelData(level);
        enemyCount = data.Count;
        for (int i = 0; i < data.Count; i++)
        {
            //enemyStatus.Add(true);
            GameObject enemy = Instantiate(enemyPrefab[data[i].Item1], data[i].Item2, Quaternion.identity, enemies.transform);
            // Assign an ID???
        }
        audioManager.Play("zombieSpawn");
    }

    private IEnumerator DisplayBox(float duration, string message)
    {
        if (coroutineActive == true || duration < 0)
            yield break;
        else
            coroutineActive = true;

        textBox.GetComponent<Text>().text = message;
        holidayCard.SetActive(true);

        Vector3 centerInScreenSpace = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        Vector3 start = centerInScreenSpace + new Vector3(0, Screen.height, 0);
        Vector3 end = centerInScreenSpace;
        float t = 0;

        while (duration > 0)
        {
            holidayCard.transform.position = Vector3.Lerp(start, end, t);

            if (duration < 0.5f && end == centerInScreenSpace)
            {
                start = centerInScreenSpace;
                end = centerInScreenSpace + new Vector3(0, Screen.height, 0);
                t = 0;
            }
            t += Time.deltaTime * 2;
            duration -= Time.deltaTime;
            yield return null;
        }

        holidayCard.SetActive(false);
        coroutineActive = false;
    }

    private void GameOver(bool result)
    {
        Destroy(enemies);
        string closingMessage;
        if (result)
        {
            closingMessage = "You win!";
            audioManager.Play("win");
        }
        else
        {
            closingMessage = "You lose";
            audioManager.Play("lose");
        }
        StartCoroutine(DisplayBox(300, closingMessage));
    }
}
