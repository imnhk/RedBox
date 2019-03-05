using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static int score;
    public Text scoreText;

	// Use this for initialization
	void Start () {
        score = 0;
	}
	
	// Update is called once per frame
	void Update () {
        scoreText.text = "" + score;

        if (Input.GetKeyDown("r"))
            Application.LoadLevel("WhiteBox");
            
	}
}
