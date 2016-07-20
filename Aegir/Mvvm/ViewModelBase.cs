﻿using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyMessenger;

namespace Aegir.Mvvm
{
    public class ViewModelBase : ObservableObject
    {
        private ITinyMessengerHub messengerHub;

        public ITinyMessengerHub Messenger
        {
            get
            {
                if(messengerHub==null)
                {
                    messengerHub = 
                        SimpleIoc.Default.GetInstance<ITinyMessengerHub>();
                }
                return messengerHub;
            }
            set
            {
                messengerHub = value;
            }
        }

        public ViewModelBase()
        {
            
        }
    }
}
