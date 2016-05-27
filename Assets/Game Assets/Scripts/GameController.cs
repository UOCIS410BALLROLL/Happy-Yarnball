using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public GameObject player;
    public UpgradeController ug;
    public float start_x = 0;
    public float start_y = 0;
    public float start_z = 0;
    public Text morselText;
    public Text alertText;
	public Text timerText;
    public Text starTimerText;
	public Texture[] starTextures;

    public int minMorsels;
	public int goalTime;
	public float jumpHeight;

	public Button nextLevel;
	public Button restartLevel;
	public Button mainMenu;


	float currentTime = 0.0f; //here

	private bool cheatsEnabled; //click top right of main menu to enable
	private string levelName;
    private int totalMorsels;
    private int morselCount;
    private float roundTime;
	private bool endLevel;
    private bool gameOver;
    private GameObject cam;
	private float alertEndTime;
	private int[] powerupCounts;

	private const int NUMPOWS = 3;
	public const int SPEED = 0;
	public const int JUMP = 1;
	public const int SHRINK = 2;

    public AudioSource deathSound;

//	public AudioSource desertSound;

    void Start()
    {
		if (PlayerPrefs.GetInt ("Cheatyface") == 1) {
			timerText.text = "Cheater!";
			cheatsEnabled = true;
		} else {
			cheatsEnabled = false;
		}
        morselCount = 0;
        totalMorsels = GameObject.FindGameObjectsWithTag("Cat").Length;
        minMorsels = Mathf.Clamp(minMorsels, 0, totalMorsels);
        gameOver = false;
		endLevel = false;
        deathSound = GameObject.FindGameObjectWithTag("DeathBox").GetComponent<AudioSource>();
        starTimerText.text = "";
		UpdateUI ();
        player = Instantiate(player, new Vector3(start_x, start_y, start_z), player.transform.rotation) as GameObject;
		player.GetComponent<PlayerController> ().SetJumpHeight (jumpHeight);
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        cam.GetComponent<CameraController>().SetPlayer(player);
        StartLevelMessage();
		levelName = SceneManager.GetActiveScene ().name;
		nextLevel.gameObject.SetActive (false);
		mainMenu.gameObject.SetActive (false);
		restartLevel.gameObject.SetActive (false);
		powerupCounts = new int[NUMPOWS];
		for (int i = 0; i < NUMPOWS; i++) {
			powerupCounts [i] = 0;
		}
//		desertSound = GetComponent<AudioSource> ();

    }

	void OnGUI(){
		if (endLevel) {
			int stars = GetStars ();
			if (stars > 0) {
				GUI.DrawTexture (new Rect (250, 150, 400, 200), starTextures [stars - 1], ScaleMode.StretchToFill, true, 40.0F);
			}
		}
	}

    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			//Load the Menu Scene
			SceneManager.LoadScene(0);
		}
		if (Input.GetKeyDown("r"))
		{
			//Restart the Current Level
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
        if(gameOver && Input.anyKeyDown)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
		if(!gameOver)
		{
			if (cheatsEnabled) {
				CheckCheats ();
			}
		
		}
		if (currentTime >= alertEndTime) {
			alertText.text = "";
		}
		Timer ();
    }

	void CheckCheats()
	{
		//A number of cheats useful for debugging, if you think of something feel free to add it
		if (Input.GetKeyDown("1"))
		{
			//Load the 1st level
			SceneManager.LoadScene(1);
		}
		else if (Input.GetKeyDown("2"))
		{
			//Load the 2nd Level
			SceneManager.LoadScene(2);
		}
		else if (Input.GetKeyDown("3"))
		{
			//Load the 3rd Level
			SceneManager.LoadScene(3);
		}
		else if (Input.GetKeyDown("4"))
		{
			//Load the 4th Level
			SceneManager.LoadScene(4);
		}
		else if (Input.GetKeyDown("5"))
		{
			//Load the 5th Level
			SceneManager.LoadScene(5);
		}
		else if (Input.GetKeyDown("j"))
		{
			//Moon Jump button which lets you jump infinite times for as long as you hold down the button
			player.GetComponent<PlayerController>().MoonJump();
		}
		else if (Input.GetKeyDown("+"))
		{
			//Increase the player's maximum speed
			player.GetComponent<PlayerController>().speed += 1;
		}
		else if (Input.GetKeyDown("-"))
		{
			//Decrease the player's maximum speed
			player.GetComponent<PlayerController>().speed -= 1;
		}
		else if (Input.GetKeyDown("t"))
		{
			//Teleport the player to the goal
			player.transform.position = GameObject.FindGameObjectWithTag ("Goal").transform.position;
		}
		else if (Input.GetKeyDown("m"))
		{
			//Increase the number of cats the player has collected
			AddMorsel ();
		}
		else if (Input.GetKeyDown("y"))
		{
			//Reset the timer
			currentTime = 0;
		}
	}

    void LevelComplete()
    {
		//alertText.text = string.Format("Level Complete in {0} Seconds, you got {1}/3 stars!", currentTime, GetStars());
		endLevel = true;

		if(GetStars() > PlayerPrefs.GetInt(string.Format ("{0}-Stars", levelName))) {
			PlayerPrefs.SetInt (string.Format ("{0}-Stars", levelName), GetStars ());
			PlayerPrefs.SetFloat (string.Format ("{0}-Time", levelName), currentTime);
		}
		else if(GetStars() == PlayerPrefs.GetInt(string.Format ("{0}-Stars", levelName))) {
			if (PlayerPrefs.GetInt (string.Format ("{0}-Cats", levelName)) < morselCount) {
				PlayerPrefs.SetInt (string.Format ("{0}-Cats", levelName), morselCount);
				PlayerPrefs.SetFloat (string.Format ("{0}-Time", levelName), currentTime);
			}
			else if (PlayerPrefs.GetInt (string.Format ("{0}-Cats", levelName)) == morselCount) {
				PlayerPrefs.SetFloat (string.Format ("{0}-Time", levelName), Mathf.Min (currentTime, PlayerPrefs.GetFloat (string.Format ("{0}-Time", levelName))));
			}
		}
		PlayerPrefs.Save ();

        player.GetComponent<Rigidbody>().isKinematic = true;
		alertText.text = "";
		if (GetStars () == 2) {
			starTimerText.text = "Collect all cats in " + goalTime + " seconds to get 3 stars!";
		}
		nextLevel.gameObject.SetActive (true);
		mainMenu.gameObject.SetActive (true);
		restartLevel.gameObject.SetActive (true);
    }

	public void NextLevel()
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex+1);
	}

	public void MainMenu()
	{
		SceneManager.LoadScene (0);
	}

	public void RestartLevel()
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}


	public int GetStars()
	{
		if (PlayerPrefs.GetInt ("Cheatyface") == 1) {
			return 0;
		}
		if (morselCount >= totalMorsels) 
		{
			if (currentTime <= goalTime) {
				return 3;
			}
			return 2;
		}
		return 1;
	}

    public int MorselCount()
    {
        return morselCount;
    }

    void UpdateUI()
    {
        morselText.text = string.Format("{0:D2}/{1:D2}", morselCount, totalMorsels);
		morselText.color = morselCount >= minMorsels ? Color.green : Color.red;
    }

	[SerializeField]
	public void SetAlertText(string str, float duration) {
		alertText.text = str;
		alertEndTime = currentTime + duration;
	}

    [SerializeField]
    public void AddMorsel()
    {
        morselCount++;
        UpdateUI();
	}

	[SerializeField]
	public void AddPower(int powerCons) {
		if (powerCons >= 0 && powerCons < NUMPOWS) {
			powerupCounts [powerCons]++;
		}
	}

	[SerializeField]
	public void RemPower(int powerCons) {
		if (powerCons >= 0 && powerCons < NUMPOWS) {
			powerupCounts [powerCons]--;
		}
	}

	[SerializeField]
	public int GetPowerCount(int powerCons) {
		return powerupCounts [powerCons];
	}

    [SerializeField]
    public void PlayerDestroy()
    {
        Destroy(player.gameObject);
        StartCoroutine(GameOver());
    }

    [SerializeField]
    public void ExitReached()
    {
        if (morselCount >= minMorsels)
        {
			LevelComplete ();
        }
        else
        {
			NotFinishedMessage();
        }
    }

    void StartLevelMessage()
    {
		SetAlertText (string.Format ("Level Goal: {0}/{1} Cats", minMorsels, totalMorsels), 3.0f);
    }
	
	void NotFinishedMessage() {
		SetAlertText(string.Format ("Need {0} Cats to Advance", minMorsels - morselCount), 2.0f);
	}

    IEnumerator GameOver()
    {
		gameOver = true;
		GameObject.FindGameObjectWithTag ("UIPickup").GetComponent<PickupDisplayer> ().HidePickup ();
		GameObject.FindGameObjectWithTag ("UISpeed").GetComponent<PickupDisplayer> ().HidePickup ();
		GameObject.FindGameObjectWithTag ("UIDouble").GetComponent<PickupDisplayer> ().HidePickup ();
		GameObject.FindGameObjectWithTag ("UIShrink").GetComponent<PickupDisplayer> ().HidePickup ();
        deathSound.Play();
		SetAlertText("Game Over", 2.0f);
        yield return new WaitForSeconds(2);
		while (true) {
			SetAlertText ("Game Over\nPress Any Key to Restart", 2.0f);
			yield return new WaitForSeconds (2);
		}
    }

	void Timer()
	{
		if (PlayerPrefs.GetInt ("Cheatyface") == 1) {
			timerText.text = "Cheater!";
		}
		else if (gameOver) { // change to death condition
			currentTime = 0;
		} else if (gameOver || nextLevel.gameObject.activeSelf) {}
		else{
            currentTime *= 100;
            roundTime = Mathf.Round(currentTime);
            currentTime = (roundTime / 100);
            timerText.text = "Time:\n" + currentTime.ToString ();
			currentTime += Time.deltaTime;
		}
	}
}
