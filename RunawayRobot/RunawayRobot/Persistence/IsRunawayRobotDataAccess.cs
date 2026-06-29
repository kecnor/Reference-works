using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunawayRobot.Persistence
{
    public interface IsRunawayRobotDataAccess
    {
        Task<(RunawayRobotTable,RunawayRobotRobot,int)> LoadAsync(String path);
        Task SaveAsync(String path, RunawayRobotTable table,RunawayRobotRobot robot, int time);
    }
}
