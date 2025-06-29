namespace RNGenerator
{
    partial class Controller
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
            metrixName = new TextBox();
            groupBox1 = new GroupBox();
            newMetrix = new Button();
            metrixLoad = new Button();
            metrixSave = new Button();
            label1 = new Label();
            delBtn = new Button();
            groupBox2 = new GroupBox();
            groupBox4 = new GroupBox();
            chkOuter = new CheckBox();
            chkInner = new CheckBox();
            groupBox3 = new GroupBox();
            chkA = new CheckBox();
            chkB = new CheckBox();
            chkC = new CheckBox();
            chkO = new CheckBox();
            chkD = new CheckBox();
            chkN = new CheckBox();
            chkE = new CheckBox();
            chkM = new CheckBox();
            chkF = new CheckBox();
            chkL = new CheckBox();
            chkG = new CheckBox();
            chkK = new CheckBox();
            chkH = new CheckBox();
            chkJ = new CheckBox();
            chkI = new CheckBox();
            allSelect = new Button();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox4.SuspendLayout();
            groupBox3.SuspendLayout();
            SuspendLayout();
            // 
            // metrixName
            // 
            metrixName.Location = new Point(55, 35);
            metrixName.Name = "metrixName";
            metrixName.Size = new Size(401, 23);
            metrixName.TabIndex = 0;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(newMetrix);
            groupBox1.Controls.Add(metrixLoad);
            groupBox1.Controls.Add(metrixSave);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(metrixName);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(472, 117);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "격판관리";
            // 
            // newMetrix
            // 
            newMetrix.Location = new Point(55, 73);
            newMetrix.Name = "newMetrix";
            newMetrix.Size = new Size(100, 23);
            newMetrix.TabIndex = 4;
            newMetrix.Text = "새로 만들기";
            newMetrix.UseVisualStyleBackColor = true;
            newMetrix.Click += newMetrix_Click;
            // 
            // metrixLoad
            // 
            metrixLoad.Location = new Point(205, 73);
            metrixLoad.Name = "metrixLoad";
            metrixLoad.Size = new Size(100, 23);
            metrixLoad.TabIndex = 3;
            metrixLoad.Text = "불러오기";
            metrixLoad.UseVisualStyleBackColor = true;
            metrixLoad.Click += metrixLoad_Click;
            // 
            // metrixSave
            // 
            metrixSave.Location = new Point(356, 73);
            metrixSave.Name = "metrixSave";
            metrixSave.Size = new Size(100, 23);
            metrixSave.TabIndex = 2;
            metrixSave.Text = "저장";
            metrixSave.UseVisualStyleBackColor = true;
            metrixSave.Click += metrixSave_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 38);
            label1.Name = "label1";
            label1.Size = new Size(43, 15);
            label1.TabIndex = 1;
            label1.Text = "체계명";
            // 
            // delBtn
            // 
            delBtn.Location = new Point(332, 80);
            delBtn.Name = "delBtn";
            delBtn.Size = new Size(116, 23);
            delBtn.TabIndex = 2;
            delBtn.Text = "삭제";
            delBtn.UseVisualStyleBackColor = true;
            delBtn.Click += delBtn_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(groupBox4);
            groupBox2.Controls.Add(groupBox3);
            groupBox2.Controls.Add(delBtn);
            groupBox2.Controls.Add(allSelect);
            groupBox2.Location = new Point(12, 150);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(472, 260);
            groupBox2.TabIndex = 3;
            groupBox2.TabStop = false;
            groupBox2.Text = "삭제옵션";
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(chkOuter);
            groupBox4.Controls.Add(chkInner);
            groupBox4.Location = new Point(23, 159);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(255, 79);
            groupBox4.TabIndex = 22;
            groupBox4.TabStop = false;
            // 
            // chkOuter
            // 
            chkOuter.AutoSize = true;
            chkOuter.Location = new Point(14, 22);
            chkOuter.Name = "chkOuter";
            chkOuter.Size = new Size(50, 19);
            chkOuter.TabIndex = 19;
            chkOuter.Text = "변치";
            chkOuter.UseVisualStyleBackColor = true;
            // 
            // chkInner
            // 
            chkInner.AutoSize = true;
            chkInner.Location = new Point(14, 47);
            chkInner.Name = "chkInner";
            chkInner.Size = new Size(62, 19);
            chkInner.TabIndex = 20;
            chkInner.Text = "내부치";
            chkInner.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(chkA);
            groupBox3.Controls.Add(chkB);
            groupBox3.Controls.Add(chkC);
            groupBox3.Controls.Add(chkO);
            groupBox3.Controls.Add(chkD);
            groupBox3.Controls.Add(chkN);
            groupBox3.Controls.Add(chkE);
            groupBox3.Controls.Add(chkM);
            groupBox3.Controls.Add(chkF);
            groupBox3.Controls.Add(chkL);
            groupBox3.Controls.Add(chkG);
            groupBox3.Controls.Add(chkK);
            groupBox3.Controls.Add(chkH);
            groupBox3.Controls.Add(chkJ);
            groupBox3.Controls.Add(chkI);
            groupBox3.Location = new Point(23, 22);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(255, 136);
            groupBox3.TabIndex = 21;
            groupBox3.TabStop = false;
            // 
            // chkA
            // 
            chkA.AutoSize = true;
            chkA.Location = new Point(14, 22);
            chkA.Name = "chkA";
            chkA.Size = new Size(34, 19);
            chkA.TabIndex = 3;
            chkA.Text = "A";
            chkA.UseVisualStyleBackColor = true;
            // 
            // chkB
            // 
            chkB.AutoSize = true;
            chkB.Location = new Point(64, 22);
            chkB.Name = "chkB";
            chkB.Size = new Size(33, 19);
            chkB.TabIndex = 4;
            chkB.Text = "B";
            chkB.UseVisualStyleBackColor = true;
            // 
            // chkC
            // 
            chkC.AutoSize = true;
            chkC.Location = new Point(113, 22);
            chkC.Name = "chkC";
            chkC.Size = new Size(34, 19);
            chkC.TabIndex = 5;
            chkC.Text = "C";
            chkC.UseVisualStyleBackColor = true;
            // 
            // chkO
            // 
            chkO.AutoSize = true;
            chkO.Location = new Point(214, 102);
            chkO.Name = "chkO";
            chkO.Size = new Size(35, 19);
            chkO.TabIndex = 17;
            chkO.Text = "O";
            chkO.UseVisualStyleBackColor = true;
            // 
            // chkD
            // 
            chkD.AutoSize = true;
            chkD.Location = new Point(163, 22);
            chkD.Name = "chkD";
            chkD.Size = new Size(35, 19);
            chkD.TabIndex = 6;
            chkD.Text = "D";
            chkD.UseVisualStyleBackColor = true;
            // 
            // chkN
            // 
            chkN.AutoSize = true;
            chkN.Location = new Point(163, 102);
            chkN.Name = "chkN";
            chkN.Size = new Size(35, 19);
            chkN.TabIndex = 16;
            chkN.Text = "N";
            chkN.UseVisualStyleBackColor = true;
            // 
            // chkE
            // 
            chkE.AutoSize = true;
            chkE.Location = new Point(214, 22);
            chkE.Name = "chkE";
            chkE.Size = new Size(32, 19);
            chkE.TabIndex = 7;
            chkE.Text = "E";
            chkE.UseVisualStyleBackColor = true;
            // 
            // chkM
            // 
            chkM.AutoSize = true;
            chkM.Location = new Point(113, 102);
            chkM.Name = "chkM";
            chkM.Size = new Size(37, 19);
            chkM.TabIndex = 15;
            chkM.Text = "M";
            chkM.UseVisualStyleBackColor = true;
            // 
            // chkF
            // 
            chkF.AutoSize = true;
            chkF.Location = new Point(14, 62);
            chkF.Name = "chkF";
            chkF.Size = new Size(32, 19);
            chkF.TabIndex = 8;
            chkF.Text = "F";
            chkF.UseVisualStyleBackColor = true;
            // 
            // chkL
            // 
            chkL.AutoSize = true;
            chkL.Location = new Point(62, 102);
            chkL.Name = "chkL";
            chkL.Size = new Size(32, 19);
            chkL.TabIndex = 14;
            chkL.Text = "L";
            chkL.UseVisualStyleBackColor = true;
            // 
            // chkG
            // 
            chkG.AutoSize = true;
            chkG.Location = new Point(64, 62);
            chkG.Name = "chkG";
            chkG.Size = new Size(34, 19);
            chkG.TabIndex = 9;
            chkG.Text = "G";
            chkG.UseVisualStyleBackColor = true;
            // 
            // chkK
            // 
            chkK.AutoSize = true;
            chkK.Location = new Point(13, 102);
            chkK.Name = "chkK";
            chkK.Size = new Size(33, 19);
            chkK.TabIndex = 13;
            chkK.Text = "K";
            chkK.UseVisualStyleBackColor = true;
            // 
            // chkH
            // 
            chkH.AutoSize = true;
            chkH.Location = new Point(113, 62);
            chkH.Name = "chkH";
            chkH.Size = new Size(35, 19);
            chkH.TabIndex = 10;
            chkH.Text = "H";
            chkH.UseVisualStyleBackColor = true;
            // 
            // chkJ
            // 
            chkJ.AutoSize = true;
            chkJ.Location = new Point(214, 62);
            chkJ.Name = "chkJ";
            chkJ.Size = new Size(30, 19);
            chkJ.TabIndex = 12;
            chkJ.Text = "J";
            chkJ.UseVisualStyleBackColor = true;
            // 
            // chkI
            // 
            chkI.AutoSize = true;
            chkI.Location = new Point(163, 62);
            chkI.Name = "chkI";
            chkI.Size = new Size(29, 19);
            chkI.TabIndex = 11;
            chkI.Text = "I";
            chkI.UseVisualStyleBackColor = true;
            // 
            // allSelect
            // 
            allSelect.Location = new Point(332, 40);
            allSelect.Name = "allSelect";
            allSelect.Size = new Size(116, 23);
            allSelect.TabIndex = 18;
            allSelect.Text = "전체 선택";
            allSelect.UseVisualStyleBackColor = true;
            allSelect.Click += allSelect_Click;
            // 
            // Controller
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(495, 419);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Name = "Controller";
            Text = "Controller";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        public TextBox metrixName;
        private GroupBox groupBox1;
        private Button newMetrix;
        private Button metrixLoad;
        private Button metrixSave;
        private Label label1;
        private Button delBtn;
        private GroupBox groupBox2;
        private CheckBox chkO;
        private CheckBox chkN;
        private CheckBox chkM;
        private CheckBox chkL;
        private CheckBox chkK;
        private CheckBox chkJ;
        private CheckBox chkI;
        private CheckBox chkH;
        private CheckBox chkG;
        private CheckBox chkF;
        private CheckBox chkE;
        private CheckBox chkD;
        private CheckBox chkC;
        private CheckBox chkB;
        private CheckBox chkA;
        private CheckBox chkInner;
        private CheckBox chkOuter;
        private Button allSelect;
        private GroupBox groupBox4;
        private GroupBox groupBox3;
    }
}