using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SteppedMotor.RedPlate
{

    public class PortAccess
    {
        /* For sending to the ports */
        [DllImport("inpout32.dll", EntryPoint = "Out32")]
        public static extern void Output(short adress, short value);
        /* For receiving from the ports */
        [DllImport("inpout32.dll", EntryPoint = "Inp32")]
        public static extern int Input(short adress);
    }
}
