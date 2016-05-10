using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public GameObject player;
    public float start_x = 0;
    public float start_y = 0;
    public float start_z = 0;
    public Text morselText;
    public Text alertText;
	public Text timerText;
	public bool cheatsEnabled; // set True for development, False for testing (maybe easter egg way to enable)
    public int minMorsels;
	public int goalTime;

	float currentTime = 0.0f; //here

    private int totalMorsels;
    private int morselCount;
    private bool gameOver;
	private bool displayingMessage;
    private GameObject cam;

//	public AudioSource desertSound;

    void Start()
    {
        morselCount = 0;
        totalMorsels = GameObject.FindGameObjectsWithTag("Cat").Length;
        morselText.text = string.Format("{0}/{1} Cats", morselCount, totalMorsels);
        minMorsels = Mathf.Clamp(minMorsels, 0, totalMorsels);
        gameOver = false;
        player = Instantiate(player, new Vector3(start_x, start_y, start_z), player.transform.rotation) as GameObject;
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        cam.GetComponent<CameraController>().SetPlayer(player);
        StartCoroutine(StartLevelMessage());
		displayingMessage = false;

//		desertSound = GetComponent<AudioSource> ();

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

    IEnumerator LevelComplete()
    {
		alertText.text = string.Format("Level Complete in {0} Seconds, you got {1}/3 stars!", currentTime, GetStars());
        player.GetComponent<Rigidbody>().isKinematic = true;
        yield return new WaitForSeconds(2);
        alertText.text = "\n" + alertText.text + "\nPress Any Key to go to the next level";
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex+1);
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
            StartCoroutine(LevelComplete());
        }
        else if (!displayingMessage)
        {
            displayingMessage = true;
			StartCoroutine(NotFinishedMessage());
        }
    }

    IEnumerator StartLevelMessage()
    {
        alertText.text = string.Format("Level Goal: {0}/{1} Cats", minMorsels, totalMorsels);
        yield return new WaitForSeconds(3);
        alertText.text = "";
    }
	
	IEnumerator NotFinishedMessage() {
		alertText.text = string.Format("Need {0} Cats to Advance", minMorsels-morselCount);
		yield return new WaitForSeconds(2);
		alertText.text = "";
		displayingMessage = false;
	}

    IEnumerator GameOver()
    {
        alertText.text = "Game Over";
        yield return new WaitForSeconds(2);
        alertText.text = "\n" + alertText.text + "\nPress Any Key to Restart";
        gameOver = true;
    }

	void Timer()
	{
		if (alertText.text == "Game Over") { // change to death condition
			currentTime = 0;
		}
		else{
			timerText.text = "Time: " + Mathf.Round (currentTime).ToString ();
			currentTime += Time.deltaTime;
		}
	}
}
