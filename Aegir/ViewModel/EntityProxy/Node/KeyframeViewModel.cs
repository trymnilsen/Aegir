﻿using Aegir.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.ViewModel.EntityProxy.Node
{
    public class KeyframeViewModel : ViewModelBase, ISceneNode
    {
        private int v { get; set; }

        public KeyframeViewModel(int v)
        {
            this.v = v;
        }

        public List<ISceneNode> Children
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Name
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsEnabled
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
