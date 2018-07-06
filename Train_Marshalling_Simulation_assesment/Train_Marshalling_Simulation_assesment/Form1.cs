using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Train_Marshalling_Simulation_assesment
{
    public partial class Form1 : Form
    {
        public Circuit blue, black;
        public Thread t_blue, t_black, t_blink;
        public Thread t_sem;
        Panel[] bluelabs = new Panel[10];
        Panel[] blacklabs = new Panel[10];
        Signal[] pb = new Signal[20];
        Tuple<Panel,int>[,] cars = new Tuple<Panel, int>[4,10];
        public Semaphore semaphore = new Semaphore();
        public bool one = true;
        public Train trainblue, trainblack;
       

        public Form1()
        {
            InitializeComponent();
            b_go.Enabled = false;
            panel3.BackColor = Color.FromArgb(150, 0, 0, 255);
            label1.BackColor = Color.FromArgb(150, 255, 0, 0);
            label2.BackColor = Color.FromArgb(150, 255, 0, 0);
            label3.BackColor = Color.FromArgb(150, 255, 0, 0);
            label4.BackColor = Color.FromArgb(150, 255, 0, 0);
            panel4.BackColor = Color.FromArgb(150, 0, 0, 0);
            panel3.BackColor = Color.FromArgb(150, 0, 0, 255);
            panel3.BackColor = Color.FromArgb(150, 0, 0, 255);

            pb[0] = new Signal(pb_bluea, 0);
            pb[1] = new Signal(pb_blue1, 1);
            pb[2] = new Signal(pb_blue2, 1);
            pb[3] = new Signal(pb_blue3, 0);
            pb[4] = new Signal(pb_blue4, 0);
            pb[5] = new Signal(pb_red, 6);
            pb[6] = new Signal(pb_green, 6);
            pb[7] = new Signal(pb_aig1, 1);
            pb[8] = new Signal(pb_blacka, 0);
            pb[9] = new Signal(pb_black1, 1);
            pb[10] = new Signal(pb_black2, 1);
            pb[11] = new Signal(pb_black3, 0);
            pb[12] = new Signal(pb_black4, 0);
            pb[13] = new Signal(pb_black, 6);
            pb[14] = new Signal(pb_purple, 6);
            pb[15] = new Signal(pb_aig2, 1);



            cars[0, 0] = new Tuple<Panel, int>(p_red1, 0);
            cars[0, 1] = new Tuple<Panel, int>(p_red2, 0);
            cars[0, 2] = new Tuple<Panel, int>(p_red3, 0);
            cars[0, 3] = new Tuple<Panel, int>(p_red4, 0);
            cars[0, 4] = new Tuple<Panel, int>(p_red5, 0);
            cars[0, 5] = new Tuple<Panel, int>(p_red6, 0);
            cars[0, 6] = new Tuple<Panel, int>(p_red7, 0);
            cars[0, 7] = new Tuple<Panel, int>(p_red8, 0);
            cars[0, 8] = new Tuple<Panel, int>(p_red9, 0);
            cars[0, 9] = new Tuple<Panel, int>(p_red10, 0);
            cars[1, 0] = new Tuple<Panel, int>(p_green1, 0);
            cars[1, 1] = new Tuple<Panel, int>(p_green2, 0);
            cars[1, 2] = new Tuple<Panel, int>(p_green3, 0);
            cars[1, 3] = new Tuple<Panel, int>(p_green4, 0);
            cars[1, 4] = new Tuple<Panel, int>(p_green5, 0);
            cars[1, 5] = new Tuple<Panel, int>(p_green6, 0);
            cars[1, 6] = new Tuple<Panel, int>(p_green7, 0);
            cars[1, 7] = new Tuple<Panel, int>(p_green8, 0);
            cars[1, 8] = new Tuple<Panel, int>(p_green9, 0);
            cars[1, 9] = new Tuple<Panel, int>(p_green10, 0);
            cars[2, 0] = new Tuple<Panel, int>(p_wag1, 0);
            cars[2, 1] = new Tuple<Panel, int>(p_wag2, 0);
            cars[2, 2] = new Tuple<Panel, int>(p_wag3, 0);
            cars[2, 3] = new Tuple<Panel, int>(p_wag4, 0);
            cars[2, 4] = new Tuple<Panel, int>(p_wag5, 0);
            cars[2, 5] = new Tuple<Panel, int>(p_wag6, 0);
            cars[2, 6] = new Tuple<Panel, int>(p_wag7, 0);
            cars[2, 7] = new Tuple<Panel, int>(p_wag8, 0);
            cars[2, 8] = new Tuple<Panel, int>(p_wag9, 0);
            cars[2, 9] = new Tuple<Panel, int>(p_wag10, 0);
            cars[3, 0] = new Tuple<Panel, int>(p_purple1, 0);
            cars[3, 1] = new Tuple<Panel, int>(p_purple2, 0);
            cars[3, 2] = new Tuple<Panel, int>(p_purple3, 0);
            cars[3, 3] = new Tuple<Panel, int>(p_purple4, 0);
            cars[3, 4] = new Tuple<Panel, int>(p_purple5, 0);
            cars[3, 5] = new Tuple<Panel, int>(p_purple6, 0);
            cars[3, 6] = new Tuple<Panel, int>(p_purple7, 0);
            cars[3, 7] = new Tuple<Panel, int>(p_purple8, 0);
            cars[3, 8] = new Tuple<Panel, int>(p_purple9, 0);
            cars[3, 9] = new Tuple<Panel, int>(p_purple10, 0);
        }

        public void start()
        {
            b_go.Enabled = false;
            b_go.BackColor = Color.Gray;
            t_blink.Abort();
            Panel[] bluelabs = { p_bluea, p_blue1, p_blue2, p_blue3, p_blue4, p_red, p_green, p_aig1, panel1 };
            Panel[] blacklabs = { p_blacka, p_black1, p_black2, p_black3, p_black4, p_black, p_purple, p_aig2, panel2 };

            t_sem = new Thread(new ThreadStart(semaphore.Start));

            t_sem.Start();

            this.Closing += new CancelEventHandler(this.Form1_Closing);

            if (blue == null || (!blue.Execution || !blue.Roadblue))
            {
                label3.BackColor = Color.FromArgb(150, 255, 0, 0);
                blue = new Circuit(p_train, true, trainblue, bluelabs, blacklabs, cars, pb, semaphore, p_train2);
                t_blue = new Thread(new ThreadStart(blue.start));
                t_blue.Start();
            }

            if (black == null || (!black.Execution || black.Roadblue))
            {
                label4.BackColor = Color.FromArgb(150, 255, 0, 0);
                black = new Circuit(p_train2, false, trainblack, bluelabs,blacklabs, cars, pb, semaphore, p_train);
                t_black = new Thread(new ThreadStart(black.start));
                t_black.Start();
            }


        }

        private void blink()
        {
            for (;;)
            {
                b_go.BackColor = Color.Green;
                Thread.Sleep(500);
                b_go.BackColor = Color.Gray;
                Thread.Sleep(500);
            }

        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {
 
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }



        private void b_va_Click(object sender, EventArgs e)
        {
            int speed;
            Queue<int> stops = new Queue<int>(5);
            if ((int)num_blue1.Value < 0 || (int)num_blue1.Value > 4 || (int)num_blue2.Value < 0 || (int)num_blue2.Value > 4 || (int)num_blue3.Value < 0 || (int)num_blue3.Value > 4 || (int)num_blue4.Value < 0 || (int)num_blue4.Value > 4)
                error("blue");
            else {
                if (num_blue1.Value != 0)
                    stops.Enqueue((int)num_blue1.Value);
                if (num_blue2.Value != 0)
                    stops.Enqueue((int)num_blue2.Value);
                if (num_blue3.Value != 0)
                    stops.Enqueue((int)num_blue3.Value);
                if (num_blue4.Value != 0)
                    stops.Enqueue((int)num_blue4.Value);
                if (num_blue5.Value != 0)
                    stops.Enqueue((int)num_blue5.Value);
                if (stops.Count == 0)
                    error("blue");
                else
                {
                    if (cb_speedblue.SelectedItem != null && cb_speedblue.SelectedItem.ToString() == "TGV")
                        speed = 75;
                    else
                        speed = (cb_speedblue.SelectedItem != null) ? Int32.Parse( cb_speedblue.SelectedItem.ToString()) : 10;
                    trainblue = new Train(0, "tgv", 42, stops, speed);
                    b_va.Enabled = false;
                    label3.BackColor = Color.FromArgb(150, 0, 255, 0);
                }
            }
        }

        private void b_vablack_Click(object sender, EventArgs e)
        {
            int speed = 0;
            Queue<int> stops = new Queue<int>(4);
            if ((int)num_black1.Value < 0 || (int)num_black1.Value > 4 || (int)num_black2.Value < 0 || (int)num_black2.Value > 4 || (int)num_blue3.Value < 0 || (int)num_black3.Value > 4 || (int)num_black4.Value < 0 || (int)num_black4.Value > 4)
                error("black");
            else {
                if (num_black1.Value != 0)
                    stops.Enqueue((int)num_black1.Value);
                if (num_black2.Value != 0)
                    stops.Enqueue((int)num_black2.Value);
                if (num_black3.Value != 0)
                    stops.Enqueue((int)num_black3.Value);
                if (num_black4.Value != 0)
                    stops.Enqueue((int)num_black4.Value);
                if (num_black5.Value != 0)
                    stops.Enqueue((int)num_black5.Value);
                if (stops.Count == 0)           
                    error("black");
                else
                {
                    if (cb_speedblack.SelectedItem != null && (cb_speedblack.SelectedItem.ToString() == "TGV"))
                        speed = 75;
                    else
                        speed = (cb_speedblack.SelectedItem != null) ? Int32.Parse(cb_speedblack.SelectedItem.ToString()) : 10;
                    trainblack = new Train(0, "tgv", 42, stops, speed);
                    b_vablack.Enabled = false;
                    label4.BackColor = Color.FromArgb(150, 0, 255, 0);
                }
            }
        }

        private void pb_bluea_Click(object sender, EventArgs e)
        {
            explain_four();
        }

        private void b_try_Click(object sender, EventArgs e)
        {
            if (!b_va.Enabled && !b_vablack.Enabled && (black == null && blue == null || (!blue.Execution && !black.Execution) ))
            {
                pb[7].get_pb.Image = Properties.Resources.nfred;
                pb[7].set_signal = 0;
                pb[15].get_pb.Image = Properties.Resources.nfred;
                pb[15].set_signal = 0;
                b_go.Enabled = true;
                b_go.BackColor = Color.Green;
                t_blink = new Thread(new ThreadStart(blink));
                t_blink.Start();
                b_va.Enabled = true;
                b_vablack.Enabled = true;
            }
        }

        private void pb_blacka_Click(object sender, EventArgs e)
        {
            explain_four();
        }

        private void b_go_Click(object sender, EventArgs e)
        {
            start();
        }

        private void pb_blue4_Click(object sender, EventArgs e)
        {
            explain_six();
        }

        private void pb_black3_Click(object sender, EventArgs e)
        {
            explain_six();
        }

        private void pb_black4_Click(object sender, EventArgs e)
        {
            explain_six();
        }

        private void pb_blue3_Click(object sender, EventArgs e)
        {
            explain_six();
        }

        private void pb_red_Click(object sender, EventArgs e)
        {
            explain_maneuver();
        }

        private void Form1_Closing(object sender, CancelEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
            t_blink.Abort();
        }

        private void pb_green_Click(object sender, EventArgs e)
        {
            explain_maneuver();
        }

        private void pb_black_Click(object sender, EventArgs e)
        {
            explain_maneuver();
        }

        private void pb_purple_Click(object sender, EventArgs e)
        {
            explain_maneuver();
        }

        private void pb_black2_Click(object sender, EventArgs e)
        {
            explain_three();
        }

        private void error(string str)
        {
            MessageBox.Show("Error, you can select at least 1 car by train. Error : " + str);
        }

        private void pb_black1_Click(object sender, EventArgs e)
        {
            explain_three();
        }

        private void pb_blue2_Click(object sender, EventArgs e)
        {
            explain_three();
        }

        private void pb_blue1_Click(object sender, EventArgs e)
        {
            explain_three();
        }

        private void pb_aig1_Click(object sender, EventArgs e)
        {
            explain_nf();
        }

        private void pb_aig2_Click(object sender, EventArgs e)
        {
            explain_nf();
        }

        private void explain_four()
        {
            explain_four ex = new explain_four();
            ex.ShowDialog();
        }

        private void explain_six()
        {
            explain_six ex = new explain_six();
            ex.ShowDialog();
        }

        private void explain_maneuver()
        {
            explain_two ex = new explain_two();
            ex.ShowDialog();
        }

        private void explain_three()
        {
            explain_three ex = new explain_three();
            ex.ShowDialog();
        }

        private void explain_nf()
        {
            signal_nf ex = new signal_nf();
            ex.ShowDialog();
        }

    }
}
