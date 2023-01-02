namespace CanCapter
{
    partial class Accueil
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Accueil));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.GsM = new System.Windows.Forms.Button();
            this.GsF = new System.Windows.Forms.Button();
            this.GsT = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(210, 122);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(228, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(696, 55);
            this.label1.TabIndex = 1;
            this.label1.Text = "Gestion De Centre CanCapter";
            // 
            // GsM
            // 
            this.GsM.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GsM.Location = new System.Drawing.Point(3, 3);
            this.GsM.Name = "GsM";
            this.GsM.Size = new System.Drawing.Size(268, 66);
            this.GsM.TabIndex = 32;
            this.GsM.Text = "Gestion Matiers";
            this.GsM.UseVisualStyleBackColor = true;
            this.GsM.Click += new System.EventHandler(this.GsM_Click);
            // 
            // GsF
            // 
            this.GsF.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GsF.Location = new System.Drawing.Point(3, 75);
            this.GsF.Name = "GsF";
            this.GsF.Size = new System.Drawing.Size(268, 66);
            this.GsF.TabIndex = 33;
            this.GsF.Text = "Gestion Filier";
            this.GsF.UseVisualStyleBackColor = true;
            this.GsF.Click += new System.EventHandler(this.GsF_Click);
            // 
            // GsT
            // 
            this.GsT.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GsT.Location = new System.Drawing.Point(3, 147);
            this.GsT.Name = "GsT";
            this.GsT.Size = new System.Drawing.Size(268, 66);
            this.GsT.TabIndex = 34;
            this.GsT.Text = "Gestion Tarifs";
            this.GsT.UseVisualStyleBackColor = true;
            this.GsT.Click += new System.EventHandler(this.GsT_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.GsT);
            this.panel1.Controls.Add(this.GsM);
            this.panel1.Controls.Add(this.GsF);
            this.panel1.Location = new System.Drawing.Point(12, 140);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(274, 223);
            this.panel1.TabIndex = 35;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(339, 143);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(268, 66);
            this.button1.TabIndex = 35;
            this.button1.Text = "Gestion Etudient";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Accueil
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(924, 484);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Accueil";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Accueil_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button GsM;
        private System.Windows.Forms.Button GsF;
        private System.Windows.Forms.Button GsT;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
    }
}

