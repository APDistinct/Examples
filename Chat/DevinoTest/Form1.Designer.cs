namespace DevinoTest
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
            this.textBoxSMS = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonSMS = new System.Windows.Forms.Button();
            this.buttonEmail = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBoxMess = new System.Windows.Forms.TextBox();
            this.labelText = new System.Windows.Forms.Label();
            this.buttonTestEmail = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxButton = new System.Windows.Forms.TextBox();
            this.textBoxAction = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxImage = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxViber = new System.Windows.Forms.TextBox();
            this.buttonViber = new System.Windows.Forms.Button();
            this.buttonGetStatus = new System.Windows.Forms.Button();
            this.textBoxGetStatus = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBoxSMS
            // 
            this.textBoxSMS.Location = new System.Drawing.Point(31, 41);
            this.textBoxSMS.Multiline = true;
            this.textBoxSMS.Name = "textBoxSMS";
            this.textBoxSMS.Size = new System.Drawing.Size(144, 140);
            this.textBoxSMS.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(56, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "СМС - номера";
            // 
            // buttonSMS
            // 
            this.buttonSMS.Location = new System.Drawing.Point(58, 203);
            this.buttonSMS.Name = "buttonSMS";
            this.buttonSMS.Size = new System.Drawing.Size(75, 23);
            this.buttonSMS.TabIndex = 2;
            this.buttonSMS.Text = "СМС пуск";
            this.buttonSMS.UseVisualStyleBackColor = true;
            this.buttonSMS.Click += new System.EventHandler(this.buttonSMS_ClickAsync);
            // 
            // buttonEmail
            // 
            this.buttonEmail.Location = new System.Drawing.Point(663, 203);
            this.buttonEmail.Name = "buttonEmail";
            this.buttonEmail.Size = new System.Drawing.Size(75, 23);
            this.buttonEmail.TabIndex = 5;
            this.buttonEmail.Text = "Email пуск";
            this.buttonEmail.UseVisualStyleBackColor = true;
            this.buttonEmail.Click += new System.EventHandler(this.ButtonEmail_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(557, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Email адреса";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(446, 41);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(342, 140);
            this.textBox2.TabIndex = 3;
            // 
            // textBoxMess
            // 
            this.textBoxMess.Location = new System.Drawing.Point(202, 41);
            this.textBoxMess.Multiline = true;
            this.textBoxMess.Name = "textBoxMess";
            this.textBoxMess.Size = new System.Drawing.Size(219, 64);
            this.textBoxMess.TabIndex = 6;
            // 
            // labelText
            // 
            this.labelText.AutoSize = true;
            this.labelText.Location = new System.Drawing.Point(249, 9);
            this.labelText.Name = "labelText";
            this.labelText.Size = new System.Drawing.Size(97, 13);
            this.labelText.TabIndex = 7;
            this.labelText.Text = "Текст сообщения";
            // 
            // buttonTestEmail
            // 
            this.buttonTestEmail.Location = new System.Drawing.Point(497, 203);
            this.buttonTestEmail.Name = "buttonTestEmail";
            this.buttonTestEmail.Size = new System.Drawing.Size(75, 23);
            this.buttonTestEmail.TabIndex = 8;
            this.buttonTestEmail.Text = "Test Email";
            this.buttonTestEmail.UseVisualStyleBackColor = true;
            this.buttonTestEmail.Visible = false;
            this.buttonTestEmail.Click += new System.EventHandler(this.buttonTestEmail_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(472, 242);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(156, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "SourceAddresses";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.ButtonEmail_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(260, 213);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "label3";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(632, 242);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(156, 23);
            this.button2.TabIndex = 10;
            this.button2.Text = "Messages";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(154, 203);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 11;
            this.button3.Text = "СМС пуск 2";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Button3_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(37, 257);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(122, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Наименование кнопки";
            // 
            // textBoxButton
            // 
            this.textBoxButton.Location = new System.Drawing.Point(179, 257);
            this.textBoxButton.Name = "textBoxButton";
            this.textBoxButton.Size = new System.Drawing.Size(100, 20);
            this.textBoxButton.TabIndex = 13;
            this.textBoxButton.Text = "Переход";
            // 
            // textBoxAction
            // 
            this.textBoxAction.Location = new System.Drawing.Point(129, 286);
            this.textBoxAction.Name = "textBoxAction";
            this.textBoxAction.Size = new System.Drawing.Size(282, 20);
            this.textBoxAction.TabIndex = 15;
            this.textBoxAction.Text = "https://ya.ru";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(37, 286);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "URL перехода";
            // 
            // textBoxImage
            // 
            this.textBoxImage.Location = new System.Drawing.Point(129, 312);
            this.textBoxImage.Name = "textBoxImage";
            this.textBoxImage.Size = new System.Drawing.Size(282, 20);
            this.textBoxImage.TabIndex = 17;
            this.textBoxImage.Text = "https://chat.faberlic.com/user_m.png";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(37, 312);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(57, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "URL фото";
            // 
            // textBoxViber
            // 
            this.textBoxViber.Location = new System.Drawing.Point(202, 111);
            this.textBoxViber.Multiline = true;
            this.textBoxViber.Name = "textBoxViber";
            this.textBoxViber.Size = new System.Drawing.Size(219, 70);
            this.textBoxViber.TabIndex = 18;
            // 
            // buttonViber
            // 
            this.buttonViber.Location = new System.Drawing.Point(58, 356);
            this.buttonViber.Name = "buttonViber";
            this.buttonViber.Size = new System.Drawing.Size(75, 23);
            this.buttonViber.TabIndex = 19;
            this.buttonViber.Text = "Viber пуск";
            this.buttonViber.UseVisualStyleBackColor = true;
            this.buttonViber.Click += new System.EventHandler(this.buttonViber_ClickAsync);
            // 
            // buttonGetStatus
            // 
            this.buttonGetStatus.Location = new System.Drawing.Point(59, 562);
            this.buttonGetStatus.Name = "buttonGetStatus";
            this.buttonGetStatus.Size = new System.Drawing.Size(75, 23);
            this.buttonGetStatus.TabIndex = 20;
            this.buttonGetStatus.Text = "GetStatus";
            this.buttonGetStatus.UseVisualStyleBackColor = true;
            this.buttonGetStatus.Click += new System.EventHandler(this.buttonGetStatus_Click);
            // 
            // textBoxGetStatus
            // 
            this.textBoxGetStatus.Location = new System.Drawing.Point(58, 403);
            this.textBoxGetStatus.Multiline = true;
            this.textBoxGetStatus.Name = "textBoxGetStatus";
            this.textBoxGetStatus.Size = new System.Drawing.Size(144, 140);
            this.textBoxGetStatus.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(830, 735);
            this.Controls.Add(this.buttonGetStatus);
            this.Controls.Add(this.buttonViber);
            this.Controls.Add(this.textBoxViber);
            this.Controls.Add(this.textBoxImage);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBoxAction);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.buttonTestEmail);
            this.Controls.Add(this.labelText);
            this.Controls.Add(this.textBoxMess);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonEmail);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.buttonSMS);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxGetStatus);
            this.Controls.Add(this.textBoxSMS);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxSMS;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonSMS;
        private System.Windows.Forms.Button buttonEmail;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBoxMess;
        private System.Windows.Forms.Label labelText;
        private System.Windows.Forms.Button buttonTestEmail;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxButton;
        private System.Windows.Forms.TextBox textBoxAction;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxImage;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxViber;
        private System.Windows.Forms.Button buttonViber;
        private System.Windows.Forms.Button buttonGetStatus;
        private System.Windows.Forms.TextBox textBoxGetStatus;
    }
}

