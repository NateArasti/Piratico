
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
            this.Scout = new System.Windows.Forms.Button();
            this.Up = new System.Windows.Forms.Button();
            this.Left = new System.Windows.Forms.Button();
            this.Right = new System.Windows.Forms.Button();
            this.Down = new System.Windows.Forms.Button();
            this.Shoot = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Scout
            // 
            this.Scout.Location = new System.Drawing.Point(12, 193);
            this.Scout.Name = "Scout";
            this.Scout.Size = new System.Drawing.Size(123, 42);
            this.Scout.TabIndex = 0;
            this.Scout.Text = "scout";
            this.Scout.UseVisualStyleBackColor = true;
            // 
            // Up
            // 
            this.Up.AutoEllipsis = true;
            this.Up.Font = new System.Drawing.Font("Perpetua Titling MT", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Up.Location = new System.Drawing.Point(51, 241);
            this.Up.Name = "Up";
            this.Up.Size = new System.Drawing.Size(40, 39);
            this.Up.TabIndex = 1;
            this.Up.Text = "U";
            this.Up.UseVisualStyleBackColor = true;
            this.Up.Visible = false;
            // 
            // Left
            // 
            this.Left.AutoEllipsis = true;
            this.Left.Font = new System.Drawing.Font("Perpetua Titling MT", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Left.Location = new System.Drawing.Point(11, 286);
            this.Left.Name = "Left";
            this.Left.Size = new System.Drawing.Size(40, 39);
            this.Left.TabIndex = 2;
            this.Left.Text = "L";
            this.Left.UseVisualStyleBackColor = true;
            this.Left.Visible = false;
            // 
            // Right
            // 
            this.Right.AutoEllipsis = true;
            this.Right.Font = new System.Drawing.Font("Perpetua Titling MT", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Right.Location = new System.Drawing.Point(92, 286);
            this.Right.Name = "Right";
            this.Right.Size = new System.Drawing.Size(40, 39);
            this.Right.TabIndex = 3;
            this.Right.Text = "R";
            this.Right.UseVisualStyleBackColor = true;
            this.Right.Visible = false;
            // 
            // Down
            // 
            this.Down.AutoEllipsis = true;
            this.Down.Font = new System.Drawing.Font("Perpetua Titling MT", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Down.Location = new System.Drawing.Point(51, 331);
            this.Down.Name = "Down";
            this.Down.Size = new System.Drawing.Size(40, 39);
            this.Down.TabIndex = 4;
            this.Down.Text = "D";
            this.Down.UseVisualStyleBackColor = true;
            this.Down.Visible = false;
            // 
            // Shoot
            // 
            this.Shoot.Font = new System.Drawing.Font("Perpetua Titling MT", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Shoot.Location = new System.Drawing.Point(12, 85);
            this.Shoot.Name = "Shoot";
            this.Shoot.Size = new System.Drawing.Size(123, 42);
            this.Shoot.TabIndex = 5;
            this.Shoot.Text = "shoot";
            this.Shoot.UseVisualStyleBackColor = true;
            // 
            // PiraticoGame
            // 
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1280, 720);
            this.Controls.Add(this.Shoot);
            this.Controls.Add(this.Down);
            this.Controls.Add(this.Right);
            this.Controls.Add(this.Left);
            this.Controls.Add(this.Up);
            this.Controls.Add(this.Scout);
            this.Cursor = System.Windows.Forms.Cursors.Cross;
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Perpetua Titling MT", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Red;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PiraticoGame";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button Scout;
        private System.Windows.Forms.Button Up;
        private System.Windows.Forms.Button Left;
        private System.Windows.Forms.Button Right;
        private System.Windows.Forms.Button Down;
        private System.Windows.Forms.Button Shoot;
    }
}

