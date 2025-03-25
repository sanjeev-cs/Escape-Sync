using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private Dictionary<string, int> fruitCounts = new Dictionary<string, int>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 保持在场景切换时不被销毁
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddFruit(string fruitType)
    {
        if (fruitCounts.ContainsKey(fruitType))
        {
            fruitCounts[fruitType]++; // 增加该水果类型的数量
        }
        else
        {
            fruitCounts[fruitType] = 1; // 初次出现此类水果，设置为1
        }
    }
    



    public Dictionary<string, int> GetFruitCounts()
    {
        return new Dictionary<string, int>(fruitCounts); ; // 返回水果数量字典供其他类使用
    }
}