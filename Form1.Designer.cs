namespace kinect_get_data
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.NumberOfBodiesLabel = new System.Windows.Forms.Label();
            this.IMUTimeStampLabel = new System.Windows.Forms.Label();
            this.FrameTimestampLabel = new System.Windows.Forms.Label();
            this.TotalPixelsLabel = new System.Windows.Forms.Label();
            this.Error = new System.Windows.Forms.Label();
            this.NumberOfBodies = new System.Windows.Forms.Label();
            this.IMUTimestamp = new System.Windows.Forms.Label();
            this.FrameTimestamp = new System.Windows.Forms.Label();
            this.TotalPixels = new System.Windows.Forms.Label();
            this.csvLabel = new System.Windows.Forms.Label();
            this.positionData = new System.Windows.Forms.Label();
            this.quaternionData = new System.Windows.Forms.Label();
            this.measureBtn = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.answerLabel = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(0, 25);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(533, 386);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(539, 25);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(494, 386);
            this.pictureBox2.TabIndex = 8;
            this.pictureBox2.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(184, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "By depth sensor";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(749, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 12);
            this.label2.TabIndex = 10;
            this.label2.Text = "By monocular camera";
            // 
            // NumberOfBodiesLabel
            // 
            this.NumberOfBodiesLabel.AutoSize = true;
            this.NumberOfBodiesLabel.Location = new System.Drawing.Point(6, 426);
            this.NumberOfBodiesLabel.Name = "NumberOfBodiesLabel";
            this.NumberOfBodiesLabel.Size = new System.Drawing.Size(97, 12);
            this.NumberOfBodiesLabel.TabIndex = 11;
            this.NumberOfBodiesLabel.Text = "Number of Bodies";
            // 
            // IMUTimeStampLabel
            // 
            this.IMUTimeStampLabel.AutoSize = true;
            this.IMUTimeStampLabel.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.IMUTimeStampLabel.Location = new System.Drawing.Point(123, 426);
            this.IMUTimeStampLabel.Name = "IMUTimeStampLabel";
            this.IMUTimeStampLabel.Size = new System.Drawing.Size(112, 16);
            this.IMUTimeStampLabel.TabIndex = 13;
            this.IMUTimeStampLabel.Text = "IMU Timestamp";
            // 
            // FrameTimestampLabel
            // 
            this.FrameTimestampLabel.AutoSize = true;
            this.FrameTimestampLabel.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FrameTimestampLabel.Location = new System.Drawing.Point(123, 487);
            this.FrameTimestampLabel.Name = "FrameTimestampLabel";
            this.FrameTimestampLabel.Size = new System.Drawing.Size(129, 16);
            this.FrameTimestampLabel.TabIndex = 14;
            this.FrameTimestampLabel.Text = "Frame Timestamp";
            // 
            // TotalPixelsLabel
            // 
            this.TotalPixelsLabel.AutoSize = true;
            this.TotalPixelsLabel.Location = new System.Drawing.Point(12, 483);
            this.TotalPixelsLabel.Name = "TotalPixelsLabel";
            this.TotalPixelsLabel.Size = new System.Drawing.Size(66, 12);
            this.TotalPixelsLabel.TabIndex = 15;
            this.TotalPixelsLabel.Text = "Total Pixels";
            // 
            // Error
            // 
            this.Error.AutoSize = true;
            this.Error.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Error.ForeColor = System.Drawing.Color.Red;
            this.Error.Location = new System.Drawing.Point(493, 510);
            this.Error.Name = "Error";
            this.Error.Size = new System.Drawing.Size(104, 19);
            this.Error.TabIndex = 16;
            this.Error.Text = "Error Text";
            // 
            // NumberOfBodies
            // 
            this.NumberOfBodies.AutoSize = true;
            this.NumberOfBodies.Font = new System.Drawing.Font("MS UI Gothic", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.NumberOfBodies.Location = new System.Drawing.Point(42, 446);
            this.NumberOfBodies.Name = "NumberOfBodies";
            this.NumberOfBodies.Size = new System.Drawing.Size(28, 29);
            this.NumberOfBodies.TabIndex = 17;
            this.NumberOfBodies.Text = "0";
            // 
            // IMUTimestamp
            // 
            this.IMUTimestamp.AutoSize = true;
            this.IMUTimestamp.Font = new System.Drawing.Font("MS UI Gothic", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.IMUTimestamp.Location = new System.Drawing.Point(128, 446);
            this.IMUTimestamp.Name = "IMUTimestamp";
            this.IMUTimestamp.Size = new System.Drawing.Size(115, 29);
            this.IMUTimestamp.TabIndex = 18;
            this.IMUTimestamp.Text = "00:00:00";
            // 
            // FrameTimestamp
            // 
            this.FrameTimestamp.AutoSize = true;
            this.FrameTimestamp.Font = new System.Drawing.Font("MS UI Gothic", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FrameTimestamp.Location = new System.Drawing.Point(128, 506);
            this.FrameTimestamp.Name = "FrameTimestamp";
            this.FrameTimestamp.Size = new System.Drawing.Size(115, 29);
            this.FrameTimestamp.TabIndex = 19;
            this.FrameTimestamp.Text = "00:00:00";
            // 
            // TotalPixels
            // 
            this.TotalPixels.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.TotalPixels.AutoSize = true;
            this.TotalPixels.Font = new System.Drawing.Font("MS UI Gothic", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.TotalPixels.Location = new System.Drawing.Point(12, 506);
            this.TotalPixels.Name = "TotalPixels";
            this.TotalPixels.Size = new System.Drawing.Size(28, 29);
            this.TotalPixels.TabIndex = 20;
            this.TotalPixels.Text = "0";
            this.TotalPixels.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csvLabel
            // 
            this.csvLabel.AutoSize = true;
            this.csvLabel.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.csvLabel.Location = new System.Drawing.Point(787, 513);
            this.csvLabel.Name = "csvLabel";
            this.csvLabel.Size = new System.Drawing.Size(158, 16);
            this.csvLabel.TabIndex = 22;
            this.csvLabel.Text = "CSV出力結果が表示";
            // 
            // positionData
            // 
            this.positionData.AutoSize = true;
            this.positionData.Location = new System.Drawing.Point(525, 436);
            this.positionData.Name = "positionData";
            this.positionData.Size = new System.Drawing.Size(72, 12);
            this.positionData.TabIndex = 23;
            this.positionData.Text = "Position data\r\n";
            // 
            // quaternionData
            // 
            this.quaternionData.AutoSize = true;
            this.quaternionData.Location = new System.Drawing.Point(627, 436);
            this.quaternionData.Name = "quaternionData";
            this.quaternionData.Size = new System.Drawing.Size(86, 12);
            this.quaternionData.TabIndex = 24;
            this.quaternionData.Text = "Quaternion data\r\n";
            // 
            // measureBtn
            // 
            this.measureBtn.Location = new System.Drawing.Point(907, 472);
            this.measureBtn.Name = "measureBtn";
            this.measureBtn.Size = new System.Drawing.Size(111, 23);
            this.measureBtn.TabIndex = 25;
            this.measureBtn.Text = "計測開始";
            this.measureBtn.UseVisualStyleBackColor = true;
            this.measureBtn.Click += new System.EventHandler(this.MeasureBtn_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(907, 418);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 12);
            this.label3.TabIndex = 26;
            this.label3.Text = "正解ラベル";
            // 
            // answerLabel
            // 
            this.answerLabel.Location = new System.Drawing.Point(909, 436);
            this.answerLabel.Name = "answerLabel";
            this.answerLabel.Size = new System.Drawing.Size(100, 19);
            this.answerLabel.TabIndex = 27;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1045, 553);
            this.Controls.Add(this.answerLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.measureBtn);
            this.Controls.Add(this.quaternionData);
            this.Controls.Add(this.positionData);
            this.Controls.Add(this.csvLabel);
            this.Controls.Add(this.TotalPixels);
            this.Controls.Add(this.FrameTimestamp);
            this.Controls.Add(this.IMUTimestamp);
            this.Controls.Add(this.NumberOfBodies);
            this.Controls.Add(this.Error);
            this.Controls.Add(this.TotalPixelsLabel);
            this.Controls.Add(this.FrameTimestampLabel);
            this.Controls.Add(this.IMUTimeStampLabel);
            this.Controls.Add(this.NumberOfBodiesLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label NumberOfBodiesLabel;
        private System.Windows.Forms.Label NumberOfBodies;
        private System.Windows.Forms.Label IMUTimeStampLabel;
        private System.Windows.Forms.Label FrameTimestampLabel;
        private System.Windows.Forms.Label TotalPixelsLabel;
        private System.Windows.Forms.Label Error;
        private System.Windows.Forms.Label IMUTimestamp;
        private System.Windows.Forms.Label FrameTimestamp;
        private System.Windows.Forms.Label TotalPixels;
        private System.Windows.Forms.Label csvLabel;
        private System.Windows.Forms.Label positionData;
        private System.Windows.Forms.Label quaternionData;
        private System.Windows.Forms.Button measureBtn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox answerLabel;
    }
}

