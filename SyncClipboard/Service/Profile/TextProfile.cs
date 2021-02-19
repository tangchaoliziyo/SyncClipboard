﻿using System;
using static SyncClipboard.ProfileFactory;

namespace SyncClipboard.Service
{
    class TextProfile : Profile
    {
        public TextProfile(String text)
        {
            Text = text;
        }

        protected override ClipboardType GetProfileType()
        {
            return ClipboardType.Text;
        }

        public override bool Equals(Object obj)
        {
            if (obj is null)
            {
                return false;
            }

            try
            {
                TextProfile textprofile = (TextProfile)obj;
                return this.Text == textprofile.Text;
            }

            catch
            {
                return false;
            }            
        }

        public override int GetHashCode()
        {
            return this.Text.GetHashCode();
        }

        public override void UploadProfile()
        {
            HttpWebResponseUtility.PutText(Config.GetProfileUrl(), this.ToJsonString(), Config.TimeOut, Config.GetHttpAuthHeader());
        }

        public override void SetLocalClipboard()
        {
            LocalClipboardLocker.Lock();
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    System.Windows.Forms.Clipboard.SetData(System.Windows.Forms.DataFormats.Text, this.Text);
                    break;
                }
                catch
                {
                    System.Threading.Thread.Sleep(500);
                }
            }

            LocalClipboardLocker.Unlock();
        }

    }
}