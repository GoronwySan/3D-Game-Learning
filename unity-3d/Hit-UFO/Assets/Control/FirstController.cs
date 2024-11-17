using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstController : MonoBehaviour
{
    public List<GameObject> goList;
    private SSDirector director;
    public List<int> goList_usedAndFree; //0代表飞碟空闲，1代表飞碟忙碌



    public void Init(SSDirector director)
    {
        this.director = director;
        this.director.firstcontroller = this;
        // 初始化列表
        goList = new List<GameObject>();
        goList_usedAndFree = new List<int>();
        director.firstcontroller.LoadResources();
    }

    void Awake()
    {
        director = SSDirector.getInstance();
        director.firstcontroller = this;
        director.firstcontroller.LoadResources();
    }

    // Start is called before the first frame update
    public void LoadResources()
    {
        LoadUFOs("UFO", 20);

        for (int i = 0; i < goList.Count; ++i)
        {
            goList_usedAndFree.Add(0);
            hideUFO(goList[i]);
        }

        // 再添加其他依赖于 FirstController 的组件
        //ccActionManager = gameObject.AddComponent<CCActionManager>();
        //pickupObject = gameObject.AddComponent<PickupObject>();
        //view = gameObject.AddComponent<View>();


        //Debug.Log("所有控制脚本已成功挂载并初始化。");
    }

    private void LoadUFOs(string ufoType, int count)
    {
        for (int i = 0; i < count; i++)
        {
            // 添加飞碟到列表并设置相关组件
            GameObject ufo = Instantiate((GameObject)Resources.Load($"Prefabs/{ufoType}"));
            ufo.name = "UFO";

            // 添加飞碟控制脚本
            ufo.AddComponent<UFOController>();

            // 将飞碟添加到列表中
            goList.Add(ufo);
        }
        //for (int i = 0; i < count; i++)
        //{
        //    goList.Add(Instantiate((GameObject)Resources.Load($"Prefabs/{ufoType}")));
        //}
        //GameObject ufoPrefab = Resources.Load<GameObject>($"Prefabs/{ufoType}");
        ////if (ufoPrefab == null)
        ////{
        ////    Debug.LogError($"Prefab {ufoType} not found in Resources/Prefabs.");
        ////    return;
        ////}
        //goList.Add(Instantiate(ufoPrefab));

    }


    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < goList_usedAndFree.Count; ++i)
        {
            if (goList_usedAndFree[i] == 0)
            {
                director.ccam.UFOToDestination(i);
            }
        }
    }

    public void ReStartGame()
    {
        for (int i = 0; i < goList.Count; ++i)
        {
            goList_usedAndFree[i] = 0;
        }
    }

    public int getGameObjectIndex(Vector3 position)
    {
        for (int i = 0; i < goList.Count; ++i)
        {
            if (position == goList[i].transform.position)
            {
                //goList_usedAndFree[i] = 1;
                return i;
            }
            
        }
        return -1;
    }

    //隐藏UFO
    private void hideUFO(GameObject go)
    {
        go.transform.position = new Vector3(0, 0, -5);
    }
}
