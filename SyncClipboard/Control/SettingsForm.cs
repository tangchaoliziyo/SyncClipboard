﻿using SyncClipboard.Module;

using System;
using System.Windows.Forms;

namespace SyncClipboard
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            this.textBox6.KeyPress += OnlyNum;
            this.textBox7.KeyPress += OnlyNum;
            this.textBox8.KeyPress += OnlyNum;
        }

        private void OnlyNum(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b' && !Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            this.textBox1.Text = UserConfig.Config.SyncService.RemoteURL;
            this.textBox2.Text = UserConfig.Config.SyncService.UserName;
            this.textBox3.Text = UserConfig.Config.SyncService.Password;
            this.textBox6.Text = UserConfig.Config.Program.RetryTimes.ToString();
            this.textBox7.Text = (UserConfig.Config.Program.TimeOut / 1000).ToString();
            this.textBox8.Text = (UserConfig.Config.Program.IntervalTime / 1000).ToString();

            #region 同步选项
            foreach (var item in UserConfig.Config.SyncType.GetType().GetProperties())
            {
                checkedListBox1.Items.Add(item.Name, (bool)item.GetValue(UserConfig.Config.SyncType));
            }
            #endregion
        }

        private void OKButtenClicked(object sender, EventArgs e)
        {
            SaveConfig();
            this.Close();
        }

        private void CancelButtonClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ApplicationButtonClicked(object sender, EventArgs e)
        {
            SaveConfig();
        }
        private void SaveConfig()
        {
            UserConfig.Config.SyncService.RemoteURL = this.textBox1.Text;
            UserConfig.Config.SyncService.UserName = this.textBox2.Text;
            UserConfig.Config.SyncService.Password = this.textBox3.Text;

            if (this.textBox8.Text != "")
                UserConfig.Config.Program.IntervalTime = Convert.ToInt32(this.textBox8.Text) * 1000;
            if (this.textBox7.Text != "")
                UserConfig.Config.Program.TimeOut = Convert.ToInt32(this.textBox7.Text) * 1000;
            if (this.textBox6.Text != "")
                UserConfig.Config.Program.RetryTimes = Convert.ToInt32(this.textBox6.Text);

            #region 同步选项
            foreach (var item in checkedListBox1.Items)
            {
                UserConfig.Config.SyncType.GetType().GetProperty(item.ToString()).SetValue(UserConfig.Config.SyncType, checkedListBox1.GetItemChecked(checkedListBox1.Items.IndexOf(item)));
            }
            #endregion
            UserConfig.Save();
        }
    }
}
