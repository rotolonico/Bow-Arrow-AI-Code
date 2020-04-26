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
            if (!other.CompareTag("Balloon") && !other.CompareTag("EvilBalloon"))
            {
                player.inCombo = false;
                return;
            }
            if (other.CompareTag("Balloon"))
            {
                player.balloonsHit++;
                player.balloonsPoints += Math.Abs(transform.position.x - player.lastBalloonX); 
                if (transform.position.x < 2) player.balloonsLeftHit++;
                else player.balloonsRightHit++;
                player.spawnedArrows = Math.Max(0, player.spawnedArrows - 1);
                player.UpdateArrowText();
                if (player.inCombo) player.comboPoints++;
                player.inCombo = true;
                player.hit = true;
            }
            else player.evilBalloonsHit++;
            if (player.isPlayer || player.genome.Best) other.GetComponent<SpriteRenderer>().color = Color.clear;
        }
    }
}
