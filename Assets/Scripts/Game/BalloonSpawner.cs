using System;
using UnityEngine;
using Utils;

namespace Game
{
    public class BalloonSpawner : MonoBehaviour
    {
        public GameObject balloonPrefab;
        public GameObject evilBalloonPrefab;
        
        public float spawnDelay;

        public int number;
        public int instanceId;
        public bool visible;
        public bool evil;

        public BalloonHandler currentBalloon;
        private float currentDelay;

        private void Start() => currentDelay = spawnDelay;

        private void Update()
        {
            currentDelay += Time.deltaTime;
            if (currentDelay < spawnDelay / Settings.Instance.gameSpeed) return;
            SpawnBalloon();
            currentDelay = 0;

        }

        private void SpawnBalloon()
        {
            if (currentBalloon != null) Destroy(currentBalloon.gameObject);
            var newBalloon = Instantiate(evil ? evilBalloonPrefab : balloonPrefab, transform.position, Quaternion.identity);
            newBalloon.name = instanceId.ToString();
            var newBalloonHandler = newBalloon.GetComponent<BalloonHandler>();
            currentBalloon = newBalloonHandler;
            newBalloonHandler.speed = RandomnessHandler.RandomMinMax(3, 5);
            newBalloonHandler.instanceId = instanceId;
            if (visible) newBalloon.GetComponent<SpriteRenderer>().color = evil ? Color.yellow : Color.red;

        }
    }
}
