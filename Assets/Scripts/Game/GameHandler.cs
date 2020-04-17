using System;
using AI;
using AI.NEAT;
using NNUtils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class GameHandler : MonoBehaviour
    {
        public static GameHandler Instance;

        public Toggle autoInputs;
        public Toggle useTrainedNetwork;
        public TextMeshProUGUI trainNetworkText;
        public BalloonSpawner globalBalloonSpawner;

        public GameObject player;
        public bool playerAlive;

        private float gameTime;
        private float idleTime;

        private bool isWebGL;
        private bool genomeDownloaded;
        private GenomeWrapper trainedGenome;

        private void Awake() => Instance = this;

        private void Start()
        {
            isWebGL = Application.streamingAssetsPath.Contains("://") ||
                      Application.streamingAssetsPath.Contains(":///");
            LoadTrainedGenome();
        }

        private void LoadTrainedGenome()
        {
            var genomePath = Application.streamingAssetsPath + "/Genome";

            if (isWebGL)
                NetworkStorage.Instance.DownloadGenome(genomePath, downloadedGenome =>
                {
                    trainedGenome = new GenomeWrapper(downloadedGenome);
                    genomeDownloaded = true;
                }, Debug.Log);
            else
            {
                trainedGenome = new GenomeWrapper(NetworkStorage.LoadGenome(genomePath));
                genomeDownloaded = true;
            }
        }

        private void Update()
        {
            gameTime += Time.deltaTime;
            if (globalBalloonSpawner.balloonsSpawned >= Settings.Instance.maxBalloons) idleTime += Time.deltaTime;
            if (idleTime < 5) return;
            NEATHandler.Instance.evaluator.Evaluate();
            if (useTrainedNetwork.isOn) SwitchNetwork();
            else ResetGame();
            gameTime = 0;
            idleTime = 0;
        }

        public void SwitchNetwork()
        {
            if (!genomeDownloaded)
            {
                useTrainedNetwork.isOn = !useTrainedNetwork.isOn;
                return;
            }
            
            ResetGameAndNetwork(useTrainedNetwork.isOn ? trainedGenome : null);
            trainNetworkText.text = useTrainedNetwork.isOn ? "Train your network" : "Use trained network";
        }

        public void ResetGameAndNetwork(GenomeWrapper startingGenome = null)
        {
            globalBalloonSpawner.balloonsSpawned = 0;
            globalBalloonSpawner.balloonsText.text = $"{0}/{Settings.Instance.maxBalloons}";

            foreach (var food in GameObject.FindGameObjectsWithTag("Arrow")) Destroy(food);
            foreach (var tail in GameObject.FindGameObjectsWithTag("Player")) Destroy(tail);
            foreach (var snake in GameObject.FindGameObjectsWithTag("Balloon")) Destroy(snake);
            foreach (var snake in GameObject.FindGameObjectsWithTag("EvilBalloon")) Destroy(snake);

            NEATHandler.Instance.InitializeNetwork(startingGenome);

            if (!useTrainedNetwork.isOn) Instantiate(player, transform.position, Quaternion.identity);
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