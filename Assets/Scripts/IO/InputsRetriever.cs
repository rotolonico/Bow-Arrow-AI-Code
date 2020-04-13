using System;
using System.Collections.Generic;
using System.Linq;
using AI;
using Game;
using UnityEngine;

namespace IO
{
    public static class InputsRetriever
    {
        public static float[] GetInputs(PlayerController player)
        {
            var inputs = new float[3];
            inputs[0] = (player.transform.position.y + 5) / 10;
            inputs[1] = player.balloonSpawners[0].currentBalloon == null ? 1f : (player.balloonSpawners[0].currentBalloon.transform.position.y + 5) / 10;
            inputs[2] = player.balloonSpawners[0].currentBalloon == null ? 1f : (player.balloonSpawners[0].currentBalloon.speed - 2) / 3;
            //inputs[2] = player.balloonSpawners[1].currentBalloon == null ? 1f : (player.balloonSpawners[1].currentBalloon.transform.position.y + 5) / 10;
            //inputs[4] = player.balloonSpawners[1].currentBalloon == null ? 1f : player.balloonSpawners[1].currentBalloon.speed / 3;
            //inputs[3] = player.balloonSpawners[2].currentBalloon == null ? 1f : (player.balloonSpawners[2].currentBalloon.transform.position.y + 5) / 10;
            //inputs[6] = player.balloonSpawners[2].currentBalloon == null ? 1f : player.balloonSpawners[2].currentBalloon.speed / 3;

            return inputs;
        }
    }
}