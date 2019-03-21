using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace pa8_C00124478_C00012875
{
    public partial class Form1 : Form
    {
        List<Button> selfposition;
        List<Button> opponentposition;
        Random r = new Random();
        int totalships = 1;
        int totalopponent = 1;
        int totalselfscore = 0;
        int totalopponentscore =0;
        int rounds = 10;

        public Form1()
        {
            InitializeComponent();
            loadbuttons();
            attack.Enabled = false;
            opponentLocationlist.Text = null;
            
        }

     

        private void selfpicksposition(object sender, EventArgs e)
        {
            if(totalships> 0)
            {
                var button = (Button)sender;
                button.Enabled = false;
                button.Tag = "selfship";
                button.BackColor = System.Drawing.Color.Blue;
                totalships--;
            }

            if(totalships == 0)
            {
                attack.Enabled = true;
                attack.BackColor = System.Drawing.Color.Red;
                helpText.Top = 55;
                helpText.Left = 230;
                helpText.Text = "2) Pick the attack position from the drop down";

            }
        }

        private void attackopponentposition(object sender, EventArgs e)
        {
            if (opponentLocationlist.Text != "")
            {
                var attackPos = opponentLocationlist.Text;
                attackPos = attackPos.ToLower();
                int index = opponentposition.FindIndex(a => a.Name == attackPos);

                if(opponentposition[index].Enabled && rounds > 0)
                {
                    rounds--;
                    roundsText.Text = "Rounds" + rounds;
                    if(opponentposition[index].Tag == "opponentship")
                    {
                        opponentposition[index].Enabled = false;
                        opponentposition[index].BackColor = System.Drawing.Color.DarkBlue;
                        totalselfscore++;
                        selfscore.Text = "" + totalselfscore;
                        opponentplaytimer.Start();
                    }
                    else
                    {
                        opponentposition[index].Enabled = false;
                        opponentposition[index].BackColor = System.Drawing.Color.DarkBlue;
                        opponentplaytimer.Start();
                    }
                }
            }
            else
            {
                MessageBox.Show("Please choose a location from the drop down list");
            }
        }

        private void opponentpicksposition(object sender, EventArgs e)
        {
            int index = r.Next(opponentposition.Count);

            if(opponentposition[index].Enabled == true && opponentposition[index].Tag == null)
            {
                opponentposition[index].Tag = "opponentship";
                totalopponent--;

                Debug.WriteLine("Opponent position " + opponentposition[index].Text);

           
            }
            else
            {
                index = r.Next(opponentposition.Count);
            }

            if(totalopponent < 1)
            {
                opponentpositionpicker.Stop();
            }
        }

        private void opponentattackself(object sender, EventArgs e)
        {
            if(selfposition.Count > 0 && rounds > 0)
            {
                rounds--;
                roundsText.Text = "Rounds " + rounds;

                int index = r.Next(selfposition.Count);

                if(selfposition[index].Tag == "SelfShip")
                {
                    opponentmoves.Text = "" + selfposition[index].Text;
                    selfposition[index].Enabled = false;
                    selfposition[index].BackColor = System.Drawing.Color.DarkBlue;
                    selfposition.RemoveAt(index);
                    totalopponentscore++;
                    opponentscore.Text = " " + totalopponentscore;
                    opponentplaytimer.Stop();
                     
                }
                else
                {
                    opponentmoves.Text = " " + selfposition[index].Text;
                    selfposition[index].Enabled = false;
                    selfposition[index].BackColor = System.Drawing.Color.DarkBlue;
                    selfposition.RemoveAt(index);
                    opponentplaytimer.Stop();

                }
            }

            if(rounds < 1 || totalselfscore > 2 || totalopponentscore > 2)
            {
                if(totalselfscore > totalopponentscore)
                {
                    MessageBox.Show("You win", "winning");
                }
                if(totalselfscore == totalopponentscore)
                {
                    MessageBox.Show("its a draw.");
               
                }

                if(totalopponentscore > totalselfscore)
                {
                    MessageBox.Show("you lost boy!");
                }
            }
        }

        private void loadbuttons()
        {
            selfposition = new List<Button> { a1, a2, a3, a4, a5, b1, b2, b3, b4, b5, c1, c2, c3, c4, c5, d1, d2, d3, d4, d5, e1, e2, e3, e4, e5 };
            opponentposition = new List<Button> {a6,a7,a8,a9,a10,b6,b7,b8,b9,b10,c6,c7,c8,c9,c10,d6,d7,d8,d9,d10,e6,e7,e8,e9,e10};

            for(int i =0;i < opponentposition.Count; i++)
            {
                opponentposition[i].Tag = null;
                opponentLocationlist.Items.Add(opponentposition[i].Text);
            }
                
        }
    }
}
