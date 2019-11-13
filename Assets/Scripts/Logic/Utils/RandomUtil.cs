using System.Collections;
using System.Collections.Generic;

public class RandomUtil {
    private static RandomUtil instance = new RandomUtil();
    private static Queue<int> randomQueue;
    private RandomUtil() {
        randomQueue = new Queue<int>();
        init();
    }

    private void init () {
        for (int i = 0;i < 10000;i++) {
            System.Random r = new System.Random(System.Guid.NewGuid().GetHashCode());
            randomQueue.Enqueue(r.Next());
        } 
    }

    public static int next() {
        int result = randomQueue.Dequeue();
        randomQueue.Enqueue(result);
        return result;
    }
}