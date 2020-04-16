using System;
using System.Linq;
using AI.NEAT;
using IO;
using NN;
using TMPro;
using UnityEngine;

namespace Game
{
    public class PlayerController : MonoBehaviour
    {
        public GameObject arrowPrefab;
        public GameObject balloonSpawnerPrefab;

        public TextMeshPro arrowsText; 

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
            arrowsText.text = $"{spawnedArrows}/{Settings.Instance.maxArrows}";
        }

        private void SpawnBalloonSpawner(Vector3 position, int number, bool evil)
        {
            var newBalloonSpawner = Instantiate(balloonSpawnerPrefab, position, Quaternion.identity)
                .GetComponent<BalloonSpawner>();
            newBalloonSpawner.instanceId = instanceId;
            newBalloonSpawner.evil = evil;
            balloonSpawners[number] = newBalloonSpawner; 
            newBalloonSpawner.visible = isPlayer || genome.Best;
            newBalloonSpawner.balloonsSpawned = number + 1;
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
                genome.Genome.Score = Settings.Scenario == 3 || Settings.Scenario == 4 ? Math.Max(-evilBalloonsHit * 50f + spawnedArrows, 0) : Math.Max((balloonsHit - evilBalloonsHit) * 5f, 0);
                if (currentDelay < spawnDelay) return;
                if (spawnedArrows < Settings.Instance.maxArrows && outputs[4] - outputs[5] >= 0) SpawnArrow();
            }
            else
            {
                if (currentDelay < spawnDelay) return;

                if (spawnedArrows < Settings.Instance.maxArrows && isPlayer && Input.GetKeyDown(KeyCode.Space)) SpawnArrow();
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
            var spawnPosition = transform.position;
            spawnPosition.y += 0.5f;
            var newArrow = Instantiate(arrowPrefab, spawnPosition, Quaternion.identity);
            newArrow.name = instanceId.ToString();
            if (isPlayer || genome.Best) newArrow.GetComponent<SpriteRenderer>().color = Color.white;
            var newArrowHandler = newArrow.GetComponent<ArrowHandler>();
            newArrowHandler.instanceId = instanceId;
            newArrowHandler.player = this;
            currentDelay = 0;
            spawnedArrows++;
            arrowsText.text = $"{spawnedArrows}/{Settings.Instance.maxArrows}";
        }
    }
}