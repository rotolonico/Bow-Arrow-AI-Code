﻿using System;
using AI;
using TMPro;
using UnityEngine;
using Utils;
using Random = System.Random;

namespace Game
{
    public class BalloonSpawner : MonoBehaviour
    {
        public Random random = new Random();
        
        public GameObject balloonPrefab;
        public GameObject evilBalloonPrefab;

        public TextMeshProUGUI balloonsText;
        
        public float spawnDelay;

        public int balloonsSpawned;
        public int instanceId;
        public bool visible;
        public bool evil;

        public BalloonHandler currentBalloon;
        private float currentDelay;

        private Camera mainCamera;

        private void Start()
        {
            mainCamera = Camera.main;
            currentDelay = spawnDelay;
            if (Settings.Scenario > 2) evil = true;
        }

        private void Update()
        {
            if (balloonsSpawned >= Settings.Instance.maxBalloons) return;
            
            currentDelay += !GameHandler.Instance.autoInputs.isOn ? Time.deltaTime * 3 : Time.deltaTime;
            if (currentDelay < spawnDelay / Settings.Instance.gameSpeed) return;

            if (!GameHandler.Instance.autoInputs.isOn)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    GameHandler.Instance.globalBalloonSpawner.SpawnBalloon(
                        mainCamera.ScreenToWorldPoint(Input.mousePosition).x -
                        GameHandler.Instance.globalBalloonSpawner.transform.position.x);
                    balloonsText.text = $"{balloonsSpawned}/{Settings.Instance.maxBalloons}";
                    currentDelay = 0;
                    NEATHandler.Instance.alivePopulation.ForEach(p => p.hit = false);
                }
            }
            else
            {
                SpawnBalloon((float) random.NextDouble() * 6 - 4);
                balloonsText.text = $"{balloonsSpawned}/{Settings.Instance.maxBalloons}";
                currentDelay = 0;
                NEATHandler.Instance.alivePopulation.ForEach(p => p.hit = false);
            }

        }

        public void SpawnBalloon(float xOffset)
        {
            var position = transform.position;
            xOffset = Mathf.Clamp(xOffset, -4, 2); 
            position.x += xOffset;
            var newBalloon = Instantiate(evil ? evilBalloonPrefab : balloonPrefab, position, Quaternion.identity);
            newBalloon.name = instanceId.ToString();
            var newBalloonHandler = newBalloon.GetComponent<BalloonHandler>();
            currentBalloon = newBalloonHandler;
            newBalloonHandler.speed = Settings.Scenario == 1 || Settings.Scenario == 3 ? 3 : balloonsSpawned % 5 + 1;
            newBalloonHandler.xOffset = xOffset;
            newBalloonHandler.instanceId = instanceId;
            if (visible) newBalloon.GetComponent<SpriteRenderer>().color = evil ? Color.yellow : Color.red;
            balloonsSpawned++;
        }
    }
}
