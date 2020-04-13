using System;
using UnityEngine;

namespace Game
{
    public class ArrowHandler : MonoBehaviour
    {
        public float speed;
        public PlayerController player;
        public int instanceId;

        private float age;

        private void Update()
        {
            transform.position += new Vector3(Time.deltaTime * speed * Settings.Instance.gameSpeed, 0, 0);
            age += Time.deltaTime;
            if (age > 2) Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Balloon") && !other.CompareTag("EvilBalloon") || other.name != name) return;
            if (other.CompareTag("Balloon")) player.balloonsHit++;
            else player.evilBalloonsHit++;
            Destroy(other.gameObject);
        }
    }
}
