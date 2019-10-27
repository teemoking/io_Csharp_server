namespace io_20191021
{
    partial class ProForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProForm));
            this.ProForm_IP = new System.Windows.Forms.Label();
            this.ProForm_Run = new System.Windows.Forms.Button();
            this.ProFrom_Port = new System.Windows.Forms.Label();
            this.ProForm_txtIP = new System.Windows.Forms.TextBox();
            this.ProForm_txtPort = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // ProForm_IP
            // 
            this.ProForm_IP.AutoSize = true;
            this.ProForm_IP.BackColor = System.Drawing.Color.Transparent;
            this.ProForm_IP.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ProForm_IP.ForeColor = System.Drawing.Color.Cornsilk;
            this.ProForm_IP.Location = new System.Drawing.Point(351, 234);
            this.ProForm_IP.Name = "ProForm_IP";
            this.ProForm_IP.Size = new System.Drawing.Size(108, 19);
            this.ProForm_IP.TabIndex = 0;
            this.ProForm_IP.Text = "SERVER IP";
            // 
            // ProForm_Run
            // 
            this.ProForm_Run.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ProForm_Run.ForeColor = System.Drawing.Color.DarkBlue;
            this.ProForm_Run.Location = new System.Drawing.Point(517, 380);
            this.ProForm_Run.Name = "ProForm_Run";
            this.ProForm_Run.Size = new System.Drawing.Size(90, 35);
            this.ProForm_Run.TabIndex = 1;
            this.ProForm_Run.Text = "RUN";
            this.ProForm_Run.UseVisualStyleBackColor = true;
            this.ProForm_Run.Click += new System.EventHandler(this.btRun_Click);
            // 
            // ProFrom_Port
            // 
            this.ProFrom_Port.AutoSize = true;
            this.ProFrom_Port.BackColor = System.Drawing.Color.Transparent;
            this.ProFrom_Port.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ProFrom_Port.ForeColor = System.Drawing.Color.Cornsilk;
            this.ProFrom_Port.Location = new System.Drawing.Point(351, 296);
            this.ProFrom_Port.Name = "ProFrom_Port";
            this.ProFrom_Port.Size = new System.Drawing.Size(130, 19);
            this.ProFrom_Port.TabIndex = 2;
            this.ProFrom_Port.Text = "SERVER PORT";
            // 
            // ProForm_txtIP
            // 
            this.ProForm_txtIP.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ProForm_txtIP.ForeColor = System.Drawing.Color.DarkBlue;
            this.ProForm_txtIP.Location = new System.Drawing.Point(498, 231);
            this.ProForm_txtIP.Name = "ProForm_txtIP";
            this.ProForm_txtIP.Size = new System.Drawing.Size(180, 29);
            this.ProForm_txtIP.TabIndex = 5;
            // 
            // ProForm_txtPort
            // 
            this.ProForm_txtPort.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ProForm_txtPort.ForeColor = System.Drawing.Color.DarkBlue;
            this.ProForm_txtPort.Location = new System.Drawing.Point(498, 291);
            this.ProForm_txtPort.Name = "ProForm_txtPort";
            this.ProForm_txtPort.Size = new System.Drawing.Size(180, 29);
            this.ProForm_txtPort.TabIndex = 6;
            // 
            // ProForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(793, 462);
            this.Controls.Add(this.ProForm_txtPort);
            this.Controls.Add(this.ProForm_txtIP);
            this.Controls.Add(this.ProFrom_Port);
            this.Controls.Add(this.ProForm_Run);
            this.Controls.Add(this.ProForm_IP);
            this.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "ProForm";
            this.Text = "io-设备控制";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ProForm_IP;
        private System.Windows.Forms.Button ProForm_Run;
        private System.Windows.Forms.Label ProFrom_Port;
        private System.Windows.Forms.TextBox ProForm_txtIP;
        private System.Windows.Forms.TextBox ProForm_txtPort;
    }
}

