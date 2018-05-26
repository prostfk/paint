namespace MultiplayerPaint
{
    partial class frmPaint
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.настройкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.выбратьЦветToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.выбратьРазмерКистиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.выйтиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.открытьНовоеОкноToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.настройкиToolStripMenuItem,
            this.открытьНовоеОкноToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(799, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // настройкиToolStripMenuItem
            // 
            this.настройкиToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.выбратьЦветToolStripMenuItem,
            this.выбратьРазмерКистиToolStripMenuItem,
            this.выйтиToolStripMenuItem});
            this.настройкиToolStripMenuItem.Name = "настройкиToolStripMenuItem";
            this.настройкиToolStripMenuItem.Size = new System.Drawing.Size(79, 20);
            this.настройкиToolStripMenuItem.Text = "Настройки";
            // 
            // выбратьЦветToolStripMenuItem
            // 
            this.выбратьЦветToolStripMenuItem.Name = "выбратьЦветToolStripMenuItem";
            this.выбратьЦветToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.выбратьЦветToolStripMenuItem.Text = "Выбрать цвет ";
            this.выбратьЦветToolStripMenuItem.Click += new System.EventHandler(this.выбратьЦветToolStripMenuItem_Click);
            // 
            // выбратьРазмерКистиToolStripMenuItem
            // 
            this.выбратьРазмерКистиToolStripMenuItem.Name = "выбратьРазмерКистиToolStripMenuItem";
            this.выбратьРазмерКистиToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.выбратьРазмерКистиToolStripMenuItem.Text = "Выбрать размер кисти";
            this.выбратьРазмерКистиToolStripMenuItem.Click += new System.EventHandler(this.выбратьРазмерКистиToolStripMenuItem_Click);
            // 
            // выйтиToolStripMenuItem
            // 
            this.выйтиToolStripMenuItem.Name = "выйтиToolStripMenuItem";
            this.выйтиToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.выйтиToolStripMenuItem.Text = "Выйти";
            this.выйтиToolStripMenuItem.Click += new System.EventHandler(this.выйтиToolStripMenuItem_Click);
            // 
            // открытьНовоеОкноToolStripMenuItem
            // 
            this.открытьНовоеОкноToolStripMenuItem.Name = "открытьНовоеОкноToolStripMenuItem";
            this.открытьНовоеОкноToolStripMenuItem.Size = new System.Drawing.Size(135, 20);
            this.открытьНовоеОкноToolStripMenuItem.Text = "Открыть новое окно ";
            this.открытьНовоеОкноToolStripMenuItem.Click += new System.EventHandler(this.открытьНовоеОкноToolStripMenuItem_Click);
            // 
            // frmPaint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(799, 467);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmPaint";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Графический редактор";
            this.TopMost = true;
            this.Click += new System.EventHandler(this.frmPaint_Click);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmPaint_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmPaint_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frmPaint_KeyPress);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem настройкиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem выбратьЦветToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem открытьНовоеОкноToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem выбратьРазмерКистиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem выйтиToolStripMenuItem;
    }
}

