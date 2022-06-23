namespace BoD_OTLToolingGUI
{
    partial class Home
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Home));
            this.label1 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxSubset = new System.Windows.Forms.TextBox();
            this.buttonSubset = new System.Windows.Forms.Button();
            this.textBoxDWG = new System.Windows.Forms.TextBox();
            this.buttonDWG = new System.Windows.Forms.Button();
            this.checkNewDrawing = new System.Windows.Forms.CheckBox();
            this.buttonInject = new System.Windows.Forms.Button();
            this.checkoverwrite = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(506, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "This application will inject an OTL subset database into a Civil3D Acad file (dra" +
    "wing file versions AC1032). ";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(140, 25);
            this.linkLabel1.Margin = new System.Windows.Forms.Padding(0, 3, 3, 0);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(104, 13);
            this.linkLabel1.TabIndex = 3;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "the AWV subset tool";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 25);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Create a subset fisrt with ";
            // 
            // textBoxSubset
            // 
            this.textBoxSubset.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSubset.Location = new System.Drawing.Point(174, 72);
            this.textBoxSubset.Margin = new System.Windows.Forms.Padding(3, 50, 3, 3);
            this.textBoxSubset.Name = "textBoxSubset";
            this.textBoxSubset.Size = new System.Drawing.Size(614, 20);
            this.textBoxSubset.TabIndex = 7;
            // 
            // buttonSubset
            // 
            this.buttonSubset.Location = new System.Drawing.Point(15, 70);
            this.buttonSubset.Margin = new System.Windows.Forms.Padding(3, 50, 3, 3);
            this.buttonSubset.Name = "buttonSubset";
            this.buttonSubset.Size = new System.Drawing.Size(153, 23);
            this.buttonSubset.TabIndex = 6;
            this.buttonSubset.Text = "Select a Subset...";
            this.buttonSubset.UseVisualStyleBackColor = true;
            this.buttonSubset.Click += new System.EventHandler(this.buttonSubset_Click);
            // 
            // textBoxDWG
            // 
            this.textBoxDWG.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxDWG.Location = new System.Drawing.Point(174, 105);
            this.textBoxDWG.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.textBoxDWG.Name = "textBoxDWG";
            this.textBoxDWG.Size = new System.Drawing.Size(614, 20);
            this.textBoxDWG.TabIndex = 9;
            this.textBoxDWG.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // buttonDWG
            // 
            this.buttonDWG.Location = new System.Drawing.Point(15, 102);
            this.buttonDWG.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.buttonDWG.Name = "buttonDWG";
            this.buttonDWG.Size = new System.Drawing.Size(153, 23);
            this.buttonDWG.TabIndex = 8;
            this.buttonDWG.Text = "Select a drawing file...";
            this.buttonDWG.UseVisualStyleBackColor = true;
            this.buttonDWG.Click += new System.EventHandler(this.buttonDWG_Click);
            // 
            // checkNewDrawing
            // 
            this.checkNewDrawing.AutoSize = true;
            this.checkNewDrawing.Location = new System.Drawing.Point(174, 131);
            this.checkNewDrawing.Name = "checkNewDrawing";
            this.checkNewDrawing.Size = new System.Drawing.Size(182, 17);
            this.checkNewDrawing.TabIndex = 14;
            this.checkNewDrawing.Text = "Create a new drawing file instead";
            this.checkNewDrawing.UseVisualStyleBackColor = true;
            this.checkNewDrawing.CheckedChanged += new System.EventHandler(this.checkNewDrawingFile);
            // 
            // buttonInject
            // 
            this.buttonInject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonInject.Enabled = false;
            this.buttonInject.Location = new System.Drawing.Point(618, 205);
            this.buttonInject.Name = "buttonInject";
            this.buttonInject.Size = new System.Drawing.Size(170, 23);
            this.buttonInject.TabIndex = 15;
            this.buttonInject.Text = "Inject DWG File";
            this.buttonInject.UseVisualStyleBackColor = true;
            this.buttonInject.Click += new System.EventHandler(this.buttonInject_Click);
            // 
            // checkoverwrite
            // 
            this.checkoverwrite.AutoSize = true;
            this.checkoverwrite.Location = new System.Drawing.Point(174, 154);
            this.checkoverwrite.Name = "checkoverwrite";
            this.checkoverwrite.Size = new System.Drawing.Size(371, 17);
            this.checkoverwrite.TabIndex = 16;
            this.checkoverwrite.Text = "Overwrite OTL class where possible (used classes cannot be overwritten)";
            this.checkoverwrite.UseVisualStyleBackColor = true;
            // 
            // Home
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 240);
            this.Controls.Add(this.checkoverwrite);
            this.Controls.Add(this.buttonInject);
            this.Controls.Add(this.checkNewDrawing);
            this.Controls.Add(this.textBoxDWG);
            this.Controls.Add(this.buttonDWG);
            this.Controls.Add(this.textBoxSubset);
            this.Controls.Add(this.buttonSubset);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Home";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OTL to C3D by BitsofData";
            this.Load += new System.EventHandler(this.Home_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxSubset;
        private System.Windows.Forms.Button buttonSubset;
        private System.Windows.Forms.TextBox textBoxDWG;
        private System.Windows.Forms.Button buttonDWG;
        private System.Windows.Forms.CheckBox checkNewDrawing;
        private System.Windows.Forms.Button buttonInject;
        private System.Windows.Forms.CheckBox checkoverwrite;
    }
}

