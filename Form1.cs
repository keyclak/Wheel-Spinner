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
using System.Runtime.InteropServices;

namespace Spin_The_Wheel
{
    public partial class Main : Form
    {
        public class Item
        {
            public double relativeProb;
            public String name;
            public double normalProb;
            public int ID;

            public Item(string n, int i, double p)
            {
                name = n;
                relativeProb = p;
                ID = i;
            }

            public void setNormalProb(double np)
            {
                normalProb = np;
            }
        }

        static List<Item> items = new List<Item>()
        {
            new Item("A", 1, 0), //0
            new Item("B", 2, 0), //1,2
            new Item("C", 3, 0), //3,4
            new Item("D", 4, 1), //5
            new Item("E", 5, 0), //6
            new Item("F", 6, 0), //7
            new Item("G", 7, 0), //8
            new Item("H", 8, 0)  //9
        };


        public const Int32 WM_SYSCOMMAND = 0x112;
        public const Int32 MF_BYPOSITION = 0x400;
        public const Int32 MYMENU1 = 1000;
        public const Int32 MUMENU2 = 1001;

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32.dll")]
        private static extern bool InsertMenu(IntPtr hMenu, Int32 wPosition, Int32 wFlags, Int32 wIDNewItem, string lpNewItem);

        static string livePath = Environment.CurrentDirectory;

        static Image[] images = new Image[8];
        static int i = 0;
        static bool isWheelSpinning = false;
        static Thread spinWheel = new Thread(()=>SpinTheWheel(items));
        static PictureBox wh = new PictureBox(); //did this to be able to close the spinWheel thread anywher in code

        private static Main mainFormForUpdating;
        public static Main MainFormForUpdating
        {
            get
            {
                return mainFormForUpdating;
            }
        }

        public Main()
        {
            InitializeComponent();

            if (mainFormForUpdating == null)
            {
                mainFormForUpdating = this;
            }

            wh = Wheel_PictureBox;
        }

        static void ChangeWheel_Image()
        {
            if (i + 1 >= images.Length)
                i = 0;
            else
                i++;

            images[i] = Image.FromFile(livePath + "\\resources\\Slot " + (i + 1) + " active.png");
            wh.Invoke(new Action(() => wh.BackgroundImage = images[i]));
        }

        static void SpinTheWheel(List<Item> items)
        {
            Random rand = new Random();
            double totalSum = 0;
            Item result = new Item("-", 1, 0);

            foreach (Item item in items)
            {
                totalSum = totalSum + item.relativeProb; //10
            }

            foreach (Item item in items)
            {
                item.setNormalProb(item.relativeProb / totalSum);
            }

            double sum = 0.0;
            double p = rand.NextDouble();

            foreach (Item item in items)
            {
                sum += item.normalProb;
                if (sum > p)
                {
                    result = item;
                    break;
                }
            }

            Random rand2 = new Random();
            int cycle = 1;
            int resultInt = result.ID;

            for (int y = 0; y < 3; y++)
            {
                //Fast spin speed
                int fastSpin = rand2.Next((55 - ((cycle - 1) * 20)), (65 - ((cycle - 1) * 15)));
                for (int x = 0; x < fastSpin; x++)
                {
                    ChangeWheel_Image();
                    Thread.Sleep(40 * cycle);
                }
                cycle++;
            }

            while (i + 1 != resultInt)
            {
                ChangeWheel_Image();
                Thread.Sleep(40 * cycle);
            }

            isWheelSpinning = false;
            MessageBox.Show("Congrats!! You won prize " + (i + 1));
            spinWheel.Abort();
        }

        public void changeProb(List<double> p)
        {
            int counter = 0;
            foreach (int i in p)
            {
                items[counter].relativeProb = i;
                counter++;
            }
        }

        private void Spin_Button_Click(object sender, EventArgs e)
        {
            if (!isWheelSpinning)
            {
                Thread spinWheel = new Thread(()=>SpinTheWheel(items));
                isWheelSpinning = true;
                spinWheel.Start();
            }
        }

        protected override void WndProc(ref Message msg)
        {
            if (msg.Msg == WM_SYSCOMMAND)
            {
                switch (msg.WParam.ToInt32())
                {
                    case MYMENU1:
                        Settings formPopup = new Settings(this);
                        formPopup.Show(this);
                        //this.Close();
                        return;
                    default:
                        break;
                }
            }
            base.WndProc(ref msg);
        }

        private void Main_Load(object sender, EventArgs e)
        {
            IntPtr MenuHandle = GetSystemMenu(this.Handle, false);
            InsertMenu(MenuHandle, 5, MF_BYPOSITION, MYMENU1, "Adjust Wheel...");
        }
    }
}