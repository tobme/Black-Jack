using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.Threading;

namespace Blackjack
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public static int Cards = 0;
        List<int[]> activeCards = new List<int[]>();
        double cash = 10000;
        int bankscore, playerscore, splitscore = 0;
        double bet;
        bool blackjacks = true;
        bool ifbet = true;
        bool playerturn = true;
        bool bankstarts = true;
        bool insurance, doubledown, ace, dressed = false;
        bool splits = false;

        Random r = new Random();
        List<kort> playerList = new List<kort>();
        List<kort> bankList = new List<kort>();
        List<kort> splitlist = new List<kort>();

#region reset
        private void reset()
        {
            // Återstället alla värden och listor

            foreach (kort k in playerList)
                this.Controls.Remove(k);
            foreach (kort k in bankList)
                this.Controls.Remove(k);
            foreach (kort k in splitlist)
                this.Controls.Remove(k);
            if (activeCards.Count >= 45)
                activeCards = new List<int[]>();
            playerList = new List<kort>();
            bankList = new List<kort>();
            splitlist = new List<kort>();
            bet = 0;
            Cards = bankscore = playerscore = 0;
            button1.Enabled = label5.Visible = true;
            button2.Enabled = button3.Enabled = pictureBox12.Visible = pictureBox1.Visible = false;
            textBox1.Clear();
            button4567();
            insurance = splits = doubledown = false;
            blackjacks = ifbet = playerturn = bankstarts = true;
            label2.Text = "Player points: 0";
            label3.Text = "Bank points: 0"; 
            label4.Text = "";
            if (cash < 10)
            {
                MessageBox.Show("You lost");
            }
            waittimer.Stop();
 
        }
