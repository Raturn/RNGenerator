﻿namespace RNGenerator
{
    partial class SearchBox
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
            label1 = new Label();
            searchTextBox = new TextBox();
            button1 = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(70, 15);
            label1.TabIndex = 0;
            label1.Text = "검색 대상 : ";
            // 
            // searchTextBox
            // 
            searchTextBox.Location = new Point(81, 6);
            searchTextBox.Name = "searchTextBox";
            searchTextBox.Size = new Size(253, 23);
            searchTextBox.TabIndex = 1;
            // 
            // button1
            // 
            button1.Location = new Point(340, 6);
            button1.Name = "button1";
            button1.Size = new Size(56, 23);
            button1.TabIndex = 2;
            button1.Text = "검색";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // SearchBox
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(410, 35);
            Controls.Add(button1);
            Controls.Add(searchTextBox);
            Controls.Add(label1);
            Name = "SearchBox";
            Text = "SearchBox";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox searchTextBox;
        private Button button1;
    }
}