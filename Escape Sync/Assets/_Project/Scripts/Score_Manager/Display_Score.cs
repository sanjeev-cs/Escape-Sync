using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class FruitCountDisplay : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro; // TextMesh Pro组件，用于显示计数
    public string fruitType; // 默认为“Apple”，可以根据需要在Inspector中更改

    private void Update()
    {
        // 确保GameManager实例存在
        if (GameManager.Instance != null)
        {
            // 获取指定水果的数量
            Dictionary<string, int> fruitCounts = GameManager.Instance.GetFruitCounts();
            int fruitCount = fruitCounts.TryGetValue(fruitType, out int count) ? count : 0;
            UpdateFruitDisplay(fruitCount);
        }
    }

    private void UpdateFruitDisplay(int fruitCount)
    {
        // 更新TextMesh Pro文本内容以显示水果名称和数量
        textMeshPro.text = fruitCount.ToString(); 
    }
}