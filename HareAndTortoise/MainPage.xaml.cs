using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace HareAndTortoise
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void btnRace_Click(object sender, RoutedEventArgs e)
        {
            //Variables
            int distance = 0;
            int restInc;
            int restStart;
            //tortoise
            int tMinSpeed = 0;
            int tMaxSpeed = 0;
            //hare
            int hMinSpeed = 0;
            int hMaxSpeed = 0;

            //In
            distance = ErrorCheck(tbDistance.Text, 100, 1000);
            SpeedCheck(tbTSpeedMin.Text, tbTSpeedMax.Text, 1, 20, out tMinSpeed, out tMaxSpeed );
            SpeedCheck(tbHSpeedMin.Text, tbHSpeedMax.Text, 1, 20, out hMinSpeed, out hMaxSpeed);
            restInc = ErrorCheck(tbHRestIncrease.Text, 1, 25);
            restStart = ErrorCheck(tbHRestBase.Text, 1, 100);
            
            if (distance == 0 || tMinSpeed == 0 || tMaxSpeed == 0 || hMinSpeed == 0 || hMaxSpeed == 0 || restInc == 0 || restStart == 0)
            {
                tbWin.Text = "One or more textbox has invalid text. Please fill in all boxes and ensure minimum speeds are lower than maximum speeds";
            }
            else
            {
                race(distance, tMinSpeed, tMaxSpeed, hMinSpeed, hMaxSpeed, restInc, restStart);
            }
        }

        private int ErrorCheck(string toCheck, int minVal, int maxVal)//0 means invalid
        {
            int check = 0;//temp Var to carry number.
            if (toCheck == "")//Check for blank string
            {
                return 0;
            }
            else
            {
                check = Int16.Parse(toCheck);//convert to integer
                if (check < minVal)//less than 100m
                {
                    return 0;
                }
                else if (check > maxVal)//greater than 1000m
                {
                    return 0;
                }
                else//acceptable value
                {
                    return check;//return checked value to variable
                }
            }
        }

        private void SpeedCheck(string minSpeed, string maxSpeed, int minVal, int maxVal, out int minChecked, out int maxChecked)//Checking against two values for speeds
        {
            //min\maxCHECK - internal check vars, min\maxCHECKED - returned values
            int minCheck = 0;//temp Var to carry number.
            int maxCheck = 0;
            if (minSpeed == "" || maxSpeed == "")//Check for blank string
            {
                minChecked = 0;
                maxChecked = 0;
            }
            else
            {
                minCheck = Int16.Parse(minSpeed);//convert to integer
                maxCheck = Int16.Parse(maxSpeed);
                if (minCheck < minVal || maxCheck < minVal)//less than 
                {
                    minChecked = 0;
                    maxChecked = 0;
                }
                else if (minCheck > maxVal || maxCheck > maxVal)//greater than 
                {
                    minChecked = 0;
                    maxChecked = 0;
                }
                else if (minCheck > maxCheck || maxCheck < minCheck)
                {
                    minChecked = 0;
                    maxChecked = 0;
                }
                else//acceptable value
                {
                    minChecked = minCheck;
                    maxChecked = maxCheck;//return checked value to variable
                }
            }
        }

        private void race(int distance, int tMinSpeed, int tMaxSpeed, int hMinSpeed, int hMaxSpeed, int restIncrease, int restChance)
        {
            int tLocation = 0;
            int hlocation = 0;
            int restRand = 0;


            Random random = new Random();

            //Process
            while((tLocation <= distance) && (hlocation <= distance))
            {
                restRand = random.Next(0,100);//generate rest chance (0 -> 100%)
                if (restRand <= (100 - restChance))
                {
                    hlocation += random.Next(hMinSpeed, hMaxSpeed);
                    restChance += restIncrease;
                    if (restChance > 100)//Stops over 100% chance
                    {
                        restChance = 100;
                    }
                }
                else
                {
                    restChance -= (restIncrease * 2);
                    if (restChance < 1) //Stops negative chance
                    {
                        restChance = 0;
                    }
                }
                tLocation += random.Next(tMinSpeed, tMaxSpeed);//always change

                tbWin.Text = hlocation.ToString();
            } 

            //Out
            if ((tLocation >= distance) && (hlocation >= distance))
            {
                tbWin.Text = "Its a Draw!";
            }
            else if (tLocation >= distance)
            {
                tbWin.Text = "Tortoise Wins!";
            }
            else if (hlocation >= distance)
            {
                tbWin.Text = "Hare Wins!";
            }
        }    
        
    }
}
