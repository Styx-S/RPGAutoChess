using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* ChessUI: 挂在Chess Sprite上进行控制的脚本 */
public class ChessUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /* 处理棋子被点击事件 */
    public void OnMouseDown() {

    }

    /* 播放棋子动效 */
    public void playAnimation(ChessAnimationArgs args) {
        switch(args.animationType) {
            case ChessAnimationType.Move:
                ChessAnimationMoveArgs moveArgs = (ChessAnimationMoveArgs) args;
                break;
            case ChessAnimationType.Attach:
                ChessAnimationAttachArgs attachArgs = (ChessAnimationAttachArgs) args;
                break;
            case ChessAnimationType.Hurt:
                ChessAnimationHurtArgs hurtArgs = (ChessAnimationHurtArgs) args;
                break;
            default:
                Debug.Log("无法支持的动效类型");
                break;
        }
    }

    /* 在棋子头上显示信息 */
    private void showMessage(string message) {

    }
}
