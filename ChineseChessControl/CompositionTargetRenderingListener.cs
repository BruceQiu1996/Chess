using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows;
using System.Windows.Threading;
using System.Diagnostics;

namespace ChineseChess
{
    internal class CompositionTargetRenderingListener : DispatcherObject, IDisposable
    {
        public CompositionTargetRenderingListener() { }

        public void StartListening()
        {
            requireAccessAndNotDisposed();

            if (!m_isListening)
            {
                m_isListening = true;
                CompositionTarget.Rendering += compositionTarget_Rendering;
            }
        }

        public void StopListening()
        {
            requireAccessAndNotDisposed();

            if (m_isListening)
            {
                m_isListening = false;
                CompositionTarget.Rendering -= compositionTarget_Rendering;
            }
        }

        public void WireParentLoadedUnloaded(FrameworkElement parent)
        {
            requireAccessAndNotDisposed();
            if (parent == null)
            {
                throw new ArgumentNullException("parent");
            }

            parent.Loaded += delegate(object sender, RoutedEventArgs e)
            {
                this.StartListening();
            };

            parent.Unloaded += delegate(object sender, RoutedEventArgs e)
            {
                this.StopListening();
            };
        }

        public bool IsDisposed
        {
            get
            {
                VerifyAccess();
                return m_disposed;
            }
        }

        public event EventHandler Rendering;

        protected virtual void OnRendering(EventArgs args)
        {
            requireAccessAndNotDisposed();

            EventHandler handler = Rendering;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        public void Dispose()
        {
            requireAccessAndNotDisposed();
            StopListening();

            Delegate[] invocationlist = Rendering.GetInvocationList();
            foreach (Delegate d in invocationlist)
            {
                Rendering -= (EventHandler)d;
            }

            m_disposed = true;
        }

        #region Implementation

        [DebuggerStepThrough]
        private void requireAccessAndNotDisposed()
        {
            VerifyAccess();
            if (m_disposed)
            {
                throw new ObjectDisposedException(string.Empty);
            }
        }

        private void compositionTarget_Rendering(object sender, EventArgs e)
        {
            OnRendering(e);
        }

        private bool m_isListening;
        private bool m_disposed;

        #endregion

    }
}
