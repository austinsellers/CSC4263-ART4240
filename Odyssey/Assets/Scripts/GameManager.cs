using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TextListA
{

    private int[] list = new int[12];
    private int n = 0;

    public TextListA()
    {
    }

    public void AddUpgrade(int upgrade)
    {
        list[n] = upgrade;
        n++;
    }

    public int RandomUpgrade()
    {
        int randInt = Random.Range(0, n - 1);
        int rand = list[randInt];

        for (int i = randInt; i < n - 1; i++)
        {
            list[i] = list[i + 1];
        }
           
        n--;
        return rand;
    }

}

public class GameManager : MonoBehaviour 
{
	private static bool paused = false;
	private static bool gameOver = false;
	private static bool upgrade = false;

	private string curLevel;

	public int wave;
	public GameObject pauseCanvas;
	public GameObject gameOverCanvas;
	public GameObject upgradeCanvas;
	public GameObject mainCanvas;
	public GameObject startCanvas;
	public GameObject[] hud;

    //list stuff
    private TextListA textList = new TextListA();
    private GameObject tempObj;

    private int upgrade1Int;
    private int upgrade2Int;
    private int upgrade3Int;
    private GameObject upgrade1;
	private GameObject upgrade2;
	private GameObject upgrade3;
    private GameObject upgrade1c0;
    private GameObject upgrade2c0;
    private GameObject upgrade3c0;
    private GameObject upgrade1c1;
    private GameObject upgrade2c1;
    private GameObject upgrade3c1;

    public GameObject UpgradePrefab;

    private string[] txts = {   "Increased Bark Damage",
                                "Increased Bite Damage",
                                "Increased Movement Speed",
                                "Decreased Bark Cooldown",
                                "Decreased Bite Cooldown",
                                "Increased Defense",
                                "Increased Bite Range",
                                "Healing Chance On Kill",
                                "Increased Bark Speed",
                                "Increased Bite Size",
                                "Increased Bark Size",
                                "Increased Experience Gain" };

    private Text gameOverText;

	[HideInInspector]
	public PlayerStats playerStats;
	public EnemyManager[] enemyManagers;
	[HideInInspector]
    public MapManager mapManager;
	private GameObject player;
    private PlayerController playerController;

	public static GameManager instance = null;

	void Awake () 
	{
		// Makes sure there is only 1 GameManager
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		// Doesn't destroy when reloading scene
		DontDestroyOnLoad (gameObject);
		mapManager = GetComponent<MapManager> ();
		player = GameObject.Find ("Player");
		playerStats = GameObject.FindObjectOfType<PlayerStats> ();
		playerController = player.GetComponent<PlayerController>();
		enemyManagers = GameObject.FindObjectsOfType<EnemyManager> ();
		DontDestroyOnLoad (mapManager);
		DontDestroyOnLoad (player);
		SetupGame ();
	}

	void SetupGame()
	{
		mapManager.SetupMap ("level");
	}

	void Start()
	{
		gameOverText = gameOverCanvas.GetComponentsInChildren<Text> ()[1];
        for (int i = 0; i < 12; i++)
        {
            textList.AddUpgrade(i);
        }

        DontDestroyOnLoad (upgradeCanvas);
		DontDestroyOnLoad (pauseCanvas);
		DontDestroyOnLoad (gameOverCanvas);
		DontDestroyOnLoad (mainCanvas);
		DontDestroyOnLoad (playerStats);
		DontDestroyOnLoad (startCanvas);
		StartCoroutine (attractMode ());
	}
	IEnumerator attractMode(){
		startCanvas.SetActive (true);
		pauseSpawning (true);
		while (true) {
			if (Input.GetKeyDown (KeyCode.UpArrow)||(Input.GetKeyDown(KeyCode.DownArrow))||(Input.GetKeyDown(KeyCode.LeftArrow))||(Input.GetKeyDown(KeyCode.RightArrow))|| Input.GetKeyDown(KeyCode.D) || (Input.GetKeyDown(KeyCode.S)) || (Input.GetKeyDown(KeyCode.A)) || (Input.GetKeyDown(KeyCode.W))) 
			{
				pauseSpawning (false);
				startCanvas.SetActive (false);

				for (int i = 0; i < hud.Length; i++) 
				{
					hud [i].SetActive (true);
				}
				Debug.Log ("Yo");
				break;
			}
			yield return null;
		}
	}
	void pauseSpawning(bool pauseSpawn)
	{
		for (int i = 0; i < enemyManagers.Length; i++) 
		{
			enemyManagers [i].pauseSpawn = pauseSpawn;
		}
	}

