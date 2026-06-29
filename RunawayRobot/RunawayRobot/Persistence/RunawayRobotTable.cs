using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RunawayRobot.Persistence
{
    public class RunawayRobotTable
    {
        #region Fields

        private int mapsize;
        /// <summary>
        /// 0 = free, 1 = wall, 2 = broken wall, 5 = magnet
        /// </summary>
        private int[,] mapstructor = null!;
        private bool[,] mapplace = null!;
        private int middlecordinate;

        #endregion
        #region Properties
        public int Size { get { return mapsize; } }
        public int Middle { get { return middlecordinate; } }

        #endregion
        #region Constructor

        public RunawayRobotTable(int mapsize)
        {
            if (mapsize < 0)
                throw new ArgumentOutOfRangeException(nameof(mapsize), "The table size is less than 0.");
            this.mapsize = mapsize;
            SetupMaps();
            SetMiddle();
            SetMapMiddle();
        }

        #endregion
        #region Public methods

        #region mapstructor
        public void SetFieldValue(int i, int j, int num)
        {
            if (i < 0 || i >= mapsize)
                throw new ArgumentOutOfRangeException(nameof(i), "The X coordinate is out of range.");
            if (j < 0 || j >= mapsize)
                throw new ArgumentOutOfRangeException(nameof(j), "The Y coordinate is out of range.");
            mapstructor[i, j] = num;
        }
        public int GetFieldValue(int i, int j)
        {
            if (i < 0 || i >= mapsize)
                throw new ArgumentOutOfRangeException(nameof(i), "The X coordinate is out of range.");
            if (j < 0 || j >= mapsize)
                throw new ArgumentOutOfRangeException(nameof(j), "The Y coordinate is out of range.");
            return mapstructor[i, j];
        }
        #endregion
        #region mapplace
        public void LockField(int i, int j)
        {
            if (i < 0 || i >= mapsize)
                throw new ArgumentOutOfRangeException(nameof(i), "The X coordinate is out of range.");
            if (j < 0 || j >= mapsize)
                throw new ArgumentOutOfRangeException(nameof(j), "The Y coordinate is out of range.");
            mapplace[i, j] = false;
        }
        public void UnLockField(int i, int j)
        {
            if (i < 0 || i >= mapsize)
                throw new ArgumentOutOfRangeException(nameof(i), "The X coordinate is out of range.");
            if (j < 0 || j >= mapsize)
                throw new ArgumentOutOfRangeException(nameof(j), "The Y coordinate is out of range.");
            mapplace[i, j] = true;
        }
        public bool IsLocked(int i, int j)
        {
            if (i < 0 || i >= mapsize)
                throw new ArgumentOutOfRangeException(nameof(i), "The X coordinate is out of range.");
            if (j < 0 || j >= mapsize)
                throw new ArgumentOutOfRangeException(nameof(j), "The Y coordinate is out of range.");
            return mapplace[i, j];
        }
        public void SetupMaps()
        {
            mapstructor = new int[mapsize, mapsize];
            mapplace = new bool[mapsize, mapsize];
            for (int i = 0; i < mapsize; i++)
            {
                for (int j = 0; j < mapsize; j++)
                {
                    mapstructor[i, j] = 0;
                    mapplace[i, j] = true;
                }
            }
        }
        #endregion

        #endregion
        #region Private methods

        #region Constructor methods
        private void SetMiddle()
        {
            middlecordinate = (mapsize / 2);
        }
        private void SetMapMiddle()
        {
            mapstructor[middlecordinate, middlecordinate] = 5;
        }
        #endregion

        #endregion
    }
}
