using UnityEngine;

namespace COMP305
{
    public class EnemyPatrol_MetalHead : MonoBehaviour
    {
        [Header("Patrol Points")]
        [SerializeField] private Transform leftEdge;
        [SerializeField] private Transform rightEdge;

        [Header("Enemy")]
        [SerializeField] private Transform enemy;

        [Header("Movement Parameter")]
        [SerializeField] private float speed;
        private Vector3 initScale;
        private bool movingLeft;

        [Header("Enemy Idle Behaviour")]
        [SerializeField] private float idleDuration;
        private float idleTimer;
        
        private void Awake()
        {
            initScale = enemy.localScale;
        }
        
        private void Update()
        {
            if (movingLeft)
            {
                if (enemy.position.x >= leftEdge.position.x)
                {
                    MoveInDirection(-1);
                }
                else
                {
                    DirectionChange();
                }
            }
            else
            {
                if (enemy.position.x <= rightEdge.position.x)
                {
                    MoveInDirection(1);
                }
                else
                {
                    DirectionChange();
                }

            }

        }

        private void DirectionChange()
        {
            idleTimer += Time.deltaTime;
            if (idleTimer > idleDuration)
            {
                movingLeft = !movingLeft;
                idleTimer = 0; // Reset timer after switching direction
            }
        }

        private void MoveInDirection(int _directon)
        {
            idleTimer = 0;
            // make enemy face direction
            enemy.localScale = new Vector3(initScale.x * -_directon, initScale.y, initScale.z);

            // Move in that direction
            enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _directon * speed, enemy.position.y, enemy.position.z);
        }
    }
}