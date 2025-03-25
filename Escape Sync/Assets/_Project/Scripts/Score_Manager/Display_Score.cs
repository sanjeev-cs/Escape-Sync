using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class FruitCountDisplay : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro; // TextMesh Pro�����������ʾ����
    public string fruitType; // Ĭ��Ϊ��Apple�������Ը�����Ҫ��Inspector�и���

    private void Update()
    {
        // ȷ��GameManagerʵ������
        if (GameManager.Instance != null)
        {
            // ��ȡָ��ˮ��������
            Dictionary<string, int> fruitCounts = GameManager.Instance.GetFruitCounts();
            int fruitCount = fruitCounts.TryGetValue(fruitType, out int count) ? count : 0;
            UpdateFruitDisplay(fruitCount);
        }
    }

    private void UpdateFruitDisplay(int fruitCount)
    {
        // ����TextMesh Pro�ı���������ʾˮ�����ƺ�����
        textMeshPro.text = fruitCount.ToString(); 
    }
}