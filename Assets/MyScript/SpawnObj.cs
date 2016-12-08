using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnObj : Photon.MonoBehaviour
{

    GameObject prefabMogura_;

    int frame_;
    public string userId_;

    int loopStartNum = 0;
    static public float bpm = 200;
	string musicTitle;
	int level;

    private List<float> appearTime_ = new List<float>();
    private List<int> appearLane_ = new List<int>();


    void Start()
    {
        prefabMogura_ = Resources.Load("Sphere") as GameObject;

        //object userName;
        //object userId;
        //photonView.owner.customProperties.TryGetValue("userName", out userName);
        //photonView.owner.customProperties.TryGetValue("userId", out userId);

        string userName = "Name : aaaa";
        //string userId = "000" + (PhotonNetwork.room.playerCount - 1);
        string userId = "000" + (PhotonNetwork.room.playerCount);
        //PhotonNetwork.autoCleanUpPlayerObjects = false; // room入ってる間は設定できない

        // ルームプロパティ
        ExitGames.Client.Photon.Hashtable customProp = new ExitGames.Client.Photon.Hashtable();
        customProp.Add("userName", userName);
        customProp.Add("userId", userId);

        photonView.owner.SetCustomProperties(customProp);

        Debug.Log("userName(" + userName + ")  userId(" + userId + ")");

        userId_ = userId.ToString();


        //ノーツの設定 
        NoteInfoSet("Holiday");

        SoundManager.Instance.PlayBGM(SoundManager.BGM.Main);

    }


    int lane = 0;
    void Update()
    {
        if (!PhotonNetwork.inRoom) return;

        // master(1) + client(5) = total (6)
        // client が揃うまでは、処理をしない
        //if (PhotonNetwork.room.playerCount < 6) return;

        if (photonView.isMine &&
            PhotonNetwork.isMasterClient)
        {

            float nowTime = SoundManager.Instance.GetBgmTime(SoundManager.BGM.Main);
            Debug.Log("time:" + nowTime);

            for (int i = loopStartNum; i < appearTime_.Count; i++)
            {
                // トータル拍数
                // time * (BPM / 60)
                float total = nowTime * (bpm / 60.0f);

                float offset = (bpm / 60.0f / 60.0f * 30.0f); // 30 frame 分 早く出現させる

                if (total + offset >= appearTime_[i])
                {
                    var diff = (total + offset) - appearTime_[i]; // 拍

                    var fdiff = diff / (bpm / 60.0f / 60.0f); // diff -> frame変換

                    CreateMogura(appearLane_[i], fdiff);
                    //CreateNote(GetLanePosX(appearNum[i]));
                    ++loopStartNum;
                    //break;
                }
            }


        }

    }

    /// <summary>
    /// mogura 作成
    /// </summary>
    /// <param name="lane">0 ~ 4</param>
    void CreateMogura(int lane, float fdiff)
    {
        float x = -5.0f + 2.5f * lane;

        // 0: -5.0f
        // 1: -2.5f
        // 2: -0.0f
        // 3:  2.5f
        // 4:  5.0f

        // 0.5f - 1.5f = -1.0f

        // spd: 0.05f / frame


        var pos = new Vector3(x, -1.0f + fdiff * 0.05f, 10.0f);

        var obj = PhotonNetwork.Instantiate(
            prefabMogura_.name,
            pos,
            Quaternion.identity,
            0);

        obj.GetComponent<MoguraController>().SetLane(lane);

    }


    public void ClearInfo()
    {
        appearTime_.Clear();
        appearLane_.Clear();
    }


    public void SetInfo(float time, int num)
    {
        appearTime_.Add(time);
        appearLane_.Add(num);
    }
		

	void NoteInfoSet(string fname)
	{
		ClearInfo();

		TextAsset t = Resources.Load("NoteInfo/" + fname) as TextAsset;
		Debug.Log(t.text);

		string[] s = t.text.Split(new char[] { '\n', ',' });
		//Debug.Log(s[0]);

		//Debug.Log("time : " + s[0] + " num : " + s[1]);
		int idx = 0;
		while (true) {
			if (s [idx] == "Title") {
				musicTitle = s [++idx];
			} 
			else if (s [idx] == "BPM") {
				bpm = float.Parse (s [++idx]);
			}
			else if(s[idx] == "level"){
				level = int.Parse (s [++idx]);
			}
			else if(s[idx] == "NotesNum"){
				idx += 6;
				break;
			}

			idx++;
		}

		//Debug.Log (s [idx]);
		for (int i = idx; i < s.Length - 1; i += 6)
		{
			//Debug.Log (s [i]);
			for (int j = 1; j < 6; j++) {
				if (s [i + j] != "") {
					//Debug.Log (s [i + j]);
					SetInfo(float.Parse(s[i]), j - 1);
				}
			}
			//SetInfo(float.Parse(s[i]), int.Parse(s[i + 1]));
		}

	}


    void OnClick()
    {

        if (!PhotonNetwork.inRoom)
        {
            // only use PhotonNetwork.Instantiate while in a room.
            return;
        }

        object userId;
        photonView.owner.customProperties.TryGetValue("userId", out userId);

        //CreateMogura(photonView.ownerId);
        //CreateMogura(int.Parse(userId.ToString()));

    }


    void OnGUI()
    {
        var pos = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);

        var rect = new Rect(
             /* x */ pos.x - 50,
             /* y */ -pos.y + 200 + Screen.height / 2,
             /* w */ 100, /* h */ 20);

        var str = string.Format(
            " userId_({0})",
            userId_);

        GUI.Label(rect, str, MyGUIUtil.Instance.getGUIStyle(new Color(0.0f, 0.0f, 0.0f, 0.5f), Color.white));

    }


}
