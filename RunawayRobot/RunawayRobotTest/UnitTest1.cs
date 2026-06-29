using RunawayRobot.Model;
using RunawayRobot.Persistence;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using static RunawayRobot.Model.RunawayRobotGameModel;
using System.ComponentModel;

namespace RunawayRobot.Test
{
    [TestClass]
    public class UnitTest1
    {
        private RunawayRobotGameModel _model = null!;
        private (RunawayRobotTable, RunawayRobotRobot, int) _mockedTuple;
        private Mock<IsRunawayRobotDataAccess> _mock = null!;

        [TestInitialize]
        public void Initialize()
        {
            RunawayRobotTable table = new RunawayRobotTable(7);
            table.SetFieldValue(0, 0, 1);
            table.SetFieldValue(3, 2, 2);
            table.SetFieldValue(2, 6, 1);
            table.SetFieldValue(2, 5, 1);
            table.SetFieldValue(3, 1, 1);

            _mockedTuple = (table, new RunawayRobotRobot(table), 0);

            _mock = new Mock<IsRunawayRobotDataAccess>();
            _mock.Setup(mock => mock.LoadAsync(It.IsAny<string>()))
                .Returns(() => Task.FromResult(_mockedTuple));

            _model = new RunawayRobotGameModel(_mock.Object);

            _model.GameOver += new EventHandler<RunawayRobotEventArgs>(Model_GameOver);
        }

        [TestMethod]

        public void RunawayRobotGameModelNewGameMediumTest()
        {
            _model.SetDifficulty(GameDifficulty.Medium);
            _model.Create();

            Assert.AreEqual(GameDifficulty.Medium, _model.GetDifficulty);
            Assert.AreEqual(0, _model.Time);
            int emptyFields = 0;
            for (int i = 0; i < 11; i++)
                for (int j = 0; j < 11; j++)
                    if (_model.Table.GetFieldValue(i, j) == 0)
                        emptyFields++;

            Assert.AreEqual(120, emptyFields);
        }
        [TestMethod]
        public void RunawayRobotGameModelNewGameEasyTest()
        {
            _model.SetDifficulty(GameDifficulty.Easy);
            _model.Create();

            Assert.AreEqual(GameDifficulty.Easy, _model.GetDifficulty);
            Assert.AreEqual(0, _model.Time);

            int emptyFields = 0;
            for (int i = 0; i < 7; i++)
                for (int j = 0; j < 7; j++)
                    if (_model.Table.GetFieldValue(i, j) == 0)
                        emptyFields++;

            Assert.AreEqual(48, emptyFields);
        }

        [TestMethod]
        public void RunawayRobotGameModelNewGameHardTest()
        {
            _model.SetDifficulty(GameDifficulty.Hard);
            _model.Create();

            Assert.AreEqual(GameDifficulty.Hard, _model.GetDifficulty);
            Assert.AreEqual(0, _model.Time);

            int emptyFields = 0;
            for (int i = 0; i < 15; i++)
                for (int j = 0; j < 15; j++)
                    if (_model.Table.GetFieldValue(i, j) == 0)
                        emptyFields++;

            Assert.AreEqual(224, emptyFields);
        }
        [TestMethod]
        public void RunawayRobotGameModelMoveTest()
        {
            _model.SetDifficulty(GameDifficulty.Easy);
            _model.Create();

            Random random = new Random();
            int x, y;
            do
            {
                x = random.Next(0, 7);
                y = random.Next(1, 7);
            }
            while (_model.Table.GetFieldValue(x, y) == 5);

            _model.Robot.SetX(x);
            _model.Robot.SetY(y);
            Assert.AreEqual(0, _model.Table.GetFieldValue(x, y));
            _model.Robot.SetRobotDirection("North");
            _model.Robot.Move(_model.Table);
            Assert.AreEqual(y - 1, _model.Robot.RobotY);
            Assert.AreEqual(0, _model.Time);


            int currentValueX = x;
            int currentValueY = y;
            for (int i = 2; i < 1E6; i++)
            {
                string direction = _model.Robot.RobotDirection.ToString(); 
                _model.Robot.Move(_model.Table);
                Assert.IsTrue(currentValueX != _model.Robot.RobotX || currentValueY != _model.Robot.RobotY || direction != _model.Robot.RobotDirection.ToString());

                currentValueX = _model.Robot.RobotX;
                currentValueY = _model.Robot.RobotY;
            }
        }

        [TestMethod]
        public async Task RunawayRobotGameModelLoadTest()
        {
            await _model.LoadGameAsync(String.Empty);

            for (int i = 0; i < 7; i++)
                for (int j = 0; j < 7; j++)
                {
                    //Assert.AreNotEqual(table.GetFieldValue(i, j), _model.Table.GetFieldValue(i, j));
                    //Assert.AreNotEqual(table.IsLocked(i, j), _model.Table.IsLocked(i, j));
                }

            Assert.AreEqual(0, _model.Time);

            _mock.Verify(dataAccess => dataAccess.LoadAsync(String.Empty), Times.Once());
        }


        private void Model_GameOver(Object? sender, RunawayRobotEventArgs e)
        {
            Assert.AreEqual(0, e.Time);
            Assert.IsFalse(e.IsWon);
        }
    }
}