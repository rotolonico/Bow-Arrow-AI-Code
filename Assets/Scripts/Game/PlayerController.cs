using System;
using System.Linq;
using AI.NEAT;
using IO;
using NN;
using UnityEngine;

namespace Game
{
    public class PlayerController : MonoBehaviour
    {
        public GameObject arrowPrefab;
        public GameObject balloonSpawnerPrefab;

        public float spawnDelay;

        public float playerSpeed;
        public int instanceId;
        public bool isPlayer;

        public int balloonsHit;
        public int evilBalloonsHit;
        public int spawnedArrows;

        [HideInInspector] public BalloonSpawner[] balloonSpawners;

        public GenomeWrapper genome;

        private float currentDelay;

        private void Start()
        {
            currentDelay = spawnDelay;
            balloonSpawners = new BalloonSpawner[1];
            SpawnBalloonSpawner(new Vector3(3, -5, 0), 0, false);
//            SpawnBalloonSpawner(new Vector3(5, -5, 0), 1, false);
//            SpawnBalloonSpawner(new Vector3(7, -5, 0), 2, false);
        }

        private void SpawnBalloonSpawner(Vector3 position, int number, bool evil)
        {
            var newBalloonSpawner = Instantiate(balloonSpawnerPrefab, position, Quaternion.identity)
                .GetComponent<BalloonSpawner>();
            newBalloonSpawner.instanceId = instanceId;
            newBalloonSpawner.evil = evil;
            balloonSpawners[number] = newBalloonSpawner; 
            newBalloonSpawner.visible = isPlayer || genome.Best;
            newBalloonSpawner.number = number + 1;
        }

        private void Update()
        {
            var playerPosition = transform.position;
            if (isPlayer)
                transform.position = new Vector3(playerPosition.x,
                    Mathf.Clamp(
                        playerPosition.y + Input.GetAxisRaw("Vertical") * playerSpeed * Settings.Instance.gameSpeed, -3,
                        3), 0);
            currentDelay += Time.deltaTime;

            if (!isPlayer)
            {
                var outputs = NetworkCalculator.TestNetworkGenome(genome.Network, InputsRetriever.GetInputs(this));
                if (outputs[0] - outputs[1] >= 0) Up();
                if (outputs[2] - outputs[3] >= 0) Down();
                genome.Genome.Score = Math.Max((balloonsHit + evilBalloonsHit) * 5f - spawnedArrows, 0);
                if (currentDelay < spawnDelay) return;
                if (outputs[4] - outputs[5] >= 0) SpawnArrow();
            }
            else
            {
                if (currentDelay < spawnDelay) return;

                if (isPlayer && Input.GetKeyDown(KeyCode.Space)) SpawnArrow();
            }
        }

        private void Up()
        {
            var playerPosition = transform.position;
            transform.position = new Vector3(playerPosition.x,
                Mathf.Clamp(playerPosition.y + playerSpeed * Settings.Instance.gameSpeed, -3, 3), 0);
        }

        private void Down()
        {
            var playerPosition = transform.position;
            transform.position = new Vector3(playerPosition.x,
                Mathf.Clamp(playerPosition.y - playerSpeed * Settings.Instance.gameSpeed, -3, 3), 0);
        }

        private void SpawnArrow()
        {
            var newArrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
            newArrow.name = instanceId.ToString();
            if (isPlayer || genome.Best) newArrow.GetComponent<SpriteRenderer>().color = Color.white;
            var newArrowHandler = newArrow.GetComponent<ArrowHandler>();
            newArrowHandler.instanceId = instanceId;
            newArrowHandler.player = this;
            currentDelay = 0;
            spawnedArrows++;
        }
    }
}