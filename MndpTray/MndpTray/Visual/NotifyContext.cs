﻿using MndpTray.Protocol;
using System;
using System.Threading;
using System.Windows.Forms;

namespace MndpTray
{
    public class NotifyContext : ApplicationContext
    {
        private readonly ListForm _listForm;
        private readonly AboutBox _aboutBox;
        private NotifyIcon _notifyIcon;

        public NotifyContext()
        {
        
            this.InizializeComponets();
            this._listForm = new ListForm();
            this._aboutBox = new AboutBox();
            MndpLog.SetInfoAction(Program.Log);
            MndpListener.Instance.Start();
            MndpSender.Instance.Start(MndpHostInfo.Instance);
        }

        #region Event Handlers

        private void Exit_Click(object sender, EventArgs e)
        {
            MndpListener.Instance.Stop();
            MndpSender.Instance.Stop();
            this._notifyIcon.Dispose();
            this._listForm.Close();

            Thread.Sleep(100);
            Application.Exit();
        }

        private void List_Click(object sender, EventArgs e)
        {
            if (!this._listForm.Visible)            
                this._listForm.ShowDialog();            
            else            
                this._listForm.WindowState = FormWindowState.Normal;

            this._listForm.BringToFront();
            
        }

        private void Send_Click(object sender, System.EventArgs e)
        {
            MndpSender.Instance.SendHostInfoNow();
        }

        private void About_Click(object sender, System.EventArgs e)
        {
            if (!this._aboutBox.Visible)
                this._aboutBox.ShowDialog();
            else
                this._aboutBox.WindowState = FormWindowState.Normal;

            this._aboutBox.BringToFront();
        }

        #endregion Event Handlers

        #region Init

        private void InizializeComponets()
        {
            var notifyIcon = new NotifyIcon();
            notifyIcon.Icon = MndpTray.Properties.Resources.favicon_ico;
            notifyIcon.Text = nameof(MndpTray);
            notifyIcon.Visible = true;

            var contextMenuStrip = new ContextMenuStrip();

            var listMenuStrip = new ToolStripMenuItem();
            listMenuStrip.Text = "List";
            listMenuStrip.Click += this.List_Click;
            contextMenuStrip.Items.Add(listMenuStrip);

            contextMenuStrip.Items.Add(new ToolStripSeparator());

            var sendMenuStrip = new ToolStripMenuItem();
            sendMenuStrip.Text = "Send";
            sendMenuStrip.Click += this.Send_Click;
            contextMenuStrip.Items.Add(sendMenuStrip);

            contextMenuStrip.Items.Add(new ToolStripSeparator());

            var aboutMenuStrip = new ToolStripMenuItem();
            aboutMenuStrip.Text = "About";
            aboutMenuStrip.Click += this.About_Click;
            contextMenuStrip.Items.Add(aboutMenuStrip);

            contextMenuStrip.Items.Add(new ToolStripSeparator());

            var exitMenuStrip = new ToolStripMenuItem();
            exitMenuStrip.Text = "Exit";
            exitMenuStrip.Click += this.Exit_Click;
            contextMenuStrip.Items.Add(exitMenuStrip);

            notifyIcon.ContextMenuStrip = contextMenuStrip;

            this._notifyIcon = notifyIcon;
        }

        #endregion Init
    }
}