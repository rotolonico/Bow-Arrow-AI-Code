using AI;
using AI.NEAT;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class GameHandler : MonoBehaviour
    {
        public static GameHandler Instance;

        public Toggle autoInputs;
        public BalloonSpawner globalBalloonSpawner;
        
        public GameObject player;
        public bool playerAlive;

        private float gameTime;
        private float idleTime;

        private void Awake() => Instance = this;

        private void Update()
        {
            gameTime += Time.deltaTime;
            if (globalBalloonSpawner.balloonsSpawned >= Settings.Instance.maxBalloons) idleTime += Time.deltaTime;
            if (idleTime < 5) return;
            NEATHandler.Instance.evaluator.Evaluate();
            ResetGame();
            gameTime = 0;
            idleTime = 0;
        }

        public void ResetGameAndNetwork(Genome startingGenome = null)
        {
            globalBalloonSpawner.balloonsSpawned = 0;
            globalBalloonSpawner.balloonsText.text = $"{0}/{Settings.Instance.maxBalloons}";
            
            foreach (var food in GameObject.FindGameObjectsWithTag("Arrow")) Destroy(food);
            foreach (var tail in GameObject.FindGameObjectsWithTag("Player")) Destroy(tail);
            foreach (var snake in GameObject.FindGameObjectsWithTag("Balloon")) Destroy(snake);
            foreach (var snake in GameObject.FindGameObjectsWithTag("EvilBalloon")) Destroy(snake);

            NEATHandler.Instance.InitializeNetwork(startingGenome);
            Instantiate(player, transform.position, Quaternion.identity);
            playerAlive = true;
        }

        public void ResetGame()
        {
            globalBalloonSpawner.balloonsSpawned = 0;
            globalBalloonSpawner.balloonsText.text = $"{0}/{Settings.Instance.maxBalloons}";
            
            foreach (var food in GameObject.FindGameObjectsWithTag("Arrow")) Destroy(food);
            foreach (var tail in GameObject.FindGameObjectsWithTag("Player")) Destroy(tail);
            foreach (var snake in GameObject.FindGameObjectsWithTag("Balloon")) Destroy(snake);
            foreach (var snake in GameObject.FindGameObjectsWithTag("EvilBalloon")) Destroy(snake);

            NEATHandler.Instance.InitiateGeneration();
            //Instantiate(snakePlayer, transform.position, Quaternion.identity);
            //playerAlive = true;
        }
    }
}