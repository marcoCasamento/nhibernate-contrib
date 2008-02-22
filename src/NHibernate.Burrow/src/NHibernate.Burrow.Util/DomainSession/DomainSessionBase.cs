using System;

namespace NHibernate.Burrow.Util.DomainSession {
    /// <summary>
    /// DomainSession is data container for a domainLayer. Helper class with states can be stored in 
    /// this container
    /// </summary>
    /// <remarks>
    /// DomainSession is not thread safe. If you use MHGeneralLib then its lifetime is decided by the MHGeneralLib
    /// Normally the domain Layer has the same lifetime as a HttpSession if the domain is runned under a httpApplication.
    /// So, please avoid store persistent objects in the domainLayer. It's mainly for storing non-entity ojbects.
    /// </remarks>
    public abstract class DomainSessionBase : IDomainSession {
        private bool isDisposing = false;

        #region static part

        /// <summary>
        /// Return the current layer from the current Container's DomainSession property
        /// if Container is not used, then it will use a treadStatic instance;
        /// </summary>
        /// <remarks>
        /// This should be thread safe since dlc.DomainSession should be thread safe.
        /// </remarks>
        public static DomainSessionBase Current {
            get {
                if (DomainSessionContainer.DomainSession == null)
                    DomainSessionContainer.DomainSession = GetInstance();
                return (DomainSessionBase) DomainSessionContainer.DomainSession;
            }
        }

        private static DomainSessionBase GetInstance() {
            return (DomainSessionBase) DSConfig.GetDomainSessionFactory().Create();
        }

        /// <summary>
        /// 
        /// </summary>
        ~DomainSessionBase() {
            Dispose();
        }

        #endregion static part

        /// <summary>
        /// before the Domain Layer get closed
        /// </summary>
        public event EventHandler PreClose;

        #region public methods

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        public void Dispose() {
            if (isDisposing) return;
            isDisposing = true;
        }

        /// <summary>
        /// Call it when a http requests ends
        /// </summary>
        public void Close() {
            if (PreClose != null) PreClose(this, new EventArgs());
        }

        /// <summary>
        /// Call it when a http request starts
        /// </summary>
        public virtual void Open() {}

        #endregion public methods
    }
}