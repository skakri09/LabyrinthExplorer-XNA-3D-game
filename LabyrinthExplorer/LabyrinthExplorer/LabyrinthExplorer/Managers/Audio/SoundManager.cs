//CRedits goes to : http://www.xnawiki.com/index.php/Advanced_Sound_Manager_For_XNA_GS_3.1
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

/// <summary>
/// Manages playback of sounds and music.
/// </summary>

namespace LabyrinthExplorer
{
    public struct SoundLoopInfo
    {
        public SoundLoopInfo(int timesToPlay)
        {
            this.timesToPlay = timesToPlay;
            timesPlayed = 1;
        }
        public int timesPlayed;
        public int timesToPlay;
        
    }

    public struct PlayingSound
    {
        public SoundEffectInstance instance;
        public string keyName;
        public I3DSound owner;
        public bool isStoppable;
    }

    public class AudioManager : GameComponent
    {
        
        #region Private fields
        private ContentManager _content;
        private string sfxContentPath = @"Sound\Effects\";
        private string songContentpath = @"Sound\Music\";

        private Dictionary<string, Song> _songs = new Dictionary<string, Song>();
        private Dictionary<string, SoundEffect> _sounds = new Dictionary<string, SoundEffect>();
        private Dictionary<SoundEffectInstance, SoundLoopInfo> loopingEffects = new Dictionary<SoundEffectInstance, SoundLoopInfo>();

        private Song _currentSong = null;

        private PlayingSound[] _playingSounds = new PlayingSound[MaxSounds];

        private bool _isMusicPaused = false;

        private bool _isFading = false;
        private MusicFadeEffect _fadeEffect;
        #endregion

        // Change MaxSounds to set the maximum number of simultaneous sounds that can be playing.
        private const int MaxSounds = 32;

        /// <summary>
        /// Gets the name of the currently playing song, or null if no song is playing.
        /// </summary>
        public string CurrentSong { get; private set; }

        /// <summary>
        /// Gets or sets the volume to play songs. 1.0f is max volume.
        /// </summary>
        public float MusicVolume
        {
            get { return MediaPlayer.Volume; }
            set { MediaPlayer.Volume = value; }
        }

        /// <summary>
        /// Gets or sets the master volume for all sounds. 1.0f is max volume.
        /// </summary>
        public float SoundVolume
        {
            get { return SoundEffect.MasterVolume; }
            set { SoundEffect.MasterVolume = value; }
        }

        /// <summary>
        /// Gets whether a song is playing or paused (i.e. not stopped).
        /// </summary>
        public bool IsSongActive { get { return _currentSong != null && MediaPlayer.State != MediaState.Stopped; } }

        /// <summary>
        /// Gets whether the current song is paused.
        /// </summary>
        public bool IsSongPaused { get { return _currentSong != null && _isMusicPaused; } }

        /// <summary>
        /// Creates a new Audio Manager. Add this to the Components collection of your Game.
        /// </summary>
        /// <param name="game">The Game</param>
        public AudioManager(Game game)
            : base(game)
        {
            _content = new ContentManager(game.Content.ServiceProvider, game.Content.RootDirectory);
        }

        /// <summary>
        /// Creates a new Audio Manager. Add this to the Components collection of your Game.
        /// </summary>
        /// <param name="game">The Game</param>
        /// <param name="contentFolder">Root folder to load audio content from</param>
        public AudioManager(Game game, string contentFolder)
            : base(game)
        {
            _content = new ContentManager(game.Content.ServiceProvider, contentFolder);
        }

