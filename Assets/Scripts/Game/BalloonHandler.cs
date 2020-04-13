using System;
using UnityEngine;

namespace Game
{
    public class BalloonHandler : MonoBehaviour
    {
        public float speed;
        public int instanceId;
        public bool evil;
        
        private void Update()
        {
            transform.position += new Vector3(0, Time.deltaTime * speed * Settings.Instance.gameSpeed, 0);
        }
    }
}
