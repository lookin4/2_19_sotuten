using UnityEngine;
using System.Collections;

public class MyGUIUtil : MonoBehaviour
{

    static MyGUIUtil instance_;

    static public MyGUIUtil Instance
    {
        get { return instance_; }
    }


    void Start()
    {
        instance_ = this;
    }


    // -----------------------------------------------------------------------------
    // PointedAtGameObjectInfo

    public GUIStyle getGUIStyle(Color bgColor, Color textColor)
    {
        GUIStyle guiStyle = new GUIStyle();
        GUIStyleState styleState = new GUIStyleState();

        // GUI背景色のバックアップ 
        Color backColor = GUI.backgroundColor;

        // GUI背景の色を設定 
        GUI.backgroundColor = bgColor;

        // 背景用テクスチャを設定 
        styleState.background = Texture2D.whiteTexture;

        // テキストの色を設定 
        styleState.textColor = textColor;

        // スタイルの設定。 
        guiStyle.normal = styleState;

        return guiStyle;
    }


}
