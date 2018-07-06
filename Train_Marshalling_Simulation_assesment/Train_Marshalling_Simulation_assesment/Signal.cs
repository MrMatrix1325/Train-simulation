using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace Train_Marshalling_Simulation_assesment
{
    public class Signal
    {
        private PictureBox pb;
        private int signal; /* 0 = close , 1 = semaphore , 2 = yellow , 3 = yellow + speed low , 4 = speed low , 5 = open , 6 = purple , 7 = white*/

        public Signal(PictureBox _pb , int signal)
        {
            this.pb = _pb;
            this.signal = signal;
        }

        public PictureBox get_pb
        {
            get { return pb; }
        }

        public int get_signal
        {
            get { return signal; }
        }

        public int set_signal
        {
            set { signal = value; }
        }
    }
}