        public void LoadContent()
        {
            
            LoadEffect("ChestClose", "");
            LoadEffect("ChestOpen", "");
            LoadEffect("LeverUsed", "");
            LoadEffect("GateDoorOpening", "");
            LoadEffect("GateDoorClosing", "");
            LoadEffect("Clock", "");
            LoadEffect("footsteps", "");
            LoadEffect("Loot", "");
            LoadEffect("PortalUse", "");
            LoadEffect("Portal1", "");
            LoadEffect("Portal2", "");
            LoadEffect("Portal3", "");
            LoadEffect("SpiderCurry2", "");
            LoadEffect("SpiderCurry1", "");
            LoadEffect("SpiderSteps", "");
            LoadEffect("GemPickup", "");
            LoadEffect("RedAura", "");
            LoadEffect("BlueAura", "");
            LoadEffect("YellowAura", "");
            LoadEffect("GemPuzzleOpen", "");
            LoadEffect("DoorOpen", "");
            LoadEffect("RedGemEntered", "");
            LoadEffect("YellowGemEntered", "");
            LoadEffect("BlueGemEntered", "");
            LoadEffect("PillarRotate", "");
            LoadEffect("GroundShaking", "");
            LoadEffect("ThereIsNoEscape", "");
            LoadEffect("DeathIsClose", "");
            LoadEffect("YouAreWeak", "");
            LoadEffect("HopeIsAnIllusion", "");
            LoadEffect("TheyAreComingForYou", "");
            LoadEffect("GiveInToYourFear", "");
            LoadEffect("PedistalSound", "");
            LoadEffect("GateOpening1", "");
            LoadEffect("GateOpening2", "");
            LoadEffect("GateOpening3", "");
            LoadEffect("apollo11", "");
            LoadEffect("alex", "");
            LoadEffect("egypt", "");
            LoadEffect("arena", "");
            LoadEffect("ww2", "");

            LoadAmbient("Area2Ambient", "");
            LoadAmbient("spiderAmbient", "");
            
            LoadSong("LOD", "");
            LoadSong("space", "");
        }

        /// <summary>
        /// Loads a Song into the AudioManager.
        /// </summary>
        /// <param name="songName">Name of the song to load</param>
        public void LoadSong(string songName, string subDirectory)
        {
            if (_songs.ContainsKey(songName))
            {
                throw new InvalidOperationException(string.Format("Song '{0}' has already been loaded", songName));
            }

            _songs.Add(songName, _content.Load<Song>(songContentpath + "/Music/" + subDirectory + "/" + songName));
           
        }

        /// <summary>
        /// Loads a SoundEffect into the AudioManager.
        /// </summary>
        /// <param name="soundName">Name of the sound to load</param>
        /// <param name="soundPath">Path to the song asset file</param>
        public void LoadEffect(string soundName, string subDirectory)
        {
            if (_sounds.ContainsKey(soundName))
            {
                throw new InvalidOperationException(string.Format("Sound '{0}' has already been loaded", soundName));
            }

            _sounds.Add(soundName, _content.Load<SoundEffect>(sfxContentPath + subDirectory + "/" + soundName));
        }

        public void LoadAmbient(string soundName, string subDirectory)
        {
             if (_sounds.ContainsKey(soundName))
            {
                throw new InvalidOperationException(string.Format("Sound '{0}' has already been loaded", soundName));
            }

            _sounds.Add(soundName, _content.Load<SoundEffect>(songContentpath + "/ambient/" +subDirectory+ "/" + soundName));
        }
        
        /// <summary>
        /// Unloads all loaded songs and sounds.
        /// </summary>
        public void UnloadContent()
        {
            _content.Unload();
        }

        /// <summary>
        /// Starts playing the song with the given name. If it is already playing, this method
        /// does nothing. If another song is currently playing, it is stopped first.
        /// </summary>
        /// <param name="songName">Name of the song to play</param>
        public void PlaySong(string songName)
        {
            PlaySong(songName, false);
        }

