using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour
{


    static ScoreManager instance_;

    public static ScoreManager Instance
    {
        get { return instance_; }
    }


    private Text scoreText_;
    int score_ = 0;


    void Awake()
    {
        instance_ = this;

        var obj = GameObject.Find("Canvas/TotalScoreText");
        scoreText_ = obj.GetComponent<Text>();
    }


    void Update()
    {
        scoreText_.text = "Score : " + score_;

    }


    public void AddScore(int add) { score_ += add; }


}
