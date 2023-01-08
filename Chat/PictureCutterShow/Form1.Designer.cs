namespace PictureCutterShow
{
    partial class Form1
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.buttonCut = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1w = new System.Windows.Forms.Label();
            this.label1h = new System.Windows.Forms.Label();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label2h = new System.Windows.Forms.Label();
            this.label2w = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(36, 45);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(420, 420);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(497, 45);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(420, 420);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // buttonCut
            // 
            this.buttonCut.Location = new System.Drawing.Point(497, 12);
            this.buttonCut.Name = "buttonCut";
            this.buttonCut.Size = new System.Drawing.Size(75, 23);
            this.buttonCut.TabIndex = 2;
            this.buttonCut.Text = "Cut";
            this.buttonCut.UseVisualStyleBackColor = true;
            this.buttonCut.Click += new System.EventHandler(this.buttonCut_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(36, 497);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Size";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1w
            // 
            this.label1w.AutoSize = true;
            this.label1w.Location = new System.Drawing.Point(237, 497);
            this.label1w.Name = "label1w";
            this.label1w.Size = new System.Drawing.Size(35, 13);
            this.label1w.TabIndex = 4;
            this.label1w.Text = "label1";
            // 
            // label1h
            // 
            this.label1h.AutoSize = true;
            this.label1h.Location = new System.Drawing.Point(237, 521);
            this.label1h.Name = "label1h";
            this.label1h.Size = new System.Drawing.Size(35, 13);
            this.label1h.TabIndex = 5;
            this.label1h.Text = "label2";
            // 
            // buttonLoad
            // 
            this.buttonLoad.Location = new System.Drawing.Point(36, 12);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(75, 23);
            this.buttonLoad.TabIndex = 6;
            this.buttonLoad.Text = "Load";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // label2h
            // 
            this.label2h.AutoSize = true;
            this.label2h.Location = new System.Drawing.Point(647, 521);
            this.label2h.Name = "label2h";
            this.label2h.Size = new System.Drawing.Size(35, 13);
            this.label2h.TabIndex = 8;
            this.label2h.Text = "label3";
            // 
            // label2w
            // 
            this.label2w.AutoSize = true;
            this.label2w.Location = new System.Drawing.Point(647, 497);
            this.label2w.Name = "label2w";
            this.label2w.Size = new System.Drawing.Size(35, 13);
            this.label2w.TabIndex = 7;
            this.label2w.Text = "label4";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(958, 546);
            this.Controls.Add(this.label2h);
            this.Controls.Add(this.label2w);
            this.Controls.Add(this.buttonLoad);
            this.Controls.Add(this.label1h);
            this.Controls.Add(this.label1w);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.buttonCut);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button buttonCut;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1w;
        private System.Windows.Forms.Label label1h;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label2h;
        private System.Windows.Forms.Label label2w;
    }
}

