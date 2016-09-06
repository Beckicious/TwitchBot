﻿using System;
using System.Windows.Forms;
using TwitchBot;
using RawChatModule;
using CleanChatModule;
using UptimeModule;
using DashboardModule;
using EightBallModule;

namespace TwitchBotStarter
{
    public partial class TwitchBotStarterForm : Form
    {
        private BasicTwitchBot tb;

        //channel which will be connected to
        private const string channelName = "beckicious";

        public TwitchBotStarterForm()
        {
            tb = new BasicTwitchBot("samplebot", "oauth:kj299d8n505eqn861pct0lrceoseph");
            
            //Add Modules here
            tb.AddModule(new RawChat());
            tb.AddModule(new CleanChat());
            tb.AddModule(new Uptime());
            tb.AddModule(new Dashboard());
            tb.AddModule(new EightBall());


            InitializeComponent();



            foreach(TwitchBotModule mod in tb.GetModules())
            {
                comModules.Items.Add(mod.GetName());
                mod.Activate();
            }

            tb.Connect();

        }

        private void butJoin_Click(object sender, EventArgs e)
        {
            tb.JoinChannel(channelName);
        }

        private void butLeave_Click(object sender, EventArgs e)
        {
            tb.LeaveChannel();
        }

        private void comModules_SelectedIndexChanged(object sender, EventArgs e)
        {
            var module = comModules.Text;

            foreach (TwitchBotModule mod in tb.GetModules())
            {
                if (mod.GetName() == module)
                {
                    mod.Open();
                    break;
                }
            }
        }
    }
}
