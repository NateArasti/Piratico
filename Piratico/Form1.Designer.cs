
namespace Piratico
{
    partial class PiraticoGame
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PiraticoGame));
            this.Scout = new System.Windows.Forms.Button();
            this.UpButton = new System.Windows.Forms.Button();
            this.LeftButton = new System.Windows.Forms.Button();
            this.RightButton = new System.Windows.Forms.Button();
            this.DownButton = new System.Windows.Forms.Button();
            this.Shoot = new System.Windows.Forms.Button();
            this.Crew = new System.Windows.Forms.Label();
            this.Gold = new System.Windows.Forms.Label();
            this.Strength = new System.Windows.Forms.Label();
            this.Skip = new System.Windows.Forms.Button();
            this.IntroText = new System.Windows.Forms.Label();
            this.StartButton = new System.Windows.Forms.Button();
            this.IntroPanel = new System.Windows.Forms.Panel();
            this.UpgradeCost = new System.Windows.Forms.Label();
            this.Upgrade = new System.Windows.Forms.Button();
            this.Consumables = new System.Windows.Forms.Label();
            this.EndGamePanel = new System.Windows.Forms.Panel();
            this.EndGameText = new System.Windows.Forms.Label();
            this.RestartButton = new System.Windows.Forms.Button();
            this.IntroPanel.SuspendLayout();
            this.EndGamePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // Scout
            // 
            this.Scout.Location = new System.Drawing.Point(12, 258);
            this.Scout.Name = "Scout";
            this.Scout.Size = new System.Drawing.Size(123, 42);
            this.Scout.TabIndex = 0;
            this.Scout.Text = "scout";
            this.Scout.UseVisualStyleBackColor = true;
            // 
            // UpButton
            // 
            this.UpButton.AutoEllipsis = true;
            this.UpButton.Font = new System.Drawing.Font("Perpetua Titling MT", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UpButton.Location = new System.Drawing.Point(51, 306);
            this.UpButton.Name = "UpButton";
            this.UpButton.Size = new System.Drawing.Size(40, 39);
            this.UpButton.TabIndex = 1;
            this.UpButton.Text = "U";
            this.UpButton.UseVisualStyleBackColor = true;
            this.UpButton.Visible = false;
            // 
            // LeftButton
            // 
            this.LeftButton.AutoEllipsis = true;
            this.LeftButton.Font = new System.Drawing.Font("Perpetua Titling MT", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LeftButton.Location = new System.Drawing.Point(11, 351);
            this.LeftButton.Name = "LeftButton";
            this.LeftButton.Size = new System.Drawing.Size(40, 39);
            this.LeftButton.TabIndex = 2;
            this.LeftButton.Text = "L";
            this.LeftButton.UseVisualStyleBackColor = true;
            this.LeftButton.Visible = false;
            // 
            // RightButton
            // 
            this.RightButton.AutoEllipsis = true;
            this.RightButton.Font = new System.Drawing.Font("Perpetua Titling MT", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RightButton.Location = new System.Drawing.Point(92, 351);
            this.RightButton.Name = "RightButton";
            this.RightButton.Size = new System.Drawing.Size(40, 39);
            this.RightButton.TabIndex = 3;
            this.RightButton.Text = "R";
            this.RightButton.UseVisualStyleBackColor = true;
            this.RightButton.Visible = false;
            // 
            // DownButton
            // 
            this.DownButton.AutoEllipsis = true;
            this.DownButton.Font = new System.Drawing.Font("Perpetua Titling MT", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DownButton.Location = new System.Drawing.Point(51, 396);
            this.DownButton.Name = "DownButton";
            this.DownButton.Size = new System.Drawing.Size(40, 39);
            this.DownButton.TabIndex = 4;
            this.DownButton.Text = "D";
            this.DownButton.UseVisualStyleBackColor = true;
            this.DownButton.Visible = false;
            // 
            // Shoot
            // 
            this.Shoot.Font = new System.Drawing.Font("Perpetua Titling MT", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Shoot.Location = new System.Drawing.Point(12, 200);
            this.Shoot.Name = "Shoot";
            this.Shoot.Size = new System.Drawing.Size(123, 42);
            this.Shoot.TabIndex = 5;
            this.Shoot.Text = "shoot";
            this.Shoot.UseVisualStyleBackColor = true;
            // 
            // Crew
            // 
            this.Crew.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Crew.Font = new System.Drawing.Font("Perpetua Titling MT", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Crew.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.Crew.Location = new System.Drawing.Point(1130, 196);
            this.Crew.Name = "Crew";
            this.Crew.Size = new System.Drawing.Size(129, 52);
            this.Crew.TabIndex = 13;
            this.Crew.Text = "Crew";
            this.Crew.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Gold
            // 
            this.Gold.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Gold.Font = new System.Drawing.Font("Perpetua Titling MT", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Gold.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.Gold.Location = new System.Drawing.Point(1130, 255);
            this.Gold.Name = "Gold";
            this.Gold.Size = new System.Drawing.Size(129, 52);
            this.Gold.TabIndex = 14;
            this.Gold.Text = "Gold";
            this.Gold.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Strength
            // 
            this.Strength.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Strength.Font = new System.Drawing.Font("Perpetua Titling MT", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Strength.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.Strength.Location = new System.Drawing.Point(1130, 135);
            this.Strength.Name = "Strength";
            this.Strength.Size = new System.Drawing.Size(129, 52);
            this.Strength.TabIndex = 12;
            this.Strength.Text = "Strength\r\n100%";
            this.Strength.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Skip
            // 
            this.Skip.Font = new System.Drawing.Font("Perpetua Titling MT", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Skip.Location = new System.Drawing.Point(12, 145);
            this.Skip.Name = "Skip";
            this.Skip.Size = new System.Drawing.Size(123, 42);
            this.Skip.TabIndex = 16;
            this.Skip.Text = "skip";
            this.Skip.UseVisualStyleBackColor = true;
            // 
            // IntroText
            // 
            this.IntroText.Font = new System.Drawing.Font("Perpetua Titling MT", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IntroText.Location = new System.Drawing.Point(222, 9);
            this.IntroText.Name = "IntroText";
            this.IntroText.Size = new System.Drawing.Size(833, 637);
            this.IntroText.TabIndex = 1;
            this.IntroText.Text = resources.GetString("IntroText.Text");
            this.IntroText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // StartButton
            // 
            this.StartButton.BackColor = System.Drawing.Color.Azure;
            this.StartButton.ForeColor = System.Drawing.Color.Crimson;
            this.StartButton.Location = new System.Drawing.Point(575, 660);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(130, 54);
            this.StartButton.TabIndex = 0;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = false;
            // 
            // IntroPanel
            // 
            this.IntroPanel.AutoSize = true;
            this.IntroPanel.BackColor = System.Drawing.Color.LightSkyBlue;
            this.IntroPanel.Controls.Add(this.IntroText);
            this.IntroPanel.Controls.Add(this.StartButton);
            this.IntroPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.IntroPanel.Location = new System.Drawing.Point(0, 0);
            this.IntroPanel.Name = "IntroPanel";
            this.IntroPanel.Size = new System.Drawing.Size(1280, 720);
            this.IntroPanel.TabIndex = 17;
            // 
            // UpgradeCost
            // 
            this.UpgradeCost.AutoSize = true;
            this.UpgradeCost.BackColor = System.Drawing.Color.Transparent;
            this.UpgradeCost.Font = new System.Drawing.Font("Perpetua Titling MT", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UpgradeCost.Location = new System.Drawing.Point(1145, 427);
            this.UpgradeCost.Name = "UpgradeCost";
            this.UpgradeCost.Size = new System.Drawing.Size(75, 26);
            this.UpgradeCost.TabIndex = 22;
            this.UpgradeCost.Text = "Cost:";
            // 
            // Upgrade
            // 
            this.Upgrade.BackColor = System.Drawing.Color.SeaGreen;
            this.Upgrade.Font = new System.Drawing.Font("Perpetua Titling MT", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Upgrade.ForeColor = System.Drawing.Color.White;
            this.Upgrade.Location = new System.Drawing.Point(1128, 380);
            this.Upgrade.Name = "Upgrade";
            this.Upgrade.Size = new System.Drawing.Size(137, 44);
            this.Upgrade.TabIndex = 23;
            this.Upgrade.Text = "Upgrade";
            this.Upgrade.UseVisualStyleBackColor = false;
            // 
            // Consumables
            // 
            this.Consumables.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Consumables.Font = new System.Drawing.Font("Perpetua Titling MT", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Consumables.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.Consumables.Location = new System.Drawing.Point(1125, 316);
            this.Consumables.Name = "Consumables";
            this.Consumables.Size = new System.Drawing.Size(140, 52);
            this.Consumables.TabIndex = 21;
            this.Consumables.Text = "Consumables";
            this.Consumables.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // EndGamePanel
            // 
            this.EndGamePanel.AutoSize = true;
            this.EndGamePanel.BackColor = System.Drawing.Color.LightBlue;
            this.EndGamePanel.Controls.Add(this.EndGameText);
            this.EndGamePanel.Controls.Add(this.RestartButton);
            this.EndGamePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EndGamePanel.Location = new System.Drawing.Point(0, 0);
            this.EndGamePanel.Name = "EndGamePanel";
            this.EndGamePanel.Size = new System.Drawing.Size(1280, 720);
            this.EndGamePanel.TabIndex = 20;
            // 
            // EndGameText
            // 
            this.EndGameText.Location = new System.Drawing.Point(320, 340);
            this.EndGameText.Name = "EndGameText";
            this.EndGameText.Size = new System.Drawing.Size(693, 63);
            this.EndGameText.TabIndex = 1;
            this.EndGameText.Text = "Неплохо держался, попробуй ещё раз :)";
            this.EndGameText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // RestartButton
            // 
            this.RestartButton.BackColor = System.Drawing.Color.Azure;
            this.RestartButton.ForeColor = System.Drawing.Color.Crimson;
            this.RestartButton.Location = new System.Drawing.Point(575, 406);
            this.RestartButton.Name = "RestartButton";
            this.RestartButton.Size = new System.Drawing.Size(148, 54);
            this.RestartButton.TabIndex = 0;
            this.RestartButton.Text = "Restart";
            this.RestartButton.UseVisualStyleBackColor = false;
            // 
            // PiraticoGame
            // 
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1280, 720);
            this.Controls.Add(this.Skip);
            this.Controls.Add(this.Gold);
            this.Controls.Add(this.Crew);
            this.Controls.Add(this.Strength);
            this.Controls.Add(this.Upgrade);
            this.Controls.Add(this.UpgradeCost);
            this.Controls.Add(this.Consumables);
            this.Controls.Add(this.Shoot);
            this.Controls.Add(this.DownButton);
            this.Controls.Add(this.RightButton);
            this.Controls.Add(this.LeftButton);
            this.Controls.Add(this.UpButton);
            this.Controls.Add(this.Scout);
            this.Controls.Add(this.IntroPanel);
            this.Controls.Add(this.EndGamePanel);
            this.Cursor = System.Windows.Forms.Cursors.Cross;
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Perpetua Titling MT", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Red;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PiraticoGame";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.IntroPanel.ResumeLayout(false);
            this.EndGamePanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button Scout;
        private System.Windows.Forms.Button UpButton;
        private System.Windows.Forms.Button LeftButton;
        private System.Windows.Forms.Button RightButton;
        private System.Windows.Forms.Button DownButton;
        private System.Windows.Forms.Button Shoot;
        private System.Windows.Forms.Label Crew;
        private System.Windows.Forms.Label Gold;
        private System.Windows.Forms.Label Strength;
        private System.Windows.Forms.Button Skip;
        private System.Windows.Forms.Label IntroText;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Panel IntroPanel;
        private System.Windows.Forms.Panel EndGamePanel;
        private System.Windows.Forms.Label EndGameText;
        private System.Windows.Forms.Button RestartButton;
        private System.Windows.Forms.Label Consumables;
        private System.Windows.Forms.Label UpgradeCost;
        private System.Windows.Forms.Button Upgrade;
    }
}

