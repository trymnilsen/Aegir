using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirLib.Component
{
    public class Component
    {
        [Category("Switches and Bitches")]
        public bool ThisIsBool { get; set; }
        [Category("Floating And Gloating")]
        public float ThisIsFloat { get; set; }
        [Category("Strings and Wings")]
        [Description("Some Profound Description")]
        public string StringGoesHere { get; set; }
        public Component()
        {
            this.StringGoesHere = "Foobar";
        }
    }
}
