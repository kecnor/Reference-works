using RunawayRobot.Model;
using RunawayRobot.Persistence;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;

namespace RunawayRobotWinForm
{
    public partial class Form1 : Form
    {
        #region Fields

        private IsRunawayRobotDataAccess dataAccess;
        RunawayRobotGameModel model = null!;
        private Label[,] gamefield = null!;

        #endregion
        #region Constructor
        public Form1()
        {
            InitializeComponent();
            dataAccess = new RunawayRobotFlieDataAccess();
            model = new RunawayRobotGameModel(dataAccess);
            model.GameOver += new EventHandler<RunawayRobotEventArgs>(Game_GameOver);
        }

        #endregion
        #region StartMenu
        private void NewGame_Click(object sender, EventArgs e)
        {
            if (model.GetMapSize != 0)
            {
                model.Create();
                StartMenu.Visible = false;
                ClientSize = new Size(model.GameFieldWidht, model.GameFieldHeight);
                GameBox.Visible = true;
                SetupGameField();
            }
            else
            {
                MessageBox.Show("Firstly please choose the size of the game field", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private async void LoadGame_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    await model.LoadGameAsync(openFileDialog.FileName);
                    LoadGame.Enabled = true;
                    StartMenu.Visible = false;
                    ClientSize = new Size(model.GameFieldWidht, model.GameFieldHeight);
                    GameBox.Visible = true;
                    GameField.Visible = true;
                    model.SetFieldSize();
                    SetupGameField();
                    ColorMap();
                    UpdateTime(sender, e);
                }
                catch (RunawayRobotDataException)
                {
                    MessageBox.Show("Loading failed!" + Environment.NewLine + "Wrong path or file type.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    model.Create();
                    LoadGame.Enabled = true;
                }
            }

        }
        private void SizeGame_Click(object sender, EventArgs e)
        {
            StartMenu.Visible = false;
            SizeMenu.Visible = true;
        }

        private void QuitGame_Click(object? sender, EventArgs e)
        {
            QuitGame.Click += new EventHandler(QuitGame_Click);
            Close();
        }
        #endregion
        #region SizeMenu

        #region Difficulty
        private void Map7_Click(object sender, EventArgs e)
        {
            model.SetDifficulty(RunawayRobotGameModel.GameDifficulty.Easy);
            BackMenu_Click(sender, e);
        }
        private void Map11_Click(object sender, EventArgs e)
        {
            model.SetDifficulty(RunawayRobotGameModel.GameDifficulty.Medium);
            BackMenu_Click(sender, e);
        }
        private void Map15_Click(object sender, EventArgs e)
        {
            model.SetDifficulty(RunawayRobotGameModel.GameDifficulty.Hard);
            BackMenu_Click(sender, e);
        }
        #endregion
        private void BackMenu_Click(object sender, EventArgs e)
        {
            SizeMenu.Visible = false;
            StartMenu.Visible = true;
            UpdateSizeStatus();
        }
        private void UpdateSizeStatus()
        {
            SizeStatus.Text = $"Playing field size: {model.GetMapSize} x  {model.GetMapSize}";
        }

        #endregion
        #region Map

        private void SetupGameField()
        {
            GameField.Controls.Clear();
            gamefield = new Label[model.Table.Size, model.Table.Size];
            for (int i = 0; i < model.Table.Size; i++)
            {
                for (int j = 0; j < model.Table.Size; j++)
                {
                    Label field = new Label();
                    field.Size = new Size(model.FieldSize, model.FieldSize);
                    field.Location = new Point(i * model.FieldSize, j * model.FieldSize);
                    field.BorderStyle = BorderStyle.FixedSingle;
                    field.MouseClick += new MouseEventHandler(field_Click);
                    GameField.Controls.Add(field);
                    gamefield[i, j] = field;
                }
            }
            EnableMap();
        }
        private void field_Click(object? sender, MouseEventArgs e)
        {
            Label? field = sender as Label;
            int location = GameField.Controls.GetChildIndex(field);
            int x = location / model.Table.Size;
            int y = location - (x * model.Table.Size);
            model.Table.SetFieldValue(x, y, 1);
            model.Table.LockField(x, y);
            ColorMap();
        }
        public void ColorMap()
        {
            for (int i = 0; i < model.Table.Size; i++)
            {
                for (int j = 0; j < model.Table.Size; j++)
                {
                    switch (model.Table.GetFieldValue(i, j))
                    {
                        case 0:
                            gamefield[i, j].BackColor = Color.White;
                            break;
                        case 1:
                            gamefield[i, j].BackColor = Color.Brown;
                            break;
                        case 2:
                            gamefield[i, j].BackColor = Color.Wheat;
                            break;
                        case 5:
                            gamefield[i, j].BackColor = Color.Red;
                            break;
                    }
                    if (model.Table.GetFieldValue(i, j) != 0)
                        model.Table.LockField(i, j);
                }
            }
            EnableMap();
            SetupBot();
        }
        private void EnableMap()
        {
            for (int i = 0; i < model.Table.Size; i++)
            {
                for (int j = 0; j < model.Table.Size; j++)
                {
                    if (!model.Table.IsLocked(i, j))
                    {
                        gamefield[i, j].Enabled = false;
                    }
                    else
                    {
                        gamefield[i, j].Enabled = true;
                    }
                }
            }
        }
        private void UpdateTime(object? sender, System.EventArgs e)
        {
            model.SetTime(model.Time + 1);
            TimerLabel.Text = $"{model.Time / 60}:{model.Time % 60:F0} ";
        }
        private void RobotMove(object? sender, EventArgs e)
        {
            model.Robot.Move(model.Table);
            ColorMap();
            model.IsGameOver();
        }
        private void SetupBot()
        {
            gamefield[model.Robot.RobotX, model.Robot.RobotY].BackColor = Color.Black;
            gamefield[model.Robot.RobotX, model.Robot.RobotY].Enabled = false;
        }
        #endregion
        #region Start

        private void StartGame_Click(object sender, EventArgs e)
        {
            StartGame.Visible = false;
            timer.Start();
            robottimer.Start();
            GameField.Enabled = true;
        }

        #endregion
        #region Pause
        private void PauseGame_Click(object sender, EventArgs e)
        {
            if (PauseGame.Text == "||" && StartGame.Visible == false)
            {
                PauseGame.Text = "|>";
                MainMenuButton.Visible = true;
                SaveBotton.Visible = true;
                GameField.Enabled = false;
                timer.Stop();
                robottimer.Stop();
            }
            else if (PauseGame.Text == "|>")
            {
                PauseGame.Text = "||";
                MainMenuButton.Visible = false;
                SaveBotton.Visible = false;
                GameField.Enabled = true;
                timer.Start();
                robottimer.Start();
            }
        }
        private void MainMenuButton_Click(object? sender, EventArgs e)
        {
            GameBox.Visible = false;
            StartMenu.Visible = true;
            StartGame.Visible = true;
            MainMenuButton.Visible = false;
            SaveBotton.Visible = false;
            GameField.Enabled = false;
            model.SetTime(0);
            TimerLabel.Text = "0:00";
            PauseGame.Text = "||";
            SizeStatus.Text = $"Playing field size:";
            ClientSize = new Size(model.MenuFieldWidht, model.MenuFieldHeight);
            gamefield = null!;
            model = new RunawayRobotGameModel(dataAccess);
            model.GameOver += new EventHandler<RunawayRobotEventArgs>(Game_GameOver);
        }
        private async void SaveBotton_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    await model.SaveGameAsync(saveFileDialog.FileName, model.Time);
                }
                catch (RunawayRobotDataException)
                {
                    MessageBox.Show("Save unsuccsessfull!" + Environment.NewLine + "Wrong path.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        #endregion
        #region GameOver

        private void Game_GameOver(Object? sender, RunawayRobotEventArgs e)
        {
            if (e.IsWon)
            {
                EndGame(sender, e, "Congratulation You have won the game!");
            }
            else
            {
                EndGame(sender, e, "You have Lost the game!");
            }
        }

        public void EndGame(object? sender, RunawayRobotEventArgs e, string content)
        {
            timer.Stop();
            robottimer.Stop();
            MessageBox.Show($"{content}" + Environment.NewLine + $"{e.Time} másodperc alatt.", "Runaway Robot", MessageBoxButtons.OK, MessageBoxIcon.Information);
            MainMenuButton_Click(sender, e);
        }
        #endregion

    }
}