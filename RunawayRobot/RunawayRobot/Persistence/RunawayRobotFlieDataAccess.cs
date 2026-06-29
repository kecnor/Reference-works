using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunawayRobot.Persistence
{
    public class RunawayRobotFlieDataAccess : IsRunawayRobotDataAccess
    {
        public async Task<(RunawayRobotTable,RunawayRobotRobot,int)> LoadAsync(String path)
        {
            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    String line = await reader.ReadLineAsync() ?? String.Empty;
                    String[] numbers = line.Split(' ');
                    int time = int.Parse(numbers[0]);
                    int size = int.Parse(numbers[1]);
                    line = await reader.ReadLineAsync() ?? String.Empty;
                    numbers = line.Split(' ');
                    int x = int.Parse(numbers[0]);
                    int y = int.Parse(numbers[1]);
                    RunawayRobotTable table = new RunawayRobotTable(size);
                    RunawayRobotRobot robot = new RunawayRobotRobot(table);
                    robot.SetX(x);
                    robot.SetY(y);
                    robot.SetRobotDirection(numbers[2]);
                    for (int i = 0; i < size; i++)
                    {
                        line = await reader.ReadLineAsync() ?? String.Empty;
                        numbers = line.Split(' ');

                        for (int j = 0; j < size; j++)
                        {
                            int value = int.Parse(numbers[j]);
                            table.SetFieldValue(i, j, value);
                            if (value != 0)
                            {
                                table.LockField(i, j);
                            }
                        }
                    }
                    return (table,robot, time);
                }
            }
            catch
            {
                throw new RunawayRobotDataException();
            }
        }
        public async Task SaveAsync(String path, RunawayRobotTable table, RunawayRobotRobot robot, int time)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path))
                {
                    writer.Write($"{time} ");
                    writer.Write($"{table.Size}\n");
                    writer.WriteLine($"{robot.RobotX} {robot.RobotY} {robot.RobotDirection}");
                    for (int i = 0; i < table.Size; i++)
                    {
                        for (int j = 0; j < table.Size; j++)
                        {
                            await writer.WriteAsync($"{table.GetFieldValue(i, j)} ");
                        }
                        await writer.WriteLineAsync();
                    }
                }
            }
            catch
            {
                throw new RunawayRobotDataException();
            }
        }

    }
}
