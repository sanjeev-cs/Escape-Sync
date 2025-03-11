using UnityEngine;

namespace COMP305
{
    public class ParallaxBackground : MonoBehaviour
    {
        [SerializeField] private Vector2 parallaxEffectMultiplier;
        private Transform cameraTrasform;
        private Vector3 lastCameraPosition;
        private float textureUnitSizeX;

        private void Start()
        {
            cameraTrasform = Camera.main.transform;
            lastCameraPosition = cameraTrasform.position;
            Sprite sprite = GetComponent<SpriteRenderer>().sprite;
            Texture2D texture = sprite.texture;
            textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
        }

        private void LateUpdate()
        {
            Vector3 deltaMovement = cameraTrasform.position - lastCameraPosition;
            transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier.x, deltaMovement.y * parallaxEffectMultiplier.y);
            lastCameraPosition = cameraTrasform.position;

            //if (Mathf.Abs(cameraTrasform.position.x - transform.position.x) >= textureUnitSizeX)
            //{
            //    float offsetPositionX = (cameraTrasform.position.x - transform.position.x) % textureUnitSizeX;
            //    transform.position = new Vector3(cameraTrasform.position.x + offsetPositionX, transform.position.y);
            //}
        }
    }
}
