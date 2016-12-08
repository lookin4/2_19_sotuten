using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MoguraController : Photon.MonoBehaviour
{

    static KeyCode[] keyAssign = {
        KeyCode.A,
        KeyCode.S,
        KeyCode.D,
        KeyCode.F,
        KeyCode.G,
    };

    enum STATUS
    {
        UP,
        DOWN
    }

    // var

    private const int DeleteCount = 240;

    private int laneNum_ = 0; // モグラ番号
    private STATUS status_ = STATUS.UP;

    public bool DestroyByRpc;

    // func

    void Start()
    {

    }


    public void SetLane(int lane)
    {
        //photonView.TransferOwnership(1); // master 所有     いらない。
        laneNum_ = lane;
    }


    void Update()
    {

        if (photonView.isMine)
        {
            Move();

            // 対応するボタンが押された瞬間
            if (Input.GetKeyDown(keyAssign[laneNum_]))
            {
                Judge();
            }

        }
        else
        {

        }

    }


    void Move()
    {
        Vector3 spd = new Vector3();

        //上に出てくる処理。上限まで来たら下がる処理へ移行
        if (status_ == STATUS.UP)
        {
            if (transform.position.y <= 0.5f)
            {
                spd.y = 0.05f;/*= new Vector3(0, 0.05f, 0);*/
                //Vector3 pos = transform.position;
                //pos.y += 0.05f;
                //transform.position = pos;
                transform.position += spd;
            }
            else
            {
                status_ = STATUS.DOWN;
            }
        }
        //下に降りる処理。下限まで来たら待ち状態へ移行
        else if (status_ == STATUS.DOWN)
        {
            if (transform.position.y >= -1.0f)
            {
                spd.y = -0.05f;/*= new Vector3(0, 0.05f, 0);*/
                //Vector3 pos = transform.position;
                //pos.y -= 0.05f;
                //transform.position = pos;
                transform.position += spd;
            }
            else
            {
                ShouldDestroy();
            }
        }

        GetComponent<PhotonTransformView>().SetSynchronizedValues(speed: spd, turnSpeed: 0);

    }


    void Judge()
    {
        //position.yは-1.0f~0.5f間で移行0.5fに近いほど高得点
        if (transform.position.y > 0.4f)
        {
            CreateScoreText("Perfect!");
            ScoreManager.Instance.AddScore(1000);
            ShouldDestroy();
        }
        else if (transform.position.y > 0.2f)
        {
            CreateScoreText("Nice!");
            ScoreManager.Instance.AddScore(500);
            ShouldDestroy();
        }
        else /*if (transform.position.y > 0)*/
        {
            CreateScoreText("Bad..");
            ScoreManager.Instance.AddScore(100);
            ShouldDestroy();
        }

    }


    void CreateScoreText(string text)
    {
        GameObject prefab = Resources.Load("ScoreTextPrefab") as GameObject;
        GameObject obj = Instantiate(prefab);

        var parent = GameObject.Find("Canvas");
        obj.transform.SetParent(parent.transform, false);

        var pos = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);
        pos.y += 15.0f;
        obj.transform.position = pos;

        obj.GetComponent<ScoreText>().SetText(text);

    }


    void OnGUI()
    {
        var pos = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);

        var rect = new Rect(
             /* x */ pos.x - 100,
             /* y */ -pos.y + 200 + Screen.height / 2,
             /* w */ 200, /* h */ 20);

        int num = ClientManager.Instance.number_;

        var str = string.Format(
            "lane({0}) ViewID({1}) {2} {3}",
            laneNum_,
            photonView.viewID,
            (photonView.isSceneView) ? "scene" : "",
            (photonView.isMine) ? "mine" : "owner: " + photonView.ownerId);

        GUI.Label(rect, str, MyGUIUtil.Instance.getGUIStyle(new Color(0.0f, 0.0f, 0.0f, 0.5f), Color.white));

    }


    // -----------------------------------------------------------------------------
    // OnClickDestroy()

    public void ShouldDestroy()
    {
        if (!DestroyByRpc)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
        else
        {
            this.photonView.RPC("DestroyRpc", PhotonTargets.AllBuffered);
        }
    }


    [PunRPC]
    public IEnumerator DestroyRpc()
    {
        GameObject.Destroy(this.gameObject);
        yield return 0; // if you allow 1 frame to pass, the object's OnDestroy() method gets called and cleans up references.
        PhotonNetwork.UnAllocateViewID(this.photonView.viewID);
    }


}