        /// <summary>
        /// Starts playing the song with the given name. If it is already playing, this method
        /// does nothing. If another song is currently playing, it is stopped first.
        /// </summary>
        /// <param name="songName">Name of the song to play</param>
        /// <param name="loop">True if song should loop, false otherwise</param>
        public void PlaySong(string songName, bool loop)
        {
            if (CurrentSong != songName)
            {
                if (_currentSong != null)
                {
                    MediaPlayer.Stop();
                }

                if (!_songs.TryGetValue(songName, out _currentSong))
                {
                    throw new ArgumentException(string.Format("Song '{0}' not found", songName));
                }

                CurrentSong = songName;

                _isMusicPaused = false;
                MediaPlayer.IsRepeating = loop;
                MediaPlayer.Play(_currentSong);

                if (!Enabled)
                {
                    MediaPlayer.Pause();
                }
            }
        }

        /// <summary>
        /// Pauses the currently playing song. This is a no-op if the song is already paused,
        /// or if no song is currently playing.
        /// </summary>
        public void PauseSong()
        {
            if (_currentSong != null && !_isMusicPaused)
            {
                if (Enabled) MediaPlayer.Pause();
                _isMusicPaused = true;
            }
        }

        /// <summary>
        /// Resumes the currently paused song. This is a no-op if the song is not paused,
        /// or if no song is currently playing.
        /// </summary>
        public void ResumeSong()
        {
            if (_currentSong != null && _isMusicPaused)
            {
                if (Enabled) MediaPlayer.Resume();
                _isMusicPaused = false;
            }
        }

        /// <summary>
        /// Stops the currently playing song. This is a no-op if no song is currently playing.
        /// </summary>
        public void StopSong()
        {
            if (_currentSong != null && MediaPlayer.State != MediaState.Stopped)
            {
                MediaPlayer.Stop();
                _isMusicPaused = false;
            }
        }

        public void StopSound(string soundName)
        {
            for (int i = 0; i < _playingSounds.Length; ++i)
            {
                if (_playingSounds[i].instance != null 
                    && _playingSounds[i].instance.State == SoundState.Playing
                    && _playingSounds[i].keyName == soundName)
                {
                    _playingSounds[i].instance.Dispose();
                    _playingSounds[i].instance = null;
                }
            }
        }

        /// <summary>
        /// Smoothly transition between two volumes.
        /// </summary>
        /// <param name="targetVolume">Target volume, 0.0f to 1.0f</param>
        /// <param name="duration">Length of volume transition</param>
        public void FadeSong(float targetVolume, TimeSpan duration)
        {
            if (duration <= TimeSpan.Zero)
            {
                throw new ArgumentException("Duration must be a positive value");
            }

            _fadeEffect = new MusicFadeEffect(MediaPlayer.Volume, targetVolume, duration);
            _isFading = true;
        }

        /// <summary>
        /// Stop the current fade.
        /// </summary>
        /// <param name="option">Options for setting the music volume</param>
        public void CancelFade(FadeCancelOptions option)
        {
            if (_isFading)
            {
                switch (option)
                {
                    case FadeCancelOptions.Source: MediaPlayer.Volume = _fadeEffect.SourceVolume; break;
                    case FadeCancelOptions.Target: MediaPlayer.Volume = _fadeEffect.TargetVolume; break;
                }

                _isFading = false;
            }
        }

        /// <summary>
        /// Plays the sound of the given name.
        /// </summary>
        /// <param name="soundName">Name of the sound</param>
        public void PlaySound(string soundName, I3DSound owner = null, int loopAmnt = 0, bool isStoppable = true)
        {
            PlaySound(soundName, 1.0f, 0.0f, 0.0f, owner, loopAmnt, isStoppable);
        }

        /// <summary>
        /// Plays the sound of the given name at the given volume.
        /// </summary>
        /// <param name="soundName">Name of the sound</param>
        /// <param name="volume">Volume, 0.0f to 1.0f</param>
        public void PlaySound(string soundName, float volume, I3DSound owner = null, int loopAmnt = 0, bool isStoppable = true)
        {
            PlaySound(soundName, volume, 0.0f, 0.0f, owner, loopAmnt, isStoppable);
        }

