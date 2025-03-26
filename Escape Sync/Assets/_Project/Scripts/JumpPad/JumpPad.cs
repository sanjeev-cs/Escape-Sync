using System;
using UnityEngine;

namespace COMP305
{
    public class JumpPad : MonoBehaviour
    {
        [SerializeField] private float bouncForce;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * bouncForce, ForceMode2D.Impulse);
            }
        }
    }
}
