using System.Collections.Generic;
using UnityEngine;

namespace COMP305
{
    [RequireComponent(typeof(Camera))]
    public class MultipleTargetCamera : MonoBehaviour
    {
        [SerializeField] private List<Transform> targets = new List<Transform>();
        [SerializeField] private Vector3 offset;
        [SerializeField] private float smoothTime = 0.5f;
        [SerializeField] private float minZoom = 40.0f;
        [SerializeField] private float maxZoom = 10.0f;
        [SerializeField] private float zoomLimit = 50.0f;

        private Vector3 velocity;
        private Camera cam;

        private void Start()
        {
            cam = GetComponent<Camera>();
        }

        // LateUpdate is called just after the Update method.
        private void LateUpdate()
        {
            // If there are no targets || targets = 0 
            if (targets.Count == 0)
            {
                return;
            }

            Move();
            Zoom();
        }

        private void Zoom()
        {
            //Debug.Log(GetGreatestDistance());
            float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimit);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
        }

        private void Move()
        {
            Vector3 centerPoint = GetCenterPoint(); // Storing the centerpoint of targets
            Vector3 newPosition = centerPoint + offset; // adding some offset to camaera before updating the position
            transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
        }

        private float GetGreatestDistance()
        {
            var bounds = new Bounds(targets[0].position, Vector3.zero);
            for (int i = 0; i < targets.Count; i++)
            {
                bounds.Encapsulate(targets[i].position);
            }

            return bounds.size.x;
        }

        // Method to get the centerppoint of targets
        private Vector3 GetCenterPoint()
        {
            // If there is only one target
            if (targets.Count == 1)
            {
                return targets[0].position; // return the target's position as a centerpoint
            }

            // Stroring the boound(box) around a target
            var bounds = new Bounds(targets[0].position, Vector3.zero);

            // Loop to create bound(box) around the all targets
            for (int i = 0; i < targets.Count; i++)
            {
                bounds.Encapsulate(targets[i].position);
            }

            return bounds.center; // return the center point of the bound(box)
        }
    }
}
