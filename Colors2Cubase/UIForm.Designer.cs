/*
 * Created by SharpDevelop.
 * User: perivar.nerseth
 * Date: 24.10.2014
 * Time: 17:58
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Colors2Cubase
{
	partial class UIForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.TextBox txtFilePath;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.TextBox txtInput;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button btnChangeCubase;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Button btnReadColorConfigList;
		private System.Windows.Forms.Label labelVersion;
		private System.Windows.Forms.CheckBox chkUseDefaultList;
		private System.Windows.Forms.Button btnConvertToHtmlHex;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UIForm));
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.txtFilePath = new System.Windows.Forms.TextBox();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.txtInput = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.btnChangeCubase = new System.Windows.Forms.Button();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.btnReadColorConfigList = new System.Windows.Forms.Button();
			this.labelVersion = new System.Windows.Forms.Label();
			this.chkUseDefaultList = new System.Windows.Forms.CheckBox();
			this.btnConvertToHtmlHex = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(12, 44);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(134, 22);
			this.label1.TabIndex = 0;
			this.label1.Text = "Find Cubase Defaults.xml:";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(12, 9);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(396, 35);
			this.label2.TabIndex = 1;
			this.label2.Text = "This tool will modify Cubase\'s configuration file and insert the colors below ins" +
	"tead \r\n(while making a backup of the configuration file)\r\n";
			// 
			// openFileDialog
			// 
			this.openFileDialog.FileName = "openFileDialog1";
			// 
			// txtFilePath
			// 
			this.txtFilePath.Location = new System.Drawing.Point(12, 66);
			this.txtFilePath.Name = "txtFilePath";
			this.txtFilePath.Size = new System.Drawing.Size(507, 20);
			this.txtFilePath.TabIndex = 2;
			// 
			// btnBrowse
			// 
			this.btnBrowse.Location = new System.Drawing.Point(525, 63);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(65, 23);
			this.btnBrowse.TabIndex = 3;
			this.btnBrowse.Text = "Browse";
			this.btnBrowse.UseVisualStyleBackColor = true;
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowseClick);
			// 
			// txtInput
			// 
			this.txtInput.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtInput.Location = new System.Drawing.Point(13, 141);
			this.txtInput.Multiline = true;
			this.txtInput.Name = "txtInput";
			this.txtInput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtInput.Size = new System.Drawing.Size(321, 250);
			this.txtInput.TabIndex = 4;
			this.txtInput.TextChanged += new System.EventHandler(this.txtInputTextChanged);
			this.txtInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtInputKeyDown);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(13, 101);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(577, 37);
			this.label3.TabIndex = 5;
			this.label3.Text = resources.GetString("label3.Text");
			// 
			// btnChangeCubase
			// 
			this.btnChangeCubase.Location = new System.Drawing.Point(449, 397);
			this.btnChangeCubase.Name = "btnChangeCubase";
			this.btnChangeCubase.Size = new System.Drawing.Size(142, 23);
			this.btnChangeCubase.TabIndex = 6;
			this.btnChangeCubase.Text = "Make Changes to Cubase!";
			this.btnChangeCubase.UseVisualStyleBackColor = true;
			this.btnChangeCubase.Click += new System.EventHandler(this.btnChangeCubaseClick);
			// 
			// pictureBox1
			// 
			this.pictureBox1.Location = new System.Drawing.Point(340, 141);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(250, 250);
			this.pictureBox1.TabIndex = 7;
			this.pictureBox1.TabStop = false;
			// 
			// btnReadColorConfigList
			// 
			this.btnReadColorConfigList.Location = new System.Drawing.Point(13, 397);
			this.btnReadColorConfigList.Name = "btnReadColorConfigList";
			this.btnReadColorConfigList.Size = new System.Drawing.Size(146, 23);
			this.btnReadColorConfigList.TabIndex = 8;
			this.btnReadColorConfigList.Text = "Read colors from config file";
			this.btnReadColorConfigList.UseVisualStyleBackColor = true;
			this.btnReadColorConfigList.Click += new System.EventHandler(this.BtnReadColorConfigListClick);
			// 
			// labelVersion
			// 
			this.labelVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelVersion.Location = new System.Drawing.Point(428, 9);
			this.labelVersion.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.labelVersion.Name = "labelVersion";
			this.labelVersion.Size = new System.Drawing.Size(163, 14);
			this.labelVersion.TabIndex = 9;
			this.labelVersion.Text = "Version: ";
			this.labelVersion.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// chkUseDefaultList
			// 
			this.chkUseDefaultList.Location = new System.Drawing.Point(270, 397);
			this.chkUseDefaultList.Name = "chkUseDefaultList";
			this.chkUseDefaultList.Size = new System.Drawing.Size(173, 24);
			this.chkUseDefaultList.TabIndex = 10;
			this.chkUseDefaultList.Text = "Change Cubase Default Colors";
			this.chkUseDefaultList.UseVisualStyleBackColor = true;
			// 
			// btnConvertToHtmlHex
			// 
			this.btnConvertToHtmlHex.Location = new System.Drawing.Point(165, 397);
			this.btnConvertToHtmlHex.Name = "btnConvertToHtmlHex";
			this.btnConvertToHtmlHex.Size = new System.Drawing.Size(75, 23);
			this.btnConvertToHtmlHex.TabIndex = 11;
			this.btnConvertToHtmlHex.Text = "To html hex";
			this.btnConvertToHtmlHex.UseVisualStyleBackColor = true;
			this.btnConvertToHtmlHex.Click += new System.EventHandler(this.BtnConvertToHtmlHexClick);
			// 
			// UIForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(602, 432);
			this.Controls.Add(this.btnConvertToHtmlHex);
			this.Controls.Add(this.chkUseDefaultList);
			this.Controls.Add(this.labelVersion);
			this.Controls.Add(this.btnReadColorConfigList);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.btnChangeCubase);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.txtInput);
			this.Controls.Add(this.btnBrowse);
			this.Controls.Add(this.txtFilePath);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Name = "UIForm";
			this.Text = "Color Palette 2 Cubase";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
