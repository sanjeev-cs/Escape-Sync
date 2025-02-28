using UnityEngine;

namespace COMP305
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private float followSpeed = 2.0f;
        [SerializeField] private Transform target;
        [SerializeField] private float yOffset = 1f;

        private void Update()
        {
            Vector3 newPosition = new Vector3(target.position.x, target.position.y + yOffset, -10.0f);
            transform.position = Vector3.Slerp(transform.position, newPosition, followSpeed * Time.deltaTime);
        }
    }
}
