using System.Drawing;
using System.Windows.Forms;

namespace FeriaDelAgricultorUI
{
    partial class SeleccionPuntoFeriaView
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
            this.cbxProvincia = new ComboBox();
            this.label1 = new Label();
            this.label2 = new Label();
            this.cbxCanton = new ComboBox();
            this.label3 = new Label();
            this.cbxPuntoFeria = new ComboBox();
            this.btnContinuar = new Button();
            this.SuspendLayout();
            // 
            // cbxProvincia
            // 
            this.cbxProvincia.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbxProvincia.FormattingEnabled = true;
            this.cbxProvincia.Location = new Point(75, 135);
            this.cbxProvincia.Name = "cbxProvincia";
            this.cbxProvincia.Size = new Size(207, 23);
            this.cbxProvincia.TabIndex = 1;
            this.cbxProvincia.SelectedIndexChanged += this.cbxProvincia_SelectedIndexChanged;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new Point(75, 100);
            this.label1.Name = "label1";
            this.label1.Size = new Size(56, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Provincia";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new Point(462, 100);
            this.label2.Name = "label2";
            this.label2.Size = new Size(46, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Cantón";
            // 
            // cbxCanton
            // 
            this.cbxCanton.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbxCanton.FormattingEnabled = true;
            this.cbxCanton.Location = new Point(462, 135);
            this.cbxCanton.Name = "cbxCanton";
            this.cbxCanton.Size = new Size(207, 23);
            this.cbxCanton.TabIndex = 4;
            this.cbxCanton.SelectedIndexChanged += this.cbxCanton_SelectedIndexChanged;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new Point(336, 220);
            this.label3.Name = "label3";
            this.label3.Size = new Size(81, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "Punto de feria";
            // 
            // cbxPuntoFeria
            // 
            this.cbxPuntoFeria.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbxPuntoFeria.FormattingEnabled = true;
            this.cbxPuntoFeria.Location = new Point(196, 248);
            this.cbxPuntoFeria.Name = "cbxPuntoFeria";
            this.cbxPuntoFeria.Size = new Size(355, 23);
            this.cbxPuntoFeria.TabIndex = 6;
            // 👆 IMPORTANTE: ya NO tiene SelectedIndexChanged enganchado a btnContinuar_Click
            // 
            // btnContinuar
            // 
            this.btnContinuar.Location = new Point(75, 320);
            this.btnContinuar.Name = "btnContinuar";
            this.btnContinuar.Size = new Size(594, 35);
            this.btnContinuar.TabIndex = 7;
            this.btnContinuar.Text = "Continuar";
            this.btnContinuar.UseVisualStyleBackColor = true;
            this.btnContinuar.Click += this.btnContinuar_Click;
            // 
            // SeleccionPuntoFeriaView
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(800, 450);
            this.Controls.Add(this.btnContinuar);
            this.Controls.Add(this.cbxPuntoFeria);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbxCanton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbxProvincia);
            this.Name = "SeleccionPuntoFeriaView";
            this.Text = "SeleccionPuntoFeriaView";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private ComboBox cbxProvincia;
        private Label label1;
        private Label label2;
        private ComboBox cbxCanton;
        private Label label3;
        private ComboBox cbxPuntoFeria;
        private Button btnContinuar;
    }
}
