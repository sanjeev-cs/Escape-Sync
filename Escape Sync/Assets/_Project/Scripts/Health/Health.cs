using UnityEngine;

namespace COMP305
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float startingHealth;
        private Animator anim;
        private bool dead;
        public float currentHealth { get; private set; }

        private void Awake()
        {
            currentHealth = startingHealth;
            anim = GetComponent<Animator>();
        }



        public void TakeDamage(float _damage)
        {
            currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

            if (currentHealth > 0)
            {
                // Player Hurt
                anim.SetTrigger("hurt");
            }
            else
            {
                if (!dead)
                {
                    // Player dead
                    //anim.SetTrigger("die");
                    Debug.Log("Die Animation");
                    GetComponent<PlayerController>().enabled = false;
                    GetComponent<Animator>().enabled = false;
                    dead = true;
                }
            }
        }

        public void AddHealth(float _value)
        {
            currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
        }
    }
}
