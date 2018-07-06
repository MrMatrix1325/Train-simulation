using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace Train_Marshalling_Simulation_assesment
{
    public class Circuit
    {
        /// <summary>
        /// The thread which control the two circuits.
        /// </summary>

        #region Variables
        private Panel motorcycle;
        private bool roadblue;
        private Train train;
        private bool execution = false;
        private Panel[] blue;
        private Panel[] black;
        private Tuple<Panel, int>[,] cars;
        private int dest;
        private bool ok;
        private bool one = true;
        private bool youcan = true;
        private Signal[] signals;
        private Semaphore semaphore;
        private Panel[] carscoupling;
        private Panel other;
        private bool isswitch;
        private bool go;
        private int speed;

        delegate void Setbringtofront(Panel pan);
        delegate void SetSendToBack(Panel pan);
        delegate void SafePan(Panel pan, bool visible);
        delegate void modify(Panel pan, Point point);
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor of class "Circuit"
        /// </summary>

        public Circuit(Panel _current, bool road, Train train, Panel[] blues, Panel[] blacks, Tuple<Panel, int>[,] _cars, Signal[] _signals, Semaphore _semaphore, Panel _other)
        {
            this.motorcycle = _current;
            this.roadblue = road;
            this.train = train;
            this.blue = blues;
            this.black = blacks;
            this.ok = true;
            this.cars = _cars;
            this.signals = _signals;
            this.semaphore = _semaphore;
            this.carscoupling = new Panel[6];
            this.isswitch = false;
            this.other = _other;
            this.go = false;
            this.speed = train.get_speed;
        }

        #endregion

        #region Methods
        ///<summary>
        ///Start the thread.
        ///</summary>
        public void start()
        {
            execution = true;

            motorcycle.BackColor = Color.Pink;
            if (roadblue)
                blue_road();
            else
                black_road();

            execution = false;
        }

        ///<summary>
        ///Move the point on the blue circuit.
        ///</summary>
        private void blue_road()
        {
            int j = 0;
            if (!isswitch)
            {
                dest = train.get_dest;
                if (dest == 1)
                {
                    signals[0].get_pb.Image = Properties.Resources.Yellow_carrée;
                    signals[0].set_signal = 2;
                    signals[2].get_pb.Image = Properties.Resources.f_vert;
                    signals[2].set_signal = 5;
                }
                else if (dest == 2)
                {
                    signals[0].get_pb.Image = Properties.Resources.vert_carrée;
                    signals[0].set_signal = 5;
                    signals[1].get_pb.Image = Properties.Resources.fjaune;
                    signals[1].set_signal = 2;
                }
                else
                {
                    one = false;
                    go = true;
                    signals[0].get_pb.Image = Properties.Resources.vert_carrée;
                    signals[0].set_signal = 5;
                    signals[1].get_pb.Image = Properties.Resources.f_vert;
                    signals[1].set_signal = 5;
                    signals[2].get_pb.Image = Properties.Resources.f_vert;
                    signals[2].set_signal = 5;
                    if (signals[7].get_signal == 1)
                    {
                        signals[3].get_pb.Image = Properties.Resources.Jaune_combiné;
                        signals[3].set_signal = 3;
                    }
                    else
                    {
                        signals[3].get_pb.Image = Properties.Resources.Ralentissement;
                        signals[3].set_signal = 4;
                    }

                }
                Modify(motorcycle, new Point(0, 77));

                while (motorcycle.Location.X + speed <= 172)
                {
                    if (motorcycle.Location.X >= 40)
                    {
                        Safepan(motorcycle, true);
                        SetBring(motorcycle);
                    }

                    Modify(motorcycle, new Point(motorcycle.Location.X + speed, 77));
                    System.Threading.Thread.Sleep(100);
                }
                signals[0].get_pb.Image = Properties.Resources.semaphore_carrée;
                signals[0].set_signal = 1;
            }
            while (youcan || train.get_queue.Count != 0)
            {
                if (isswitch)
                {
                    goto here;
                }
                j = 1;
                Modify(motorcycle, new Point(172, 77));
                System.Threading.Thread.Sleep(20);
                while (motorcycle.Location.X + speed <= 1140)
                {

                    if (j - 1 < train.get_carts)
                    {
                        Modify(motorcycle, new Point(motorcycle.Location.X + 20, 77));
                        Modify(train.get_cars[j - 1], new Point(172, 77));
                        for (int i = 0; i < train.get_carts - (train.get_carts - j + 1); ++i)
                            Modify(train.get_cars[i], new Point(train.get_cars[i].Location.X + 20, 77));
                        for (int i = train.get_carts - 1; i >= j; --i)
                            Modify(train.get_cars[i], new Point(172, train.get_cars[i].Location.Y - speed));
                    }
                    else {
                        Modify(motorcycle, new Point(motorcycle.Location.X + speed, 77));
                        for (int i = 0; i < train.get_carts; ++i)
                            Modify(train.get_cars[i], new Point(train.get_cars[i].Location.X + speed, 77));
                    }
                    System.Threading.Thread.Sleep(50);
                    j++;
                }
                Modify(motorcycle, new Point(1147, 77));

                j = 1;
                System.Threading.Thread.Sleep(50);
                while (motorcycle.Location.Y + speed <= 440)
                {
                    if (j - 1 < train.get_carts)
                    {
                        Modify(motorcycle, new Point(1147, motorcycle.Location.Y + 20));
                        Modify(train.get_cars[j - 1], new Point(1147, 77));
                        for (int i = 0; i < train.get_carts - (train.get_carts - j + 1); ++i)
                            Modify(train.get_cars[i], new Point(1147, train.get_cars[i].Location.Y + 20));
                        for (int i = train.get_carts - 1; i >= j; --i)
                            Modify(train.get_cars[i], new Point(train.get_cars[i].Location.X + speed, 77));
                    }
                    else
                    {
                        Modify(motorcycle, new Point(1147, motorcycle.Location.Y + speed));
                        for (int i = 0; i < train.get_carts; ++i)
                            Modify(train.get_cars[i], new Point(1147, train.get_cars[i].Location.Y + speed));
                    }
                    System.Threading.Thread.Sleep(50);
                    j++;
                }
                here:
                Modify(motorcycle, new Point(1147, 452));
                System.Threading.Thread.Sleep(20);
                SetBring(this.motorcycle);
                for (int i = 0; i < train.get_carts; ++i)
                    SetBring(this.train.get_cars[i]);
                int currentspeed = speed;
                j = 1;
                while (motorcycle.Location.X - currentspeed >= 172)
                {
 
                    if (j - 1 < train.get_carts)
                    {

                        Modify(motorcycle, new Point(motorcycle.Location.X - 20, 452));
                        Modify(train.get_cars[j - 1], new Point(1147, 452));
                        for (int i = 0; i < train.get_carts - (train.get_carts - j + 1); ++i)
                            Modify(train.get_cars[i], new Point(train.get_cars[i].Location.X - 20, 452));
                        for (int i = train.get_carts - 1; i >= j; --i)
                            Modify(train.get_cars[i], new Point(1147, train.get_cars[i].Location.Y + currentspeed)); 
                    }
                    else
                    {
                        Modify(motorcycle, new Point(motorcycle.Location.X - currentspeed, 452));
                        for (int i = 0; i < train.get_carts; ++i)
                            Modify(train.get_cars[i], new Point(train.get_cars[i].Location.X - currentspeed, 452));
                    }
                    if (motorcycle.Location.X - currentspeed <= 1050 && dest == 1 && ok)
                    {
                        ok = false;
                        Modify(motorcycle, new Point(1050 - 20 * train.get_carts, 452));
                        for (int i = 0; i < train.get_carts; ++i)
                            Modify(train.get_cars[i], new Point(1070 - 20 * (train.get_carts - i), 452));
                        signals[5].get_pb.Image = Properties.Resources.white;
                        signals[5].set_signal = 7;
                        clearlance(1);
                        signals[1].get_pb.Image = Properties.Resources.f_vert;
                        signals[1].set_signal = 5;
                        signals[3].get_pb.Image = Properties.Resources.signal_vert;
                        signals[3].set_signal = 5;
                        signals[5].get_pb.Image = Properties.Resources.purple;
                        signals[5].set_signal = 6;
                        currentspeed = speed;
                        j = train.get_carts;
                    }
                    if (motorcycle.Location.X - currentspeed <= 907 && signals[1].get_signal != 1)
                    {
                        if (signals[1].get_signal == 2)
                            currentspeed = 10;
                        signals[1].get_pb.Image = Properties.Resources.fsemaphore;
                        signals[1].set_signal = 1;
                    }

                    if (motorcycle.Location.X - speed <= 700 && signals[2].get_signal != 1)
                    {
                        signals[2].get_pb.Image = Properties.Resources.fsemaphore;
                        signals[2].set_signal = 1;
                    }
                    if (motorcycle.Location.X - speed <= 250 && signals[3].get_signal != 0)
                    {
                        signals[3].get_pb.Image = Properties.Resources.signal_carree;
                        signals[3].set_signal = 0;
                    }


                    if (motorcycle.Location.X - speed <= 887 && dest == 2 && ok)
                    {
                        ok = false;
                        Modify(motorcycle, new Point(887 - 20 * train.get_carts, 452));
                        for (int i = 0; i < train.get_carts; ++i)
                            Modify(train.get_cars[i], new Point(907 - 20 * (train.get_carts - i), 452));
                        signals[6].get_pb.Image = Properties.Resources.white;
                        signals[6].set_signal = 7;
                        clearlance(2);
                        signals[6].get_pb.Image = Properties.Resources.purple;
                        signals[6].set_signal = 6;
                        signals[2].get_pb.Image = Properties.Resources.f_vert;
                        signals[2].set_signal = 5;
                        signals[3].get_pb.Image = Properties.Resources.signal_vert;
                        signals[3].set_signal = 5;
                        currentspeed = speed;
                        j = train.get_carts;
                    }
                    System.Threading.Thread.Sleep(80);
                    ++j;
                }
                Modify(motorcycle, new Point(172, 452));
                j = 1;
                System.Threading.Thread.Sleep(20);
                signals[0].get_pb.Image = Properties.Resources.carrée;
                signals[0].set_signal = 0;
                while (motorcycle.Location.Y - speed >= 77)
                {
                    if (j - 1 < train.get_carts)
                    {
                        Modify(motorcycle, new Point(172, motorcycle.Location.Y - 20));
                        Modify(train.get_cars[j - 1], new Point(172, 452));
                        for (int i = 0; i < train.get_carts - (train.get_carts - j + 1); ++i)
                            Modify(train.get_cars[i], new Point(172, train.get_cars[i].Location.Y - 20));
                        for (int i = train.get_carts - 1; i >= j; --i)
                            Modify(train.get_cars[i], new Point(train.get_cars[i].Location.X - speed, 452));
                    }
                    else {
                        Modify(motorcycle, new Point(172, motorcycle.Location.Y - speed));
                        for (int i = 0; i < train.get_carts; ++i)
                            Modify(train.get_cars[i], new Point(172, train.get_cars[i].Location.Y - speed));
                    }
                    if (motorcycle.Location.Y - speed <= 400 && one)
                    {
                        if (train.get_queue.Count != 0)
                        {
                            go = false;
                            one = false;
                            dest = train.get_dest;
                            signals[4].get_pb.Image = Properties.Resources.Ralentissement;
                            signals[4].set_signal = 4;
                            if (dest == 1)
                            {
                                signals[2].get_pb.Image = Properties.Resources.f_vert;
                                signals[2].set_signal = 5;
                                signals[4].get_pb.Image = Properties.Resources.Jaune_combiné;
                                signals[4].set_signal = 3;
                            }
                            if (dest == 2)
                            {
                                signals[1].get_pb.Image = Properties.Resources.fjaune;
                                signals[1].set_signal = 2;
                            }
                            else if (dest == 3 || dest == 4)
                            {
                                signals[1].get_pb.Image = Properties.Resources.f_vert;
                                signals[1].set_signal = 5;
                                signals[2].get_pb.Image = Properties.Resources.f_vert;
                                signals[2].set_signal = 5;
                                if (signals[7].get_signal == 1)
                                {
                                    signals[3].get_pb.Image = Properties.Resources.Jaune_combiné;
                                    signals[3].set_signal = 3;
                                }
                                else
                                {
                                    signals[3].get_pb.Image = Properties.Resources.Ralentissement;
                                    signals[3].set_signal = 4;
                                }
                            }
                        }
                        else if (dest != 4 && dest != 3)
                        {
                            signals[4].get_pb.Image = Properties.Resources.signal_vert;
                            signals[4].set_signal = 5;
                            youcan = false;
                        }
                    }
                    if (motorcycle.Location.Y - speed <= 150 )
                    {
                        signals[4].set_signal = 0;
                        signals[4].get_pb.Image = Properties.Resources.signal_carree;
                    }
                    if (motorcycle.Location.Y <= 372 && (dest == 3 || dest == 4) && go)
                    {
                        semaphore.Signal();
                        signals[15].get_pb.Image = Properties.Resources.nfgreen;
                        signals[15].set_signal = 5;
                        switching();
                        goto end;
                    }
                    System.Threading.Thread.Sleep(50);
                    j++;
                }

                Modify(motorcycle, new Point(172, 77));
                System.Threading.Thread.Sleep(20);
                ok = true;
                go = true;
                isswitch = false;
                if (dest != 3 && dest != 4)
                    one = true;
            }
            j = 1;
            SetBring(blue[8]);
            while (motorcycle.Location.X - speed >= -100)
            {

                if (j - 1 < train.get_carts)
                {
                    Modify(motorcycle, new Point(motorcycle.Location.X - 20, 77));
                    Modify(train.get_cars[j - 1], new Point(172, 77));
                    for (int i = 0; i < train.get_carts - (train.get_carts - j + 1); ++i)
                        Modify(train.get_cars[i], new Point(train.get_cars[i].Location.X - 20, 77));
                    for (int i = train.get_carts - 1; i >= j; --i)
                        Modify(train.get_cars[i], new Point(172, train.get_cars[i].Location.Y - 20));
                }
                else
                {
                    Modify(motorcycle, new Point(motorcycle.Location.X - speed, 77));
                    for (int i = 0; i < train.get_carts; ++i)
                        Modify(train.get_cars[i], new Point(train.get_cars[i].Location.X - speed, 77));
                }
                System.Threading.Thread.Sleep(500);
                j++;
            }
            for (int i = 0; i < train.get_carts; ++i)
            {
                Modify(train.get_cars[i], new Point(30, 77));
                Safepan(train.get_cars[i], false);
            }
        end:
            SetBring(blue[8]);
            semaphore.Signal();
            signals[15].get_pb.Image = Properties.Resources.nfgreen;
            signals[15].set_signal = 5;
            freecars();
        }

        ///<summary>
        ///Move the point on the black circuit.
        ///</summary>
        private void black_road()
        {
            int j = 1;
            if (!isswitch)
            {
                dest = train.get_dest;
                if (dest == 3)
                {
                    signals[8].get_pb.Image = Properties.Resources.Yellow_carrée;
                    signals[8].set_signal = 2;
                    signals[10].get_pb.Image = Properties.Resources.f_vert;
                    signals[10].set_signal = 5;
                    signals[11].get_pb.Image = Properties.Resources.signal_vert;
                    signals[11].set_signal = 5;
                }
                else if (dest == 4)
                {
                    signals[8].get_pb.Image = Properties.Resources.vert_carrée;
                    signals[8].set_signal = 5;
                    signals[9].get_pb.Image = Properties.Resources.fjaune;
                    signals[9].set_signal = 2;
                    signals[11].get_pb.Image = Properties.Resources.signal_vert;
                    signals[11].set_signal = 5;
                }
                else
                {
                    signals[8].get_pb.Image = Properties.Resources.vert_carrée;
                    signals[8].set_signal = 5;
                    signals[9].get_pb.Image = Properties.Resources.f_vert;
                    signals[9].set_signal = 5;
                    signals[10].get_pb.Image = Properties.Resources.f_vert;
                    signals[10].set_signal = 5;
                    signals[11].get_pb.Image = Properties.Resources.Ralentissement;
                    signals[11].set_signal = 4;
                    if (signals[15].get_signal == 1)
                    {
                        signals[11].get_pb.Image = Properties.Resources.Jaune_combiné;
                        signals[11].set_signal = 3;
                    }
                }
                ok = true;
                motorcycle.BackColor = Color.Pink;
                Safepan(motorcycle, true);
                setback(this.motorcycle);
                for (int k = 0; k < 8; k++)
                    setback(this.black[k]);
                Modify(motorcycle, new Point(1300, 373));
                Safepan(motorcycle, true);
                while (motorcycle.Location.X - speed > 900)
                {
                    Modify(motorcycle, new Point(motorcycle.Location.X - speed, 373));
                    System.Threading.Thread.Sleep(100);
                }
                Modify(motorcycle, new Point(900, 373));
                signals[8].get_pb.Image = Properties.Resources.semaphore_carrée;
                signals[8].set_signal = 1;
                System.Threading.Thread.Sleep(100);

            }
            while (youcan)
            {
                if (isswitch)
                    goto here;
                Modify(motorcycle, new Point(900, 373));
                j = 1;
                while (motorcycle.Location.X - speed > 290)
                {
                    if (j - 1 < train.get_carts)
                    {
                        Modify(motorcycle, new Point(motorcycle.Location.X - 20, 373));
                        Modify(train.get_cars[j - 1], new Point(900, 373));
                        for (int i = 0; i < train.get_carts - (train.get_carts - j + 1); ++i)
                            Modify(train.get_cars[i], new Point(train.get_cars[i].Location.X - 20, 373));
                        for (int i = train.get_carts - 1; i >= j; --i)
                            Modify(train.get_cars[i], new Point(900, train.get_cars[i].Location.Y + speed));
                    }
                    else
                    {
                        Modify(motorcycle, new Point(motorcycle.Location.X - speed, 373));
                        for (int i = 0; i < train.get_carts; ++i)
                            Modify(train.get_cars[i], new Point(train.get_cars[i].Location.X - speed, 373));
                    }
                    Thread.Sleep(100);
                    if (motorcycle.Location.X - speed <= 697 && dest == 3 && ok)
                    {
                        ok = false;
                        Modify(motorcycle, new Point(697 - 20 * train.get_carts, 373));
                        signals[13].get_pb.Image = Properties.Resources.white;
                        signals[13].set_signal = 7;
                        for (int i = 0; i < train.get_carts; ++i)
                            Modify(train.get_cars[i], new Point(717 - 20 * (train.get_carts - i), 373));
                        clearlance(3);
                        signals[13].get_pb.Image = Properties.Resources.purple;
                        signals[13].set_signal = 6;
                        signals[9].get_pb.Image = Properties.Resources.f_vert;
                        signals[9].set_signal = 5;
                        j = train.get_carts;
                    }
                    if (motorcycle.Location.X - speed <= 630 && signals[9].get_signal != 1)
                    {
                        signals[9].get_pb.Image = Properties.Resources.fsemaphore;
                        signals[9].set_signal = 1;
                    }
                    if (motorcycle.Location.X - speed <= 527 && dest == 4 && ok)
                    {
                        ok = false;
                        Modify(motorcycle, new Point(527 - 20 * train.get_carts, 373));
                        for (int i = 0; i < train.get_carts; ++i)
                            Modify(train.get_cars[i], new Point(547 - 20 * (train.get_carts - i), 373));
                        signals[14].get_pb.Image = Properties.Resources.white;
                        signals[14].set_signal = 7;
                        clearlance(4);
                        signals[14].get_pb.Image = Properties.Resources.purple;
                        signals[14].set_signal = 6;
                        signals[10].get_pb.Image = Properties.Resources.f_vert;
                        signals[10].set_signal = 5;
                        j = train.get_carts;
                    }
                    if (motorcycle.Location.X - 20 <= 440 && signals[10].get_signal != 1)
                    {
                        signals[10].get_pb.Image = Properties.Resources.fsemaphore;
                        signals[10].set_signal = 1;
                    }
                    j++;
                }
                here:
                j = 1;
                Modify(motorcycle, new Point(290, 373));
                System.Threading.Thread.Sleep(100);
                while (motorcycle.Location.Y - speed > 135)
                {
                    if (j - 1 < train.get_carts)
                    {
                        Modify(motorcycle, new Point(290, motorcycle.Location.Y - 20));
                        Modify(train.get_cars[j - 1], new Point(290, 373));
                        for (int i = 0; i < train.get_carts - (train.get_carts - j + 1); ++i)
                            Modify(train.get_cars[i], new Point(290, train.get_cars[i].Location.Y - 20));
                        for (int i = train.get_carts - 1; i >= j; --i)
                            Modify(train.get_cars[i], new Point(train.get_cars[i].Location.X - speed, 373));
                    }
                    else
                    {
                        Modify(motorcycle, new Point(290, motorcycle.Location.Y - speed));
                        for (int i = 0; i < train.get_carts; ++i)
                            Modify(train.get_cars[i], new Point(290, train.get_cars[i].Location.Y - speed));
                    }
                    System.Threading.Thread.Sleep(100);
                    j++;
                }
                signals[11].get_pb.Image = Properties.Resources.signal_carree;
                signals[11].set_signal = 0;
                Modify(motorcycle, new Point(290, 133));
                System.Threading.Thread.Sleep(100);
                j = 1;
                while (motorcycle.Location.X + speed < 907)
                {
                    if (j - 1 < train.get_carts)
                    {
                        Modify(motorcycle, new Point(motorcycle.Location.X + 20, 133));
                        Modify(train.get_cars[j - 1], new Point(290, 133));
                        for (int i = 0; i < train.get_carts - (train.get_carts - j + 1); ++i)
                            Modify(train.get_cars[i], new Point(train.get_cars[i].Location.X + 20, 133));
                        for (int i = train.get_carts - 1; i >= j; --i)
                            Modify(train.get_cars[i], new Point(290, train.get_cars[i].Location.Y - speed));
                    }
                    else
                    {
                        Modify(motorcycle, new Point(motorcycle.Location.X + speed, 133));
                        for (int i = 0; i < train.get_carts; ++i)
                            Modify(train.get_cars[i], new Point(train.get_cars[i].Location.X + speed, 133));
                    }
                    System.Threading.Thread.Sleep(100);
                    j++;
                }
                if (dest == 1 || dest == 2)
                {
                    semaphore.Signal();
                    signals[7].get_pb.Image = Properties.Resources.nfgreen;
                    signals[7].set_signal = 5;
                    signals[3].get_pb.Image = Properties.Resources.Ralentissement;
                    signals[3].set_signal = 4;
                    switching();
                    goto end;
                }
                j = 1;
                if (train.get_queue.Count != 0 && !isswitch)
                {
                    ok = true;
                    dest = train.get_dest;
                }
                else if (isswitch)
                    isswitch = false;
                else
                    youcan = false;
                if (youcan || isswitch)
                {
                    switch (dest)
                    {
                        case 3:
                            signals[12].get_pb.Image = Properties.Resources.Jaune_combiné;
                            signals[12].set_signal = 3;
                            signals[10].get_pb.Image = Properties.Resources.f_vert;
                            signals[10].set_signal = 5;
                            signals[11].get_pb.Image = Properties.Resources.signal_vert;
                            signals[11].set_signal = 5;
                            break;
                        case 4:
                            signals[12].get_pb.Image = Properties.Resources.Ralentissement;
                            signals[12].set_signal = 4;
                            signals[9].get_pb.Image = Properties.Resources.fjaune;
                            signals[9].set_signal = 2;
                            signals[11].get_pb.Image = Properties.Resources.signal_vert;
                            signals[11].set_signal = 5;
                            break;
                        default:
                            signals[12].get_pb.Image = Properties.Resources.Ralentissement;
                            signals[12].set_signal = 4;
                            signals[9].get_pb.Image = Properties.Resources.f_vert;
                            signals[9].set_signal = 5;
                            signals[10].get_pb.Image = Properties.Resources.f_vert;
                            signals[10].set_signal = 5;
                            signals[11].get_pb.Image = Properties.Resources.Ralentissement;
                            signals[11].set_signal = 4;
                            if (signals[15].get_signal == 1)
                            {
                                signals[11].get_pb.Image = Properties.Resources.Jaune_combiné;
                                signals[11].set_signal = 3;
                            }
                            break;
                    }
                }
                else
                {
                    signals[12].get_pb.Image = Properties.Resources.signal_vert;
                    signals[12].set_signal = 5;
                }
                while (motorcycle.Location.Y + speed < 373)
                {
                    if (j - 1 < train.get_carts)
                    {
                        Modify(motorcycle, new Point(905, motorcycle.Location.Y + 20));
                        Modify(train.get_cars[j - 1], new Point(905, 133));
                        for (int i = 0; i < train.get_carts - (train.get_carts - j + 1); ++i)
                            Modify(train.get_cars[i], new Point(905, train.get_cars[i].Location.Y + 20));
                        for (int i = train.get_carts - 1; i >= j; --i)
                            Modify(train.get_cars[i], new Point(train.get_cars[i].Location.X + speed, 133));
                    }
                    else
                    {
                        Modify(motorcycle, new Point(905, motorcycle.Location.Y + speed));
                        for (int i = 0; i < train.get_carts; ++i)
                            Modify(train.get_cars[i], new Point(905, train.get_cars[i].Location.Y + speed));
                    }
                    if (motorcycle.Location.Y >= 200)
                    {
                        signals[12].get_pb.Image = Properties.Resources.signal_carree;
                        signals[12].set_signal = 0;
                    }
                    System.Threading.Thread.Sleep(100);
                    j++;
                }

            }
            j = 1;
            SetBring(black[8]);
            setback(motorcycle);
            for (int i = 0; i < train.get_carts; ++i)
                setback(train.get_cars[i]);
            setback(black[0]);
            setback(black[1]);
            setback(black[4]);
            while (motorcycle.Location.X + speed < 1500)
            {
                if (j - 1 < train.get_carts)
                {
                    Modify(motorcycle, new Point(motorcycle.Location.X + 20, 373));
                    Modify(train.get_cars[j - 1], new Point(905, 373));
                    for (int i = 0; i < train.get_carts - (train.get_carts - j + 1); ++i)
                        Modify(train.get_cars[i], new Point(train.get_cars[i].Location.X + 20, 373));
                    for (int i = train.get_carts - 1; i >= j; --i)
                        Modify(train.get_cars[i], new Point(train.get_cars[i].Location.X + 20, 373));
                }
                else
                {
                    Modify(motorcycle, new Point(motorcycle.Location.X + speed, 373));
                    for (int i = 0; i < train.get_carts; ++i)
                        Modify(train.get_cars[i], new Point(train.get_cars[i].Location.X + speed, 373));
                }
                System.Threading.Thread.Sleep(100);
                j++;
            }
        end:
            semaphore.Signal();
            signals[7].get_pb.Image = Properties.Resources.nfgreen;
            signals[7].set_signal = 5;
            signals[3].get_pb.Image = Properties.Resources.Ralentissement;
            signals[3].set_signal = 4;
            freecars();
        }

        ///<summary>
        ///Remove all cars of the train has terminated.
        ///</summary>
        private void freecars()
        {
            int x = 0;
            bool ok = true;
            while (x < train.get_carts)
            {
                for (int i = 0; i < 4; ++i)
                {
                    for (int j = 0; j < 8; ++j)
                    {
                        if (cars[i, j].Item1 == carscoupling[x])
                        {
                            cars[i, j] = new Tuple<Panel, int>(cars[i, j].Item1, 0);
                            ok = false;
                            ++x;
                            break;
                        }
                    }
                    if (!ok)
                        break;
                }
                ok = true;
            }
        }

        ///<summary>
        ///Go out a car.
        ///</summary>
        /// <param name=way>Which cars should be go out</param>
        private void clearlance(int way)
        {
            int i;
            switch (way)
            {
                case 1:
                    i = 0;
                    while (cars[0, i].Item2 == 1)
                        i++;
                    cars[0, i] = new Tuple<Panel, int>(cars[0, i].Item1, 1);
                    cars[0, i].Item1.BackColor = Color.Red;
                    train.add_cars = cars[0, i].Item1;
                    train.add_carts = 1;
                    Modify(train.get_cars[train.get_carts - 1], new Point(1075, 502));
                    Safepan(train.get_cars[train.get_carts - 1], true);
                    System.Threading.Thread.Sleep(100);
                    SetBring(this.train.get_cars[train.get_carts - 1]);
                    while (train.get_cars[train.get_carts - 1].Location.Y > 452)
                    {
                        Modify(train.get_cars[train.get_carts - 1], new Point(1075, train.get_cars[train.get_carts - 1].Location.Y - 10));
                        System.Threading.Thread.Sleep(100);
                    }
                    Modify(train.get_cars[train.get_carts - 1], new Point(1070, 452));
                    carscoupling[train.get_carts - 1] = cars[0, i].Item1;
                    break;
                case 2:
                    i = 0;
                    while (cars[1, i].Item2 == 1)
                        i++;
                    cars[1, i] = new Tuple<Panel, int>(cars[1, i].Item1, 1);
                    cars[1, i].Item1.BackColor = Color.Green;
                    train.add_cars = cars[1, i].Item1;
                    train.add_carts = 1;
                    Modify(train.get_cars[train.get_carts - 1], new Point(910, 502));
                    Safepan(train.get_cars[train.get_carts - 1], true);
                    SetBring(this.train.get_cars[train.get_carts - 1]);
                    System.Threading.Thread.Sleep(100);
                    while (train.get_cars[train.get_carts - 1].Location.Y > 452)
                    {
                        Modify(train.get_cars[train.get_carts - 1], new Point(910, train.get_cars[train.get_carts - 1].Location.Y - 10));
                        System.Threading.Thread.Sleep(100);
                    }
                    Modify(train.get_cars[train.get_carts - 1], new Point(907, 452));
                    carscoupling[train.get_carts - 1] = cars[1, i].Item1;
                    break;
                case 3:
                     i = 0;
                    while (cars[2, i].Item2 == 1)
                        i++;
                    cars[2, i] = new Tuple<Panel, int>(cars[2, i].Item1, 1);
                    cars[2, i].Item1.BackColor = Color.Black;
                    train.add_cars = cars[2, i].Item1;
                    train.add_carts = 1;
                    Modify(train.get_cars[train.get_carts - 1], new Point(722, 562));
                    Safepan(train.get_cars[train.get_carts - 1], true);
                    System.Threading.Thread.Sleep(100);
                    while (train.get_cars[train.get_carts - 1].Location.Y > 373)
                    {
                        Modify(train.get_cars[train.get_carts - 1], new Point(722, train.get_cars[train.get_carts - 1].Location.Y - 10));
                        System.Threading.Thread.Sleep(100);
                    }
                    Modify(train.get_cars[train.get_carts - 1], new Point(717, 373));
                    carscoupling[train.get_carts - 1] = cars[2, i].Item1;
                    break;
                case 4:
                    i = 0;
                    while (cars[3, i].Item2 == 1)
                        i++;
                    cars[3, i] = new Tuple<Panel, int>(cars[3, i].Item1, 1);
                    cars[3, i].Item1.BackColor = Color.Purple;
                    train.add_cars = cars[3, i].Item1;
                    train.add_carts = 1;
                    Modify(train.get_cars[train.get_carts - 1], new Point(554, 562));
                    Safepan(train.get_cars[train.get_carts - 1], true);
                    System.Threading.Thread.Sleep(100);
                    while (train.get_cars[train.get_carts - 1].Location.Y > 373)
                    {
                        Modify(train.get_cars[train.get_carts - 1], new Point(552, train.get_cars[train.get_carts - 1].Location.Y - 10));
                        System.Threading.Thread.Sleep(100);
                    }
                    Modify(train.get_cars[train.get_carts - 1], new Point(547, 373));
                    carscoupling[train.get_carts - 1] = cars[3, i].Item1;
                    break;
            }
        }
        ///<summary>
        ///Switch of way.
        ///</summary>
        private void switching()
        {
            int j = 1;
            if (roadblue)
            {
                while (motorcycle.Location.X < 262)
                {
                    Modify(motorcycle, new Point(motorcycle.Location.X + 20, 373));
                    if (j - 1 < train.get_carts)
                    {
                        Modify(train.get_cars[j - 1], new Point(172, 373));
                        for (int i = 0; i < train.get_carts - (train.get_carts - j + 1); ++i)
                            Modify(train.get_cars[i], new Point(train.get_cars[i].Location.X + 20, 373));
                        for (int i = train.get_carts - 1; i >= j; --i)
                            Modify(train.get_cars[i], new Point(172, train.get_cars[i].Location.Y - 20));
                    }
                    else
                    {
                        for (int i = 0; i < train.get_carts; ++i)
                            Modify(train.get_cars[i], new Point(train.get_cars[i].Location.X + 20, 373));
                    }
                    Thread.Sleep(500);
                    ++j;
                }
                Modify(motorcycle, new Point(262, 373));
                for (int i = 0; i < train.get_carts; ++i)
                    Modify(train.get_cars[i], new Point(242 - i * 20, 373));
                if (signals[7].get_signal != 5)
                {
                    semaphore.Wait();
                    semaphore.Wait();
                }
                isswitch = true;
                roadblue = false;
                one = true;
                semaphore.Signal();
                signals[7].get_pb.Image = Properties.Resources.nfred;
                signals[7].set_signal = 0;
                black_road();
            }
            else
            {
                while (motorcycle.Location.X < 1105)
                {
                    Modify(motorcycle, new Point(motorcycle.Location.X + 10, 133));

                    for (int i = 0; i < train.get_carts; ++i)
                        Modify(train.get_cars[i], new Point(train.get_cars[i].Location.X + 10, 133));
                    Thread.Sleep(100);
                }
                Modify(motorcycle, new Point(1125, 133));
                for (int i = 0; i < train.get_carts; ++i)
                    Modify(train.get_cars[i], new Point(1105 - i * 20, 133));
                if (signals[15].get_signal != 5)
                {
                    semaphore.Wait();
                    semaphore.Wait();
                }
                isswitch = true;
                roadblue = true;
                SetBring(motorcycle);
                for (int i = 0; i < train.get_carts; ++i)
                    SetBring(carscoupling[i]);
                one = true;
                Modify(motorcycle, new Point(1147, 133));
                j = 1;
                System.Threading.Thread.Sleep(50);
                while (motorcycle.Location.Y + 20 <= 440)
                {
                    Modify(motorcycle, new Point(1147, motorcycle.Location.Y + 20));
                    if (j - 1 < train.get_carts)
                    {
                        Modify(train.get_cars[j - 1], new Point(1147, 133));
                        for (int i = 0; i < train.get_carts - (train.get_carts - j + 1); ++i)
                            Modify(train.get_cars[i], new Point(1147, train.get_cars[i].Location.Y + 20));
                        for (int i = train.get_carts - 1; i >= j; --i)
                            Modify(train.get_cars[i], new Point(train.get_cars[i].Location.X + 20, 77));
                    }
                    else
                    {
                        for (int i = 0; i < train.get_carts; ++i)
                            Modify(train.get_cars[i], new Point(1147, train.get_cars[i].Location.Y + 20));
                    }
                    System.Threading.Thread.Sleep(50);
                    j++;
                }
                semaphore.Signal();
                signals[15].get_pb.Image = Properties.Resources.nfred;
                signals[15].set_signal = 0;
                if (dest == 1)
                {
                    signals[2].get_pb.Image = Properties.Resources.f_vert;
                    signals[2].set_signal = 5;
                }
                else if (dest == 2)
                {
                    signals[1].get_pb.Image = Properties.Resources.fjaune;
                    signals[1].set_signal = 2;
                }
                blue_road();
            }

        }
        #endregion

        #region More_functions
        ///<summary>
        ///Thread safe for bring to front the panel pan
        ///</summary>
        ///<param name = pan> Panel that we want to bring to front </param>
        public void SetBring(Panel pan)
        {
            if (pan.InvokeRequired)
            {
                Setbringtofront d = new Setbringtofront(SetBring);
                pan.Invoke(d, new object[] { pan });
            }
            else
            {
                pan.BringToFront();
            }
        }

        ///<summary>
        ///Thread safe for send to back the panel pan
        ///</summary>
        ///<param name = pan> Panel that we want to send to back </param>
        public void setback(Panel pan)
        {
            if (pan.InvokeRequired)
            {
                SetSendToBack d = new SetSendToBack(setback);
                pan.Invoke(d, new object[] { pan });
            }
            else
                pan.SendToBack();
        }

        ///<summary>
        ///Thread safe for set the parameters "visible" of a panel
        ///</summary>
        ///<param name = pan> This is the panel</param>
        ///<param name = visible> boolean which set the parameters "visible" of the panel</param>
        public void Safepan(Panel pan, bool visible)
        {
            if (pan.InvokeRequired)
            {
                SafePan d = new SafePan(Safepan);
                pan.Invoke(d, new object[] { pan, visible });
            }
            else
            {
                pan.Visible = visible;
            }
        }

        public void Modify(Panel pan, Point point)
        {
            if (pan.InvokeRequired)
            {
                modify d = new modify(Modify);
                pan.Invoke(d, new object[] { pan, point });
            }
            else
            {
                pan.Location = point;
            }
        }
        #endregion

        #region get/set
        public bool Execution
        {
            get
            { return execution; }
        }

        public bool Roadblue
        {
            get { return roadblue; }
        }
        #endregion
    }
}
