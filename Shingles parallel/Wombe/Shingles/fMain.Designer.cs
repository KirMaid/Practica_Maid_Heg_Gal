namespace Shingles
{
    partial class fMain
    {
        /// <summary>
        /// Требуется переменная конструктора.
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
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.tbOne = new System.Windows.Forms.RichTextBox();
            this.tbTwo = new System.Windows.Forms.RichTextBox();
            this.btnLoadDocOne = new System.Windows.Forms.Button();
            this.btnLoadDocTwo = new System.Windows.Forms.Button();
            this.btnCalc = new System.Windows.Forms.Button();
            this.lResult = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.TimeLabel = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tbFiles = new System.Windows.Forms.RichTextBox();
            this.btnCompare = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tbFile = new System.Windows.Forms.RichTextBox();
            this.stub = new System.Windows.Forms.RichTextBox();
            this.btnLoadFile = new System.Windows.Forms.Button();
            this.tbFileCompare = new System.Windows.Forms.RichTextBox();
            this.btnSelectFolder = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.canonizationTimeLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.shinglingTimeLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.comparisonTimeLabel = new System.Windows.Forms.Label();
            this.hashSelectionMethodComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbOne
            // 
            this.tbOne.Location = new System.Drawing.Point(6, 3);
            this.tbOne.Name = "tbOne";
            this.tbOne.Size = new System.Drawing.Size(540, 541);
            this.tbOne.TabIndex = 0;
            this.tbOne.Text = "";
            // 
            // tbTwo
            // 
            this.tbTwo.Location = new System.Drawing.Point(706, 3);
            this.tbTwo.Name = "tbTwo";
            this.tbTwo.Size = new System.Drawing.Size(531, 541);
            this.tbTwo.TabIndex = 0;
            this.tbTwo.Text = "";
            // 
            // btnLoadDocOne
            // 
            this.btnLoadDocOne.Location = new System.Drawing.Point(553, 13);
            this.btnLoadDocOne.Name = "btnLoadDocOne";
            this.btnLoadDocOne.Size = new System.Drawing.Size(147, 50);
            this.btnLoadDocOne.TabIndex = 1;
            this.btnLoadDocOne.Text = "Загрузить первый док файл";
            this.btnLoadDocOne.UseVisualStyleBackColor = true;
            this.btnLoadDocOne.Click += new System.EventHandler(this.btnLoadDocOne_Click);
            // 
            // btnLoadDocTwo
            // 
            this.btnLoadDocTwo.Location = new System.Drawing.Point(552, 69);
            this.btnLoadDocTwo.Name = "btnLoadDocTwo";
            this.btnLoadDocTwo.Size = new System.Drawing.Size(147, 50);
            this.btnLoadDocTwo.TabIndex = 1;
            this.btnLoadDocTwo.Text = "Загрузить второй док файл";
            this.btnLoadDocTwo.UseVisualStyleBackColor = true;
            this.btnLoadDocTwo.Click += new System.EventHandler(this.btnLoadDocTwo_Click);
            // 
            // btnCalc
            // 
            this.btnCalc.Location = new System.Drawing.Point(7, 86);
            this.btnCalc.Name = "btnCalc";
            this.btnCalc.Size = new System.Drawing.Size(133, 50);
            this.btnCalc.TabIndex = 0;
            this.btnCalc.Text = "Расчитать степень похожести текстов";
            this.btnCalc.UseVisualStyleBackColor = true;
            this.btnCalc.Click += new System.EventHandler(this.btnCalc_Click);
            // 
            // lResult
            // 
            this.lResult.AutoSize = true;
            this.lResult.Location = new System.Drawing.Point(6, 42);
            this.lResult.Name = "lResult";
            this.lResult.Size = new System.Drawing.Size(0, 13);
            this.lResult.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lResult);
            this.groupBox1.Controls.Add(this.btnCalc);
            this.groupBox1.Location = new System.Drawing.Point(553, 126);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(146, 142);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Алгоритм Шинглов 3x";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1251, 576);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.hashSelectionMethodComboBox);
            this.tabPage1.Controls.Add(this.comparisonTimeLabel);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.shinglingTimeLabel);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.canonizationTimeLabel);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.TimeLabel);
            this.tabPage1.Controls.Add(this.tbOne);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.tbTwo);
            this.tabPage1.Controls.Add(this.btnLoadDocTwo);
            this.tabPage1.Controls.Add(this.btnLoadDocOne);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1243, 550);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Сравнение файлов";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // TimeLabel
            // 
            this.TimeLabel.AutoSize = true;
            this.TimeLabel.Location = new System.Drawing.Point(562, 513);
            this.TimeLabel.Name = "TimeLabel";
            this.TimeLabel.Size = new System.Drawing.Size(31, 13);
            this.TimeLabel.TabIndex = 3;
            this.TimeLabel.Text = "0000";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1243, 550);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Сравнение папок";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tbFiles);
            this.groupBox2.Controls.Add(this.btnCompare);
            this.groupBox2.Location = new System.Drawing.Point(7, 7);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1230, 537);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Сравнение папок";
            // 
            // tbFiles
            // 
            this.tbFiles.Location = new System.Drawing.Point(6, 19);
            this.tbFiles.Name = "tbFiles";
            this.tbFiles.Size = new System.Drawing.Size(471, 512);
            this.tbFiles.TabIndex = 1;
            this.tbFiles.Text = "";
            // 
            // btnCompare
            // 
            this.btnCompare.Location = new System.Drawing.Point(483, 19);
            this.btnCompare.Name = "btnCompare";
            this.btnCompare.Size = new System.Drawing.Size(114, 58);
            this.btnCompare.TabIndex = 0;
            this.btnCompare.Text = "Выбрать папку и расчитать сходства";
            this.btnCompare.UseVisualStyleBackColor = true;
            this.btnCompare.Click += new System.EventHandler(this.btnCompare_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBox3);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1243, 550);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Сравнение файла с папкой";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tbFile);
            this.groupBox3.Controls.Add(this.stub);
            this.groupBox3.Controls.Add(this.btnLoadFile);
            this.groupBox3.Controls.Add(this.tbFileCompare);
            this.groupBox3.Controls.Add(this.btnSelectFolder);
            this.groupBox3.Location = new System.Drawing.Point(6, 7);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1230, 537);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Сравнение";
            // 
            // tbFile
            // 
            this.tbFile.Location = new System.Drawing.Point(6, 19);
            this.tbFile.Name = "tbFile";
            this.tbFile.Size = new System.Drawing.Size(533, 512);
            this.tbFile.TabIndex = 3;
            this.tbFile.Text = "";
            // 
            // stub
            // 
            this.stub.Location = new System.Drawing.Point(29, 49);
            this.stub.Name = "stub";
            this.stub.Size = new System.Drawing.Size(114, 418);
            this.stub.TabIndex = 4;
            this.stub.Text = "";
            // 
            // btnLoadFile
            // 
            this.btnLoadFile.Location = new System.Drawing.Point(545, 20);
            this.btnLoadFile.Name = "btnLoadFile";
            this.btnLoadFile.Size = new System.Drawing.Size(114, 23);
            this.btnLoadFile.TabIndex = 2;
            this.btnLoadFile.Text = "Загрузить файл";
            this.btnLoadFile.UseVisualStyleBackColor = true;
            this.btnLoadFile.Click += new System.EventHandler(this.btnLoadFile_Click);
            // 
            // tbFileCompare
            // 
            this.tbFileCompare.Location = new System.Drawing.Point(665, 19);
            this.tbFileCompare.Name = "tbFileCompare";
            this.tbFileCompare.Size = new System.Drawing.Size(559, 512);
            this.tbFileCompare.TabIndex = 1;
            this.tbFileCompare.Text = "";
            // 
            // btnSelectFolder
            // 
            this.btnSelectFolder.Location = new System.Drawing.Point(545, 49);
            this.btnSelectFolder.Name = "btnSelectFolder";
            this.btnSelectFolder.Size = new System.Drawing.Size(114, 58);
            this.btnSelectFolder.TabIndex = 0;
            this.btnSelectFolder.Text = "Выбрать папку и расчитать сходства";
            this.btnSelectFolder.UseVisualStyleBackColor = true;
            this.btnSelectFolder.Click += new System.EventHandler(this.btnSelectFolder_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(559, 310);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Время канонизации";
            // 
            // canonizationTimeLabel
            // 
            this.canonizationTimeLabel.AutoSize = true;
            this.canonizationTimeLabel.Location = new System.Drawing.Point(562, 340);
            this.canonizationTimeLabel.Name = "canonizationTimeLabel";
            this.canonizationTimeLabel.Size = new System.Drawing.Size(31, 13);
            this.canonizationTimeLabel.TabIndex = 5;
            this.canonizationTimeLabel.Text = "0000";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(559, 374);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(122, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Время шинглирования";
            // 
            // shinglingTimeLabel
            // 
            this.shinglingTimeLabel.AutoSize = true;
            this.shinglingTimeLabel.Location = new System.Drawing.Point(562, 398);
            this.shinglingTimeLabel.Name = "shinglingTimeLabel";
            this.shinglingTimeLabel.Size = new System.Drawing.Size(31, 13);
            this.shinglingTimeLabel.TabIndex = 7;
            this.shinglingTimeLabel.Text = "0000";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(562, 497);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Общее время";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(562, 428);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Время сравнения";
            // 
            // comparisonTimeLabel
            // 
            this.comparisonTimeLabel.AutoSize = true;
            this.comparisonTimeLabel.Location = new System.Drawing.Point(565, 445);
            this.comparisonTimeLabel.Name = "comparisonTimeLabel";
            this.comparisonTimeLabel.Size = new System.Drawing.Size(31, 13);
            this.comparisonTimeLabel.TabIndex = 10;
            this.comparisonTimeLabel.Text = "0000";
            // 
            // hashSelectionMethodComboBox
            // 
            this.hashSelectionMethodComboBox.FormattingEnabled = true;
            this.hashSelectionMethodComboBox.Location = new System.Drawing.Point(560, 275);
            this.hashSelectionMethodComboBox.Name = "hashSelectionMethodComboBox";
            this.hashSelectionMethodComboBox.Size = new System.Drawing.Size(121, 21);
            this.hashSelectionMethodComboBox.TabIndex = 11;
            this.hashSelectionMethodComboBox.SelectedIndexChanged += new System.EventHandler(this.hashSelectionMethodComboBox_SelectedIndexChanged);
            // 
            // fMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1266, 591);
            this.Controls.Add(this.tabControl1);
            this.Name = "fMain";
            this.Text = "Shingles";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox tbOne;
        private System.Windows.Forms.RichTextBox tbTwo;
        private System.Windows.Forms.Button btnLoadDocOne;
        private System.Windows.Forms.Button btnLoadDocTwo;
        private System.Windows.Forms.Button btnCalc;
        private System.Windows.Forms.Label lResult;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RichTextBox tbFiles;
        private System.Windows.Forms.Button btnCompare;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RichTextBox tbFile;
        private System.Windows.Forms.Button btnLoadFile;
        private System.Windows.Forms.RichTextBox tbFileCompare;
        private System.Windows.Forms.Button btnSelectFolder;
        private System.Windows.Forms.RichTextBox stub;
        private System.Windows.Forms.Label TimeLabel;
        private System.Windows.Forms.Label shinglingTimeLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label canonizationTimeLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label comparisonTimeLabel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox hashSelectionMethodComboBox;
    }
}

