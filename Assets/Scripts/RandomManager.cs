using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomManager : ManagerInterface //硬编码部分之后改
{
    private Queue<int> randomQueue = new Queue<int>();
    string ManagerInterface.getName() {
        return CommonDefine.kManagerRandomName;
    }
    void ManagerInterface.init() {
        for (int i = 0;i < 10000;i++) {
            System.Random r = new System.Random(System.Guid.NewGuid().GetHashCode());
            randomQueue.Enqueue(r.Next());
        }      
    }
    void ManagerInterface.update() {
    
    }

    public int next() {
        int result = randomQueue.Dequeue();
        randomQueue.Enqueue(result);
        return result;
    }
}
