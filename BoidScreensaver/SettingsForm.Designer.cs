namespace BoidScreensaver {
    partial class SettingsForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.BoidsBox = new System.Windows.Forms.PictureBox();
            this.infiniteWorld_checkBox = new System.Windows.Forms.CheckBox();
            this.Flock_checkBox = new System.Windows.Forms.CheckBox();
            this.avoid_checkBox = new System.Windows.Forms.CheckBox();
            this.align_checkBox = new System.Windows.Forms.CheckBox();
            this.flockStrength_bar = new System.Windows.Forms.TrackBar();
            this.alignStrength_bar = new System.Windows.Forms.TrackBar();
            this.avoidStrength_bar = new System.Windows.Forms.TrackBar();
            this.defaultsButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.BoidsBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.flockStrength_bar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.alignStrength_bar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.avoidStrength_bar)).BeginInit();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(188, 591);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(12, 591);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 1;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(7, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(182, 25);
            this.label3.TabIndex = 4;
            this.label3.Text = "Boid Screensaver";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "By Shayne Winn";
            // 
            // BoidsBox
            // 
            this.BoidsBox.Location = new System.Drawing.Point(270, 12);
            this.BoidsBox.Name = "BoidsBox";
            this.BoidsBox.Size = new System.Drawing.Size(990, 602);
            this.BoidsBox.TabIndex = 6;
            this.BoidsBox.TabStop = false;
            // 
            // infiniteWorld_checkBox
            // 
            this.infiniteWorld_checkBox.AutoSize = true;
            this.infiniteWorld_checkBox.Location = new System.Drawing.Point(13, 75);
            this.infiniteWorld_checkBox.Name = "infiniteWorld_checkBox";
            this.infiniteWorld_checkBox.Size = new System.Drawing.Size(82, 17);
            this.infiniteWorld_checkBox.TabIndex = 7;
            this.infiniteWorld_checkBox.Text = "Avoid Walls";
            this.infiniteWorld_checkBox.UseVisualStyleBackColor = true;
            this.infiniteWorld_checkBox.CheckedChanged += new System.EventHandler(this.infiniteWorld_checkBox_CheckedChanged);
            // 
            // Flock_checkBox
            // 
            this.Flock_checkBox.AutoSize = true;
            this.Flock_checkBox.Location = new System.Drawing.Point(13, 110);
            this.Flock_checkBox.Name = "Flock_checkBox";
            this.Flock_checkBox.Size = new System.Drawing.Size(52, 17);
            this.Flock_checkBox.TabIndex = 8;
            this.Flock_checkBox.Text = "Flock";
            this.Flock_checkBox.UseVisualStyleBackColor = true;
            this.Flock_checkBox.CheckedChanged += new System.EventHandler(this.Flock_checkBox_CheckedChanged);
            // 
            // avoid_checkBox
            // 
            this.avoid_checkBox.AutoSize = true;
            this.avoid_checkBox.Location = new System.Drawing.Point(13, 180);
            this.avoid_checkBox.Name = "avoid_checkBox";
            this.avoid_checkBox.Size = new System.Drawing.Size(53, 17);
            this.avoid_checkBox.TabIndex = 9;
            this.avoid_checkBox.Text = "Avoid";
            this.avoid_checkBox.UseVisualStyleBackColor = true;
            this.avoid_checkBox.CheckedChanged += new System.EventHandler(this.avoid_checkBox_CheckedChanged);
            // 
            // align_checkBox
            // 
            this.align_checkBox.AutoSize = true;
            this.align_checkBox.Location = new System.Drawing.Point(13, 145);
            this.align_checkBox.Name = "align_checkBox";
            this.align_checkBox.Size = new System.Drawing.Size(49, 17);
            this.align_checkBox.TabIndex = 10;
            this.align_checkBox.Text = "Align";
            this.align_checkBox.UseVisualStyleBackColor = true;
            this.align_checkBox.CheckedChanged += new System.EventHandler(this.align_checkBox_CheckedChanged);
            // 
            // flockStrength_bar
            // 
            this.flockStrength_bar.Location = new System.Drawing.Point(101, 110);
            this.flockStrength_bar.Maximum = 100;
            this.flockStrength_bar.Minimum = 1;
            this.flockStrength_bar.Name = "flockStrength_bar";
            this.flockStrength_bar.Size = new System.Drawing.Size(163, 45);
            this.flockStrength_bar.TabIndex = 11;
            this.flockStrength_bar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.flockStrength_bar.Value = 1;
            this.flockStrength_bar.Scroll += new System.EventHandler(this.flockStrength_bar_Scroll);
            // 
            // alignStrength_bar
            // 
            this.alignStrength_bar.Location = new System.Drawing.Point(101, 145);
            this.alignStrength_bar.Maximum = 100;
            this.alignStrength_bar.Minimum = 1;
            this.alignStrength_bar.Name = "alignStrength_bar";
            this.alignStrength_bar.Size = new System.Drawing.Size(163, 45);
            this.alignStrength_bar.TabIndex = 12;
            this.alignStrength_bar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.alignStrength_bar.Value = 1;
            this.alignStrength_bar.Scroll += new System.EventHandler(this.alignStrength_bar_Scroll);
            // 
            // avoidStrength_bar
            // 
            this.avoidStrength_bar.Location = new System.Drawing.Point(101, 180);
            this.avoidStrength_bar.Maximum = 100;
            this.avoidStrength_bar.Minimum = 1;
            this.avoidStrength_bar.Name = "avoidStrength_bar";
            this.avoidStrength_bar.Size = new System.Drawing.Size(163, 45);
            this.avoidStrength_bar.TabIndex = 13;
            this.avoidStrength_bar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.avoidStrength_bar.Value = 1;
            this.avoidStrength_bar.Scroll += new System.EventHandler(this.avoidStrength_bar_Scroll);
            // 
            // defaultsButton
            // 
            this.defaultsButton.Location = new System.Drawing.Point(100, 591);
            this.defaultsButton.Name = "defaultsButton";
            this.defaultsButton.Size = new System.Drawing.Size(75, 23);
            this.defaultsButton.TabIndex = 14;
            this.defaultsButton.Text = "Defaults";
            this.defaultsButton.UseVisualStyleBackColor = true;
            this.defaultsButton.Click += new System.EventHandler(this.defaultsButton_Click);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1272, 626);
            this.Controls.Add(this.defaultsButton);
            this.Controls.Add(this.avoidStrength_bar);
            this.Controls.Add(this.alignStrength_bar);
            this.Controls.Add(this.flockStrength_bar);
            this.Controls.Add(this.align_checkBox);
            this.Controls.Add(this.avoid_checkBox);
            this.Controls.Add(this.Flock_checkBox);
            this.Controls.Add(this.infiniteWorld_checkBox);
            this.Controls.Add(this.BoidsBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Name = "SettingsForm";
            this.Text = "SettingsForm";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.BoidsBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.flockStrength_bar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.alignStrength_bar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.avoidStrength_bar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox BoidsBox;
        private System.Windows.Forms.CheckBox infiniteWorld_checkBox;
        private System.Windows.Forms.CheckBox Flock_checkBox;
        private System.Windows.Forms.CheckBox avoid_checkBox;
        private System.Windows.Forms.CheckBox align_checkBox;
        private System.Windows.Forms.TrackBar flockStrength_bar;
        private System.Windows.Forms.TrackBar alignStrength_bar;
        private System.Windows.Forms.TrackBar avoidStrength_bar;
        private System.Windows.Forms.Button defaultsButton;
    }
}