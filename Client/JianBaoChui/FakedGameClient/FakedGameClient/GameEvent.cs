using System.Drawing;

namespace FakedGameClient
{
    public partial class Game
    {
        public delegate void DisConnectedEventHandler(object sender);
        public event DisConnectedEventHandler DisConnectedEvent;

        public delegate void ConnectedEventHandler(object sender);
        public event ConnectedEventHandler ConnectedEvent;

        public delegate void UnSignedEventHandler(object sender);
        public event UnSignedEventHandler UnSignedEvent;

        public delegate void SignedEventHandler(object sender);
        public event SignedEventHandler SignedEvent;

        public delegate void WatchingEventHandler(object sender);
        public event WatchingEventHandler WatchingEvent;

        public delegate void BeforeKOStartEventHandler(object sender);
        public event BeforeKOStartEventHandler BeforeKOStartEvent;

        public delegate void KOStartEventHandler(object sender);
        public event KOStartEventHandler KOStartEvent;

        public delegate void KOEventHandler(object sender);
        public event KOEventHandler KOEvent;

        public delegate void KOEndEventHandler(object sender);
        public event KOEndEventHandler KOEndEvent;

        public delegate void BeforePKStartEventHandler(object sender);
        public event BeforePKStartEventHandler BeforePKStartEvent;

        public delegate void BetStartEventHandler(object sender);
        public event BetStartEventHandler BetStartEvent;

        public delegate void BetEndEventHandler(object sender);
        public event BetEndEventHandler BetEndEvent;

        public delegate void PKStartEventHandler(object sender);
        public event PKStartEventHandler PKStartEvent;

        public delegate void PKEventHandler(object sender);
        public event PKEventHandler PKEvent;

        public delegate void PKEndEventHandler(object sender);
        public event PKEndEventHandler PKEndEvent;

        public delegate void BeforeRoundStartEventHandler(object sender);
        public event BeforeRoundStartEventHandler BeforeRoundStartEvent;

        public delegate void RoundStartEventHandler(object sender);
        public event RoundStartEventHandler RoundStartEvent;

        public delegate void InRoundEventHandler(object sender);
        public event InRoundEventHandler InRoundEvent;

        public delegate void RoundDrawHandler(object sender);
        public event RoundDrawHandler RoundDrawEvent;

        public delegate void RoundEndEventHandler(object sender);
        public event RoundEndEventHandler RoundEndEvent;

        public delegate void LogEventHandler(object sender, string content, KnownColor color = KnownColor.White);
        public event LogEventHandler LogEvent;

        public delegate void GameReportEventHandler(object sender, string content, KnownColor color = KnownColor.DarkGreen);
        public event GameReportEventHandler GameReportEvent;

        public delegate void AnnounceEventHandler(object sender, string content, KnownColor color = KnownColor.Yellow);
        public event AnnounceEventHandler AnnounceEvent;

    }
}
