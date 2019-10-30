using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Blackjack
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Backgroundplay()
        {
            //System.Media.SoundPlayer Backgroudmusic = new System.Media.SoundPlayer(Properties.Resources.Las_Vegas_Casino_Music_Video_For_Night_Game_of_Poker_Blackjack_Roulette_Wheel__Slots);
            //System.Media.SoundPlayer Backgroudmusic=new System.Media.SoundPlayer("F:\\Blackjack - Klasser\\Blackjack\\Resources\\Las Vegas Casino Music Video For Night Game of Poker Blackjack Roulette Wheel  Slots.WAV"); 
            //Backgroudmusic.Play();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            // Startar form2
            Form2 load=new Form2();
            load.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Special regler

            textBox1.Visible = true;
            textBox1.Text = "You compete against the 'dealer' and the goal is to come as close as possible to 21 points. The cards points are equal the number except for J,Q and K which gives 10 points. Ace can be 11 or 1 (players choice). At the start 2 cards are given to you and the dealer, you may 'hit' and take another card or stay on your current points. The dealer has to 'hit' until he gets 17 or more points.";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
             //Fixar musik
            Thread background = new Thread(Backgroundplay);
            background.Start();
            textBox1.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Regler
            MessageBox.Show("Blackjack is when you get an ace and a 10 point card in your first 2 cards and gives you 3 times your bet"+"\n"+"You can split if your 2 first to cards gives the same amount of points, this will cost you your betting money and you will get 2 decks and each deck has a chance to win"+"\n"+"You can insurance if the dealer gets an ace as a first card, this costs 1/2 of your bet and if he gets blackjack your money will be returned"+"\n"+"Doubling down will double your bet and give you one more card");
        }
    }
}
