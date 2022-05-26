using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
	void Awake()
	{
		Time.timeScale = 0.0f;
	}

	public Text scoreText;

	public void StartButtonClicked()
	{
		foreach (Transform eachChild in transform)
		{
			if (eachChild.name != "Score")
			{
				Debug.Log("Child found. Name: " + eachChild.name);
				// disable them
				eachChild.gameObject.SetActive(false);
				Time.timeScale = 1.0f;
			}
		}
		scoreText.text = "Score: 0"; 
	}

	public void EndGame()
	{
		foreach (Transform eachChild in transform)
		{
			if (eachChild.name != "Score")
			{
				Debug.Log("Child found. Name: " + eachChild.name);
				// disable them
				eachChild.gameObject.SetActive(true);
			}
		}
	}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0.0f)
		{
			EndGame();
		}
    }
}
