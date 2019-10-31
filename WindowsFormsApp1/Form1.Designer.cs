namespace WindowsFormsApp1
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
            this.components = new System.ComponentModel.Container();
            this.zedGraphControl1 = new ZedGraph.ZedGraphControl();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Range = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dimy = new System.Windows.Forms.NumericUpDown();
            this.dim3 = new System.Windows.Forms.NumericUpDown();
            this.dim2 = new System.Windows.Forms.NumericUpDown();
            this.dim1 = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.PolinoType = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.Rankx3 = new System.Windows.Forms.NumericUpDown();
            this.Rankx2 = new System.Windows.Forms.NumericUpDown();
            this.Rankx1 = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.Result = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.label12 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Range)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dimy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dim3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dim2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dim1)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Rankx3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Rankx2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Rankx1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // zedGraphControl1
            // 
            this.zedGraphControl1.Location = new System.Drawing.Point(644, -1);
            this.zedGraphControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.zedGraphControl1.Name = "zedGraphControl1";
            this.zedGraphControl1.ScrollGrace = 0D;
            this.zedGraphControl1.ScrollMaxX = 0D;
            this.zedGraphControl1.ScrollMaxY = 0D;
            this.zedGraphControl1.ScrollMaxY2 = 0D;
            this.zedGraphControl1.ScrollMinX = 0D;
            this.zedGraphControl1.ScrollMinY = 0D;
            this.zedGraphControl1.ScrollMinY2 = 0D;
            this.zedGraphControl1.Size = new System.Drawing.Size(630, 328);
            this.zedGraphControl1.TabIndex = 0;
            this.zedGraphControl1.UseExtendedPrintDialog = true;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 286);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(303, 41);
            this.progressBar1.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.Range);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 22);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(218, 108);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Вхідні дані";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Файл вхідних даних";
            // 
            // Range
            // 
            this.Range.AutoSize = true;
            this.Range.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Range.Location = new System.Drawing.Point(95, 15);
            this.Range.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.Range.MaximumSize = new System.Drawing.Size(50, 0);
            this.Range.Name = "Range";
            this.Range.Size = new System.Drawing.Size(41, 20);
            this.Range.TabIndex = 1;
            this.Range.Value = new decimal(new int[] {
            45,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Розмір вибірки";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dimy);
            this.groupBox2.Controls.Add(this.dim3);
            this.groupBox2.Controls.Add(this.dim2);
            this.groupBox2.Controls.Add(this.dim1);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(12, 136);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(218, 117);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Вектори";
            // 
            // dimy
            // 
            this.dimy.AutoSize = true;
            this.dimy.ForeColor = System.Drawing.SystemColors.WindowText;
            this.dimy.Location = new System.Drawing.Point(150, 26);
            this.dimy.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.dimy.MaximumSize = new System.Drawing.Size(50, 0);
            this.dimy.Name = "dimy";
            this.dimy.Size = new System.Drawing.Size(41, 20);
            this.dimy.TabIndex = 7;
            this.dimy.Value = new decimal(new int[] {
            45,
            0,
            0,
            0});
            // 
            // dim3
            // 
            this.dim3.AutoSize = true;
            this.dim3.ForeColor = System.Drawing.SystemColors.WindowText;
            this.dim3.Location = new System.Drawing.Point(48, 71);
            this.dim3.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.dim3.MaximumSize = new System.Drawing.Size(50, 0);
            this.dim3.Name = "dim3";
            this.dim3.Size = new System.Drawing.Size(41, 20);
            this.dim3.TabIndex = 6;
            this.dim3.Value = new decimal(new int[] {
            45,
            0,
            0,
            0});
            // 
            // dim2
            // 
            this.dim2.AutoSize = true;
            this.dim2.ForeColor = System.Drawing.SystemColors.WindowText;
            this.dim2.Location = new System.Drawing.Point(48, 49);
            this.dim2.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.dim2.MaximumSize = new System.Drawing.Size(50, 0);
            this.dim2.Name = "dim2";
            this.dim2.Size = new System.Drawing.Size(41, 20);
            this.dim2.TabIndex = 5;
            this.dim2.Value = new decimal(new int[] {
            45,
            0,
            0,
            0});
            // 
            // dim1
            // 
            this.dim1.AutoSize = true;
            this.dim1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.dim1.Location = new System.Drawing.Point(48, 26);
            this.dim1.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.dim1.MaximumSize = new System.Drawing.Size(50, 0);
            this.dim1.Name = "dim1";
            this.dim1.Size = new System.Drawing.Size(41, 20);
            this.dim1.TabIndex = 4;
            this.dim1.Value = new decimal(new int[] {
            45,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(113, 28);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(31, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "dim y";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 73);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "dim x3";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 51);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "dim x2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "dim x1";
            // 
            // PolinoType
            // 
            this.PolinoType.FormattingEnabled = true;
            this.PolinoType.Items.AddRange(new object[] {
            "Чебишева",
            "Лежандра",
            "Лагера",
            "Ерміта"});
            this.PolinoType.Location = new System.Drawing.Point(61, 19);
            this.PolinoType.Name = "PolinoType";
            this.PolinoType.Size = new System.Drawing.Size(120, 21);
            this.PolinoType.TabIndex = 4;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(26, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "Вид";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.Rankx3);
            this.groupBox3.Controls.Add(this.Rankx2);
            this.groupBox3.Controls.Add(this.Rankx1);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.PolinoType);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Location = new System.Drawing.Point(237, 22);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 109);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Поліноми";
            // 
            // Rankx3
            // 
            this.Rankx3.Location = new System.Drawing.Point(61, 88);
            this.Rankx3.Name = "Rankx3";
            this.Rankx3.Size = new System.Drawing.Size(120, 20);
            this.Rankx3.TabIndex = 11;
            this.Rankx3.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // Rankx2
            // 
            this.Rankx2.Location = new System.Drawing.Point(61, 71);
            this.Rankx2.Name = "Rankx2";
            this.Rankx2.Size = new System.Drawing.Size(120, 20);
            this.Rankx2.TabIndex = 10;
            this.Rankx2.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // Rankx1
            // 
            this.Rankx1.Location = new System.Drawing.Point(61, 52);
            this.Rankx1.Name = "Rankx1";
            this.Rankx1.Size = new System.Drawing.Size(120, 20);
            this.Rankx1.TabIndex = 9;
            this.Rankx1.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 90);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(49, 13);
            this.label10.TabIndex = 8;
            this.label10.Text = "Rank P3";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 71);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(49, 13);
            this.label9.TabIndex = 7;
            this.label9.Text = "Rank P2";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 54);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(49, 13);
            this.label8.TabIndex = 6;
            this.label8.Text = "Rank P1";
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(12, 335);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(487, 336);
            this.panel1.TabIndex = 7;
            // 
            // textBox1
            // 
            this.textBox1.AllowDrop = true;
            this.textBox1.Location = new System.Drawing.Point(321, 229);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(316, 100);
            this.textBox1.TabIndex = 8;
            // 
            // Result
            // 
            this.Result.AllowDrop = true;
            this.Result.Location = new System.Drawing.Point(505, 335);
            this.Result.Multiline = true;
            this.Result.Name = "Result";
            this.Result.Size = new System.Drawing.Size(769, 336);
            this.Result.TabIndex = 9;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(243, 148);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(136, 13);
            this.label11.TabIndex = 10;
            this.label11.Text = "Зобразити на графіку У_і";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(383, 146);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(48, 20);
            this.numericUpDown1.TabIndex = 12;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.FileName = "openFileDialog2";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(246, 186);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(205, 13);
            this.label12.TabIndex = 13;
            this.label12.Text = "Шукати лямбду з трьох систем рівнянь";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(457, 186);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(80, 17);
            this.checkBox1.TabIndex = 14;
            this.checkBox1.Text = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1275, 675);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.Result);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.zedGraphControl1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Range)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dimy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dim3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dim2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dim1)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Rankx3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Rankx2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Rankx1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ZedGraph.ZedGraphControl zedGraphControl1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown Range;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown dimy;
        private System.Windows.Forms.NumericUpDown dim3;
        private System.Windows.Forms.NumericUpDown dim2;
        private System.Windows.Forms.NumericUpDown dim1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox PolinoType;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.NumericUpDown Rankx3;
        private System.Windows.Forms.NumericUpDown Rankx2;
        private System.Windows.Forms.NumericUpDown Rankx1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox Result;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}

