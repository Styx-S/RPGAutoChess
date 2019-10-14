using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityControllerCenter : MonoBehaviour
{
    private Queue<ControllerMessage> messageQueue = new Queue<ControllerMessage>();
    private Dictionary<ChessBase, GameObject> gameObjectDic = new Dictionary<ChessBase, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        dispose();
    }

    public static UnityControllerCenter getCenter() {
        UnityControllerCenter center = GameObject.FindGameObjectWithTag(CommonDefine.kControllerName)
            .GetComponent<UnityControllerCenter>();
        if (center == null) {
            GameObject.FindGameObjectWithTag(CommonDefine.kControllerName)
                .AddComponent<UnityControllerCenter>();
            return  GameObject.FindGameObjectWithTag(CommonDefine.kControllerName)
                .GetComponent<UnityControllerCenter>();
        }
        return center;
    }

    public void sendMessage(ControllerMessage message) {
        messageQueue.Enqueue(message);
    }

    private void dispose() {
        while(messageQueue.Count != 0) {
            ControllerMessage message = messageQueue.Dequeue();
            switch(message.type) {
            case ControllerMessageType.chessCreate:
                ControllerMessage_chessCreate create = (ControllerMessage_chessCreate)message;
                createChess(create.chess);
                break;
            case ControllerMessageType.chessRemove:
                ControllerMessage_chessRemove remove = (ControllerMessage_chessRemove)message;
                removeChess(remove.chess);
                break;
            case ControllerMessageType.chessMove:
                forwardMessage_move((ControllerMessage_chessMove)message);
                break;
            case ControllerMessageType.chessAttach:
                forwardMessage_attach((ControllerMessage_chessAttach)message);
                break;
            default:
                Debug.Log("不支持的控制器消息类型");
                break;
            }
        }
    }

    private void createChess(ChessBase chess) {
        GameObject simpleChess = (GameObject) Resources.Load(CommonDefine.kChessPrefabPath);
        GameObject instance = Instantiate(simpleChess);
        Transform transform =  instance.GetComponent<Transform>();
        transform.position = ChessUI.calTransformPosition(chess.location);
        gameObjectDic[chess] = instance;
    }

    private void removeChess(ChessBase chess) {
        if (gameObjectDic.ContainsKey(chess)) {
            GameObject instance = gameObjectDic[chess];
            gameObjectDic.Remove(chess);
            GameObject.Destroy(instance, 0);
            return;
        }
        Debug.Log("can't find chess");
    }

    private void forward(ChessBase chess, ChessAnimationArgs args) {
        if (gameObjectDic.ContainsKey(chess)) {
            GameObject instance = gameObjectDic[chess];
            ChessUI chessUI = instance.GetComponent<ChessUI>();
            if (chessUI != null) {
                chessUI.playAnimation(args);
                return;
            }
        }
        Debug.Log("can't find chess");
    }

    private void forwardMessage_move(ControllerMessage_chessMove message) {
        forward(message.chess, ChessAnimationMoveArgs.transfrom(message));
    }

    private void forwardMessage_attach(ControllerMessage_chessAttach message) {
        if (gameObjectDic.ContainsKey(message.attacher) && gameObjectDic.ContainsKey(message.victim)) {
            ChessUI attacher = gameObjectDic[message.attacher].GetComponent<ChessUI>(),
                victim = gameObjectDic[message.victim].GetComponent<ChessUI>();
            if (attacher != null && victim != null) {
                attacher.playAnimation(new ChessAnimationAttachArgs());
                victim.playAnimation(new ChessAnimationHurtArgs(message.causeDamage));
            }
        }
    }
}
