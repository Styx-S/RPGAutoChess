﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterModule_Chess : IUnityControllerCenterModule
{
    private bool hasInit = false;   // 在收到全量信息后才处理增量信息
    private Dictionary<ChessBase, GameObject> gameObjectDic = new Dictionary<ChessBase, GameObject>();

    bool IUnityControllerCenterModule.tryHandleMessage(MessageBase message) {

        if (message.type != MessageInfoType.Chess) {
            return false;
        }
        
        if (message.kindType == MessageInfoKindType.Completion) {
            hasInit = true;
            ChessCompletionMessage completionMessage = (ChessCompletionMessage)message;
            // 棋子全量信息处理：创建每个棋子对应的GameObject/脚本
            if (completionMessage.chessNum > 0) {
                foreach(ChessBase chess in completionMessage.data) {
                    createChess(chess);
                }
            }
        } else if (message.kindType == MessageInfoKindType.Increment && hasInit) {
            switch((ChessIncrementMessageType)message.messageId) {
            case ChessIncrementMessageType.chessCreate:
                ChessIncrementMessage_chessCreate create = (ChessIncrementMessage_chessCreate)message;
                createChess(create.chess);
                break;
            case ChessIncrementMessageType.chessRemove:
                ChessIncrementMessage_chessRemove remove = (ChessIncrementMessage_chessRemove)message;
                removeChess(remove.chess);
                break;
            case ChessIncrementMessageType.chessMove:
                forwardMessage_move((ChessIncrementMessage_chessMove)message);
                break;
            case ChessIncrementMessageType.chessAttach:
                forwardMessage_attack((ChessIncrementMessage_chessAttach)message);
                break;
            default:
                Debug.Log("不支持的控制器消息类型");
                break;
            }
        }

        return true;
    }

        private void createChess(ChessBase chess) {
        GameObject simpleChess = (GameObject) Resources.Load(CommonDefine.kChessPrefabPath);
        GameObject instance = GameObject.Instantiate(simpleChess);
        Transform transform =  instance.GetComponent<Transform>();
        transform.position = ChessUI.calTransformPosition(chess.location);
        gameObjectDic[chess] = instance;
        ChessUI ui = instance.GetComponent<ChessUI>();
        ui.initColor(chess.owner);
        ui.chess = chess;
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
            // 同步ChessBase的状态
            chessUI.chess = chess;
            if (chessUI != null) {
                chessUI.playAnimation(args);
                return;
            }
        }
        Debug.Log("can't find chess");
    }

    private void forwardMessage_move(ChessIncrementMessage_chessMove message) {
        forward(message.chess, ChessAnimationMoveArgs.transFrom(message));
    }

    private void forwardMessage_attack(ChessIncrementMessage_chessAttach message) {
        if (gameObjectDic.ContainsKey(message.attacher) && gameObjectDic.ContainsKey(message.victim)) {
            ChessUI attacher = gameObjectDic[message.attacher].GetComponent<ChessUI>(),
                victim = gameObjectDic[message.victim].GetComponent<ChessUI>();
            if (attacher != null && victim != null) {
                // 更新一下状态
                attacher.chess = message.attacher;
                victim.chess = message.victim;
                attacher.playAnimation(new ChessAnimationAttachArgs());
                victim.playAnimation(new ChessAnimationHurtArgs(message.causeDamage));
            }
        }
    }
}
