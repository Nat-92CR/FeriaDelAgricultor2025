using System.Drawing;
using System.Windows.Forms;

namespace FeriaDelAgricultorUI
{
    partial class ReportesEstadisticasView
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador

        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.cmbTipoReporte = new System.Windows.Forms.ComboBox();
            this.btnGenerar = new System.Windows.Forms.Button();
            this.lvwResultados = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Location = new System.Drawing.Point(12, 9);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(140, 15);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Reportes y estadísticas";
            // 
            // cmbTipoReporte
            // 
            this.cmbTipoReporte.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTipoReporte.FormattingEnabled = true;
            this.cmbTipoReporte.Location = new System.Drawing.Point(12, 35);
            this.cmbTipoReporte.Name = "cmbTipoReporte";
            this.cmbTipoReporte.Size = new System.Drawing.Size(250, 23);
            this.cmbTipoReporte.TabIndex = 1;
            // 
            // btnGenerar
            // 
            this.btnGenerar.Location = new System.Drawing.Point(280, 34);
            this.btnGenerar.Name = "btnGenerar";
            this.btnGenerar.Size = new System.Drawing.Size(120, 25);
            this.btnGenerar.TabIndex = 2;
            this.btnGenerar.Text = "Generar reporte";
            this.btnGenerar.UseVisualStyleBackColor = true;
            // 
            // lvwResultados
            // 
            this.lvwResultados.FullRowSelect = true;
            this.lvwResultados.HideSelection = false;
            this.lvwResultados.Location = new System.Drawing.Point(12, 70);
            this.lvwResultados.Name = "lvwResultados";
            this.lvwResultados.Size = new System.Drawing.Size(600, 280);
            this.lvwResultados.TabIndex = 3;
            this.lvwResultados.UseCompatibleStateImageBehavior = false;
            this.lvwResultados.View = System.Windows.Forms.View.Details;
            // 
            // ReportesEstadisticasView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 370);
            this.Controls.Add(this.lvwResultados);
            this.Controls.Add(this.btnGenerar);
            this.Controls.Add(this.cmbTipoReporte);
            this.Controls.Add(this.lblTitulo);
            this.Name = "ReportesEstadisticasView";
            this.Text = "Reportes y estadísticas";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private ComboBox cmbTipoReporte;
        private Button btnGenerar;
        private ListView lvwResultados;
    }
}
