namespace ApagarEquipos
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.BtnApagarPantalla = new System.Windows.Forms.Button();
            this.btnApagarPC = new System.Windows.Forms.Button();
            this.tmObtenerTiempo = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // BtnApagarPantalla
            // 
            this.BtnApagarPantalla.Location = new System.Drawing.Point(110, 187);
            this.BtnApagarPantalla.Name = "BtnApagarPantalla";
            this.BtnApagarPantalla.Size = new System.Drawing.Size(105, 38);
            this.BtnApagarPantalla.TabIndex = 0;
            this.BtnApagarPantalla.Text = "Apagar Monitor";
            this.BtnApagarPantalla.UseVisualStyleBackColor = true;
            this.BtnApagarPantalla.Click += new System.EventHandler(this.BtnApagarPantalla_Click);
            // 
            // btnApagarPC
            // 
            this.btnApagarPC.Location = new System.Drawing.Point(290, 187);
            this.btnApagarPC.Name = "btnApagarPC";
            this.btnApagarPC.Size = new System.Drawing.Size(93, 38);
            this.btnApagarPC.TabIndex = 1;
            this.btnApagarPC.Text = "Apagar PC";
            this.btnApagarPC.UseVisualStyleBackColor = true;
            this.btnApagarPC.Click += new System.EventHandler(this.btnApagarPC_Click);
            // 
            // tmObtenerTiempo
            // 
            this.tmObtenerTiempo.Tick += new System.EventHandler(this.tmObtenerTiempo_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnApagarPC);
            this.Controls.Add(this.BtnApagarPantalla);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Apagar Equipo";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnApagarPantalla;
        private System.Windows.Forms.Button btnApagarPC;
        private System.Windows.Forms.Timer tmObtenerTiempo;
    }
}

