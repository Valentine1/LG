using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LG.Models;

namespace LG.ViewModels
{
    public class TopicVM : AssetVM
    {
        public event AvatarVMChanged OnAvatarVMChanged;

        public Topic TopicM
        {
            get
            {
                return (Topic)this.BaseBlockM;
            }
        }
        public LicenseVM LicInfoVModel
        {
            get;
            set;
        }

        private ObservableCollection<PictureBoxVM> _pictures;
        public ObservableCollection<PictureBoxVM> Pictures
        {
            get
            {
                if (_pictures == null)
                {
                    _pictures = new ObservableCollection<PictureBoxVM>();
                }
                return _pictures;
            }
            set
            {
                _pictures = value;
            }
        }

        private ObservableCollection<LetterVM> _nameInLetters;
        public ObservableCollection<LetterVM> NameInLetters
        {
            get
            {
                if (_nameInLetters == null)
                {
                    _nameInLetters = new ObservableCollection<LetterVM>();
                }
                return _nameInLetters;
            }
            set
            {
                _nameInLetters = value;
            }
        }

        public string PathData { get; set; }

        private AvatarVM _levelAvaVM;
        public AvatarVM LevelAvaVM
        {
            get
            {
                return _levelAvaVM;
            }
            set
            {
                _levelAvaVM = value;
                if (this.OnAvatarVMChanged != null)
                {
                    this.OnAvatarVMChanged(_levelAvaVM);
                }
            }
        }
     
        public TopicVM(Topic top) : base(top)
        {
            this.PathData = top.PathData;
            this.LevelAvaVM = new AvatarVM(top.HierarchyLevel);
            this.LicInfoVModel = new LicenseVM(top.LicenseInfo);
            foreach (Letter let in top.NameInLetters)
            {
                LetterVM lvm = new LetterVM(let);
                this.NameInLetters.Add(lvm);
            }

            foreach (PictureBoxM pic in top.Pictures)
            {
                PictureBoxVM pvm = new PictureBoxVM(pic);
                this.Pictures.Add(pvm);
            }
            this.TopicM.OnAvatarChanged += TopicM_OnAvatarChanged;
            this.TopicM.OnLicenseInfoChanged += TopicM_OnLicenseInfoChanged;
        }

        private void TopicM_OnAvatarChanged(Avatar ava)
        {
            this.LevelAvaVM = new AvatarVM(ava);
        }
        private void TopicM_OnLicenseInfoChanged(Data.License lic)
        {
            this.LicInfoVModel = new LicenseVM(lic);
        }
        public override void DetachEvents(Unit m)
        {
            base.DetachEvents(m);
            (m as Topic).OnAvatarChanged -= TopicM_OnAvatarChanged;
        }
        public override void DeleteItself(Unit m)
        {
            base.DeleteItself(m);
            this.Pictures.Clear();
            this.NameInLetters.Clear();
        }
    }
    public delegate void AvatarVMChanged(AvatarVM ava);
}
