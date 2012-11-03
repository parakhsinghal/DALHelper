using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DALHelper
{
    /// <summary>
    /// The enum is used to suggest whether the passed sql text is a 
    /// SQL query or a stored procedure.
    /// </summary>
    public enum SQLTextType
    {
        /// <summary>
        /// Signifies that passed sql text is an ad hoc SQL query.
        /// </summary>
        Query=  0,

        /// <summary>
        /// Signifies that passed sql text is a stored procedure.
        /// </summary>
        Stored_Proc = 1
    }
}
