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
            DontDestroyOnLoad(gameObject); // �����ڳ����л�ʱ��������
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
            fruitCounts[fruitType]++; // ���Ӹ�ˮ�����͵�����
        }
        else
        {
            fruitCounts[fruitType] = 1; // ���γ��ִ���ˮ��������Ϊ1
        }
    }
    



    public Dictionary<string, int> GetFruitCounts()
    {
        return new Dictionary<string, int>(fruitCounts); ; // ����ˮ�������ֵ乩������ʹ��
    }
}