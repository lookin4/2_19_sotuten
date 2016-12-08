using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NoteManager : MonoBehaviour
{

    public ScoreManager score;

    static public float spdRatio = 1.5f;

    int loopStartNum = 0;

    static public float bpm = 180;

    private List<float> appearTime = new List<float>();
    private List<int> appearNum = new List<int>();


    public void ClearInfo()
    {
        appearTime.Clear();
        appearNum.Clear();
    }


    public void SetInfo(int time, int num)
    {
        appearTime.Add(time);
        appearNum.Add(num);
    }


    void Update()
    {
        //float nowTime = SoundManager.GetBgmTime() / 1000.0f; // ミリ秒取得なので、秒に戻す

        //for (int i = loopStartNum; i < appearTime.Count; i++)
        //{
        //    // トータル拍数
        //    // time * (BPM / 60)
        //    if ((nowTime * (bpm / 60.0f)) >= appearTime[i])
        //    {
        //        CreateNote(GetLanePosX(appearNum[i]));
        //        ++loopStartNum;
        //        //break;
        //    }
        //}

    }


    void NoteInfoSet(string fname)
    {
        ClearInfo();

        TextAsset t = Resources.Load("NoteInfo/stage0") as TextAsset;
        //Debug.Log(t.text);

        string[] s = t.text.Split(new char[] { '\n', ',' });
        //Debug.Log(s.Length);

        //Debug.Log("time : " + s[0] + " num : " + s[1]);

        for (int i = 0; i < s.Length - 1; i += 2)
        {
            SetInfo(int.Parse(s[i]), int.Parse(s[i + 1]));
        }

    }


} // public class NoteManager : MonoBehaviour