        /// <summary>
        /// Plays the sound of the given name with the given parameters.
        /// </summary>
        /// <param name="soundName">Name of the sound</param>
        /// <param name="volume">Volume, 0.0f to 1.0f</param>
        /// <param name="pitch">Pitch, -1.0f (down one octave) to 1.0f (up one octave)</param>
        /// <param name="pan">Pan, -1.0f (full left) to 1.0f (full right)</param>
        public void PlaySound(string soundName, float volume, float pitch, float pan, I3DSound owner = null, int loopAmnt = 0, bool isStoppable = true)
        {
            SoundEffect sound;

            if (!_sounds.TryGetValue(soundName, out sound))
            {
                throw new ArgumentException(string.Format("Sound '{0}' not found", soundName));
            }

            int index = GetAvailableSoundIndex();
            _playingSounds[index].keyName = soundName;
            _playingSounds[index].owner = owner;
            _playingSounds[index].isStoppable = isStoppable;
            if (index != -1)
            {
                _playingSounds[index].instance = sound.CreateInstance();
                _playingSounds[index].instance.Volume = volume;
                _playingSounds[index].instance.Pitch = pitch;
                _playingSounds[index].instance.Pan = pan;
                if(loopAmnt == -1)
                    _playingSounds[index].instance.IsLooped = true;
                if (owner != null && owner.GetAudioEmitter() != null)
                {
                    _playingSounds[index].instance.Apply3D(NormalizedListener(Player.PlayerListener),
                        NormalizedEmitter(owner.GetAudioEmitter()));
                }

                _playingSounds[index].instance.Play();
               
                if (loopAmnt > 0)
                {
                    loopingEffects.Add(_playingSounds[index].instance, new SoundLoopInfo(loopAmnt));
                }
                
                if (!Enabled)
                {
                    _playingSounds[index].instance.Pause();
                }
            }
        }

        /// <summary>
        /// Stops all currently playing sounds.
        /// </summary>
        public void StopAllSounds()
        {
            for (int i = 0; i < _playingSounds.Length; ++i)
            {
                if (_playingSounds[i].instance != null && _playingSounds[i].isStoppable == true)
                {
                    _playingSounds[i].instance.Stop();
                    _playingSounds[i].instance.Dispose();
                    _playingSounds[i].instance = null;
                }
            }
        }

        /// <summary>
        /// Called per loop unless Enabled is set to false.
        /// </summary>
        /// <param name="gameTime">Time elapsed since last frame</param>
        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < _playingSounds.Length; ++i)
            {
                if (_playingSounds[i].instance != null && _playingSounds[i].instance.State == SoundState.Stopped)
                {
                   SoundLoopInfo thisEffect;
                   if (loopingEffects.TryGetValue(_playingSounds[i].instance, out thisEffect))
                   {
                       if (thisEffect.timesPlayed < thisEffect.timesToPlay)
                       {
                           thisEffect.timesPlayed++;
                           loopingEffects[_playingSounds[i].instance] = thisEffect;
                           _playingSounds[i].instance.Play();
                       }
                       else
                       {
                           loopingEffects.Remove(_playingSounds[i].instance);
                           _playingSounds[i].instance.Dispose();
                           _playingSounds[i].instance = null;
                       }
                   }
                    else
                    {
                        _playingSounds[i].instance.Dispose();
                        _playingSounds[i].instance = null;
                    }
                }
                else if (_playingSounds[i].owner != null && _playingSounds[i].owner.GetAudioEmitter() != null
                    && _playingSounds[i].instance != null && _playingSounds[i].instance.State == SoundState.Playing )
                {
                    if (_playingSounds[i].owner is I3DSoundCustDivFact)
                    {
                        I3DSoundCustDivFact owner = (I3DSoundCustDivFact)_playingSounds[i].owner;
                        _playingSounds[i].instance.Apply3D(NormalizedListener(Player.PlayerListener, owner.GetCustomDivisionFactor()),
                        NormalizedEmitter(_playingSounds[i].owner.GetAudioEmitter(), owner.GetCustomDivisionFactor()));
                    }
                    else
                    {
                        _playingSounds[i].instance.Apply3D(NormalizedListener(Player.PlayerListener),
                        NormalizedEmitter(_playingSounds[i].owner.GetAudioEmitter()));
                    }
                    
                }
            }

