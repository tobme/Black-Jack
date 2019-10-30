using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Blackjack
{
    public class kort : PictureBox
    {
        private Timer timer;
        private bool isPlayercard;
        public int cardNumber;
        private int cardMoving;
        private Form2 form2;
        private int color;
        private int Value;
        private bool splitt;
        private Image[,] cardBacks = new Image[16, 5];

        public kort() { }
        public kort(bool isPlayercard, Form2 f, bool split)
        {
            Initialize();
            cardMoving = 0;
            this.isPlayercard = isPlayercard;
            this.Size = new Size(100, 155);
            this.splitt = split;
            timer = new Timer();
            timer.Interval = 10;
            timer.Tick += TimerTick;
            cardNumber = Form2.Cards;
            Form2.Cards++; // Vilket kort den är
            this.form2 = f;
            timer.Start();
            int[] kortvalues = f.Slumpkort();
            color = kortvalues[1]; // Färg
            Value = kortvalues[0]; // Värde
            this.BackgroundImage = Properties.Resources.BlackJackBackgroundDesign;
        }
        public void TimerTick(object sender, EventArgs e)
        {
            // Gör så kortet åker

            cardMoving += 5;
            if (isPlayercard) // Spelarens kort
            {
                Top += 5;
                if (!splitt) // Splitt ger olika listor
                {
                    if (cardMoving == 275 - form2.getIndex(this, 0) * 25) // getIndex ger index för spelarlistan
                        doCard();
                }
                else
                {
                    if (cardMoving == 275 - form2.getIndex(this, 2) * 25) // getIndex ger index för splitlistan
                        doCard();
                }
            }
            else // Bankens kort
            {
                Top -= 5;
                if (cardMoving == 275 + form2.getIndex(this,1) * 25) // getIndex ger index för banklistan
                    doCard();
            }

        }
        public void setBackground()
        {
            this.BackgroundImage = cardBacks[Value, color]; // Ger kort bakgrund beroende på färg och värde
        }
        public void doCard() 
        {
            // Fixar bakgrund till kortet och om det är cardnummber 0,1,2 så fixar det ett nytt kort

            timer.Stop();
            if (cardNumber != 3)
            {
                setBackground();
                form2.ScoreChanger();
            }
            if (cardNumber == 0 || cardNumber == 2) // Nytt kort åt banken
                form2.CreateCard(false, splitt);
            else if (cardNumber == 1) // Fixar nytt kort åt spelaren
            {
                form2.CreateCard(true,splitt);
                form2.insurace();
            }
        }
        public int Score()
        {
            // Fixar värde på korten

            if (Value > 10 && Value < 14)
                return 10;
            return Value;
        }
        private void Initialize()
        {
            // Fixar bakgrunderna i en array
            #region hjärter
            cardBacks[2, 1] = Properties.Resources.H2;
            cardBacks[3, 1] = Properties.Resources.H3;
            cardBacks[4, 1] = Properties.Resources.H4;
            cardBacks[5, 1] = Properties.Resources.H5;
            cardBacks[6, 1] = Properties.Resources.H6;
            cardBacks[7, 1] = Properties.Resources.H7;
            cardBacks[8, 1] = Properties.Resources.H8;
            cardBacks[9, 1] = Properties.Resources.H9;
            cardBacks[10, 1] = Properties.Resources.H10;
            cardBacks[11, 1] = Properties.Resources.HJ;
            cardBacks[12, 1] = Properties.Resources.HQ;
            cardBacks[13, 1] = Properties.Resources.HK;
            cardBacks[14, 1] = Properties.Resources.HA;
            #endregion
            #region Ruter
            cardBacks[2, 2] = Properties.Resources.R2;
            cardBacks[3, 2] = Properties.Resources.R3;
            cardBacks[4, 2] = Properties.Resources.R4;
            cardBacks[5, 2] = Properties.Resources.R5;
            cardBacks[6, 2] = Properties.Resources.R6;
            cardBacks[7, 2] = Properties.Resources.R7;
            cardBacks[8, 2] = Properties.Resources.R8;
            cardBacks[9, 2] = Properties.Resources.R9;
            cardBacks[10, 2] = Properties.Resources.R10;
            cardBacks[11, 2] = Properties.Resources.RJ;
            cardBacks[12, 2] = Properties.Resources.RQ;
            cardBacks[13, 2] = Properties.Resources.RK;
            cardBacks[14, 2] = Properties.Resources.RA;
            #endregion
            #region Spader
            cardBacks[2, 3] = Properties.Resources.S2;
            cardBacks[3, 3] = Properties.Resources.S3;
            cardBacks[4, 3] = Properties.Resources.S4;
            cardBacks[5, 3] = Properties.Resources.S5;
            cardBacks[6, 3] = Properties.Resources.S6;
            cardBacks[7, 3] = Properties.Resources.S7;
            cardBacks[8, 3] = Properties.Resources.S8;
            cardBacks[9, 3] = Properties.Resources.S9;
            cardBacks[10, 3] = Properties.Resources.S10;
            cardBacks[11, 3] = Properties.Resources.SJ;
            cardBacks[12, 3] = Properties.Resources.SQ;
            cardBacks[13, 3] = Properties.Resources.SK;
            cardBacks[14, 3] = Properties.Resources.SA;
            #endregion
            #region klöver
            cardBacks[2, 4] = Properties.Resources.K2;
            cardBacks[3, 4] = Properties.Resources.K3;
            cardBacks[4, 4] = Properties.Resources.K4;
            cardBacks[5, 4] = Properties.Resources.K5;
            cardBacks[6, 4] = Properties.Resources.K6;
            cardBacks[7, 4] = Properties.Resources.K7;
            cardBacks[8, 4] = Properties.Resources.K8;
            cardBacks[9, 4] = Properties.Resources.K9;
            cardBacks[10, 4] = Properties.Resources.K10;
            cardBacks[11, 4] = Properties.Resources.KJ;
            cardBacks[12, 4] = Properties.Resources.KQ;
            cardBacks[13, 4] = Properties.Resources.KK;
            cardBacks[14, 4] = Properties.Resources.KA;
            #endregion
        }
        public int getvalue
        {
            // Ger värde på kortet
            get { return Value; }
            set { Value = value; }
        }
    }
}
