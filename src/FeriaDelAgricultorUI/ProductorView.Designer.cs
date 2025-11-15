namespace FeriaDelAgricultorUI
{
    partial class ProductorView
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
            lblBienvenidaProductor = new Label();
            SuspendLayout();
            // 
            // lblBienvenidaProductor
            // 
            lblBienvenidaProductor.AutoSize = true;
            lblBienvenidaProductor.BackColor = Color.OrangeRed;
            lblBienvenidaProductor.Font = new Font("Segoe UI", 20F);
            lblBienvenidaProductor.Location = new Point(246, 9);
            lblBienvenidaProductor.Name = "lblBienvenidaProductor";
            lblBienvenidaProductor.Size = new Size(274, 37);
            lblBienvenidaProductor.TabIndex = 0;
            lblBienvenidaProductor.Text = "Bienvenido productor";
            // 
            // ProductorView
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(lblBienvenidaProductor);
            Name = "ProductorView";
            Text = "ProductorView";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblBienvenidaProductor;
    }
}