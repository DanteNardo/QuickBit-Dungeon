﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace QuickBit_Dungeon.MANAGERS
{
    public static class AudioManager
    {
        // ======================================
		// ============== Members ===============
		// ======================================
		
		private static Song MainMusic { get; set; }
        private static SoundEffect BitHit { get; set; }
        private static SoundEffect PlayerHit { get; set; }
        private static SoundEffect PlayerSpecial { get; set; }
        private static SoundEffect PlayerWalk { get; set; }
        private static SoundEffect MenuSound { get; set; }

		// ======================================
		// ============== Methods ===============
		// ======================================

        /// <summary>
        /// Loads and saves all audio content
        /// into the static class.
        /// </summary>
        /// <param name="cm"></param>
        public static void LoadContent(ContentManager cm)
        {
            //MainMusic = cm.Load<Song>("Audio/mainMusic");
            BitHit = cm.Load<SoundEffect>("Audio/bitHit");
            PlayerHit = cm.Load<SoundEffect>("Audio/playerHit");
            PlayerSpecial = cm.Load<SoundEffect>("Audio/playerSpecial");
            PlayerWalk = cm.Load<SoundEffect>("Audio/playerWalk");
            MenuSound = cm.Load<SoundEffect>("Audio/menuSound");
        }

        /// <summary>
        /// Plays the main game music.
        /// </summary>
        public static void PlayMainMusic()
        {
	        MediaPlayer.Play(MainMusic);
	        MediaPlayer.IsRepeating = true;
	        MediaPlayer.Volume = 0.7F;
        }

        /// <summary>
        /// Plays a single bithit sound effect.
        /// </summary>
        public static void NewBitHit()
        {
            BitHit.Play();
        }

        /// <summary>
        /// Plays a single playerhit sound effect.
        /// </summary>
        public static void NewPlayerHit()
        {
            PlayerHit.Play();
        }

        /// <summary>
        /// Plays a single playerspecial sound effect.
        /// </summary>
        public static void NewPlayerSpecial()
        {
            PlayerSpecial.Play();
        }

        /// <summary>
        /// Plays a single playerwalk sound effect.
        /// </summary>
        public static void NewPlayerWalk()
        {
            PlayerWalk.Play();
        }

        /// <summary>
        /// Plays a single menusound sound effect.
        /// </summary>
        public static void NewMenuSound()
        {
            MenuSound.Play();
        }
    }
}
