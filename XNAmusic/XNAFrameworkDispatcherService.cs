﻿using System.Windows.Threading;
using Microsoft.Xna.Framework;
using System.Windows;
using System;

namespace XNAmusic
{
    public class XNAFrameworkDispatcherService : IApplicationService
    {
        // Members
        private DispatcherTimer frameworkDispatcherTimer;

        /// <summary>
        /// Constructor.
        /// </summary>
        public XNAFrameworkDispatcherService()
        {
            this.frameworkDispatcherTimer = new DispatcherTimer();
            this.frameworkDispatcherTimer.Interval = TimeSpan.FromTicks(333333);
            this.frameworkDispatcherTimer.Tick += frameworkDispatcherTimer_Tick;
            FrameworkDispatcher.Update();
        }

        /// <summary>
        /// Calls FrameworkDispatcher.Update()
        /// </summary>
        void frameworkDispatcherTimer_Tick(object sender, EventArgs e)
        {
            FrameworkDispatcher.Update();
        }

        /// <summary>
        /// Starts the dispatcher timer.
        /// </summary>
        void IApplicationService.StartService(ApplicationServiceContext context)
        {
            this.frameworkDispatcherTimer.Start();
        }

        /// <summary>
        /// Stops the dispatcher timer.
        /// </summary>
        void IApplicationService.StopService()
        {
            this.frameworkDispatcherTimer.Stop();
        }
    }

    }
