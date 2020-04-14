using System;
using AI;
using AI.NEAT;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class Settings : MonoBehaviour
    {
        public static Settings Instance;

        public static int Scenario = 1;

        public int inputs;
        public int outputs;
        public bool autoGenerateConnections;
        public float gameTime;
        public float gameSpeed;

        public void Awake() => Instance = this;

        private void Start()
        {
            if (Scenario == 1 || Scenario == 3) inputs = 2;
        }

        public void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.S)) NEATHandler.Instance.evaluator.FittestGenome.Genome.SaveGenome("Genome");
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.L)) GameHandler.Instance.ResetGameAndNetwork(Genome.LoadGenome("Genome"));
        }

        public void ChangeScenario(int scenario)
        {
            Scenario = scenario;
            SceneManager.LoadScene(0);
        }
    }
}
