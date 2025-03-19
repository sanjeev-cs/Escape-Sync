using UnityEngine;
using UnityEngine.UI;
namespace COMP305
{
    public class SliderControlledMove : MonoBehaviour
    {
        public Slider positionSlider;  // 引用Slider
        public Transform objectToMove; // 引用需要移动的物体
        public float Transformvalue1 = 0;
        public float Transformvalue2 = 0;

        void Start()
        {
            // 初始化物体位置
            //UpdateObjectPosition(positionSlider.value);

            // 绑定Slider的值变化事件
            positionSlider.onValueChanged.AddListener(UpdateObjectPosition);
        }

        void UpdateObjectPosition(float sliderValue)
        {
            float positionA = Transformvalue1; // 当slider值为0时的位置
            float positionB = Transformvalue2; // 当slider值为1时的位置

            // 使用线性插值计算新的位置
            float newPositionX = Mathf.Lerp(positionA, positionB, sliderValue);

            // 更新物体的绝对位置
            objectToMove.position = new Vector3(newPositionX, objectToMove.position.y, objectToMove.position.z);

            // 输出当前Slider的值以调试
            Debug.Log("Slider Value: " + sliderValue + ", New Position X: " + newPositionX);
        }
    }
}
