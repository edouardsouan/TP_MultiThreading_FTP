﻿namespace FTPClient
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.toolStripConnection = new System.Windows.Forms.ToolStrip();
            this.toolStripLabelServer = new System.Windows.Forms.ToolStripLabel();
            this.txtServer = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabelUserName = new System.Windows.Forms.ToolStripLabel();
            this.txtUserName = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabelPassword = new System.Windows.Forms.ToolStripLabel();
            this.txtPassword = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabelPort = new System.Windows.Forms.ToolStripLabel();
            this.txtPort = new System.Windows.Forms.ToolStripTextBox();
            this.btnConnection = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.logWindow = new System.Windows.Forms.RichTextBox();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.treeViewLocal = new System.Windows.Forms.TreeView();
            this.listViewLocal = new System.Windows.Forms.ListView();
            this.fileNameLocal = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.fileSizeLocal = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.fileTypeLocal = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lastModifiedLocal = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.splitContainer5 = new System.Windows.Forms.SplitContainer();
            this.treeViewServer = new System.Windows.Forms.TreeView();
            this.listViewServer = new System.Windows.Forms.ListView();
            this.fileNameServer = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.fileSizeServer = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.fileTypeServer = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lastModifiedServer = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.fileRights = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.fileOwner = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.fileGroup = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.splitContainer6 = new System.Windows.Forms.SplitContainer();
            this.FileQueue = new System.Windows.Forms.ListView();
            this.ServerFileLocation = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Direction = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DistFile = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.FileSizeTransfert = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Time = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.fileTransfertBar = new System.Windows.Forms.ProgressBar();
            this.toolStripConnection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).BeginInit();
            this.splitContainer5.Panel1.SuspendLayout();
            this.splitContainer5.Panel2.SuspendLayout();
            this.splitContainer5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer6)).BeginInit();
            this.splitContainer6.Panel1.SuspendLayout();
            this.splitContainer6.Panel2.SuspendLayout();
            this.splitContainer6.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "logical_disk_16.png");
            this.imageList.Images.SetKeyName(1, "directory_16.png");
            this.imageList.Images.SetKeyName(2, "file_16.png");
            // 
            // toolStripConnection
            // 
            this.toolStripConnection.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabelServer,
            this.txtServer,
            this.toolStripLabelUserName,
            this.txtUserName,
            this.toolStripLabelPassword,
            this.txtPassword,
            this.toolStripLabelPort,
            this.txtPort,
            this.btnConnection});
            this.toolStripConnection.Location = new System.Drawing.Point(0, 0);
            this.toolStripConnection.Name = "toolStripConnection";
            this.toolStripConnection.Size = new System.Drawing.Size(877, 25);
            this.toolStripConnection.TabIndex = 0;
            this.toolStripConnection.Text = "toolStrip1";
            // 
            // toolStripLabelServer
            // 
            this.toolStripLabelServer.Name = "toolStripLabelServer";
            this.toolStripLabelServer.Size = new System.Drawing.Size(48, 22);
            this.toolStripLabelServer.Text = "Server : ";
            // 
            // txtServer
            // 
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(128, 25);
            this.txtServer.Text = "ftp.kimsavinfo.fr";
            // 
            // toolStripLabelUserName
            // 
            this.toolStripLabelUserName.Name = "toolStripLabelUserName";
            this.toolStripLabelUserName.Size = new System.Drawing.Size(71, 22);
            this.toolStripLabelUserName.Text = "UserName : ";
            // 
            // txtUserName
            // 
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(128, 25);
            this.txtUserName.Text = "kimsavin";
            // 
            // toolStripLabelPassword
            // 
            this.toolStripLabelPassword.Name = "toolStripLabelPassword";
            this.toolStripLabelPassword.Size = new System.Drawing.Size(66, 22);
            this.toolStripLabelPassword.Text = "Password : ";
            // 
            // txtPassword
            // 
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(128, 25);
            this.txtPassword.Text = "Se8yBapG";
            // 
            // toolStripLabelPort
            // 
            this.toolStripLabelPort.Name = "toolStripLabelPort";
            this.toolStripLabelPort.Size = new System.Drawing.Size(38, 22);
            this.toolStripLabelPort.Text = "Port : ";
            // 
            // txtPort
            // 
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(58, 25);
            this.txtPort.Text = "21";
            // 
            // btnConnection
            // 
            this.btnConnection.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnConnection.Image = global::FTPClient.Properties.Resources.connexion;
            this.btnConnection.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnConnection.Name = "btnConnection";
            this.btnConnection.Size = new System.Drawing.Size(23, 22);
            this.btnConnection.Text = "toolStripButton1";
            this.btnConnection.Click += new System.EventHandler(this.btnConnection_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer6);
            this.splitContainer1.Size = new System.Drawing.Size(877, 643);
            this.splitContainer1.SplitterDistance = 551;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 1;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.logWindow);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer2.Size = new System.Drawing.Size(877, 551);
            this.splitContainer2.SplitterDistance = 100;
            this.splitContainer2.SplitterWidth = 3;
            this.splitContainer2.TabIndex = 0;
            // 
            // logWindow
            // 
            this.logWindow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logWindow.Location = new System.Drawing.Point(0, 0);
            this.logWindow.Margin = new System.Windows.Forms.Padding(2);
            this.logWindow.Name = "logWindow";
            this.logWindow.ReadOnly = true;
            this.logWindow.Size = new System.Drawing.Size(877, 100);
            this.logWindow.TabIndex = 0;
            this.logWindow.Text = "";
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.splitContainer4);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.splitContainer5);
            this.splitContainer3.Size = new System.Drawing.Size(877, 448);
            this.splitContainer3.SplitterDistance = 426;
            this.splitContainer3.SplitterWidth = 3;
            this.splitContainer3.TabIndex = 0;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.treeViewLocal);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.listViewLocal);
            this.splitContainer4.Size = new System.Drawing.Size(426, 448);
            this.splitContainer4.SplitterDistance = 237;
            this.splitContainer4.TabIndex = 0;
            // 
            // treeViewLocal
            // 
            this.treeViewLocal.AllowDrop = true;
            this.treeViewLocal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewLocal.ImageIndex = 0;
            this.treeViewLocal.ImageList = this.imageList;
            this.treeViewLocal.Location = new System.Drawing.Point(0, 0);
            this.treeViewLocal.Margin = new System.Windows.Forms.Padding(2);
            this.treeViewLocal.Name = "treeViewLocal";
            this.treeViewLocal.SelectedImageIndex = 0;
            this.treeViewLocal.Size = new System.Drawing.Size(426, 237);
            this.treeViewLocal.TabIndex = 0;
            this.treeViewLocal.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewLocal_NodeMouseDoubleClick);
            // 
            // listViewLocal
            // 
            this.listViewLocal.AllowDrop = true;
            this.listViewLocal.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.fileNameLocal,
            this.fileSizeLocal,
            this.fileTypeLocal,
            this.lastModifiedLocal});
            this.listViewLocal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewLocal.Location = new System.Drawing.Point(0, 0);
            this.listViewLocal.Name = "listViewLocal";
            this.listViewLocal.Size = new System.Drawing.Size(426, 207);
            this.listViewLocal.SmallImageList = this.imageList;
            this.listViewLocal.TabIndex = 1;
            this.listViewLocal.UseCompatibleStateImageBehavior = false;
            this.listViewLocal.View = System.Windows.Forms.View.Details;
            this.listViewLocal.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.listViewLocal_ItemDrag);
            this.listViewLocal.DragDrop += new System.Windows.Forms.DragEventHandler(this.listViewLocal_DragDrop);
            this.listViewLocal.DragEnter += new System.Windows.Forms.DragEventHandler(this.listViewLocal_DragEnter);
            // 
            // fileNameLocal
            // 
            this.fileNameLocal.Text = "File Name";
            this.fileNameLocal.Width = 112;
            // 
            // fileSizeLocal
            // 
            this.fileSizeLocal.Text = "File Size";
            this.fileSizeLocal.Width = 85;
            // 
            // fileTypeLocal
            // 
            this.fileTypeLocal.Text = "File Type";
            this.fileTypeLocal.Width = 94;
            // 
            // lastModifiedLocal
            // 
            this.lastModifiedLocal.Text = "Last Modified";
            this.lastModifiedLocal.Width = 127;
            // 
            // splitContainer5
            // 
            this.splitContainer5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer5.Location = new System.Drawing.Point(0, 0);
            this.splitContainer5.Name = "splitContainer5";
            this.splitContainer5.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer5.Panel1
            // 
            this.splitContainer5.Panel1.Controls.Add(this.treeViewServer);
            // 
            // splitContainer5.Panel2
            // 
            this.splitContainer5.Panel2.Controls.Add(this.listViewServer);
            this.splitContainer5.Size = new System.Drawing.Size(448, 448);
            this.splitContainer5.SplitterDistance = 237;
            this.splitContainer5.TabIndex = 0;
            // 
            // treeViewServer
            // 
            this.treeViewServer.AllowDrop = true;
            this.treeViewServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewServer.ImageIndex = 0;
            this.treeViewServer.ImageList = this.imageList;
            this.treeViewServer.Location = new System.Drawing.Point(0, 0);
            this.treeViewServer.Name = "treeViewServer";
            this.treeViewServer.SelectedImageIndex = 0;
            this.treeViewServer.Size = new System.Drawing.Size(448, 237);
            this.treeViewServer.TabIndex = 0;
            this.treeViewServer.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewServer_NodeMouseDoubleClick);
            // 
            // listViewServer
            // 
            this.listViewServer.AllowDrop = true;
            this.listViewServer.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.fileNameServer,
            this.fileSizeServer,
            this.fileTypeServer,
            this.lastModifiedServer,
            this.fileRights,
            this.fileOwner,
            this.fileGroup});
            this.listViewServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewServer.Location = new System.Drawing.Point(0, 0);
            this.listViewServer.Name = "listViewServer";
            this.listViewServer.Size = new System.Drawing.Size(448, 207);
            this.listViewServer.SmallImageList = this.imageList;
            this.listViewServer.TabIndex = 0;
            this.listViewServer.UseCompatibleStateImageBehavior = false;
            this.listViewServer.View = System.Windows.Forms.View.Details;
            this.listViewServer.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.listViewLocal_ItemDrag);
            this.listViewServer.DragDrop += new System.Windows.Forms.DragEventHandler(this.listViewServer_DragDrop);
            this.listViewServer.DragEnter += new System.Windows.Forms.DragEventHandler(this.listViewServer_DragEnter);
            // 
            // fileNameServer
            // 
            this.fileNameServer.Text = "File Name";
            this.fileNameServer.Width = 112;
            // 
            // fileSizeServer
            // 
            this.fileSizeServer.Text = "File Size";
            this.fileSizeServer.Width = 85;
            // 
            // fileTypeServer
            // 
            this.fileTypeServer.Text = "File Type";
            this.fileTypeServer.Width = 94;
            // 
            // lastModifiedServer
            // 
            this.lastModifiedServer.Text = "Last Modified";
            this.lastModifiedServer.Width = 127;
            // 
            // fileRights
            // 
            this.fileRights.Text = "Rights";
            // 
            // fileOwner
            // 
            this.fileOwner.Text = "Owner";
            // 
            // fileGroup
            // 
            this.fileGroup.Text = "Group";
            // 
            // splitContainer6
            // 
            this.splitContainer6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer6.Location = new System.Drawing.Point(0, 0);
            this.splitContainer6.Name = "splitContainer6";
            this.splitContainer6.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer6.Panel1
            // 
            this.splitContainer6.Panel1.Controls.Add(this.fileTransfertBar);
            // 
            // splitContainer6.Panel2
            // 
            this.splitContainer6.Panel2.Controls.Add(this.FileQueue);
            this.splitContainer6.Size = new System.Drawing.Size(877, 89);
            this.splitContainer6.SplitterDistance = 28;
            this.splitContainer6.TabIndex = 0;
            // 
            // FileQueue
            // 
            this.FileQueue.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ServerFileLocation,
            this.Direction,
            this.DistFile,
            this.FileSizeTransfert,
            this.Time});
            this.FileQueue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FileQueue.Location = new System.Drawing.Point(0, 0);
            this.FileQueue.Name = "FileQueue";
            this.FileQueue.Size = new System.Drawing.Size(877, 57);
            this.FileQueue.TabIndex = 0;
            this.FileQueue.UseCompatibleStateImageBehavior = false;
            this.FileQueue.View = System.Windows.Forms.View.Details;
            // 
            // ServerFileLocation
            // 
            this.ServerFileLocation.Text = "Server / File Location";
            this.ServerFileLocation.Width = 275;
            // 
            // Direction
            // 
            this.Direction.Text = "Direction";
            // 
            // DistFile
            // 
            this.DistFile.Text = "Dist File";
            this.DistFile.Width = 281;
            // 
            // FileSizeTransfert
            // 
            this.FileSizeTransfert.Text = "Size";
            this.FileSizeTransfert.Width = 110;
            // 
            // Time
            // 
            this.Time.Text = "Time";
            this.Time.Width = 147;
            // 
            // fileTransfertBar
            // 
            this.fileTransfertBar.Location = new System.Drawing.Point(3, 3);
            this.fileTransfertBar.Name = "fileTransfertBar";
            this.fileTransfertBar.Size = new System.Drawing.Size(871, 23);
            this.fileTransfertBar.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(877, 668);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStripConnection);
            this.Name = "Form1";
            this.Text = "FTP Client";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.toolStripConnection.ResumeLayout(false);
            this.toolStripConnection.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.splitContainer5.Panel1.ResumeLayout(false);
            this.splitContainer5.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).EndInit();
            this.splitContainer5.ResumeLayout(false);
            this.splitContainer6.Panel1.ResumeLayout(false);
            this.splitContainer6.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer6)).EndInit();
            this.splitContainer6.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ToolStrip toolStripConnection;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.RichTextBox logWindow;
        private System.Windows.Forms.TreeView treeViewLocal;
        private System.Windows.Forms.ToolStripLabel toolStripLabelServer;
        private System.Windows.Forms.ToolStripTextBox txtServer;
        private System.Windows.Forms.ToolStripLabel toolStripLabelUserName;
        private System.Windows.Forms.ToolStripTextBox txtUserName;
        private System.Windows.Forms.ToolStripLabel toolStripLabelPassword;
        private System.Windows.Forms.ToolStripTextBox txtPassword;
        private System.Windows.Forms.ToolStripLabel toolStripLabelPort;
        private System.Windows.Forms.ToolStripTextBox txtPort;
        private System.Windows.Forms.ToolStripButton btnConnection;
        private System.Windows.Forms.TreeView treeViewServer;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.SplitContainer splitContainer5;
        private System.Windows.Forms.ListView listViewServer;
        private System.Windows.Forms.ColumnHeader fileNameServer;
        private System.Windows.Forms.ColumnHeader fileSizeServer;
        private System.Windows.Forms.ColumnHeader fileTypeServer;
        private System.Windows.Forms.ColumnHeader lastModifiedServer;
        private System.Windows.Forms.ListView listViewLocal;
        private System.Windows.Forms.ColumnHeader fileNameLocal;
        private System.Windows.Forms.ColumnHeader fileSizeLocal;
        private System.Windows.Forms.ColumnHeader fileTypeLocal;
        private System.Windows.Forms.ColumnHeader lastModifiedLocal;
        private System.Windows.Forms.ColumnHeader fileRights;
        private System.Windows.Forms.ColumnHeader fileOwner;
        private System.Windows.Forms.ColumnHeader fileGroup;
        private System.Windows.Forms.SplitContainer splitContainer6;
        private System.Windows.Forms.ListView FileQueue;
        private System.Windows.Forms.ColumnHeader ServerFileLocation;
        private System.Windows.Forms.ColumnHeader Direction;
        private System.Windows.Forms.ColumnHeader DistFile;
        private System.Windows.Forms.ColumnHeader FileSizeTransfert;
        private System.Windows.Forms.ColumnHeader Time;
        private System.Windows.Forms.ProgressBar fileTransfertBar;
    }
}
