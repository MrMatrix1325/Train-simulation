using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Train_Marshalling_Simulation_assesment
{
    public class Semaphore
    {
        private int count = 1; 

        public void Wait()
        {
            lock (this)
            {
                while (count == 0)
                    Monitor.Wait(this);
                count = 0;
            }
        }

        public void Signal()
        {
            lock (this)
            {
                count = 1;
                Monitor.Pulse(this);
            }
        }

        public void Start()
        {

        }

    }
}
