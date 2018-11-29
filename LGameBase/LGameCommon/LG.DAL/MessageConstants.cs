using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LG.Data;

namespace LG.Data
{
    public class MessageConstants
    {
        private Language Lang { get; set; }

        public MessageConstants(Language lang)
        {
            Lang = lang;
        }

        public string Checking_Data_Integrity
        {
            get
            {
                switch (this.Lang.ID)
                {
                    case 1:
                        return "Copying data to local storage and downloading new available resources if necessary. Internet connection may be vital.";
                }
                return "";
            }
        }
        public string Screen_IsNot_Good
        {
            get
            {
                switch (this.Lang.ID)
                {
                    case 1:
                        return "Screen resolution or orientation is not appropriate for the game. Should be at least 1021x768, landscape orientation.";
                }
                return "";
            }
        }
        public string Checking_New_Themes
        {
            get
            {
                switch (this.Lang.ID)
                {
                    case 1:
                        return "Checking new themes being available. Internet connection is vital.";
                }
                return "";
            }
        }
        public string Register_New_Account
        {
            get
            {
                switch (this.Lang.ID)
                {
                    case 1:
                        return "Registering new account. Internet connection may be vital.";
                }
                return "";
            }
        }
        public string Update_Account
        {
            get
            {
                switch (this.Lang.ID)
                {
                    case 1:
                        return "Updating account. Internet connection may be vital.";
                }
                return "";
            }
        }
        public string Delete_Account
        {
            get
            {
                switch (this.Lang.ID)
                {
                    case 1:
                        return "Deleting account. Internet connection may be vital.";
                }
                return "";
            }
        }
        public string Error
        {
            get
            {
                switch (this.Lang.ID)
                {
                    case 1:
                        return "Error";
                }
                return "";
            }
        }

        public string Buying
        {
            get
            {
                switch (this.Lang.ID)
                {
                    case 1:
                        return "Buying application or new themes. Resources download from server or cloud storage. Internet connection is vital.";
                }
                return "";
            }
        }
    }
}
