using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Levels))]
public class GameManager : MonoBehaviour
{
    //Public Variables
    public GameObject playerPrefab;
    public GameObject[] enemyPrefab;
    public GameObject[] presentPrefab;
    public GameObject pauseText;
    public GameObject textBox;
    public GameObject holidayCard;
    public Effect[] hearts;
    public AudioManager audioManager;
    public GameObject summonAnimation;
    public GameObject explosionAnimation;
    public GameObject megaExplosionAnimation;
    public Tilemap tilemap;

    private Transform player;
    private GameObject enemies;
    private Camera mainCamera;
    private Levels levelData;
    private int enemyCount = 0;
    private bool paused = false;
    private bool coroutineActive = false;
    private bool followPlayer = true;
    private int stageNum = 0;
    private int health;

    // Parameters
    private int MAX_HEALTH = 9;
    private int MAX_STAGE = 2;
    private int STAGE_WIDTH = 10;
    private int STAGE_HEIGHT = 10;
    private int MIN_TIME = 10;
    private int MAX_TIME = 20;

    // Saves components accessed regularly
    void Start()
    {
        audioManager.Play("bgm");
        levelData = GetComponent<Levels>();
        mainCamera = Camera.main;
        pauseText.SetActive(false);
        holidayCard.SetActive(false);

        player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity).transform;
        NextLevel();
        GenerateItems(MIN_TIME, MAX_TIME);
    }

    // Currently used for testing purposes
    // TODO: Remove testing buttons
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
            audioManager.Play("bell");
        else if (Input.GetKeyDown(KeyCode.G))
            audioManager.Play("zombieDeath");
        else if (Input.GetKeyDown(KeyCode.H))
            audioManager.Play("zombieHit");
        else if (Input.GetKeyDown(KeyCode.I))
            audioManager.Play("playerHit");
        else if (Input.GetKeyDown(KeyCode.J))
            EnemyKilled(enemies.transform.GetChild(0));
    }

    // Update camera position to follow player
    void LateUpdate()
    {
        if (followPlayer)
            mainCamera.transform.position = new Vector3(player.position.x, player.position.y, mainCamera.transform.position.z);
    }

    // Change health UI
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

    // Called when an enemy's HP hits 0.
    // Plays an animation, sound, and destroys enemy
    public void EnemyKilled(Transform enemy)
    {
        enemyCount--;
        if (enemyCount <= 0)
            StartCoroutine(SlowMotionKill(enemies.transform.GetChild(0)));
        else
            DeathAnimation(enemy);
    }

    // Checks if all levels cleared and calls StartLevel with new level
    private void NextLevel()
    {
        stageNum++;
        if (stageNum > MAX_STAGE)
            GameOver(true);
        else
            StartCoroutine(StartLevel(stageNum));
    }

    // Plays the "Hohoho" sound, displays the current level, and generates new enemies
    private IEnumerator StartLevel(int level)
    {
        audioManager.Play("playerSpawn");
        StartCoroutine(DisplayBox(2.5f, "Level " + level));
        yield return new WaitForSeconds(3.25f);
        GenerateEnemies(stageNum);
    }

    // Used to resume play after pause
    private void StartAllObjects()
    {
        //player.gameObject.SetActive(true);
        //enemies.SetActive(false);
        paused = false;
        pauseText.SetActive(false);
        Time.timeScale = 1;
    }

    // Used to pause the game
    private void StopAllObjects()
    {
        //player.gameObject.SetActive(false);
        //enemies.SetActive(false);
        paused = true;
        pauseText.SetActive(true);
        Time.timeScale = 0;
    }

    // Generates enemies with data from Levels script
    // Plays "zombieSpawn" sound
    private void GenerateEnemies(int level)
    {
        enemies = new GameObject("Enemies");
        List<(int, Vector2)> data = levelData.GetLevelData(level);
        enemyCount = data.Count;
        for (int i = 0; i < data.Count; i++)
        {
            Instantiate(summonAnimation, data[i].Item2, Quaternion.identity);
            GameObject enemy = Instantiate(enemyPrefab[data[i].Item1], data[i].Item2, Quaternion.identity, enemies.transform);
            // Assign an ID???
        }
        audioManager.Play("zombieSpawn");
    }

    // Randomly generate items for the player to collect
    private void GenerateItems(int min, int max)
    {
        int time = Random.Range(min, max + 1);
        StartCoroutine(SpawnItem(time));
    }

    // Displays the textbox on screen
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

    // Ends the game and displays the result
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

    // Enables a slow motion finish on the enemy's position
    private IEnumerator SlowMotionKill(Transform enemy)
    {
        Time.timeScale = 0.05f;
        //Time.fixedDeltaTime = Time.timeScale * 0.2f;
        followPlayer = false;
        StartCoroutine(CameraPan(0.05f, 0.2f, enemy, 2));
        yield return new WaitForSeconds(0.3f);

        Time.timeScale = 1f;
        followPlayer = true;
        NextLevel();
    }

    // Moves the Camera, holds the current position, then moves to the player's position
    private IEnumerator CameraPan(float moveTime, float stayTime, Transform newTransform, float newSize)
    {
        float time = 0;
        float deadline = moveTime * 2 + stayTime;
        Vector3 oldPosition = mainCamera.transform.position;
        Vector3 newPosition = new Vector3(newTransform.position.x, newTransform.position.y, oldPosition.z);
        float oldSize = mainCamera.orthographicSize;
        bool noExplosion = true;

        while (time < deadline)
        {
            if (time <= moveTime)
            {
                float t = time / moveTime;
                mainCamera.transform.position = Vector3.Lerp(oldPosition, newPosition, t);
                mainCamera.orthographicSize = oldSize + (newSize - oldSize) * t;
            }
            else if (noExplosion)
            {
                mainCamera.transform.position = newPosition;
                mainCamera.orthographicSize = newSize;
                noExplosion = false;
                audioManager.Play("megaExplosion");
                audioManager.Play("zombieSpecialDeath");
                Instantiate(explosionAnimation, newPosition, Quaternion.identity);
                Destroy(newTransform.gameObject);
            }
            else if (time > deadline - moveTime)
            {
                float t = (time - moveTime - stayTime) / moveTime;
                mainCamera.transform.position = Vector3.Lerp(newPosition, new Vector3(player.position.x, player.position.y, mainCamera.transform.position.z), t);
                mainCamera.orthographicSize = newSize + (oldSize - newSize) * t;
            }

            yield return null;
            time += Time.deltaTime;
        }

        mainCamera.orthographicSize = oldSize;
        mainCamera.transform.position = new Vector3(player.position.x, player.position.y, -10);
    }

    private void DeathAnimation(Transform enemy)
    {
        audioManager.Play("zombieDeath");
        Instantiate(summonAnimation, enemy.position, Quaternion.identity);
        Destroy(enemy.gameObject);
    }

    private IEnumerator SpawnItem(int time)
    {
        float elapsed = 0;
        float x, y;
        int type = Random.Range(0, presentPrefab.Length);
        while(true)
        {
            x = Random.Range(-STAGE_WIDTH, STAGE_WIDTH);
            y = Random.Range(-STAGE_HEIGHT, STAGE_HEIGHT);
            //TileBase tilebase = tilemap.GetTile(tilemap.WorldToCell(new Vector3(x, y, 0)));
            TileBase tilebase = null;
            if (tilebase == null)
                break;
            yield return null;
            elapsed += Time.deltaTime;
        }

        while (true)
        {
            if (elapsed > time)
            {
                Instantiate(presentPrefab[type], new Vector3(x, y, 0), Quaternion.identity);
                break;
            }
            yield return null;
            elapsed += Time.deltaTime;
        }

        GenerateItems(MIN_TIME, MAX_TIME);
    }
}
