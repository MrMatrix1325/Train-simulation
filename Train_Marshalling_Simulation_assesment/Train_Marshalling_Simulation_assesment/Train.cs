using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace Train_Marshalling_Simulation_assesment
{
    public class Train
    {
        /// <summary>
        /// The class which define the train and his stops.
        /// </summary>
        #region variables
        private int cars;
        private int carts;
        private string name;
        private Queue<int> stops;
        private Panel[] panelcars = new Panel[6];
        private int speed;
        private int number;
        #endregion

        #region Constructor
        public Train(int _cars , string _name, int _number , Queue<int> _stops, int velocity)
        {
            this.cars = _cars;
            this.carts = 0;
            this.name = _name;
            this.number = _number;
            this.stops = _stops;
            this.speed = velocity;
            Panel[] panelscars = new Panel[5];
        }
        #endregion

        #region getters/setters
        /// <summary>
        /// Get the next destination of the train
        /// </summary>
        public int get_dest
        {
            get { return stops.Dequeue(); }
        }

        /// <summary>
        /// Get the queue.
        /// </summary>
        public Queue<int> get_queue
        {
            get { return stops; }
        }

        /// <summary>
        /// Get the number of cars on the train.
        /// </summary>
        public int get_carts
        {
            get { return carts; }
        }

        /// <summary>
        /// Add the "value" to the number of cars.
        /// </summary>
        public int add_carts
        {
            set { carts += value; }
        }

        /// <summary>
        /// Add the panel to the cars of the train.
        /// </summary>
        public Panel add_cars
        {
            set { panelcars[carts] = value; }
        }

        /// <summary>
        /// Get the cars of the train
        /// </summary>
        public Panel[] get_cars
        {
            get {return panelcars; }
        }

        public int get_speed
        {
            get { return speed; }
        }
        #endregion
    }
}
