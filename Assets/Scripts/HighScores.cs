using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HighScores
{
    [Serializable]
    public class HighScores
    {
        public HighScore[] scores;
    }
    [Serializable]
    public class HighScore
    {
        public int id = 0;
        public string playername = "";
        public float score = 0.0f;
        public string playtime = "";
    }
}