#endregion
        public void insurace() // insurance
        {
            if (bankList[0].getvalue==14) // Kollar om bankens första kort är ess
            {
                button5.Enabled = true;
            }
        }
        private void split()
        {
            if (playerList[0].getvalue==playerList[1].getvalue) // Kollar de två första korten har samma värde
            {
                button4.Enabled = true;
            }
        }
        private void button4567()
        {
             button4.Enabled = false;
             button5.Enabled = false;
             button6.Enabled = false;
             button7.Enabled = false;
        }
        private void cashWin(double winMultiplier, bool minusplus) // fixar med pengarna
        {
            if (minusplus)
                cash += winMultiplier * bet;
            else
                cash -= winMultiplier * bet;
            label1.Text = "Cash: " + cash;
        }

        private void blackjack(List<kort> List)
        {
            ace = dressed = false;
            foreach (kort i in List) // kollar om banken/spelaren har blackjack
            {
                if (i.Score() == 14)
                    ace = true;
                else if (i.Score() == 13 || i.Score() == 12 || i.Score() == 11 || i.Score() == 10)
                    dressed = true;
            }
            if (dressed && ace)
            {
                if (playerturn) //spelarens tur
                {
                    if (!blackjacks)
                        return;
                    blackjacks = false;
                    blackjack(bankList); // Checkar om banken också fått blackjack

                    Blackjacktimer.Start(); // Startar blackjack timern
                }
                else
                {
                    if (insurance == true) // insurance
                    {
                        cashWin(1.5, true);
                        pictureBox12.BackgroundImage = Properties.Resources.Insurance2;
                        pictureBox12.Visible = true;
                        resettimer.Start();
                    }
                }
            }

        }
        private void bankstart() // fixar värden
        {
            bankstarts = false;
            playerturn = false;
            ScoreChanger();
            bankList[1].setBackground();
            blackjack(bankList);
        }
        private void bankturn()
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            if (bankstarts) // första gången det är bankens tur så fixar den värden
                bankstart();
            else
            {
                if (bankscore < 17) // gör ett nytt kort om poäng < 17
                {
                    waittimer.Start();
                }
                else if (bankscore > 21) // banken förlorar
                {
                    pictureBox12.Visible = true;
                    pictureBox12.BackgroundImage = Properties.Resources.Player_win2;
                    pictureBox12.Visible = true;
                    cashWin(2,true);
                    resettimer.Start();
                }
                else // om poäng > 17 men < 21 så kollar den vinst
                {
                    CheckWin();
                }
            }
        }
        private void CheckWin() // kollar vinst
        {
            // kollar vinster vid playerscore och splitscore
            if (playerscore > bankscore)
            {
                pictureBox12.BackgroundImage = Properties.Resources.Player_win2;
                pictureBox12.Visible = true;
                cashWin(2,true);
            }
            else if (bankscore > playerscore)
            {
                pictureBox12.BackgroundImage = Properties.Resources.Bank_Wins2;
                pictureBox12.Visible = true;
            }
            else if (bankscore == playerscore && dressed && ace)
            {
                pictureBox12.BackgroundImage = Properties.Resources.Bank_Wins2;
                pictureBox12.Visible = true;
            }
            else if (bankscore == playerscore)
            {
                pictureBox12.BackgroundImage = Properties.Resources.Tie2;
                pictureBox12.Visible = true;
                cashWin(1,true);
            }
            if (splits) //kollar vinst vid split
            {
                if (splitscore > bankscore)
                {
                    pictureBox1.BackgroundImage = Properties.Resources.Second_hand_player_wins;
                    pictureBox1.Visible = true;
                    cashWin(2,true);
                }
                else if (bankscore > splitscore)
                {
                    pictureBox1.BackgroundImage = Properties.Resources.second_hand_bank_wins;
                    pictureBox1.Visible = true;
                }
                else if (bankscore == splitscore && dressed && ace)
                {
                    pictureBox1.BackgroundImage = Properties.Resources.second_hand_bank_wins;
                    pictureBox1.Visible = true;
                }
                else if (bankscore == splitscore)
                {
                    pictureBox1.BackgroundImage = Properties.Resources.Second_hand_tie;
                    pictureBox1.Visible = true;
                    cashWin(1,true);
                }
            }   
            resettimer.Start();
        }

        public int[] Slumpkort()
        {
            // Slumpar kortets färgvärde och nummervärde
            // Kollar också om kortet har varit i spel

            int kortnummer = r.Next(2, 15); // Kortnummer
            int kortfärg = r.Next(1, 5); // Färg
            int[] returnkort = { kortnummer, kortfärg };

            foreach (int[] i in activeCards)
            {
                if (returnkort[0] == i[0] && returnkort[1] == i[1]) //Kollar om kort är aktiva (Har varit i spel)
                {
                    return Slumpkort(); // Kör om funktionen
                }
            }
            activeCards.Add(returnkort); // Lägger in kortet i activeCards så den inte kan förekomma igen
            return returnkort;
        }
        public int getIndex(kort k,int p) // skaffar indexen på kortet
        {
            if (p == 0)
                return playerList.IndexOf(k);
            else if (p == 1)
                return bankList.IndexOf(k);
            else
                return splitlist.IndexOf(k);
        }
        private int CountScore(List<kort> list) // Räknar poäng
        {
            int totalscore = 0;
            int AceCount = 0;
            foreach (kort k in list)
                if (k.Score() != 14)
                    totalscore += k.Score();
            foreach (kort k in list) // fixar med ess
                if (k.Score() == 14)
                {
                    AceCount++;
                    if (totalscore < 11)
                        totalscore += 11;
                    else
                        totalscore += 1;
                }

            // Fixar om det finns 2 eller flera ess
            // Exempel, ess ,ess, 10 = 12 poäng. Men med övre metod blir det 22.
            if (AceCount >= 2 && totalscore > 21) 
                    totalscore -= 10;
            
            return totalscore;
        }
        public void ScoreChanger()
        {
            // Funktionen kollar i stort sett alla olika saker som t.ex. om poängen överstigen 21, blackjack, doubling down osv..
            // Poäng räknas med funktionen "countscore"

            playerscore = CountScore(playerList);
            label2.Text = "Player points: " + playerscore;

            if (splitlist.Count != 0) // kollar om spelaren har splittat och i så fall ger ut poäng för den högen
            {
                splitscore = CountScore(splitlist);
                label4.Text = "Players second hand: " + splitscore;
                if (splitscore > 21 && splits) // kollar om andra högen får mer poäng än 21
                {
                    MessageBox.Show("Second hand is fat");
                    splits = false;
                    if (playerscore > 21) // om playerscore är > 21 så är båda högarna > 21. Vilket gör så spelaren förlorar.
                    {
                        resettimer.Start();
                        return;
                    }
                    else // om bara andra högen är > 21 så blir det bankens tur
                        bankturn();
                }
            }

            if (playerList.Count == 2 && playerturn) // kollar blackjack och split
            {
                blackjack(playerList);
                split();
            }

            if (!playerturn) // om det inte är spelarens tur så kollas bankens poäng och "bankturn" gör så banken fortsätter
            {
                bankscore = CountScore(bankList);
                label3.Text = "Bank points: " + bankscore;
                bankturn();
            }

            if (playerscore > 21 && splitlist.Count == 2 && !splits) // Första "handen" i split
            {
                MessageBox.Show("Your first hand got fat");
                splits = true;
            }
            else if (playerscore > 21 && splitlist.Count < 2) // spelaren förlorar
            {
                pictureBox12.BackgroundImage = Properties.Resources.Bank_Wins2;
                pictureBox12.Visible = true;
                resettimer.Start();
            }
            else if (playerscore <= 21 && doubledown) //double down
            {
                bankturn();
                doubledown = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // genomför satsningen
            // Ger ut de första 4 korten

            try { bet = double.Parse(textBox1.Text); }
            catch { MessageBox.Show("You need to place a bet"); return; }

            if (bet != 0)
            {
                label5.Visible = false;
                button2.Enabled = true;
                button3.Enabled = true;
                button1.Enabled = false;
                button6.Enabled = true;
                button7.Enabled = true;
                ifbet = false;
                CreateCard(true, splits);
                //Dealtimer.Start();
            }
        }
        public void CreateCard(bool isPlayer, bool split)
        {
            // Gör kortet
            // Isplayer är en bool som kollar om den är spelarens eller bankens tur, alltså vart kortet ska hamna.
            // splits kollar vart kortet ska hamna om spelaren har "splittat"

            if (isPlayer)
            {
                kort k = new kort(true, this, split); // Gör ett picture box av klassen kort
                if (!splits)
                {
                    k.Location = new Point(418 + playerList.Count * 20, 375); // ger en punkt där kortet ska skapas
                    playerList.Add(k);
                }
                else
                {
                    k.Location = new Point(570 + splitlist.Count * 20, 375); // ger en punkt där kortet ska skapas
                    splitlist.Add(k);
                }
                this.Controls.Add(k);
                k.BringToFront();
            }
            else
            {
                kort k = new kort(false, this, split);
                k.Location = new Point(418 + bankList.Count * 20, 375);
                this.Controls.Add(k);
                bankList.Add(k);
                k.BringToFront();
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            // Tar ett nytt kort till handen

            CreateCard(true,splits);
            button4567();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Spelaren avslutar sin runda och bankens tur startas
            // Om Spelaren har splittat upp sin hand så ger första trycket på knappen så gör det så att man tar kort till andra högen

            button4567();
            if (splitlist.Count != 0 && splits == false)
            {
                MessageBox.Show("You may now hit your other hand");
                splits = true;
            }
            else
                bankturn();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            pictureBox12.BringToFront();
            pictureBox12.Visible = false;
            pictureBox1.Visible = false;
            label4.Text = "";
            button2.Enabled = false;
            button3.Enabled= false;
            button4567();
        }

        private void waittimer_Tick(object sender, EventArgs e) // Ger ut ett kort
        {
            CreateCard(false,splits);
            waittimer.Stop();
        }

        private void resettimer_Tick(object sender, EventArgs e) // Återställnings timer
        {
            reset();
            resettimer.Stop();
        }

        private void button4_Click(object sender, EventArgs e) //split
        {
            // Om spelarens två första kort får samma värde kan detta användas
            // Spelaren delar upp sina kort till två olika högar (playerList, splitlist)
            // Spelaren dubblar sin insats och ger 1 till kort till bägge högarna
            // boolen splits används för att bestämma i vilken hög som kortet ska hamna

            button4.Enabled = false;
            splitlist.Add(playerList[1]);
            playerList.RemoveAt(1);
            cashWin(1, false);
            splitlist[0].Location = new Point(570, 375+275);
            for (int i = 0; i < 2; i++)
            {
                CreateCard(true,splits);
                splits = true;
            }
            blackjack(splitlist);
            splits = false;
        }

        private void wintimer_Tick(object sender, EventArgs e) // Kolla vinst timer
        {
            CheckWin();
        }

        private void button5_Click(object sender, EventArgs e) // insurance
        {
            // Om bankens första kort blir ett ess kan detta användas
            // Spelaren satsar halva sin instats och boolen insurance gör så om banken får blackjack får spelaren tillbaka sin insats
            if (cash >= 0.5 * bet)
            {
                cashWin(0.5, false);
                insurance = true;
                button5.Enabled = false;
            }
            else
                MessageBox.Show("You dont got enough moneys");
        }

        private void button6_Click(object sender, EventArgs e) // doubling down
        {
            // Spelaren dubblar sin instats och får ett till kort
            // Efter kortet så startar så startar bankens tur med hjälp utav boolen "doubledown"

            if (cash >= bet)
            {
                cashWin(1, false);
                bet *= 2;
                CreateCard(true, splits);
                doubledown = true;
                button6.Enabled = false;
            }
            else
                MessageBox.Show("You don't got enough moneys");
        }

        private void button7_Click(object sender, EventArgs e) // Surrender
        {
            // Spelaren ger upp så får den halva sin instats tillbaka

            cashWin(0.5,true);
            MessageBox.Show("Half of your bet will return");
            resettimer.Start();
            button7.Enabled = false;
            button6.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
        }

        private void Form2_MouseClick(object sender, MouseEventArgs e)
        {
           
            //Bestämmer y och x värden för musen och get olika mycket bet beroende på vilken mark de klickar på.
            //ifbet är en bool så man inte kan betta mitt i omgången
            

            if (ifbet)
            {
                if (e.Y > 724 && e.Y < 800 && e.X > 100 && e.X < 175 && cash >= 10) // Vit
                {
                    cash -= 10;
                    bet += 10;
                }
                else if (e.X < 92 && e.X > 10 && e.Y > 725 && e.Y < 798 && cash >=25)// Röd
                {
                    cash -= 25;
                    bet += 25;
                }
                else if (e.X < 92 && e.X > 19 && e.Y > 633 && e.Y < 707 && cash >=1000)// Svart
                {
                    cash -= 1000;
                    bet += 1000;
                }
                else if (e.X > 102 && e.X < 175 && e.Y > 634 && e.Y < 707 && cash >=500) // Blå
                {
                    cash -= 500;
                    bet += 500;
                }
                else if (e.X > 185 && e.X < 259 && e.Y > 635 && e.Y < 707 && cash >=100) // Röd
                {
                    cash -= 100;
                    bet += 100;
                }
                label1.Text = "Cash: " + cash;
                textBox1.Text = bet.ToString();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Timern finns så att funtioner hinner köras innan timern kollar för blackjack

            if (!ace && !dressed)
            {
                pictureBox12.BackgroundImage = Properties.Resources.Blackjack2;
                pictureBox12.Visible = true;
                if (!splits)
                    cashWin(3, true);
                else
                {
                    cashWin(3, true);
                    return;
                }
            }
            else
            {
                bankList[1].setBackground();
                pictureBox12.BackgroundImage = Properties.Resources.Tie2;
                pictureBox12.Visible = true;
                cashWin(1, true);
            }
            resettimer.Start();
            Blackjacktimer.Stop();
        }
    }
}

