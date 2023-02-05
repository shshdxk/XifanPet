using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Iplugin
{
    public abstract class PetPlug : IPetPlug
    {
        public abstract void Close();
        public abstract Menu[] GetMenu();
        public abstract void Initialization();
        public abstract void OpenPlug();

        public virtual void MouseRecover()
        {

        }
        public virtual void MouseThrough()
        {

        }

    }
}
