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
        public Toggle survivalMode;
        public TextMeshProUGUI trainNetworkText;
        public BalloonSpawner globalBalloonSpawner;

        public GameObject player;
        public bool playerAlive;

        private float idleTime;

        private bool isWebGL;
        private bool genomeDownloaded;
        private GenomeWrapper trainedGenome;

        public int generation;

        private void Awake() => Instance = this;

        private void Start()
        {
            isWebGL = Application.streamingAssetsPath.Contains("://") ||
                      Application.streamingAssetsPath.Contains(":///");
            LoadTrainedGenome();
        }

        private void Update()
        {
            if (useTrainedNetwork.isOn
                ? NEATHandler.Instance.alivePopulation[0].spawnedArrows >= Settings.Instance.maxArrows ||
                  globalBalloonSpawner.balloonsSpawned >= Settings.Instance.maxBalloons
                : globalBalloonSpawner.balloonsSpawned >= Settings.Instance.maxBalloons) idleTime += Time.deltaTime;
            else idleTime = 0;
            if (idleTime < 5) return;
            NEATHandler.Instance.evaluator.Evaluate();
            generation++;
            Settings.Instance.maxBalloons = survivalMode.isOn ? generation + 5 : 3;
            if (useTrainedNetwork.isOn) SwitchNetwork(false);
            else ResetGame();
            idleTime = 0;
        }

        private void LoadTrainedGenome()
        {
            var genomePath = Application.streamingAssetsPath + "/Genome";

            if (isWebGL)
                NetworkStorage.Instance.DownloadGenome(genomePath, downloadedGenome =>
                {
                    trainedGenome = new GenomeWrapper(new Genome(downloadedGenome));
                    genomeDownloaded = true;
                }, Debug.Log);
            else
            {
                trainedGenome = new GenomeWrapper(new Genome(NetworkStorage.LoadGenome(genomePath)));
                genomeDownloaded = true;
            }
        }

        public void SwitchNetwork(bool resetGeneration = true)
        {
            if (!genomeDownloaded) useTrainedNetwork.SetIsOnWithoutNotify(!useTrainedNetwork.isOn);
            ResetGameAndNetwork(useTrainedNetwork.isOn ? trainedGenome : null, resetGeneration);
            trainNetworkText.text = useTrainedNetwork.isOn ? "Train your network" : "Use trained network";
        }

        public void ResetGameAndNetwork(GenomeWrapper startingGenome = null, bool resetGeneration = true)
        {
            if (resetGeneration) generation = 0;
            Settings.Instance.maxBalloons = survivalMode.isOn ? generation + 5 : 3;

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