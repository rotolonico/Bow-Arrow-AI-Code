﻿using System;
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
            var playerPosition = (player.transform.position.y + 5) / 10;
            var balloonPosition = player.hit || GameHandler.Instance.globalBalloonSpawner.currentBalloon == null
                ? 0f
                : (GameHandler.Instance.globalBalloonSpawner.currentBalloon.transform.position.y + 5) / 10;
            inputs[0] = playerPosition;
            inputs[1] = player.hit || GameHandler.Instance.globalBalloonSpawner.currentBalloon == null
                ? 0f
                : (playerPosition -
                  balloonPosition + 1) / 2;
            inputs[2] = player.hit || GameHandler.Instance.globalBalloonSpawner.currentBalloon == null
                ? 0
                : (GameHandler.Instance.globalBalloonSpawner.currentBalloon.xOffset + 4) / 6;
//            if (Settings.Scenario != 1 && Settings.Scenario != 3)
//                inputs[3] = GameHandler.Instance.globalBalloonSpawner.currentBalloon == null
//                    ? 1f
//                    : (GameHandler.Instance.globalBalloonSpawner.currentBalloon.speed - 1) / 5;
            //inputs[2] = player.balloonSpawners[1].currentBalloon == null ? 1f : (player.balloonSpawners[1].currentBalloon.transform.position.y + 5) / 10;
            //inputs[4] = player.balloonSpawners[1].currentBalloon == null ? 1f : player.balloonSpawners[1].currentBalloon.speed / 3;
            //inputs[3] = player.balloonSpawners[2].currentBalloon == null ? 1f : (player.balloonSpawners[2].currentBalloon.transform.position.y + 5) / 10;
            //inputs[6] = player.balloonSpawners[2].currentBalloon == null ? 1f : player.balloonSpawners[2].currentBalloon.speed / 3;

            return inputs;
        }
    }
}