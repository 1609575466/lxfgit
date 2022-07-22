using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PoolManager : MonoBehaviour
{
    [Serializable]
    //对象池类
    public class Pool
    {
        public string tag;//对象池的名称
        public GameObject prefab;//对象池保存的物体
        public int size;//对象池大小
    }
    public List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> poolDictionary;//对象池字典

    public static PoolManager Instance;//单例模式

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //实例化字典，key为对象池名称tag，value为对象池保存的物体
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (Pool pool in pools)
        {
            Queue<GameObject> poolQueue = new  Queue<GameObject>();//为数组中的每个对象池都创建一个队列
            for (int i = 0; i < pool.size; i++)
            {
                GameObject go = Instantiate(pool.prefab);//以对象池中的预制体设置物体对象
                go.SetActive(false);//隐藏生成的对象
                poolQueue.Enqueue(go);//将隐藏的对象加入队列
            }
            poolDictionary.Add(pool.tag,poolQueue);
        }
    }
    
    //从已创建的对象池中获取对象
    public GameObject GetFromPool(string poolTag,Vector3 position,Quaternion rotation)
    {
        //对象池字典暂不存在该对象池时
        if (!poolDictionary.ContainsKey(poolTag))
        {
            Debug.Log("Pool[" + poolTag + "] is not exist!");
        }

        GameObject acquiredGo = poolDictionary[poolTag].Dequeue();//从该Tag的对象池出队对象
        
        acquiredGo.transform.position = position;//设置获取到的对象的位置
        acquiredGo.transform.rotation = rotation;//设置获取到的对象的旋转值
        acquiredGo.SetActive(true);//将之前隐藏过的对象激活
        
        poolDictionary[poolTag].Enqueue(acquiredGo);//再次入队，可以重复使用，如果需要的对象数量超过了
                                                    //对象池的数量再考虑扩大对象池
                                                    //这样重复使用时就不用重复生成和销毁对象，节约性能
        return acquiredGo;//返回对象
    }
}
