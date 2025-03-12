using UnityEngine;

namespace COMP305
{
    public class MovingPlatform : MonoBehaviour
    {
        [SerializeField] private Transform startingPosition, endingPosition;
        [SerializeField] private float speed;
        Vector3 targetPosition;
        private Animator anim;

        private void Start()
        {
            targetPosition = endingPosition.position;
        }

        private void Update()
        {
            if (Vector2.Distance(transform.position, startingPosition.position) < 0.05f)
            {
                targetPosition = endingPosition.position;
            }

            if (Vector2.Distance(transform.position, endingPosition.position) < 0.05f)
            {
                targetPosition = startingPosition.position;
            }

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                collision.transform.parent = this.transform;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                collision.transform.parent = null;
            }
        }
    }
}
