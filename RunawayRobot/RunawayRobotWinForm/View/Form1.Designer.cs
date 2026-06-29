using RunawayRobot.Model;
using System.Windows.Forms;

namespace RunawayRobotWinForm
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            StartMenu = new Panel();
            NewGame = new Button();
            LoadGame = new Button();
            SizeGame = new Button();
            QuitGame = new Button();
            SizeStatusStrip = new StatusStrip();
            SizeStatus = new ToolStripStatusLabel();
            SizeMenu = new Panel();
            Map7 = new Button();
            Map11 = new Button();
            Map15 = new Button();
            BackMenu = new Button();
            GameBox = new Panel();
            SaveBotton = new Button();
            MainMenuButton = new Button();
            StartGame = new Button();
            TimerLabel = new Label();
            PauseGame = new Button();
            GameField = new Panel();
            timer = new System.Windows.Forms.Timer(components);
            robottimer = new System.Windows.Forms.Timer(components);
            saveFileDialog = new SaveFileDialog();
            openFileDialog = new OpenFileDialog();
            StartMenu.SuspendLayout();
            SizeStatusStrip.SuspendLayout();
            SizeMenu.SuspendLayout();
            GameBox.SuspendLayout();
            SuspendLayout();
            // 
            // StartMenu
            // 
            StartMenu.Controls.Add(NewGame);
            StartMenu.Controls.Add(LoadGame);
            StartMenu.Controls.Add(SizeGame);
            StartMenu.Controls.Add(QuitGame);
            StartMenu.Controls.Add(SizeStatusStrip);
            StartMenu.Font = new Font("Comic Sans MS", 10.875F, FontStyle.Regular, GraphicsUnit.Point);
            StartMenu.Location = new Point(0, 0);
            StartMenu.Name = "StartMenu";
            StartMenu.Size = new Size(450, 600);
            StartMenu.TabIndex = 0;
            // 
            // NewGame
            // 
            NewGame.Location = new Point(125, 75);
            NewGame.Name = "NewGame";
            NewGame.Size = new Size(200, 75);
            NewGame.TabIndex = 4;
            NewGame.Text = "New Game";
            NewGame.UseVisualStyleBackColor = true;
            NewGame.Click += NewGame_Click;
            // 
            // LoadGame
            // 
            LoadGame.Location = new Point(125, 200);
            LoadGame.Name = "LoadGame";
            LoadGame.Size = new Size(200, 75);
            LoadGame.TabIndex = 3;
            LoadGame.Text = "Load";
            LoadGame.UseVisualStyleBackColor = true;
            LoadGame.Click += LoadGame_Click;
            // 
            // SizeGame
            // 
            SizeGame.Location = new Point(125, 325);
            SizeGame.Name = "SizeGame";
            SizeGame.Size = new Size(200, 75);
            SizeGame.TabIndex = 2;
            SizeGame.Text = "Size";
            SizeGame.UseVisualStyleBackColor = true;
            SizeGame.Click += SizeGame_Click;
            // 
            // QuitGame
            // 
            QuitGame.Location = new Point(125, 450);
            QuitGame.Name = "QuitGame";
            QuitGame.Size = new Size(200, 75);
            QuitGame.TabIndex = 1;
            QuitGame.Text = "Quit";
            QuitGame.UseVisualStyleBackColor = true;
            QuitGame.Click += QuitGame_Click;
            // 
            // SizeStatusStrip
            // 
            SizeStatusStrip.ImageScalingSize = new Size(32, 32);
            SizeStatusStrip.Items.AddRange(new ToolStripItem[] { SizeStatus });
            SizeStatusStrip.Location = new Point(0, 557);
            SizeStatusStrip.Name = "SizeStatusStrip";
            SizeStatusStrip.Size = new Size(450, 43);
            SizeStatusStrip.TabIndex = 0;
            SizeStatusStrip.Text = "statusStrip1";
            // 
            // SizeStatus
            // 
            SizeStatus.Font = new Font("Comic Sans MS", 9F, FontStyle.Regular, GraphicsUnit.Point);
            SizeStatus.Name = "SizeStatus";
            SizeStatus.Size = new Size(212, 33);
            SizeStatus.Text = "Playing field size:";
            // 
            // SizeMenu
            // 
            SizeMenu.Controls.Add(Map7);
            SizeMenu.Controls.Add(Map11);
            SizeMenu.Controls.Add(Map15);
            SizeMenu.Controls.Add(BackMenu);
            SizeMenu.Font = new Font("Comic Sans MS", 10.875F, FontStyle.Regular, GraphicsUnit.Point);
            SizeMenu.Location = new Point(0, 0);
            SizeMenu.Name = "SizeMenu";
            SizeMenu.Size = new Size(450, 600);
            SizeMenu.TabIndex = 5;
            SizeMenu.Visible = false;
            // 
            // Map7
            // 
            Map7.Location = new Point(125, 75);
            Map7.Name = "Map7";
            Map7.Size = new Size(200, 75);
            Map7.TabIndex = 4;
            Map7.Text = "Easy";
            Map7.UseVisualStyleBackColor = true;
            Map7.Click += Map7_Click;
            // 
            // Map11
            // 
            Map11.Location = new Point(125, 200);
            Map11.Name = "Map11";
            Map11.Size = new Size(200, 75);
            Map11.TabIndex = 3;
            Map11.Text = "Medium";
            Map11.UseVisualStyleBackColor = true;
            Map11.Click += Map11_Click;
            // 
            // Map15
            // 
            Map15.Location = new Point(125, 325);
            Map15.Name = "Map15";
            Map15.Size = new Size(200, 75);
            Map15.TabIndex = 2;
            Map15.Text = "Hard";
            Map15.UseVisualStyleBackColor = true;
            Map15.Click += Map15_Click;
            // 
            // BackMenu
            // 
            BackMenu.Location = new Point(125, 450);
            BackMenu.Name = "BackMenu";
            BackMenu.Size = new Size(200, 75);
            BackMenu.TabIndex = 1;
            BackMenu.Text = "Back";
            BackMenu.UseVisualStyleBackColor = true;
            BackMenu.Click += BackMenu_Click;
            // 
            // GameBox
            // 
            GameBox.Controls.Add(SaveBotton);
            GameBox.Controls.Add(MainMenuButton);
            GameBox.Controls.Add(StartGame);
            GameBox.Controls.Add(TimerLabel);
            GameBox.Controls.Add(PauseGame);
            GameBox.Controls.Add(GameField);
            GameBox.Location = new Point(0, 0);
            GameBox.Name = "GameBox";
            GameBox.Size = new Size(1255, 1355);
            GameBox.TabIndex = 6;
            GameBox.Visible = false;
            // 
            // SaveBotton
            // 
            SaveBotton.Font = new Font("Comic Sans MS", 13.875F, FontStyle.Regular, GraphicsUnit.Point);
            SaveBotton.Location = new Point(272, 45);
            SaveBotton.Name = "SaveBotton";
            SaveBotton.Size = new Size(250, 75);
            SaveBotton.TabIndex = 5;
            SaveBotton.Text = "Save";
            SaveBotton.UseVisualStyleBackColor = true;
            SaveBotton.Visible = false;
            SaveBotton.Click += SaveBotton_Click;
            // 
            // MainMenuButton
            // 
            MainMenuButton.Font = new Font("Comic Sans MS", 13.875F, FontStyle.Regular, GraphicsUnit.Point);
            MainMenuButton.Location = new Point(734, 46);
            MainMenuButton.Name = "MainMenuButton";
            MainMenuButton.Size = new Size(250, 75);
            MainMenuButton.TabIndex = 4;
            MainMenuButton.Text = "Main Menu";
            MainMenuButton.UseVisualStyleBackColor = true;
            MainMenuButton.Visible = false;
            MainMenuButton.Click += MainMenuButton_Click;
            // 
            // StartGame
            // 
            StartGame.Font = new Font("Comic Sans MS", 13.875F, FontStyle.Regular, GraphicsUnit.Point);
            StartGame.Location = new Point(528, 46);
            StartGame.Name = "StartGame";
            StartGame.Size = new Size(200, 75);
            StartGame.TabIndex = 3;
            StartGame.Text = "Start";
            StartGame.UseVisualStyleBackColor = true;
            StartGame.Click += StartGame_Click;
            // 
            // TimerLabel
            // 
            TimerLabel.AutoSize = true;
            TimerLabel.Font = new Font("Comic Sans MS", 13.875F, FontStyle.Regular, GraphicsUnit.Point);
            TimerLabel.Location = new Point(50, 45);
            TimerLabel.Name = "TimerLabel";
            TimerLabel.Size = new Size(125, 52);
            TimerLabel.TabIndex = 2;
            TimerLabel.Text = "00:00";
            // 
            // PauseGame
            // 
            PauseGame.Font = new Font("Segoe UI Black", 13.875F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point);
            PauseGame.Location = new Point(1130, 35);
            PauseGame.Name = "PauseGame";
            PauseGame.RightToLeft = RightToLeft.No;
            PauseGame.Size = new Size(75, 75);
            PauseGame.TabIndex = 1;
            PauseGame.Text = "||";
            PauseGame.UseVisualStyleBackColor = true;
            PauseGame.Click += PauseGame_Click;
            // 
            // GameField
            // 
            GameField.Enabled = false;
            GameField.Location = new Point(50, 150);
            GameField.Name = "GameField";
            GameField.Size = new Size(1155, 1155);
            GameField.TabIndex = 0;
            // 
            // timer
            // 
            timer.Interval = 1000;
            timer.Tick += UpdateTime;
            // 
            // robottimer
            // 
            robottimer.Interval = 500;
            robottimer.Tick += RobotMove;
            // 
            // saveFileDialog
            // 
            saveFileDialog.Filter = "Ruaway Robot tábla (*.stl)|*.stl";
            saveFileDialog.Title = "Runaway Robot játék mentése";
            // 
            // openFileDialog
            // 
            openFileDialog.Filter = "Ruaway Robot tábla (*.stl)|*.stl";
            openFileDialog.Title = "Ruaway Robot játék betöltése";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(450, 600);
            Controls.Add(GameBox);
            Controls.Add(StartMenu);
            Controls.Add(SizeMenu);
            Name = "Form1";
            Text = "Runaway Robot";
            StartMenu.ResumeLayout(false);
            StartMenu.PerformLayout();
            SizeStatusStrip.ResumeLayout(false);
            SizeStatusStrip.PerformLayout();
            SizeMenu.ResumeLayout(false);
            GameBox.ResumeLayout(false);
            GameBox.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel StartMenu;
        private Panel SizeMenu;
        private Button Map7;
        private Button Map11;
        private Button Map15;
        private Button BackMenu;
        private Button NewGame;
        private Button LoadGame;
        private Button SizeGame;
        private Button QuitGame;
        private StatusStrip SizeStatusStrip;
        private ToolStripStatusLabel SizeStatus;
        private Panel GameBox;
        private Button PauseGame;
        private Panel GameField;
        private Label TimerLabel;
        private Button StartGame;
        private System.Windows.Forms.Timer timer;
        private Button SaveBotton;
        private Button MainMenuButton;
        private System.Windows.Forms.Timer robottimer;
        private SaveFileDialog saveFileDialog;
        private OpenFileDialog openFileDialog;
    }
}