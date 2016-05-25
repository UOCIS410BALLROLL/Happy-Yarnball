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
	public bool cheatsEnabled; // set True for development, False for testing (maybe easter egg way to enable)
    public int minMorsels;
	public int goalTime;
	public float jumpHeight;

	public Button nextLevel;
	public Button restartLevel;
	public Button mainMenu;


	float currentTime = 0.0f; //here
    float powerUpTime;

	private string levelName;
    private int totalMorsels;
    private int morselCount;
	private bool endLevel;
    private bool gameOver;
	private bool displayingMessage;
	private bool canUpdateAlert, showingImportantText;
    private GameObject cam;
	private int[] powerupCounts;

	private const int NUMPOWS = 3;
	public const int SPEED = 0;
	public const int JUMP = 1;
	public const int SHRINK = 2;

//	public AudioSource desertSound;

    void Start()
    {
        morselCount = 0;
        totalMorsels = GameObject.FindGameObjectsWithTag("Cat").Length;
        morselText.text = string.Format("{0}/{1} Cats", morselCount, totalMorsels);
        minMorsels = Mathf.Clamp(minMorsels, 0, totalMorsels);
        gameOver = false;
		endLevel = false;
		canUpdateAlert = true;
        starTimerText.text = "";
        player = Instantiate(player, new Vector3(start_x, start_y, start_z), player.transform.rotation) as GameObject;
		player.GetComponent<PlayerController> ().SetJumpHeight (jumpHeight);
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        cam.GetComponent<CameraController>().SetPlayer(player);
        StartCoroutine(StartLevelMessage());
		displayingMessage = false;
		levelName = SceneManager.GetActiveScene ().name;
		powerupCounts = new int[NUMPOWS];
		for (int i = 0; i < NUMPOWS; i++) {
			powerupCounts [i] = 0;
		}
		nextLevel.gameObject.SetActive (false);
		mainMenu.gameObject.SetActive (false);
		restartLevel.gameObject.SetActive (false);
//		desertSound = GetComponent<AudioSource> ();

    }

	void OnGUI(){
		if (endLevel) {
			int stars = GetStars ();
			GUI.DrawTexture (new Rect (250, 150, 300, 200), starTextures[stars-1], ScaleMode.StretchToFill, true, 40.0F);
		}
	}

    void Update()
    {
        if(gameOver && Input.anyKeyDown)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
		if(cheatsEnabled)
		{
			CheckCheats ();
		}
		if (canUpdateAlert && !showingImportantText) {
            powerUpTime = ug.GetDuration();
			alertText.text = string.Format ("{0}{1}{2}",
				(powerupCounts [SPEED] == 0 ? "" : "Speed Up\n"),
				(powerupCounts [JUMP] == 0 ? "" : "Double Jump\n"),
				(powerupCounts [SHRINK] == 0 ? "" : "Shrink\n")
			);
		}
		Timer ();
    }

	void CheckCheats()
	{
		//A number of cheats useful for debugging, if you think of something feel free to add it
		if (Input.GetKeyDown("r"))
		{
			//Restart the Current Level
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
		else if (Input.GetKeyDown("`"))
		{
			//Load the Menu Scene
			SceneManager.LoadScene(0);
		}
		else if (Input.GetKeyDown("1"))
		{
//			desertSound.Play ();
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
		else if (Input.GetKeyDown("0"))
		{
			//Load the prototype level
			SceneManager.LoadScene(SceneManager.GetSceneByName("PrototypeScene").name);
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
			PlayerPrefs.SetFloat (string.Format ("{0}-Time", levelName), Mathf.Min(currentTime, PlayerPrefs.GetFloat(string.Format ("{0}-Time", levelName))));
		}
		PlayerPrefs.Save ();

        player.GetComponent<Rigidbody>().isKinematic = true;
        starTimerText.text = "The time for 3 \nstars is: " + goalTime + " seconds";
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
        morselText.text = string.Format("{0}/{1} Cats", morselCount, totalMorsels);
    }

    [SerializeField]
    public void AddMorsel()
    {
        morselCount++;
        UpdateUI();
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
        else if (!displayingMessage)
        {
            displayingMessage = true;
			StartCoroutine(NotFinishedMessage());
        }
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

    IEnumerator StartLevelMessage()
    {
		showingImportantText = true;
		if (canUpdateAlert) {
			alertText.text = string.Format ("Level Goal: {0}/{1} Cats", minMorsels, totalMorsels);
		}
        yield return new WaitForSeconds(3);
		if (canUpdateAlert) {
			alertText.text = "";
		}
		showingImportantText = false;
    }
	
	IEnumerator NotFinishedMessage() {
		showingImportantText = true;
		if (canUpdateAlert) {
			alertText.text = string.Format ("Need {0} Cats to Advance", minMorsels - morselCount);
		}
		yield return new WaitForSeconds (2);
		if (canUpdateAlert) {
			alertText.text = "";
		}
		displayingMessage = false;
	}

    IEnumerator GameOver()
    {
		canUpdateAlert = false;
        alertText.text = "Game Over";
        yield return new WaitForSeconds(2);
        alertText.text = "\n" + alertText.text + "\nPress Any Key to Restart";
        gameOver = true;
    }

	void Timer()
	{
		if (alertText.text == "Game Over") { // change to death condition
			currentTime = 0;
		} else if (gameOver || nextLevel.gameObject.activeSelf) {}
		else{
            starTimerText.text = "";
            timerText.text = "Time: " + Mathf.Floor (currentTime).ToString ();
			currentTime += Time.deltaTime;
		}
	}
}
