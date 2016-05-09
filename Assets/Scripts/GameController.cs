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
    public int minMorsels;
	public int goalTime;

	float currentTime = 0.0f; //here

    private int totalMorsels;
    private int morselCount;
    private bool gameOver;
	private bool displayingMessage;
    private GameObject cam;

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
    }

    void Update()
    {
        if(gameOver && Input.anyKey)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
		//Restart level
		//Load each level 
		//Moon Jump
		//Increase Speed
		//Decrease Speed
		//Teleport to finish
		Timer ();
    }

    IEnumerator LevelComplete()
    {
		alertText.text = string.Format("Level Complete in {0} Seconds, you got {1}/3 stars!", currentTime, GetStars());
        player.GetComponent<Rigidbody>().isKinematic = true;
        yield return new WaitForSeconds(2);
        alertText.text = "\n" + alertText.text + "\nPress Any Key to go to the next level";
        gameOver = true;
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
