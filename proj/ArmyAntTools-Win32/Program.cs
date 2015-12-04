using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmyAnt
{
    class Program
    {
        [STAThreadAttribute]
        public static void Main()
        {
            new MainForm().ShowDialog(); 
        }
    }
}
