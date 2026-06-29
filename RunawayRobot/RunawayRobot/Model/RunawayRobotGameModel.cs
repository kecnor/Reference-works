using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using RunawayRobot.Persistence;

namespace RunawayRobot.Model
{
    public class RunawayRobotGameModel
    {
        #region Fields
        public enum GameDifficulty { None, Easy, Medium, Hard }
        private IsRunawayRobotDataAccess dataAccess;
        private RunawayRobotTable table =null!;
        private RunawayRobotRobot robot = null!;
        private GameDifficulty difficulty;
        private const int gamefieldwidht = 1255;
        private const int gamefieldheight = 1355;
        private const int menufieldwidht = 450;
        private const int menufieldheight = 600;
        private const int gamefieldsize = 1155;
        private int fieldsize;
        private int time;

        public event EventHandler<RunawayRobotEventArgs>? GameOver;
        #endregion
        #region Properties
        public int FieldSize { get { return fieldsize; } }
        public int GameFieldWidht { get { return gamefieldwidht; } }
        public int GameFieldHeight { get { return gamefieldheight; } }
        public int MenuFieldWidht { get { return menufieldwidht; } }
        public int MenuFieldHeight { get { return menufieldheight; } }
        public int Time { get { return time; } }
        public GameDifficulty GetDifficulty { get { return difficulty; } }

        public RunawayRobotTable Table { get { return table; } }
        public RunawayRobotRobot Robot { get { return robot; } }

        #endregion
        #region Constructor
        public RunawayRobotGameModel(IsRunawayRobotDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }
        public void Create()
        {
            table = new RunawayRobotTable(GetMapSize);
            robot = new RunawayRobotRobot(Table);
            SetFieldSize();
        }
        #endregion
        #region Public methodes

        public void SetTime(int num)
        {
            time = num;
        }
        public void SetDifficulty(GameDifficulty difficult)
        {
            difficulty = difficult;
        }
        public int GetMapSize
        {
            get
            {
                switch (difficulty)
                {
                    case GameDifficulty.None:
                        return 0;
                    case GameDifficulty.Easy:
                        return 7;
                    case GameDifficulty.Medium:
                        return 11;
                    case GameDifficulty.Hard:
                        return 15;
                }
                throw new ArgumentException(nameof(difficulty), "There is no such difficulty");
            }
        }

        public void IsGameOver()
        {
            if (Robot.RobotX == Table.Middle && Robot.RobotY == Table.Middle)
            {
                OnGameOver(true);
            }
            else if (Robot.RobotX != Table.Middle && Robot.RobotY != Table.Middle)
            {
                int valami = 0;
                for (int i = 0; i < Table.Size; ++i)
                {
                    for (int j = 0; j < Table.Size; j++)
                    {
                        if (Table.GetFieldValue(i, j) == 2)
                        {
                            if (i == Table.Middle - 1 && j != Table.Middle)
                            {
                                valami++;
                            }
                            if (i == Table.Middle + 1 && j != Table.Middle)
                            {
                                valami++;
                            }
                            if (i != Table.Middle && j == Table.Middle - 1)
                            {
                                valami++;
                            }
                            if (i != Table.Middle && j == Table.Middle + 1)
                            {
                                valami++;
                            }
                        }
                    }
                }
                if (valami == (4 * Table.Size) - 4)
                {
                    OnGameOver(false);
                }
            }
            else
            {
                int valami = 0;
                for (int i = 0; i < Table.Size; i++)
                {
                    for (int j = 0; j < Table.Size; j++)
                    {
                        if (i == 0 && Table.GetFieldValue(i, j) == 2)
                        {
                            valami++;
                        }
                        if (i == Table.Size - 1 && Table.GetFieldValue(i, j) == 2)
                        {
                            valami++;
                        }
                        if (j == 0 && Table.GetFieldValue(i, j) == 2)
                        {
                            valami++;
                        }
                        if (j == Table.Size - 1 && Table.GetFieldValue(i, j) == 2)
                        {
                            valami++;
                        }
                    }
                }
                if (valami == (4 * (Table.Size)))
                {
                    OnGameOver(false);
                }

            }
        }
        private void OnGameOver(bool isWon)
        {
            GameOver?.Invoke(this, new RunawayRobotEventArgs(time, isWon));
        }
        public async Task LoadGameAsync(String path)
        {
            if (dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");
            var items = await dataAccess.LoadAsync(path);
            table = items.Item1;
            robot = items.Item2;
            SetTime(items.Item3);
        }
        public async Task SaveGameAsync(String path, int time)
        {
            if (dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");

            await dataAccess.SaveAsync(path, table, robot, time);
        }
        public void SetFieldSize()
        {
            fieldsize = gamefieldsize / table.Size;
        }
        #endregion
    }
}
