using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace RunawayRobot.Persistence
{
    public class RunawayRobotRobot
    {
        private enum Direction { North, East, South, West }

        #region Fileds

        private int robotx = 0;
        private int roboty = 0;
        private Direction robotdirection = 0;
        private List<Direction> paths = null!;
        private Random random;

        #endregion
        #region Properties

        public int RobotX { get { return robotx; } }
        public int RobotY { get { return roboty; } }
        public string RobotDirection { get { return robotdirection.ToString(); } }

        #endregion
        #region Constructor
        public RunawayRobotRobot(RunawayRobotTable table)
        {
            random = new Random();
            BotStarterPosition(table.Size, table.Middle);
            BotDirection(table);
        }

        #endregion
        #region Public methods
        public void SetX(int x)
        {
            robotx = x;
        }
        public void SetY(int y)
        {
            roboty = y;
        }
        public void SetRobotDirection(string direction)
        {
            switch (direction)
            {
                case "North":
                    robotdirection = Direction.North;
                    break;
                case "East":
                    robotdirection = Direction.East;
                    break;
                case "South":
                    robotdirection = Direction.South;
                    break;
                case "West":
                    robotdirection = Direction.West;
                    break;
            }
        }
        public void Move(RunawayRobotTable table)
        {
            int x = 0, y = 0;
            switch (robotdirection)
            {
                case Direction.North:
                    y = -1;
                    break;
                case Direction.East:
                    x = 1;
                    break;
                case Direction.South:
                    y = 1;
                    break;
                case Direction.West:
                    x = -1;
                    break;
            }
            if (!Occupied(table, x, y))
            {
                table.UnLockField(robotx, roboty);
                robotx += x;
                roboty += y;
            }
            else
            {
                if (robotx + x != -1 && roboty + y != -1 && robotx + x != table.Size && roboty + y != table.Size)
                {
                    if (table.GetFieldValue(robotx + x, roboty + y) == 1)
                    {
                        table.SetFieldValue(robotx + x, roboty + y, 2);
                    }
                }
                BotDirection(table);
            }
        }

        #endregion
        #region Private methods

        #region Constructor methods
        private void BotStarterPosition(int size, int middle)
        {
            do
            {
                robotx = random.Next(0, size);
                roboty = random.Next(0, size);
            } while (robotx == middle && roboty == middle);
        }
        private void BotDirection(RunawayRobotTable table)
        {
            paths = new List<Direction>();
            for (int i = -1; i < 2; i += 2)
            {
                if (robotx + i >= 0 || robotx + i < table.Size)
                {
                    paths.Add((Direction)i + 2);
                }
                if (roboty + i >= 0 || roboty + i < table.Size)
                {
                    paths.Add((Direction)i + 1);
                }
            }
            paths.Remove(robotdirection);
            int direction = random.Next(0, paths.Count);
            robotdirection = paths[direction];

        }
        #endregion

        private bool Occupied(RunawayRobotTable table, int x, int y)
        {
            if (robotx + x == -1 || roboty + y == -1 || robotx + x == table.Size || roboty + y == table.Size)
            {
                return true;
            }
            return table.GetFieldValue(robotx + x, roboty + y) == 1;
        }
        #endregion
    }
}
