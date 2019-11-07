using System.Collections;
using System.Collections.Generic;

public interface CanConfigureSceneInterface{
    void configure(Scene scene);
}


public class SceneUtil {
    // 私有构造函数
    private SceneUtil() {

    }

    public static SceneUtil getUtil() {
        // 暂时不使用单例，后续可以维护一个Scene LRU缓存，到时候可以替换为单例而接口不变
        return new SceneUtil();
    }


    public void configureFromScene(CanConfigureSceneInterface configureInterface, string sceneName) {
        Scene scene = loadScene(sceneName);
        configureInterface.configure(scene);    
    }

    private Scene loadScene(string sceneName) {
        if (sceneName == null) {
            DebugLogger.log("error");
            return null;
        }
        if (sceneName.Equals("test")) {
            Scene scene = new Scene();
            // TODO: 后续考虑将NPC一方固定为一个特殊玩家
            Player playerA = new Player("1","A");
            Player playerB = new Player("2","B");
            List<ChessBase> chesses = new List<ChessBase>();
            chesses.Add(new ChessBase(playerA, new ChessLocation(0,0)));
            chesses.Add(new ChessBase(playerA, new ChessLocation(1,1)));
            chesses.Add(new ChessBase(playerA, new ChessLocation(1,2)));
            chesses.Add(new ChessBase(playerB, new ChessLocation(5,6)));
            chesses.Add(new ChessBase(playerB, new ChessLocation(4,5)));
            chesses.Add(new ChessBase(playerB, new ChessLocation(5,4)));
            scene.chesses = chesses.ToArray();
            scene.map = new Map(6, 7);
            return scene;
        }
        return null;
    }

}
