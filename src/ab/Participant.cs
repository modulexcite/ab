namespace ab
{
    public class Participant
    {
        /// <summary>
        /// The unique identity of this participant
        /// </summary>
        public string Identity { get; set; }

        /// <summary>
        /// The alternative shown to this participant, by index
        /// </summary>
        public int? Shown { get; set; }

        /// <summary>
        /// The total number of times the alternative was shown to the participant
        /// </summary>
        public int Seen { get; set; }

        /// <summary>
        /// Whether the participant converted on the alternative shown
        /// </summary>
        public bool Converted
        {
            get { return Conversions > 0; }
        }

        /// <summary>
        /// The number of times the participant acted on the alternative
        /// </summary>
        public int Conversions { get; set; }
    }
}