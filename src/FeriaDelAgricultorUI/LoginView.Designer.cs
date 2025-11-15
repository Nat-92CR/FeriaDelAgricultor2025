namespace FeriaDelAgricultorUI
{
    partial class LoginView
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
            btn_Login = new Button();
            label1 = new Label();
            label2 = new Label();
            txt_UserName = new TextBox();
            txt_Password = new TextBox();
            txt_RegisterUser = new Button();
            SuspendLayout();
            // 
            // btn_Login
            // 
            btn_Login.Location = new Point(83, 280);
            btn_Login.Margin = new Padding(3, 2, 3, 2);
            btn_Login.Name = "btn_Login";
            btn_Login.Size = new Size(132, 49);
            btn_Login.TabIndex = 0;
            btn_Login.Text = "Login";
            btn_Login.UseVisualStyleBackColor = true;
            btn_Login.Click += BtnLoginClick;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(36, 52);
            label1.Name = "label1";
            label1.Size = new Size(60, 15);
            label1.TabIndex = 1;
            label1.Text = "Username";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(36, 104);
            label2.Name = "label2";
            label2.Size = new Size(57, 15);
            label2.TabIndex = 2;
            label2.Text = "Password";
            // 
            // txt_UserName
            // 
            txt_UserName.Location = new Point(142, 52);
            txt_UserName.Margin = new Padding(3, 2, 3, 2);
            txt_UserName.Name = "txt_UserName";
            txt_UserName.Size = new Size(153, 23);
            txt_UserName.TabIndex = 3;
            txt_UserName.TextChanged += txt_UserName_TextChanged;
            // 
            // txt_Password
            // 
            txt_Password.Location = new Point(142, 104);
            txt_Password.Margin = new Padding(3, 2, 3, 2);
            txt_Password.Name = "txt_Password";
            txt_Password.PasswordChar = '*';
            txt_Password.Size = new Size(153, 23);
            txt_Password.TabIndex = 4;
            // 
            // txt_RegisterUser
            // 
            txt_RegisterUser.Location = new Point(249, 280);
            txt_RegisterUser.Margin = new Padding(3, 2, 3, 2);
            txt_RegisterUser.Name = "txt_RegisterUser";
            txt_RegisterUser.Size = new Size(132, 49);
            txt_RegisterUser.TabIndex = 5;
            txt_RegisterUser.Text = "Register User";
            txt_RegisterUser.UseVisualStyleBackColor = true;
            // 
            // LoginView
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(738, 384);
            Controls.Add(txt_RegisterUser);
            Controls.Add(txt_Password);
            Controls.Add(txt_UserName);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btn_Login);
            Margin = new Padding(3, 2, 3, 2);
            Name = "LoginView";
            Text = "LoginView";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btn_Login;
        private Label label1;
        private Label label2;
        private TextBox txt_UserName;
        private TextBox txt_Password;
        private Button txt_RegisterUser;
    }
}