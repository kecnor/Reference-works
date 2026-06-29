using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunawayRobot.Model
{
    public class RunawayRobotEventArgs : EventArgs
    {
        private int time;
        private bool iswow;

        public int Time { get { return time; } }
        public bool IsWon { get {  return iswow; } }

        public RunawayRobotEventArgs(int time, bool won)
        {
            this.time = time;
            this.iswow = won;
        }
    }
}
