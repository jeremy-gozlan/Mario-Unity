using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    // Start is called before the first frame update

    public Text scoreText;
    int score = 0;

    void Start()
    {
        scoreText = GetComponent<Text>();
    }

    // Update is called once per frame

    public void updateScore()
    {
        score +=1;
        scoreText.text = "World INF500 \nCoins:" + score.ToString();

    }


    void Update()
    {
    
    }
}
