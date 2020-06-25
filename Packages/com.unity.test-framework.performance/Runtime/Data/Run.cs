using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.PerformanceTesting.Data
{
    [Serializable]
    public class Run
    {
        public string TestSuite;
        public long Date;
        public Player Player;
        public Hardware Hardware;
        public Editor Editor;
        public List<string> Dependencies = new List<string>();
        public List<TestResult> Results = new List<TestResult>();
    }
}
