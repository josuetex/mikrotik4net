//*********************************************************
//  Autogenerated 20.2.2011 22:51:05
//*********************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
!re
=.id=*0
=time=feb/07 12:00:07
=topics=system,info
=message=router rebooted
*/
namespace Tik4Net.Objects
{
    /// <summary>
    /// Represents one row in /log on mikrotik router.
    /// </summary>
    [TikEntity("/log", TikEntityEditMode.ReadOnly)]    
    public sealed partial class Log: TikEntityBase
    {
        /// <summary>
        /// Row message property.
        /// </summary>
        [TikProperty("message", typeof(string), false, TikPropertyEditMode.Editable)]
        public string Message 
        { 
            get { return Properties.GetAsStringOrNull("message"); }
            // Entity R/O set { Properties.SetAttribute("message", value); }
        }

        /// <summary>
        /// Row time property.
        /// </summary>
        [TikProperty("time", typeof(string), false, TikPropertyEditMode.Editable)]
        public string Time 
        { 
            get { return Properties.GetAsStringOrNull("time"); }
            // Entity R/O set { Properties.SetAttribute("time", value); }
        }        	

        /// <summary>
        /// Row topics property.
        /// </summary>
        [TikProperty("topics", typeof(string), false, TikPropertyEditMode.Editable)]
        public string Topics 
        { 
            get { return Properties.GetAsStringOrNull("topics"); }
            // Entity R/O set { Properties.SetAttribute("topics", value); }
        }        	
    }
    
    /// <summary>
    /// Represents list of rows in /log on mikrotik router.
    /// </summary>    
    public sealed partial class LogList : TikUnorderedList<Log>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogList"/> class.
        /// Default active session (<see cref="TikSession.ActiveSession"/> is used).
        /// </summary>
        public LogList() 
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogList"/> class.
        /// </summary>
        /// <param name="session">The session used to access mikrotik.</param>
        public LogList(TikSession session)
            : base(session)
        {
        }
    }           
}