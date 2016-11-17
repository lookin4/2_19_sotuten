using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClientManager : MonoBehaviour
{


    static ClientManager instance_;

    public static ClientManager Instance
    {
        get { return instance_; }
    }


    public int number_;
    private Text numText_;


    void Awake()
    {
        instance_ = this;

        var obj = GameObject.Find("Canvas/ClientNumber");
        numText_ = obj.GetComponent<Text>();

    }


    void Update()
    {
        //numText_.text = "client_number : " + number_;

        GameObject obj = GameObject.Find("Cube(Clone)");

        if (obj == null) return;

        string id = obj.GetComponent<SpawnObj>().userId_;
        numText_.text = "userId_ : " + id;
        number_ = int.Parse(id);


        // --------------------------------------
        // camera pos update

        var camera = GameObject.Find("Main Camera").GetComponent<Camera>();

        /* case 1 -> master */
        switch (number_)
        {
            case 2:
                camera.transform.position = new Vector3(-5f, 0.5f, 9.0f);
                break;
            case 3:
                camera.transform.position = new Vector3(-2.5f, 0.5f, 9.0f);
                break;
            case 4:
                camera.transform.position = new Vector3(0f, 0.5f, 9.0f);
                break;
            case 5:
                camera.transform.position = new Vector3(2.5f, 0.5f, 9.0f);
                break;
            case 6:
                camera.transform.position = new Vector3(5f, 0.5f, 9.0f);
                break;

        }



    }


}
