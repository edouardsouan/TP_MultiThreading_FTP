﻿using FTP_Client.InfoEntities;
using FTP_Client.LocalEntities;
using FTP_Client.ServerEntities;

namespace FTP_Client
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer6 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnDeleteServer = new System.Windows.Forms.Button();
            this.btnConnection = new System.Windows.Forms.Button();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.labelPort = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.labelUserName = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.labelPassword = new System.Windows.Forms.Label();
            this.labelServer = new System.Windows.Forms.Label();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer5 = new System.Windows.Forms.SplitContainer();
            this.splitContainer7 = new System.Windows.Forms.SplitContainer();
            this.btnCancel = new System.Windows.Forms.Button();
            this.fileTransfertBar = new System.Windows.Forms.ProgressBar();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.btnDeleteLocal = new System.Windows.Forms.Button();
            this.logWindow = new FTP_Client.InfoEntities.LogFTPWindow(this.components);
            this.localTreeView = new FTP_Client.LocalEntities.LocalTreeView();
            this.serverTreeView = new FTP_Client.ServerEntities.ServerTreeView(this.components);
            this.localListView = new FTP_Client.LocalEntities.LocalListView();
            this.fileNameLocal = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.fileSizeLocal = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.fileTypeLocal = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lastModifiedLocal = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.serverListView = new FTP_Client.ServerEntities.ServerListView();
            this.fileNameServer = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.fileSizeServer = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.fileTypeServer = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lastModifiedServer = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.fileRightsServer = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.fileOwnerServer = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.fileGroupServer = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.fileQueue = new FTP_Client.InfoEntities.FileQueue();
            this.serverFileLocation = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.direction = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.distFile = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.fileSizeTransfert = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.time = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer6)).BeginInit();
            this.splitContainer6.Panel1.SuspendLayout();
            this.splitContainer6.Panel2.SuspendLayout();
            this.splitContainer6.SuspendLayout();
            this.panel1.SuspendLayout();
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
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer7)).BeginInit();
            this.splitContainer7.Panel1.SuspendLayout();
            this.splitContainer7.Panel2.SuspendLayout();
            this.splitContainer7.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer6);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel2.Controls.Add(this.splitter1);
            this.splitContainer1.Size = new System.Drawing.Size(934, 753);
            this.splitContainer1.SplitterDistance = 110;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer6
            // 
            this.splitContainer6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer6.Location = new System.Drawing.Point(0, 0);
            this.splitContainer6.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer6.Name = "splitContainer6";
            this.splitContainer6.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer6.Panel1
            // 
            this.splitContainer6.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer6.Panel2
            // 
            this.splitContainer6.Panel2.Controls.Add(this.logWindow);
            this.splitContainer6.Size = new System.Drawing.Size(934, 110);
            this.splitContainer6.SplitterDistance = 38;
            this.splitContainer6.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Window;
            this.panel1.Controls.Add(this.btnDeleteLocal);
            this.panel1.Controls.Add(this.btnDeleteServer);
            this.panel1.Controls.Add(this.btnConnection);
            this.panel1.Controls.Add(this.txtPort);
            this.panel1.Controls.Add(this.labelPort);
            this.panel1.Controls.Add(this.txtUserName);
            this.panel1.Controls.Add(this.txtPassword);
            this.panel1.Controls.Add(this.labelUserName);
            this.panel1.Controls.Add(this.txtServer);
            this.panel1.Controls.Add(this.labelPassword);
            this.panel1.Controls.Add(this.labelServer);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(934, 38);
            this.panel1.TabIndex = 0;
            // 
            // btnDeleteServer
            // 
            this.btnDeleteServer.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnDeleteServer.Image = global::FTP_Client.Properties.Resources.file_delete;
            this.btnDeleteServer.Location = new System.Drawing.Point(874, 6);
            this.btnDeleteServer.Name = "btnDeleteServer";
            this.btnDeleteServer.Size = new System.Drawing.Size(48, 31);
            this.btnDeleteServer.TabIndex = 9;
            this.btnDeleteServer.UseVisualStyleBackColor = false;
            this.btnDeleteServer.Click += new System.EventHandler(this.btnDeleteServer_Click);
            // 
            // btnConnection
            // 
            this.btnConnection.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnConnection.Image = global::FTP_Client.Properties.Resources.connexion;
            this.btnConnection.Location = new System.Drawing.Point(824, 6);
            this.btnConnection.Margin = new System.Windows.Forms.Padding(4);
            this.btnConnection.Name = "btnConnection";
            this.btnConnection.Size = new System.Drawing.Size(43, 31);
            this.btnConnection.TabIndex = 8;
            this.btnConnection.UseVisualStyleBackColor = false;
            this.btnConnection.Click += new System.EventHandler(this.btnConnection_Click);
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(736, 7);
            this.txtPort.Margin = new System.Windows.Forms.Padding(4);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(80, 27);
            this.txtPort.TabIndex = 4;
            this.txtPort.Text = "1200";
            // 
            // labelPort
            // 
            this.labelPort.AutoSize = true;
            this.labelPort.Location = new System.Drawing.Point(688, 9);
            this.labelPort.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPort.Name = "labelPort";
            this.labelPort.Size = new System.Drawing.Size(40, 17);
            this.labelPort.TabIndex = 5;
            this.labelPort.Text = "Port:";
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(391, 7);
            this.txtUserName.Margin = new System.Windows.Forms.Padding(4);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(100, 27);
            this.txtUserName.TabIndex = 3;
            this.txtUserName.Text = "kimSavaroche";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(580, 7);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(4);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(100, 27);
            this.txtPassword.TabIndex = 7;
            this.txtPassword.Text = "kimy";
            // 
            // labelUserName
            // 
            this.labelUserName.AutoSize = true;
            this.labelUserName.Location = new System.Drawing.Point(297, 6);
            this.labelUserName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelUserName.Name = "labelUserName";
            this.labelUserName.Size = new System.Drawing.Size(86, 17);
            this.labelUserName.TabIndex = 2;
            this.labelUserName.Text = "User Name:";
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(153, 6);
            this.txtServer.Margin = new System.Windows.Forms.Padding(4);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(136, 27);
            this.txtServer.TabIndex = 1;
            this.txtServer.Text = "192.168.1.18";
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(499, 9);
            this.labelPassword.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(73, 17);
            this.labelPassword.TabIndex = 6;
            this.labelPassword.Text = "Password";
            // 
            // labelServer
            // 
            this.labelServer.AutoSize = true;
            this.labelServer.Location = new System.Drawing.Point(90, 9);
            this.labelServer.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelServer.Name = "labelServer";
            this.labelServer.Size = new System.Drawing.Size(55, 17);
            this.labelServer.TabIndex = 0;
            this.labelServer.Text = "Server:";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(4, 0);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer3);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer7);
            this.splitContainer2.Size = new System.Drawing.Size(930, 639);
            this.splitContainer2.SplitterDistance = 413;
            this.splitContainer2.TabIndex = 1;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.splitContainer4);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.splitContainer5);
            this.splitContainer3.Size = new System.Drawing.Size(930, 413);
            this.splitContainer3.SplitterDistance = 262;
            this.splitContainer3.TabIndex = 0;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer4.Name = "splitContainer4";
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.localTreeView);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.serverTreeView);
            this.splitContainer4.Size = new System.Drawing.Size(930, 262);
            this.splitContainer4.SplitterDistance = 431;
            this.splitContainer4.TabIndex = 0;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "hard_disk.png");
            this.imageList1.Images.SetKeyName(1, "folder.png");
            this.imageList1.Images.SetKeyName(2, "text_file.png");
            // 
            // splitContainer5
            // 
            this.splitContainer5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer5.Location = new System.Drawing.Point(0, 0);
            this.splitContainer5.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer5.Name = "splitContainer5";
            // 
            // splitContainer5.Panel1
            // 
            this.splitContainer5.Panel1.Controls.Add(this.localListView);
            // 
            // splitContainer5.Panel2
            // 
            this.splitContainer5.Panel2.Controls.Add(this.serverListView);
            this.splitContainer5.Size = new System.Drawing.Size(930, 147);
            this.splitContainer5.SplitterDistance = 431;
            this.splitContainer5.TabIndex = 0;
            // 
            // splitContainer7
            // 
            this.splitContainer7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer7.Location = new System.Drawing.Point(0, 0);
            this.splitContainer7.Name = "splitContainer7";
            this.splitContainer7.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer7.Panel1
            // 
            this.splitContainer7.Panel1.Controls.Add(this.btnCancel);
            this.splitContainer7.Panel1.Controls.Add(this.fileTransfertBar);
            // 
            // splitContainer7.Panel2
            // 
            this.splitContainer7.Panel2.Controls.Add(this.fileQueue);
            this.splitContainer7.Size = new System.Drawing.Size(930, 222);
            this.splitContainer7.SplitterDistance = 28;
            this.splitContainer7.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(792, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // fileTransfertBar
            // 
            this.fileTransfertBar.Location = new System.Drawing.Point(8, 3);
            this.fileTransfertBar.Name = "fileTransfertBar";
            this.fileTransfertBar.Size = new System.Drawing.Size(760, 23);
            this.fileTransfertBar.TabIndex = 0;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Margin = new System.Windows.Forms.Padding(4);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(4, 639);
            this.splitter1.TabIndex = 0;
            this.splitter1.TabStop = false;
            // 
            // btnDeleteLocal
            // 
            this.btnDeleteLocal.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnDeleteLocal.Image = global::FTP_Client.Properties.Resources.delete;
            this.btnDeleteLocal.Location = new System.Drawing.Point(12, 4);
            this.btnDeleteLocal.Name = "btnDeleteLocal";
            this.btnDeleteLocal.Size = new System.Drawing.Size(45, 31);
            this.btnDeleteLocal.TabIndex = 10;
            this.btnDeleteLocal.UseVisualStyleBackColor = false;
            this.btnDeleteLocal.Click += new System.EventHandler(this.btnDeleteLocal_Click);
            // 
            // logWindow
            // 
            this.logWindow.BackColor = System.Drawing.SystemColors.Info;
            this.logWindow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logWindow.Location = new System.Drawing.Point(0, 0);
            this.logWindow.Margin = new System.Windows.Forms.Padding(4);
            this.logWindow.Name = "logWindow";
            this.logWindow.ReadOnly = true;
            this.logWindow.Size = new System.Drawing.Size(934, 68);
            this.logWindow.TabIndex = 0;
            this.logWindow.Text = "";
            // 
            // localTreeView
            // 
            this.localTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.localTreeView.ImageIndex = 0;
            this.localTreeView.ImageList = this.imageList1;
            this.localTreeView.LabelEdit = true;
            this.localTreeView.Location = new System.Drawing.Point(0, 0);
            this.localTreeView.Margin = new System.Windows.Forms.Padding(4);
            this.localTreeView.Name = "localTreeView";
            this.localTreeView.SelectedImageIndex = 0;
            this.localTreeView.Size = new System.Drawing.Size(431, 262);
            this.localTreeView.TabIndex = 0;
            this.localTreeView.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.localTreeView_AfterLabelEdit);
            this.localTreeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.localTreeView_NodeMouseDoubleClick);
            // 
            // serverTreeView
            // 
            this.serverTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.serverTreeView.ImageIndex = 0;
            this.serverTreeView.ImageList = this.imageList1;
            this.serverTreeView.LabelEdit = true;
            this.serverTreeView.Location = new System.Drawing.Point(0, 0);
            this.serverTreeView.Margin = new System.Windows.Forms.Padding(4);
            this.serverTreeView.Name = "serverTreeView";
            this.serverTreeView.SelectedImageIndex = 0;
            this.serverTreeView.Size = new System.Drawing.Size(495, 262);
            this.serverTreeView.TabIndex = 0;
            this.serverTreeView.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.serverTreeView_AfterLabelEdit);
            this.serverTreeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.serverTreeView_NodeMouseDoubleClick);
            // 
            // localListView
            // 
            this.localListView.AllowDrop = true;
            this.localListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.fileNameLocal,
            this.fileSizeLocal,
            this.fileTypeLocal,
            this.lastModifiedLocal});
            this.localListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.localListView.Location = new System.Drawing.Point(0, 0);
            this.localListView.Name = "localListView";
            this.localListView.Size = new System.Drawing.Size(431, 147);
            this.localListView.SmallImageList = this.imageList1;
            this.localListView.TabIndex = 0;
            this.localListView.UseCompatibleStateImageBehavior = false;
            this.localListView.View = System.Windows.Forms.View.Details;
            this.localListView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.localListView_ItemDrag);
            this.localListView.DragDrop += new System.Windows.Forms.DragEventHandler(this.localListView_DragDrop);
            this.localListView.DragEnter += new System.Windows.Forms.DragEventHandler(this.localListView_DragEnter);
            this.localListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.localListView_MouseDoubleClick);
            // 
            // fileNameLocal
            // 
            this.fileNameLocal.Text = "Name";
            this.fileNameLocal.Width = 154;
            // 
            // fileSizeLocal
            // 
            this.fileSizeLocal.Text = "Size";
            // 
            // fileTypeLocal
            // 
            this.fileTypeLocal.Text = "Type";
            // 
            // lastModifiedLocal
            // 
            this.lastModifiedLocal.Text = "Last Modified";
            this.lastModifiedLocal.Width = 129;
            // 
            // serverListView
            // 
            this.serverListView.AllowDrop = true;
            this.serverListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.fileNameServer,
            this.fileSizeServer,
            this.fileTypeServer,
            this.lastModifiedServer,
            this.fileRightsServer,
            this.fileOwnerServer,
            this.fileGroupServer});
            this.serverListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.serverListView.Location = new System.Drawing.Point(0, 0);
            this.serverListView.Name = "serverListView";
            this.serverListView.Size = new System.Drawing.Size(495, 147);
            this.serverListView.SmallImageList = this.imageList1;
            this.serverListView.TabIndex = 0;
            this.serverListView.UseCompatibleStateImageBehavior = false;
            this.serverListView.View = System.Windows.Forms.View.Details;
            this.serverListView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.serverListView_ItemDrag);
            this.serverListView.DragDrop += new System.Windows.Forms.DragEventHandler(this.serverListView_DragDrop);
            this.serverListView.DragEnter += new System.Windows.Forms.DragEventHandler(this.serverListView_DragEnter);
            this.serverListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.serverListView_MouseDoubleClick);
            // 
            // fileNameServer
            // 
            this.fileNameServer.Text = "Name";
            this.fileNameServer.Width = 175;
            // 
            // fileSizeServer
            // 
            this.fileSizeServer.Text = "Size";
            // 
            // fileTypeServer
            // 
            this.fileTypeServer.Text = "Type";
            // 
            // lastModifiedServer
            // 
            this.lastModifiedServer.Text = "Last Modified";
            // 
            // fileRightsServer
            // 
            this.fileRightsServer.Text = "Rights";
            // 
            // fileOwnerServer
            // 
            this.fileOwnerServer.Text = "Owner";
            // 
            // fileGroupServer
            // 
            this.fileGroupServer.Text = "Group";
            // 
            // fileQueue
            // 
            this.fileQueue.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.serverFileLocation,
            this.direction,
            this.distFile,
            this.fileSizeTransfert,
            this.time});
            this.fileQueue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileQueue.Location = new System.Drawing.Point(0, 0);
            this.fileQueue.Name = "fileQueue";
            this.fileQueue.Size = new System.Drawing.Size(930, 190);
            this.fileQueue.TabIndex = 0;
            this.fileQueue.UseCompatibleStateImageBehavior = false;
            this.fileQueue.View = System.Windows.Forms.View.Details;
            // 
            // serverFileLocation
            // 
            this.serverFileLocation.Text = "Server / File Location";
            this.serverFileLocation.Width = 326;
            // 
            // direction
            // 
            this.direction.Text = "Direction";
            this.direction.Width = 81;
            // 
            // distFile
            // 
            this.distFile.Text = "Dist File";
            this.distFile.Width = 342;
            // 
            // fileSizeTransfert
            // 
            this.fileSizeTransfert.Text = "Size";
            // 
            // time
            // 
            this.time.Text = "Time Left (sec)";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(934, 753);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Lucida Sans Unicode", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FTP Client";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer6.Panel1.ResumeLayout(false);
            this.splitContainer6.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer6)).EndInit();
            this.splitContainer6.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
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
            this.splitContainer7.Panel1.ResumeLayout(false);
            this.splitContainer7.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer7)).EndInit();
            this.splitContainer7.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelServer;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.Label labelUserName;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label labelPort;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnConnection;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.SplitContainer splitContainer5;
        private ServerTreeView serverTreeView;
        private System.Windows.Forms.SplitContainer splitContainer6;
        private LogFTPWindow logWindow;
        private LocalEntities.LocalTreeView localTreeView;
        private System.Windows.Forms.ImageList imageList1;
        private ServerListView serverListView;
        private System.Windows.Forms.ColumnHeader fileNameServer;
        private System.Windows.Forms.ColumnHeader fileSizeServer;
        private System.Windows.Forms.ColumnHeader fileTypeServer;
        private System.Windows.Forms.ColumnHeader lastModifiedServer;
        private System.Windows.Forms.ColumnHeader fileRightsServer;
        private System.Windows.Forms.ColumnHeader fileOwnerServer;
        private System.Windows.Forms.ColumnHeader fileGroupServer;
        private LocalEntities.LocalListView localListView;
        private System.Windows.Forms.ColumnHeader fileNameLocal;
        private System.Windows.Forms.ColumnHeader fileSizeLocal;
        private System.Windows.Forms.ColumnHeader fileTypeLocal;
        private System.Windows.Forms.ColumnHeader lastModifiedLocal;
        private System.Windows.Forms.SplitContainer splitContainer7;
        private FileQueue fileQueue;
        private System.Windows.Forms.ColumnHeader serverFileLocation;
        private System.Windows.Forms.ColumnHeader direction;
        private System.Windows.Forms.ColumnHeader distFile;
        private System.Windows.Forms.ColumnHeader fileSizeTransfert;
        private System.Windows.Forms.ColumnHeader time;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ProgressBar fileTransfertBar;
        private System.Windows.Forms.Button btnDeleteServer;
        private System.Windows.Forms.Button btnDeleteLocal;
    }
}

