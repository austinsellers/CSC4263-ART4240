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
	private static bool story = true;
	private static bool gameOver = false;
	private static bool win = false;
	private static bool upgrade = false;

	private string curLevel;

	public int wave;
	public GameObject pauseCanvas;
	public GameObject gameOverCanvas;
	public GameObject upgradeCanvas;
	public GameObject mainCanvas;
	public GameObject startCanvas;
	public GameObject prePreStartCanvas;
	public GameObject winCanvas;
	public Text waveText;
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

	private int codeIndex;
	private KeyCode[] konami = new KeyCode[] {
		KeyCode.UpArrow,
		KeyCode.UpArrow,
		KeyCode.DownArrow,
		KeyCode.DownArrow,
		KeyCode.LeftArrow,
		KeyCode.RightArrow,
		KeyCode.LeftArrow,
		KeyCode.RightArrow,
		KeyCode.B,
		KeyCode.A
	};

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
	private Text winText;

	[HideInInspector]
	public PlayerStats playerStats;
	public EnemyManager[] enemyManagers;
	[HideInInspector]
    public MapManager mapManager;
	[HideInInspector]
	public GameObject musicManager;
	private GameObject player;
    private PlayerController playerController;

	private float fadeTime = 0.75f;

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
		musicManager = GameObject.Find ("Music Manager");


		DontDestroyOnLoad (mapManager);
		DontDestroyOnLoad (player);
		DontDestroyOnLoad (musicManager);
		SetupGame ();
	}

	IEnumerator ShowStory(){
		prePreStartCanvas.SetActive (true);
		GameObject.Find ("Start Music Manager").GetComponent<AudioSource> ().Play();
		pauseSpawning (true);
		int panel = 2;
		Image[] images = prePreStartCanvas.GetComponentsInChildren<Image> ();
		for(int i = 1; i < images.Length; i++)
		{
			images [i].canvasRenderer.SetAlpha (0.0f);
		}
		images [1].CrossFadeAlpha (1.0f, fadeTime + 0.25f, false);
		while (true) {
			if (Input.GetKeyDown(KeyCode.Space)) 
			{
				if (panel < images.Length) 
				{
					images [panel++].CrossFadeAlpha (1.0f, fadeTime, false);
				}
				else 
				{
					StartCoroutine (attractMode ());
					prePreStartCanvas.GetComponentInChildren<Text> ().CrossFadeAlpha (0.0f, fadeTime, false);
					foreach (Image i in images) {
						i.CrossFadeAlpha (0.0f, fadeTime, false);
					}
					yield return new WaitForSecondsRealtime (fadeTime);
					prePreStartCanvas.SetActive (false);
					GameObject.Find ("Start Music Manager").GetComponent<AudioSource> ().Stop();
					musicManager.GetComponent<AudioSource> ().Play();
					story = false;
					break;
				}
			}
			yield return null;
		}
	}

	void SetupGame()
	{
		mapManager.SetupMap ("level");
		waveText.text = "Wave: " + (wave + 1);
	}

	void Start()
	{
		gameOverText = gameOverCanvas.GetComponentsInChildren<Text> ()[1];
		winText = winCanvas.GetComponentsInChildren<Text> ()[1];
        for (int i = 0; i < 12; i++)
        {
            textList.AddUpgrade(i);
        }

        DontDestroyOnLoad (upgradeCanvas);
		DontDestroyOnLoad (pauseCanvas);
		DontDestroyOnLoad (gameOverCanvas);
		DontDestroyOnLoad (winCanvas);
		DontDestroyOnLoad (mainCanvas);
		DontDestroyOnLoad (playerStats);
		DontDestroyOnLoad (prePreStartCanvas);
		//startCanvas = GameObject.Find ("Story Canvas").gameObject;
		DontDestroyOnLoad (startCanvas);
		StartCoroutine(ShowStory ());
	}
	IEnumerator attractMode(){
		startCanvas.SetActive (true);

		startCanvas.GetComponentInChildren<Image> ().canvasRenderer.SetAlpha (0.0f);
		startCanvas.GetComponentInChildren<Image> ().CrossFadeAlpha (1.0f, fadeTime, false);
		foreach (Text t in startCanvas.GetComponentsInChildren<Text>()) 
		{
			t.canvasRenderer.SetAlpha (0.0f);
			t.CrossFadeAlpha (1.0f, fadeTime, false);
		}
		pauseSpawning (true);
		while (true) {
			pauseSpawning (true);
			//print ("In Attrach mode");
			if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) ||
				(Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) ||
				(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) ||
				(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) ||
				Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.K)) 
			{
				pauseSpawning (false);
				startCanvas.GetComponentInChildren<Image> ().CrossFadeAlpha (0.0f, fadeTime, false);
				foreach (Text t in startCanvas.GetComponentsInChildren<Text>()) 
				{
					t.CrossFadeAlpha (0.0f, fadeTime, false);
				}
				yield return new WaitForSecondsRealtime (fadeTime);
				startCanvas.SetActive (false);

				for (int i = 0; i < hud.Length; i++) 
				{
					hud [i].SetActive (true);
				}
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

	void newWave (){
		StartCoroutine (WaitFor (5f));
		wave++;
		Debug.Log ("New Wave:" + wave);
		waveText.text = "Wave: " + (wave + 1);
		for (int i = 0; i < enemyManagers.Length; i++) {
			if (enemyManagers [i].spawnAtBeginning.Length > wave) {
				enemyManagers [i].spawn (enemyManagers [i].spawnAtBeginning [wave]);
			}
		}
	}

	void Update()
	{
		if (!story) {
			if (Input.GetKeyDown (konami [codeIndex])) {
				Debug.Log ("Pushed: " + konami[codeIndex]);
				if (++codeIndex == konami.Length) 
				{
					codeIndex = 0;
					Debug.Log ("KONAMI ACTIVATED");
					playerStats.AddHealth(9001 - playerStats.health);
					playerController.biteScale = new Vector2(5f, 5f);
					playerStats.biteDamage = 1000;
					playerController.distanceFromAtk += 3f;
				}
			}


			if (Input.GetKeyDown (KeyCode.Y)) {
				LoadBoss ();
			}
			if (Input.GetKeyDown (KeyCode.H)) {
				playerStats.AddHealth (2);
			}

			if (curLevel != "BossBattle") {
				if (readyForNextLevel (enemyManagers)) {
					if (readyForBoss (enemyManagers)) {
						Debug.Log ("Ready for bo$$ level");
						LoadBoss ();
					}
					newWave ();
				}
			}
			if (paused) {
				if (Input.GetKeyDown (KeyCode.Q))
					Quit ();
				if (Input.GetKeyDown (KeyCode.R)) {
					if (SceneManager.GetActiveScene().name == "BossBattle")
						RestartBoss ();
					else
						Restart ();
				}
			}
			if (gameOver || win) {
				if (Input.GetKeyDown (KeyCode.R)) {
					if (SceneManager.GetActiveScene().name == "BossBattle")
						RestartBoss ();
					else
						Restart ();
				}
			} else if (upgrade) {
				if (Input.GetKeyDown (KeyCode.Alpha1) || Input.GetKeyDown (KeyCode.Keypad1)) {
					textList.AddUpgrade (upgrade2Int);
					textList.AddUpgrade (upgrade3Int);

					DestroyImmediate (upgrade1);
					DestroyImmediate (upgrade2);
					DestroyImmediate (upgrade3);

					HandleUpgrade (upgrade1Int);
					upgrade = ToggleScale (upgradeCanvas);
				} else if (Input.GetKeyDown (KeyCode.Alpha2) || Input.GetKeyDown (KeyCode.Keypad2)) {
					textList.AddUpgrade (upgrade1Int);
					textList.AddUpgrade (upgrade3Int);

					DestroyImmediate (upgrade1);
					DestroyImmediate (upgrade2);
					DestroyImmediate (upgrade3);

					HandleUpgrade (upgrade2Int);
					upgrade = ToggleScale (upgradeCanvas);
				} else if (Input.GetKeyDown (KeyCode.Alpha3) || Input.GetKeyDown (KeyCode.Keypad3)) {
					textList.AddUpgrade (upgrade1Int);
					textList.AddUpgrade (upgrade2Int);

					DestroyImmediate (upgrade1);
					DestroyImmediate (upgrade2);
					DestroyImmediate (upgrade3);

					HandleUpgrade (upgrade3Int);
					upgrade = ToggleScale (upgradeCanvas);
				}
			} else {
				if (Input.GetKeyDown (KeyCode.P))
					paused = ToggleScale (pauseCanvas);
			}

			if(Input.GetKeyDown(KeyCode.M))
			{
				AudioSource music = GameObject.Find ("Music Manager").GetComponent<AudioSource> ();
				if (music.isPlaying)
					music.Stop ();
				else
					music.Play ();
			}
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

		upgrade1.GetComponent<Image>().color = Color.black;
		upgrade2.GetComponent<Image>().color = Color.black;
		upgrade3.GetComponent<Image>().color = Color.black;

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
		Time.timeScale = 0f;
        gameOver = true;
        gameOverText.text = "Player Reached LVL: " + playerStats.getCurrentLevel() + "\nWith Experience of: " + playerStats.getCurrentExp() + "\nPress \"R\" to Restart";
        gameOverCanvas.SetActive(true);
    }

	public void Win()
	{
		Time.timeScale = 0f;
		win = true;
		winText.text = "Player Reached LVL: " + playerStats.getCurrentLevel() + "\nWith Experience of: " + playerStats.getCurrentExp() + "\nPress \"R\" to Restart";
		winCanvas.SetActive(true);
	}
    
	public static bool isPaused()
	{
		return (paused);
	}

	public static bool isUpgrade()
	{
		return (upgrade);
	}

	public static bool isStory()
	{
		return (story);
	}

	public static bool hasWon()
	{
		return (win);
	}

	private void DeleteEnemies()
	{
		foreach(GameObject g in GameObject.FindGameObjectsWithTag("Enemy"))
			DestroyImmediate(g);
	}

	public void Restart()
	{
		if (paused)
			paused = ToggleScale (pauseCanvas);
		Time.timeScale = 1f;
		if (gameOver)
			gameOverCanvas.SetActive (false);
		else if (win)
			winCanvas.SetActive (false);
		gameOver = false;
		win = false;

		DeleteEnemies ();

		//SceneManager.LoadScene ("GameScene");
		for (int i = 0; i < hud.Length; i++) 
		{
			hud [i].SetActive (false);
		}
			
		playerController.ResetPlayerStats ();
		playerStats.ResetStats ();
		player.gameObject.transform.position = new Vector2 (17.6f, 17.7f);
		playerController.currentPos = player.transform.position;
		playerController.projectedPos = player.transform.position;
		StartCoroutine (attractMode ());
		ResetWaves ();
	}

	public void RestartBoss()
	{
		if (paused)
			paused = ToggleScale (pauseCanvas);
		Time.timeScale = 1f;
		if (gameOver)
			gameOverCanvas.SetActive (false);
		else if(win)
			winCanvas.SetActive (false);
		gameOver = false;
		win = false;

		DeleteEnemies ();

		SceneManager.LoadScene ("GameScene", LoadSceneMode.Single);
		for (int i = 0; i < hud.Length; i++) 
		{
			hud [i].SetActive (false);
		}

		playerController.ResetPlayerStats ();
		playerStats.ResetStats ();
		player.gameObject.transform.position = new Vector2 (17.6f, 17.7f);
		playerController.currentPos = player.transform.position;
		playerController.projectedPos = player.transform.position;
		musicManager.GetComponent<AudioSource> ().Play ();
		StartCoroutine (attractMode ());
		ResetWaves ();
	}

	private void ResetWaves()
	{
		wave = 0;
		foreach (EnemyManager em in enemyManagers) 
		{
			em.ResetEnemy ();
		}
		waveText.text = "Wave: " + (wave + 1);
	}

	public void Quit()
	{
		// May want to move this to the main menu later
		Debug.Log ("Game Exited");
		Application.Quit();
	}

	public void LoadBoss()
	{
		musicManager.GetComponent<AudioSource> ().Stop ();
		SceneManager.LoadScene ("BossBattle", LoadSceneMode.Single);
		//player.transform.position = new Vector3 (20.86f, 28.37f, 0f);
		//playerController.currentPos = player.transform.position;
		//playerController.projectedPos = player.transform.position;
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