using System.Collections.Generic;

namespace prediction.io
{
    /// <summary>
    ///     Return root model of Engine Client
    /// </summary>
    public partial class ItemScoresRootObject
    {
        public IList<ItemScore> ItemScores { get; set; }
    }

    /// <summary>
    ///     Return model of Engine Client
    /// </summary>
    public partial class ItemScore
    {
        public string Item { get; set; }
        public double Score { get; set; }
    }
}