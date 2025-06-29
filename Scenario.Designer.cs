namespace RNGenerator
{
    partial class Scenario
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
            scenarioName = new TextBox();
            scenarioInput = new TextBox();
            label1 = new Label();
            scenarioSave = new Button();
            scenarioLoad = new Button();
            groupBox1 = new GroupBox();
            autoFullSave = new Button();
            autoFullChange = new Button();
            autoValueBtn = new Button();
            label4 = new Label();
            maxLengthDisplay = new Label();
            label2 = new Label();
            scenarioChange = new Button();
            lengthChange = new Label();
            partsInput = new TextBox();
            groupBox2 = new GroupBox();
            RNAllSaveBtn1 = new Button();
            partsSave = new Button();
            partsNumSW = new Button();
            partsChange = new Button();
            partsInputSW = new Button();
            groupBox7 = new GroupBox();
            letterSave = new Button();
            letterNumSW = new Button();
            RNAllSaveBtn2 = new Button();
            letterInputSW = new Button();
            letterChange = new Button();
            letterInput = new TextBox();
            groupBox3 = new GroupBox();
            wordSave = new Button();
            wordNumSW = new Button();
            RNAllSaveBtn3 = new Button();
            wordInputSW = new Button();
            wordChange = new Button();
            wordInput = new TextBox();
            groupBox5 = new GroupBox();
            RNAllSaveBtn4 = new Button();
            selfSave = new Button();
            newLineBtn = new Button();
            selfNumSW = new Button();
            selfInputSW = new Button();
            selfChange = new Button();
            selfInput = new TextBox();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox7.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox5.SuspendLayout();
            SuspendLayout();
            // 
            // scenarioName
            // 
            scenarioName.Location = new Point(6, 54);
            scenarioName.Name = "scenarioName";
            scenarioName.Size = new Size(1018, 23);
            scenarioName.TabIndex = 0;
            // 
            // scenarioInput
            // 
            scenarioInput.Font = new Font("맑은 고딕", 12F);
            scenarioInput.Location = new Point(6, 106);
            scenarioInput.MaxLength = 300;
            scenarioInput.Multiline = true;
            scenarioInput.Name = "scenarioInput";
            scenarioInput.ScrollBars = ScrollBars.Vertical;
            scenarioInput.Size = new Size(1018, 208);
            scenarioInput.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 36);
            label1.Name = "label1";
            label1.Size = new Size(67, 15);
            label1.TabIndex = 7;
            label1.Text = "시나리오명";
            // 
            // scenarioSave
            // 
            scenarioSave.Location = new Point(1030, 290);
            scenarioSave.Name = "scenarioSave";
            scenarioSave.Size = new Size(127, 23);
            scenarioSave.TabIndex = 8;
            scenarioSave.Text = "시나리오 저장";
            scenarioSave.UseVisualStyleBackColor = true;
            scenarioSave.Click += scenarioSave_Click;
            // 
            // scenarioLoad
            // 
            scenarioLoad.Location = new Point(1030, 54);
            scenarioLoad.Name = "scenarioLoad";
            scenarioLoad.Size = new Size(127, 23);
            scenarioLoad.TabIndex = 9;
            scenarioLoad.Text = "불러오기";
            scenarioLoad.UseVisualStyleBackColor = true;
            scenarioLoad.Click += scenarioLoad_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(autoFullSave);
            groupBox1.Controls.Add(autoFullChange);
            groupBox1.Controls.Add(autoValueBtn);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(maxLengthDisplay);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(scenarioChange);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(scenarioLoad);
            groupBox1.Controls.Add(scenarioName);
            groupBox1.Controls.Add(scenarioSave);
            groupBox1.Controls.Add(scenarioInput);
            groupBox1.Controls.Add(lengthChange);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1163, 335);
            groupBox1.TabIndex = 10;
            groupBox1.TabStop = false;
            groupBox1.Text = "시나리오";
            // 
            // autoFullSave
            // 
            autoFullSave.Location = new Point(1030, 226);
            autoFullSave.Name = "autoFullSave";
            autoFullSave.Size = new Size(127, 23);
            autoFullSave.TabIndex = 28;
            autoFullSave.Text = "전체 저장";
            autoFullSave.UseVisualStyleBackColor = true;
            autoFullSave.Click += autoFullSave_Click;
            // 
            // autoFullChange
            // 
            autoFullChange.Location = new Point(1030, 197);
            autoFullChange.Name = "autoFullChange";
            autoFullChange.Size = new Size(127, 23);
            autoFullChange.TabIndex = 27;
            autoFullChange.Text = "전체 난수 변환";
            autoFullChange.UseVisualStyleBackColor = true;
            autoFullChange.Click += autoFullChange_Click;
            // 
            // autoValueBtn
            // 
            autoValueBtn.Location = new Point(1030, 168);
            autoValueBtn.Name = "autoValueBtn";
            autoValueBtn.Size = new Size(127, 23);
            autoValueBtn.TabIndex = 26;
            autoValueBtn.Text = "전체 내부치 할당";
            autoValueBtn.UseVisualStyleBackColor = true;
            autoValueBtn.Click += autoValueBtn_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("맑은 고딕", 9F, FontStyle.Regular, GraphicsUnit.Point, 129);
            label4.Location = new Point(116, 317);
            label4.Name = "label4";
            label4.Size = new Size(201, 15);
            label4.TabIndex = 24;
            label4.Text = "※ 최대 300자까지 작성 가능합니다.";
            // 
            // maxLengthDisplay
            // 
            maxLengthDisplay.AutoSize = true;
            maxLengthDisplay.Location = new Point(30, 317);
            maxLengthDisplay.Name = "maxLengthDisplay";
            maxLengthDisplay.Size = new Size(53, 15);
            maxLengthDisplay.TabIndex = 23;
            maxLengthDisplay.Text = "/ 300 자";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 88);
            label2.Name = "label2";
            label2.Size = new Size(83, 15);
            label2.TabIndex = 22;
            label2.Text = "시나리오 내용";
            // 
            // scenarioChange
            // 
            scenarioChange.Location = new Point(1030, 105);
            scenarioChange.Name = "scenarioChange";
            scenarioChange.Size = new Size(127, 23);
            scenarioChange.TabIndex = 10;
            scenarioChange.Text = "변환";
            scenarioChange.UseVisualStyleBackColor = true;
            scenarioChange.Click += scenarioChange_Click;
            // 
            // lengthChange
            // 
            lengthChange.AutoSize = true;
            lengthChange.Location = new Point(6, 317);
            lengthChange.Name = "lengthChange";
            lengthChange.Size = new Size(14, 15);
            lengthChange.TabIndex = 25;
            lengthChange.Text = "0";
            // 
            // partsInput
            // 
            partsInput.Font = new Font("맑은 고딕", 12F);
            partsInput.Location = new Point(7, 22);
            partsInput.Multiline = true;
            partsInput.Name = "partsInput";
            partsInput.ScrollBars = ScrollBars.Vertical;
            partsInput.Size = new Size(1018, 292);
            partsInput.TabIndex = 10;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(RNAllSaveBtn1);
            groupBox2.Controls.Add(partsSave);
            groupBox2.Controls.Add(partsNumSW);
            groupBox2.Controls.Add(partsInput);
            groupBox2.Controls.Add(partsChange);
            groupBox2.Controls.Add(partsInputSW);
            groupBox2.Location = new Point(11, 378);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(1164, 325);
            groupBox2.TabIndex = 11;
            groupBox2.TabStop = false;
            groupBox2.Text = "자모 변환";
            // 
            // RNAllSaveBtn1
            // 
            RNAllSaveBtn1.Location = new Point(1031, 247);
            RNAllSaveBtn1.Name = "RNAllSaveBtn1";
            RNAllSaveBtn1.Size = new Size(127, 23);
            RNAllSaveBtn1.TabIndex = 23;
            RNAllSaveBtn1.Text = "난수 전체 저장";
            RNAllSaveBtn1.UseVisualStyleBackColor = true;
            RNAllSaveBtn1.Click += RNAllSaveBtn1_Click;
            // 
            // partsSave
            // 
            partsSave.Location = new Point(1031, 291);
            partsSave.Name = "partsSave";
            partsSave.Size = new Size(127, 23);
            partsSave.TabIndex = 22;
            partsSave.Text = "난수저장";
            partsSave.UseVisualStyleBackColor = true;
            partsSave.Click += partsSave_Click;
            // 
            // partsNumSW
            // 
            partsNumSW.Location = new Point(1031, 90);
            partsNumSW.Name = "partsNumSW";
            partsNumSW.Size = new Size(127, 23);
            partsNumSW.TabIndex = 13;
            partsNumSW.Text = "난수 On/Off";
            partsNumSW.UseVisualStyleBackColor = true;
            partsNumSW.Click += partsNumSW_Click;
            // 
            // partsChange
            // 
            partsChange.Location = new Point(1031, 22);
            partsChange.Name = "partsChange";
            partsChange.Size = new Size(127, 23);
            partsChange.TabIndex = 11;
            partsChange.Text = "난수변환";
            partsChange.UseVisualStyleBackColor = true;
            partsChange.Click += partsChange_Click;
            // 
            // partsInputSW
            // 
            partsInputSW.Location = new Point(1031, 56);
            partsInputSW.Name = "partsInputSW";
            partsInputSW.Size = new Size(127, 23);
            partsInputSW.TabIndex = 12;
            partsInputSW.Text = "시나리오 On/Off";
            partsInputSW.UseVisualStyleBackColor = true;
            partsInputSW.Click += partsInputSW_Click;
            // 
            // groupBox7
            // 
            groupBox7.Controls.Add(letterSave);
            groupBox7.Controls.Add(letterNumSW);
            groupBox7.Controls.Add(RNAllSaveBtn2);
            groupBox7.Controls.Add(letterInputSW);
            groupBox7.Controls.Add(letterChange);
            groupBox7.Controls.Add(letterInput);
            groupBox7.Location = new Point(12, 734);
            groupBox7.Name = "groupBox7";
            groupBox7.Size = new Size(1163, 325);
            groupBox7.TabIndex = 18;
            groupBox7.TabStop = false;
            groupBox7.Text = "글자 변환";
            // 
            // letterSave
            // 
            letterSave.Location = new Point(1030, 291);
            letterSave.Name = "letterSave";
            letterSave.Size = new Size(127, 23);
            letterSave.TabIndex = 23;
            letterSave.Text = "난수저장";
            letterSave.UseVisualStyleBackColor = true;
            letterSave.Click += letterSave_Click;
            // 
            // letterNumSW
            // 
            letterNumSW.Location = new Point(1030, 90);
            letterNumSW.Name = "letterNumSW";
            letterNumSW.Size = new Size(127, 23);
            letterNumSW.TabIndex = 13;
            letterNumSW.Text = "난수 On/Off";
            letterNumSW.UseVisualStyleBackColor = true;
            letterNumSW.Click += letterNumSW_Click;
            // 
            // RNAllSaveBtn2
            // 
            RNAllSaveBtn2.Location = new Point(1030, 247);
            RNAllSaveBtn2.Name = "RNAllSaveBtn2";
            RNAllSaveBtn2.Size = new Size(127, 23);
            RNAllSaveBtn2.TabIndex = 24;
            RNAllSaveBtn2.Text = "난수 전체 저장";
            RNAllSaveBtn2.UseVisualStyleBackColor = true;
            RNAllSaveBtn2.Click += RNAllSaveBtn2_Click;
            // 
            // letterInputSW
            // 
            letterInputSW.Location = new Point(1030, 56);
            letterInputSW.Name = "letterInputSW";
            letterInputSW.Size = new Size(127, 23);
            letterInputSW.TabIndex = 12;
            letterInputSW.Text = "시나리오 On/Off";
            letterInputSW.UseVisualStyleBackColor = true;
            letterInputSW.Click += letterInputSW_Click;
            // 
            // letterChange
            // 
            letterChange.Location = new Point(1030, 22);
            letterChange.Name = "letterChange";
            letterChange.Size = new Size(127, 23);
            letterChange.TabIndex = 11;
            letterChange.Text = "난수변환";
            letterChange.UseVisualStyleBackColor = true;
            letterChange.Click += letterChange_Click;
            // 
            // letterInput
            // 
            letterInput.Font = new Font("맑은 고딕", 12F);
            letterInput.Location = new Point(6, 22);
            letterInput.Multiline = true;
            letterInput.Name = "letterInput";
            letterInput.ScrollBars = ScrollBars.Vertical;
            letterInput.Size = new Size(1018, 292);
            letterInput.TabIndex = 10;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(wordSave);
            groupBox3.Controls.Add(wordNumSW);
            groupBox3.Controls.Add(RNAllSaveBtn3);
            groupBox3.Controls.Add(wordInputSW);
            groupBox3.Controls.Add(wordChange);
            groupBox3.Controls.Add(wordInput);
            groupBox3.Location = new Point(12, 1093);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(1163, 325);
            groupBox3.TabIndex = 14;
            groupBox3.TabStop = false;
            groupBox3.Text = "단어 변환";
            // 
            // wordSave
            // 
            wordSave.Location = new Point(1030, 291);
            wordSave.Name = "wordSave";
            wordSave.Size = new Size(127, 23);
            wordSave.TabIndex = 24;
            wordSave.Text = "난수저장";
            wordSave.UseVisualStyleBackColor = true;
            wordSave.Click += wordSave_Click;
            // 
            // wordNumSW
            // 
            wordNumSW.Location = new Point(1030, 90);
            wordNumSW.Name = "wordNumSW";
            wordNumSW.Size = new Size(127, 23);
            wordNumSW.TabIndex = 13;
            wordNumSW.Text = "난수 On/Off";
            wordNumSW.UseVisualStyleBackColor = true;
            wordNumSW.Click += wordNumSW_Click;
            // 
            // RNAllSaveBtn3
            // 
            RNAllSaveBtn3.Location = new Point(1030, 247);
            RNAllSaveBtn3.Name = "RNAllSaveBtn3";
            RNAllSaveBtn3.Size = new Size(127, 23);
            RNAllSaveBtn3.TabIndex = 25;
            RNAllSaveBtn3.Text = "난수 전체 저장";
            RNAllSaveBtn3.UseVisualStyleBackColor = true;
            RNAllSaveBtn3.Click += RNAllSaveBtn3_Click;
            // 
            // wordInputSW
            // 
            wordInputSW.Location = new Point(1030, 56);
            wordInputSW.Name = "wordInputSW";
            wordInputSW.Size = new Size(127, 23);
            wordInputSW.TabIndex = 12;
            wordInputSW.Text = "시나리오 On/Off";
            wordInputSW.UseVisualStyleBackColor = true;
            wordInputSW.Click += wordInputSW_Click;
            // 
            // wordChange
            // 
            wordChange.Location = new Point(1030, 22);
            wordChange.Name = "wordChange";
            wordChange.Size = new Size(127, 23);
            wordChange.TabIndex = 11;
            wordChange.Text = "난수변환";
            wordChange.UseVisualStyleBackColor = true;
            wordChange.Click += wordChange_Click;
            // 
            // wordInput
            // 
            wordInput.Font = new Font("맑은 고딕", 12F);
            wordInput.Location = new Point(6, 22);
            wordInput.Multiline = true;
            wordInput.Name = "wordInput";
            wordInput.ScrollBars = ScrollBars.Vertical;
            wordInput.Size = new Size(1018, 292);
            wordInput.TabIndex = 10;
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(RNAllSaveBtn4);
            groupBox5.Controls.Add(selfSave);
            groupBox5.Controls.Add(newLineBtn);
            groupBox5.Controls.Add(selfNumSW);
            groupBox5.Controls.Add(selfInputSW);
            groupBox5.Controls.Add(selfChange);
            groupBox5.Controls.Add(selfInput);
            groupBox5.Location = new Point(12, 1449);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(1163, 325);
            groupBox5.TabIndex = 19;
            groupBox5.TabStop = false;
            groupBox5.Text = "수동 변환";
            // 
            // RNAllSaveBtn4
            // 
            RNAllSaveBtn4.Location = new Point(1030, 248);
            RNAllSaveBtn4.Name = "RNAllSaveBtn4";
            RNAllSaveBtn4.Size = new Size(127, 23);
            RNAllSaveBtn4.TabIndex = 26;
            RNAllSaveBtn4.Text = "난수 전체 저장";
            RNAllSaveBtn4.UseVisualStyleBackColor = true;
            RNAllSaveBtn4.Click += RNAllSaveBtn4_Click;
            // 
            // selfSave
            // 
            selfSave.Location = new Point(1030, 292);
            selfSave.Name = "selfSave";
            selfSave.Size = new Size(127, 23);
            selfSave.TabIndex = 26;
            selfSave.Text = "난수저장";
            selfSave.UseVisualStyleBackColor = true;
            selfSave.Click += selfSave_Click;
            // 
            // newLineBtn
            // 
            newLineBtn.Location = new Point(1030, 56);
            newLineBtn.Name = "newLineBtn";
            newLineBtn.Size = new Size(127, 23);
            newLineBtn.TabIndex = 14;
            newLineBtn.Text = "줄 바꿈 적용";
            newLineBtn.UseVisualStyleBackColor = true;
            newLineBtn.Click += newLineBtn_Click;
            // 
            // selfNumSW
            // 
            selfNumSW.Location = new Point(1030, 124);
            selfNumSW.Name = "selfNumSW";
            selfNumSW.Size = new Size(127, 23);
            selfNumSW.TabIndex = 13;
            selfNumSW.Text = "난수 On/Off";
            selfNumSW.UseVisualStyleBackColor = true;
            selfNumSW.Click += selfNumSW_Click;
            // 
            // selfInputSW
            // 
            selfInputSW.Location = new Point(1030, 90);
            selfInputSW.Name = "selfInputSW";
            selfInputSW.Size = new Size(127, 23);
            selfInputSW.TabIndex = 12;
            selfInputSW.Text = "시나리오 On/Off";
            selfInputSW.UseVisualStyleBackColor = true;
            selfInputSW.Click += selfInputSW_Click;
            // 
            // selfChange
            // 
            selfChange.Location = new Point(1030, 22);
            selfChange.Name = "selfChange";
            selfChange.Size = new Size(127, 23);
            selfChange.TabIndex = 11;
            selfChange.Text = "난수변환";
            selfChange.UseVisualStyleBackColor = true;
            selfChange.Click += selfChange_Click;
            // 
            // selfInput
            // 
            selfInput.Font = new Font("맑은 고딕", 12F);
            selfInput.Location = new Point(6, 22);
            selfInput.Multiline = true;
            selfInput.Name = "selfInput";
            selfInput.ScrollBars = ScrollBars.Vertical;
            selfInput.Size = new Size(1018, 292);
            selfInput.TabIndex = 10;
            // 
            // Scenario
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            AutoScrollMargin = new Size(0, 20);
            ClientSize = new Size(1266, 761);
            Controls.Add(groupBox5);
            Controls.Add(groupBox3);
            Controls.Add(groupBox7);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Name = "Scenario";
            Text = "Scenario";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox7.ResumeLayout(false);
            groupBox7.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox5.ResumeLayout(false);
            groupBox5.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TextBox scenarioName;
        private TextBox scenarioInput;
        private Label label1;
        private Button scenarioSave;
        private Button scenarioLoad;
        private GroupBox groupBox1;
        private TextBox partsInput;
        private GroupBox groupBox2;
        private Button partsNumSW;
        private Button partsInputSW;
        private Button partsChange;
        private GroupBox groupBox7;
        private Button letterNumSW;
        private Button letterInputSW;
        private Button letterChange;
        private TextBox letterInput;
        private GroupBox groupBox3;
        private Button wordNumSW;
        private Button wordInputSW;
        private Button wordChange;
        private TextBox wordInput;
        private GroupBox groupBox5;
        private Button selfNumSW;
        private Button selfInputSW;
        private Button selfChange;
        private TextBox selfInput;
        private Button scenarioChange;
        private Button newLineBtn;
        private Button partsSave;
        private Button wordSave;
        private Button selfSave;
        private Label label2;
        private Button RNAllSaveBtn1;
        private Button RNAllSaveBtn2;
        private Button RNAllSaveBtn3;
        private Button RNAllSaveBtn4;
        private Label label4;
        private Label maxLengthDisplay;
        private Button letterSave;
        private Label lengthChange;
        private Button autoValueBtn;
        private Button autoFullSave;
        private Button autoFullChange;
    }
}