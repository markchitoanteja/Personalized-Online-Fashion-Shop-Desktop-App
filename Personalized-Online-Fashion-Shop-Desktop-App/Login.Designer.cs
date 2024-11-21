namespace Personalized_Online_Fashion_Shop_Desktop_App
{
    partial class Login
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.login_username = new System.Windows.Forms.TextBox();
            this.login_password = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.login_remember_me = new System.Windows.Forms.CheckBox();
            this.login_submit = new System.Windows.Forms.Button();
            this.btn_temp = new System.Windows.Forms.Button();
            this.loading = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.main_form = new System.Windows.Forms.Panel();
            this.err_username = new System.Windows.Forms.ErrorProvider(this.components);
            this.err_password = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.loading.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.main_form.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.err_username)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.err_password)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(313, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(250, 227);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(10, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "Username";
            // 
            // login_username
            // 
            this.login_username.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.login_username.Location = new System.Drawing.Point(13, 63);
            this.login_username.Name = "login_username";
            this.login_username.Size = new System.Drawing.Size(272, 26);
            this.login_username.TabIndex = 1;
            this.login_username.KeyDown += new System.Windows.Forms.KeyEventHandler(this.login_username_KeyDown);
            // 
            // login_password
            // 
            this.login_password.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.login_password.Location = new System.Drawing.Point(13, 123);
            this.login_password.Name = "login_password";
            this.login_password.Size = new System.Drawing.Size(272, 26);
            this.login_password.TabIndex = 2;
            this.login_password.UseSystemPasswordChar = true;
            this.login_password.KeyDown += new System.Windows.Forms.KeyEventHandler(this.login_password_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(10, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 18);
            this.label2.TabIndex = 3;
            this.label2.Text = "Password";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial Rounded MT Bold", 14.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(84, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(132, 22);
            this.label3.TabIndex = 5;
            this.label3.Text = "LOGIN FORM";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.login_remember_me);
            this.panel1.Controls.Add(this.login_submit);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.login_password);
            this.panel1.Controls.Add(this.login_username);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(305, 227);
            this.panel1.TabIndex = 6;
            // 
            // login_remember_me
            // 
            this.login_remember_me.AutoSize = true;
            this.login_remember_me.Cursor = System.Windows.Forms.Cursors.Hand;
            this.login_remember_me.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.login_remember_me.Location = new System.Drawing.Point(13, 155);
            this.login_remember_me.Name = "login_remember_me";
            this.login_remember_me.Size = new System.Drawing.Size(111, 20);
            this.login_remember_me.TabIndex = 3;
            this.login_remember_me.Text = "Remember Me";
            this.login_remember_me.UseVisualStyleBackColor = true;
            // 
            // login_submit
            // 
            this.login_submit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(110)))), ((int)(((byte)(253)))));
            this.login_submit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.login_submit.FlatAppearance.BorderSize = 0;
            this.login_submit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.login_submit.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.login_submit.ForeColor = System.Drawing.Color.White;
            this.login_submit.Location = new System.Drawing.Point(13, 188);
            this.login_submit.Name = "login_submit";
            this.login_submit.Size = new System.Drawing.Size(272, 39);
            this.login_submit.TabIndex = 4;
            this.login_submit.Text = "Login";
            this.login_submit.UseVisualStyleBackColor = false;
            this.login_submit.Click += new System.EventHandler(this.login_submit_Click);
            // 
            // btn_temp
            // 
            this.btn_temp.Location = new System.Drawing.Point(608, 12);
            this.btn_temp.Name = "btn_temp";
            this.btn_temp.Size = new System.Drawing.Size(75, 23);
            this.btn_temp.TabIndex = 5;
            this.btn_temp.Text = "button2";
            this.btn_temp.UseVisualStyleBackColor = true;
            // 
            // loading
            // 
            this.loading.Controls.Add(this.label4);
            this.loading.Controls.Add(this.pictureBox2);
            this.loading.Location = new System.Drawing.Point(212, 84);
            this.loading.Name = "loading";
            this.loading.Size = new System.Drawing.Size(160, 93);
            this.loading.TabIndex = 7;
            this.loading.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial Rounded MT Bold", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(22, 64);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(117, 22);
            this.label4.TabIndex = 2;
            this.label4.Text = "Please Wait";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(55, 6);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(50, 50);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 0;
            this.pictureBox2.TabStop = false;
            // 
            // main_form
            // 
            this.main_form.Controls.Add(this.panel1);
            this.main_form.Controls.Add(this.pictureBox1);
            this.main_form.Location = new System.Drawing.Point(9, 13);
            this.main_form.Name = "main_form";
            this.main_form.Size = new System.Drawing.Size(566, 234);
            this.main_form.TabIndex = 8;
            // 
            // err_username
            // 
            this.err_username.ContainerControl = this;
            // 
            // err_password
            // 
            this.err_password.ContainerControl = this;
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 261);
            this.Controls.Add(this.main_form);
            this.Controls.Add(this.btn_temp);
            this.Controls.Add(this.loading);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Personalized Online Fashion Shop";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Login_FormClosing);
            this.Load += new System.EventHandler(this.Login_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.loading.ResumeLayout(false);
            this.loading.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.main_form.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.err_username)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.err_password)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox login_username;
        private System.Windows.Forms.TextBox login_password;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button login_submit;
        private System.Windows.Forms.CheckBox login_remember_me;
        private System.Windows.Forms.Button btn_temp;
        private System.Windows.Forms.Panel loading;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Panel main_form;
        private System.Windows.Forms.ErrorProvider err_username;
        private System.Windows.Forms.ErrorProvider err_password;
    }
}

