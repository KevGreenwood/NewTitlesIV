using GTA;
using System;
using System.IO;
using System.Windows.Forms;

namespace NewTitlesIV
{
    class Main : Script
    {
        public Texture MissionPassedTexture;
        public Texture WastedTexture;
        public Texture BustedTexture;

        //Settings
        float scale = 1;
        int duration = 5000;

        public Main()
        {
            base.Interval = 10;
            this.Tick += MainClass_Tick;
            MissionPassedTexture = new Texture(File.ReadAllBytes(".\\scripts\\NewTitlesIV\\missionpassed.png"));
            WastedTexture = new Texture(File.ReadAllBytes(".\\scripts\\NewTitlesIV\\wasted.png"));
            BustedTexture = new Texture(File.ReadAllBytes(".\\scripts\\NewTitlesIV\\busted.png"));

            ReadSettings();
        }

        private void ReadSettings()
        {
            scale = Settings.GetValueFloat("Scale", "General", 1);
            duration = Settings.GetValueInteger("Duration", "General", 5000);
        }

        /// <summary>
        /// Main tick of this class. Checks if the Mission Completed Song is playing
        /// </summary>
        private void MainClass_Tick(object sender, EventArgs e)
        {
            try
            {
                if (GTA.Native.Function.Call<bool>("IS_PLAYER_BEING_ARRESTED", new GTA.Native.Parameter("")))
                {
                    this.Wait(3250);
                    base.PerFrameDrawing += DrawBustedTexture;
                    this.Wait(2500);
                    base.PerFrameDrawing -= DrawBustedTexture;
                }
                if (GTA.Native.Function.Call<bool>("IS_PLAYER_DEAD", new GTA.Native.Parameter(0)))
                {
                    base.PerFrameDrawing += DrawWastedTexture;
                    this.Wait(duration);
                    base.PerFrameDrawing -= DrawWastedTexture;
                }

                // Only for IV & TLAD
                if (GTA.Native.Function.Call<bool>("IS_MISSION_COMPLETE_PLAYING", new GTA.Native.Parameter("")))
                {
                    base.PerFrameDrawing += DrawMissionPassedTexture;
                    this.Wait(duration);
                    base.PerFrameDrawing -= DrawMissionPassedTexture;
                }
            }
            catch (Exception ex)
            {
                // DEBUG
                //Game.DisplayText(ex.Message, 10000);
            }
        }

        /// <summary>
        /// Draws the Mission Passed Texture
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Graphics</param>
        private void DrawMissionPassedTexture(object sender, GraphicsEventArgs e)
        {
            e.Graphics.DrawSprite(MissionPassedTexture,
                Screen.PrimaryScreen.WorkingArea.Width / 2,
                Screen.PrimaryScreen.WorkingArea.Height / 2,
                1000 * scale, 390 * scale, 0);
        }
        private void DrawWastedTexture(object sender, GraphicsEventArgs e)
        {
            e.Graphics.DrawSprite(WastedTexture,
                Screen.PrimaryScreen.WorkingArea.Width / 2,
                Screen.PrimaryScreen.WorkingArea.Height / 2,
                1000 * scale, 390 * scale, 0);
        }
        private void DrawBustedTexture(object sender, GraphicsEventArgs e)
        {
            e.Graphics.DrawSprite(BustedTexture,
                Screen.PrimaryScreen.WorkingArea.Width / 2,
                Screen.PrimaryScreen.WorkingArea.Height / 2,
                1000 * scale, 390 * scale, 0);
        }
    }
}