            if (_currentSong != null && MediaPlayer.State == MediaState.Stopped)
            {
                _currentSong = null;
                CurrentSong = null;
                _isMusicPaused = false;
            }

            if (_isFading && !_isMusicPaused)
            {
                if (_currentSong != null && MediaPlayer.State == MediaState.Playing)
                {
                    if (_fadeEffect.Update(gameTime.ElapsedGameTime))
                    {
                        _isFading = false;
                    }

                    MediaPlayer.Volume = _fadeEffect.GetVolume();
                }
                else
                {
                    _isFading = false;
                }
            }

            base.Update(gameTime);
        }

        // Pauses all music and sound if disabled, resumes if enabled.
        protected override void OnEnabledChanged(object sender, EventArgs args)
        {
            if (Enabled)
            {
                for (int i = 0; i < _playingSounds.Length; ++i)
                {
                    if (_playingSounds[i].instance != null && _playingSounds[i].instance.State == SoundState.Paused)
                    {
                        _playingSounds[i].instance.Resume();
                    }
                }

                if (!_isMusicPaused)
                {
                    MediaPlayer.Resume();
                }
            }
            else
            {
                for (int i = 0; i < _playingSounds.Length; ++i)
                {
                    if (_playingSounds[i].instance != null && _playingSounds[i].instance.State == SoundState.Playing)
                    {
                        _playingSounds[i].instance.Pause();
                    }
                }

                MediaPlayer.Pause();
            }

            base.OnEnabledChanged(sender, args);
        }


        private AudioListener NormalizedListener(AudioListener originalListener, float customDivisionFactor=0)
        {
            AudioListener listener = originalListener;
            Vector3 position = originalListener.Position;
            
            if (customDivisionFactor == 0)
                position /= GameConstants.Audio3DFDivisionFactor;
            else
                position /= customDivisionFactor;

            listener.Position = position;
            return listener;
        }
        private AudioEmitter NormalizedEmitter(AudioEmitter originalEmitter, float customDivisionFactor = 0)
        {
            AudioEmitter emitter = originalEmitter;
            Vector3 position = originalEmitter.Position;
            
            if (customDivisionFactor == 0)
                position /= GameConstants.Audio3DFDivisionFactor;
            else
                position /= customDivisionFactor;

            emitter.Position = position;
            return emitter;
        }

        // Acquires an open sound slot.
        private int GetAvailableSoundIndex()
        {
            for (int i = 0; i < _playingSounds.Length; ++i)
            {
                if (_playingSounds[i].instance == null)
                {
                    return i;
                }
            }

            return -1;
        }

        #region MusicFadeEffect
        private struct MusicFadeEffect
        {
            public float SourceVolume;
            public float TargetVolume;

            private TimeSpan _time;
            private TimeSpan _duration;

            public MusicFadeEffect(float sourceVolume, float targetVolume, TimeSpan duration)
            {
                SourceVolume = sourceVolume;
                TargetVolume = targetVolume;
                _time = TimeSpan.Zero;
                _duration = duration;
            }

            public bool Update(TimeSpan time)
            {
                _time += time;

                if (_time >= _duration)
                {
                    _time = _duration;
                    return true;
                }

                return false;
            }

            public float GetVolume()
            {
                return MathHelper.Lerp(SourceVolume, TargetVolume, (float)_time.Ticks / _duration.Ticks);
            }
        }
        #endregion
    }

    /// <summary>
    /// Options for AudioManager.CancelFade
    /// </summary>
    public enum FadeCancelOptions
    {
        /// <summary>
        /// Return to pre-fade volume
        /// </summary>
        Source,
        /// <summary>
        /// Snap to fade target volume
        /// </summary>
        Target,
        /// <summary>
        /// Keep current volume
        /// </summary>
        Current
    }
}