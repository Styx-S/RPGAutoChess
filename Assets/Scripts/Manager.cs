using System.Collections;
using System.Collections.Generic;

public interface ManagerInterface {
    /* 获取该Manager的名字 */
    string getName();
    
    /* 初始化消息 */
    void init();

    /* 新一次更新机会(逻辑帧) */
    void update();
}
