using UnityEngine;
using System.Collections;

public class SpawnObj : Photon.MonoBehaviour
{

    GameObject prefabMogura_;

    int frame_;

    const int MoguraMax = 5;
    int[] appearCount_ = { 120, 240, 360, 480, 600 }; // 単位(frame)
    int[] appearLane_ = { 1, 2, 3, 4, 5 }; // 0 は master の為、使わない

    //int[] appearCount_ = new int[MoguraMax];
    //int[] appearOwner_ = new int[MoguraMax];

    public string userId_;


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
        PhotonNetwork.autoCleanUpPlayerObjects = false;

        // ルームプロパティ
        ExitGames.Client.Photon.Hashtable customProp = new ExitGames.Client.Photon.Hashtable();
        customProp.Add("userName", userName);
        customProp.Add("userId", userId);

        photonView.owner.SetCustomProperties(customProp);

        Debug.Log("userName(" + userName + ")  userId(" + userId + ")");

        userId_ = userId.ToString();

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

            //---------------------------------
            // debug code

            if (++frame_ % 60 == 0)
            {
                CreateMogura(lane % 5);
                lane++;
            }

            return;


            ////////////////////////////////////////

            frame_++;

            for (int i = 0; i < MoguraMax; i++)
            {
                if (appearCount_[i] == frame_)
                {
                    CreateMogura(appearLane_[i]);
                }
            }

            // ----------------------------------
            // debug
            if (Input.GetKeyDown(KeyCode.Space))
            {
                frame_ = 0;
            }


        }

    }


    void CreateMogura(int lane)
    {
        //float x = -5.0f + 2.5f * (lane - 1);
        float x = -5.0f + 2.5f * lane;

        // 0: -5.0f
        // 1: -2.5f
        // 2: -0.0f
        // 3:  2.5f
        // 4:  5.0f

        var pos = new Vector3(x, -1.0f, 10.0f);

        var obj = PhotonNetwork.Instantiate(
            prefabMogura_.name,
            pos,
            Quaternion.identity,
            0);

        obj.GetComponent<MoguraController>().SetLane(lane);

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
        CreateMogura(int.Parse(userId.ToString()));

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
