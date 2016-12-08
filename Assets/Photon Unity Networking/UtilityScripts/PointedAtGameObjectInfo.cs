using UnityEngine;
using System.Collections;

[RequireComponent(typeof(InputToEvent))]
public class PointedAtGameObjectInfo : MonoBehaviour
{
    void OnGUI()
    {
        if (InputToEvent.goPointedAt != null)
        {
            PhotonView pv = InputToEvent.goPointedAt.GetPhotonView();
            if (pv != null)
            {
                var rect = new Rect(
                    /* x */ Input.mousePosition.x + 5,
                    /* y */ Screen.height - Input.mousePosition.y - 15,
                    /* w */ 300, /* h */ 30);

                var str = string.Format(
                    "ViewID({0}) {1} {2}",
                    pv.viewID,
                    (pv.isSceneView) ? "scene" : "",
                    (pv.isMine) ? "mine" : "owner: " + pv.ownerId);

                GUI.Label(rect, str, getGUIStyle(new Color(0.0f, 0.0f, 0.0f, 0.5f), Color.white));
            }
        }
    }

    GUIStyle getGUIStyle(Color bgColor, Color textColor)
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
