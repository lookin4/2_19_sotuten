using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreText : MonoBehaviour
{


    int frame_ = 0;


    void Start()
    {

    }


    public void SetText(string text) { GetComponent<Text>().text = text; }


    void Update()
    {

        var pos = transform.position;
        pos.y += 0.75f;

        transform.position = pos;


        if (++frame_ > 60)
        {
            Destroy(gameObject);
        }

    }


}
