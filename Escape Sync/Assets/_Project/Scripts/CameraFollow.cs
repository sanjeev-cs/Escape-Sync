using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace COMP305
{
    public class CameraFollow : MonoBehaviour
    {
        [Header("Camera Variables")]
        [SerializeField] private Transform target;
        //[SerializeField] private List<Transform> target = new List<Transform>();
        [SerializeField] private Vector3 Offset;
        [Range(1, 10)]
        [SerializeField] private float smoothFactor;

        [Header("Camera Bounds")]
        [SerializeField] private Vector3 minValue;
        [SerializeField] private Vector3 maxValue;


        private void FixedUpdate()
        {
            Follow();
        }
        private void Follow()
        {
            Vector3 targetPosition = target.position + Offset;

            // Define minimum x,y,z values and maximum x,y,z values
            // Verify if the targetPosiiton is out of the bound or not
            //Limit it to minimum and maximum values
            Vector3 boundPosition = new Vector3(
                Mathf.Clamp(targetPosition.x, minValue.x, maxValue.x),
                Mathf.Clamp(targetPosition.y, minValue.y, maxValue.y),
                Mathf.Clamp(targetPosition.z, minValue.z, maxValue.z)
                );

            Vector3 smoothPosition = Vector3.Lerp(transform.position, boundPosition, smoothFactor * Time.deltaTime);
            transform.position = smoothPosition;
        }
    }
}