	bool readyForBoss(EnemyManager[] e)
	{
		for (int i = 0; i < e.Length; i++) 
		{
			if (e [i].spawnAmount.Length > wave) 
			{
				return false;
			}
		}
		return true;
	}

	bool readyForNextLevel(EnemyManager[] e)
	{
		bool isReady = true;
		for (int i = 0; i < e.Length; i++) 
		{
			if (e [i].enemiesAlive.Length > wave) 
			{
				if (e [i].enemiesAlive [wave] > 0)
					isReady = false;
			}
		}
		if (isReady) {
			for (int i = 0; i < e.Length; i++) 
			{
				if (e [i].spawnAmount.Length > wave) 
				{
					e [i].pauseSpawn = false;
				}
			}
		}
		return isReady;
	}

	IEnumerator WaitFor(float delay)
	{
		yield return new WaitForSecondsRealtime (delay);
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Y)) {
			LoadBoss ();
		}
		if (Input.GetKeyDown (KeyCode.U)) {
			Restart (true);
		}

		if (curLevel != "BossBattle") {
			if (readyForNextLevel (enemyManagers)) {
				if (readyForBoss (enemyManagers)) {
					Debug.Log ("Ready for bo$$ level");
					GameOver ();
				}
				StartCoroutine (WaitFor (5f));
				wave++;
				Debug.Log ("New Wave:" + wave);
			}
		}
		if (paused) 
		{
			if (Input.GetKeyDown (KeyCode.Q))
				Quit();
			if (Input.GetKeyDown (KeyCode.R))
				Restart (false);
		}
		if (gameOver) 
		{
			if (Input.GetKeyDown (KeyCode.R)) 
			{
				Restart (true);
				gameOverCanvas.SetActive (false);
			}
		} 
		else if (upgrade) 
		{
			if (Input.GetKeyDown (KeyCode.Alpha1) || Input.GetKeyDown (KeyCode.Keypad1)) 
			{
                textList.AddUpgrade(upgrade2Int);
                textList.AddUpgrade(upgrade3Int);

                DestroyImmediate(upgrade1);
                DestroyImmediate(upgrade2);
                DestroyImmediate(upgrade3);

                HandleUpgrade(upgrade1Int);
                upgrade = ToggleScale (upgradeCanvas);
			} 
			else if (Input.GetKeyDown (KeyCode.Alpha2) || Input.GetKeyDown (KeyCode.Keypad2))
            {
                textList.AddUpgrade(upgrade1Int);
                textList.AddUpgrade(upgrade3Int);

                DestroyImmediate(upgrade1);
                DestroyImmediate(upgrade2);
                DestroyImmediate(upgrade3);

                HandleUpgrade(upgrade2Int);
                upgrade = ToggleScale (upgradeCanvas);
			} 
			else if (Input.GetKeyDown (KeyCode.Alpha3) || Input.GetKeyDown (KeyCode.Keypad3)) 
			{
                textList.AddUpgrade(upgrade1Int);
                textList.AddUpgrade(upgrade2Int);

                DestroyImmediate(upgrade1);
                DestroyImmediate(upgrade2);
                DestroyImmediate(upgrade3);

                HandleUpgrade(upgrade3Int);
                upgrade = ToggleScale (upgradeCanvas);
			}
		}
		else 
		{
			if (Input.GetKeyDown (KeyCode.P))
				paused = ToggleScale (pauseCanvas);
		}
	}

	bool ToggleScale(GameObject go)
	{
		if (Time.timeScale == 0f) 
		{
			go.SetActive (false);
			Time.timeScale = 1f;
			return(false);
		} 
		else 
		{
			go.SetActive (true);
			Time.timeScale = 0f;
			return(true);
		}
	}

	public void Upgrade()
	{
        upgradeCanvas.GetComponent<RectTransform>().position = new Vector3(0, 0, 0);

        upgrade1Int = textList.RandomUpgrade();
        upgrade2Int = textList.RandomUpgrade();
        upgrade3Int = textList.RandomUpgrade();

        upgrade1 = Instantiate(UpgradePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        upgrade2 = Instantiate(UpgradePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        upgrade3 = Instantiate(UpgradePrefab, new Vector3(0, 0, 0), Quaternion.identity);

        upgrade1.transform.SetParent(upgradeCanvas.transform);
        upgrade2.transform.SetParent(upgradeCanvas.transform);
        upgrade3.transform.SetParent(upgradeCanvas.transform);

        upgrade1.GetComponent<RectTransform>().Translate(-279.7f, 0, 0);
        upgrade2.GetComponent<RectTransform>().Translate(0, 0, 0);
        upgrade3.GetComponent<RectTransform>().Translate(279.7f, 0, 0);

        upgrade1c0 = upgrade1.transform.GetChild(0).gameObject;
        upgrade2c0 = upgrade2.transform.GetChild(0).gameObject;
        upgrade3c0 = upgrade3.transform.GetChild(0).gameObject;

        upgrade1c1 = upgrade1.transform.GetChild(1).gameObject;
        upgrade2c1 = upgrade2.transform.GetChild(1).gameObject;
        upgrade3c1 = upgrade3.transform.GetChild(1).gameObject;

        upgrade1.GetComponent<Image>().color = Color.green;
        upgrade2.GetComponent<Image>().color = Color.magenta;
        upgrade3.GetComponent<Image>().color = Color.cyan;

        upgrade1c1.GetComponent<Text>().text = "Press 1";
        upgrade2c1.GetComponent<Text>().text = "Press 2";
        upgrade3c1.GetComponent<Text>().text = "Press 3";

        upgrade1c0.GetComponent<Text>().text = txts[upgrade1Int];
        upgrade2c0.GetComponent<Text>().text = txts[upgrade2Int];
        upgrade3c0.GetComponent<Text>().text = txts[upgrade3Int];

        upgrade1c0.GetComponent<Text>().fontSize = 37;
        upgrade2c0.GetComponent<Text>().fontSize = 37;
        upgrade3c0.GetComponent<Text>().fontSize = 37;

        upgradeCanvas.SetActive (true);

        upgrade = true;
        upgrade = ToggleScale (upgradeCanvas);
	}

    public void GameOver()
    {
        // TODO: handle GameOver state displaying experience
        gameOver = true;
        gameOverText.text = "Player Reached LVL: " + playerStats.getCurrentLevel() + "\nWith Experience of: " + playerStats.getCurrentExp() + "\nPress \"R\" to Restart";
        gameOverCanvas.SetActive(true);
    }
    
	public static bool isPaused()
	{
		return (paused);
	}

	public static bool isUpgrade()
	{
		return (upgrade);
	}

	public void Restart(bool begin)
	{
		if (paused)
			paused = ToggleScale (pauseCanvas);
		gameOver = false;

		if(begin)
		{
			SceneManager.LoadScene ("GameScene");
			playerController.ResetPlayerStats ();
			playerStats.ResetStats ();
		}
		else if (curLevel == "BossBattle") 
		{
			LoadBoss ();
		}
		else 
		{
			SceneManager.LoadScene (SceneManager.GetActiveScene().name);
			playerController.ResetPlayerStats ();
			playerStats.ResetStats ();
		}
	}

	public void Quit()
	{
		// May want to move this to the main menu later
		Debug.Log ("Game Exited");
		Application.Quit();
	}

	public void LoadBoss()
	{
		SceneManager.LoadScene ("BossBattle");
		player.transform.position = new Vector3 (13.3f, 12.2f, 0f);
		playerController.currentPos = player.transform.position;
		playerController.projectedPos = player.transform.position;
	}

	public void SetLevel(string level)
	{
		curLevel = level;
	}

	public GameObject GetPlayer()
	{
		return player;
	}

	public PlayerController GetPlayerController()
	{
		return playerController;
	}

    private void HandleUpgrade(int upNumber)
    {
        switch (upNumber)
        {
            case 0:
                playerStats.barkDamage += 2;
                break;
            case 1:
                playerStats.biteDamage += 2;
                break;
            case 2:
                playerController.speed += 2;
                break;
            case 3:
				playerController.barkDelay -= .25f;
                break;
            case 4:
				playerController.biteDelay -= .25f;
                break;
            case 5:
                playerStats.defense += 1;
                break;
            case 6:
				playerController.distanceFromAtk += 2;
                break;
            case 7:
                playerStats.upgrade8 = true;
                break;
            case 8:
				playerController.barkSpd = 1000f;
                break;
            case 9:
				playerController.biteScale = new Vector2(1.5f, 1.5f);
				playerController.distanceFromAtk += 1f;
                break;
            case 10:
				playerController.barkScale = .4f;
                break;
            case 11:
                playerStats.expModifier += .2f;
                break;
        }
    }
}