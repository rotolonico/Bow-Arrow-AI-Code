using System;
using System.Collections.Generic;
using System.Linq;
using AI.LiteNN;
using AI.NEAT;
using Game;
using UnityEngine;
using UnityEngine.UI;

namespace AI
{
    public class NEATHandler : MonoBehaviour
    {
        public static NEATHandler Instance;
        
        public GameObject playerAI;
        public int populationSize;
        public List<PlayerController> alivePopulation = new List<PlayerController>();

        public Evaluator evaluator;

        private void Awake() => Instance = this;

        private void Start() => InitializeNetwork();

        public void InitializeNetwork(Genome startingGenome = null)
        {
            evaluator = new Evaluator(populationSize, new Counter(), new Counter(), g => Mathf.Pow(g.Score, 3), startingGenome);
            InitiateGeneration();
        }

        public void InitiateGeneration()
        {
            alivePopulation.Clear();

            for (var i = 0; i < evaluator.Genomes.Count; i++)
            {
                var genome = evaluator.Genomes[i];
                var newPlayerAI = Instantiate(playerAI, transform.position, Quaternion.identity);
                newPlayerAI.name = i.ToString();
                var newPlayerController = newPlayerAI.GetComponent<PlayerController>();
                newPlayerController.genome = genome;
                newPlayerController.instanceId = i;
                alivePopulation.Add(newPlayerAI.GetComponent<PlayerController>());
                
                if (!genome.Best) continue;
                NetworkDisplayer.Instance.DisplayNetwork(genome.Genome);
                newPlayerAI.GetComponent<SpriteRenderer>().color = Color.white;
                newPlayerAI.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
                newPlayerAI.transform.GetChild(1).GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }
}