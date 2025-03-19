using UnityEngine;
using UnityEngine.UI;
namespace COMP305
{
    public class SliderControlledMove : MonoBehaviour
    {
        public Slider positionSlider;  // ����Slider
        public Transform objectToMove; // ������Ҫ�ƶ�������
        public float Transformvalue1 = 0;
        public float Transformvalue2 = 0;

        void Start()
        {
            // ��ʼ������λ��
            //UpdateObjectPosition(positionSlider.value);

            // ��Slider��ֵ�仯�¼�
            positionSlider.onValueChanged.AddListener(UpdateObjectPosition);
        }

        void UpdateObjectPosition(float sliderValue)
        {
            float positionA = Transformvalue1; // ��sliderֵΪ0ʱ��λ��
            float positionB = Transformvalue2; // ��sliderֵΪ1ʱ��λ��

            // ʹ�����Բ�ֵ�����µ�λ��
            float newPositionX = Mathf.Lerp(positionA, positionB, sliderValue);

            // ��������ľ���λ��
            objectToMove.position = new Vector3(newPositionX, objectToMove.position.y, objectToMove.position.z);

            // �����ǰSlider��ֵ�Ե���
            Debug.Log("Slider Value: " + sliderValue + ", New Position X: " + newPositionX);
        }
    }
}
