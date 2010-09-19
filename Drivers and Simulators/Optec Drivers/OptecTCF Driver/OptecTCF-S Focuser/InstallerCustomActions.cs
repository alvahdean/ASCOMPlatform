﻿using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;
using Optec;

namespace ASCOM.OptecTCF_S
{
    /// <summary>
    /// Custom install actions that must be carried out during product installation.
    /// </summary>
    [RunInstaller(true)]
    public class ComRegistration : Installer
    {
        /// <summary>
        /// Custom Install Action that regsiters the driver with COM Interop.
        /// Note that this will in turn trigger an methods with the [ComRegisterFunction()] attribute
        /// such as those in Driver.cs that perform ASCOM registration.
        /// </summary>
        /// <param name="stateSaver">Not used.<see cref="Installer"/></param>
        public override void Install(System.Collections.IDictionary stateSaver)
        {
            string msg = "Install custom action - Starting registration for COM Interop";
            EventLogger.LogMessage(msg, TraceLevel.Warning);
#if DEBUG
            MessageBox.Show("Attach debugger to this process now, if required", "Custom Action Debug", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
#endif
            base.Install(stateSaver);

            RegistrationServices regsrv = new RegistrationServices();
            if (!regsrv.RegisterAssembly(this.GetType().Assembly, AssemblyRegistrationFlags.SetCodeBase))
            {
                EventLogger.LogMessage("Installer failed to register driver for COM Interop", TraceLevel.Warning);
                throw new InstallException("Failed To Register driver for COM Interop");
            }
            EventLogger.LogMessage("ASCOM Registration completed successfully", TraceLevel.Warning);
          
        }

        /// <summary>
        /// Custom Install Action that removes the COM Interop component registrations.
        /// Note that this will in turn trigger any methods with the [ComUnregisterFunction()] attribute
        /// such as those in Driver.cs that remove the ASCOM registration.
        /// </summary>
        /// <param name="savedState">Not used.<see cref="Installer"/></param>
        public override void Uninstall(System.Collections.IDictionary savedState)
        {
            Trace.WriteLine("Uninstall custom action - unregistering from COM Interop");
            try
            {
                base.Uninstall(savedState);
                RegistrationServices regsrv = new RegistrationServices();
                if (!regsrv.UnregisterAssembly(this.GetType().Assembly))
                {
                    Trace.WriteLine("COM Interop deregistration failed");
                    throw new InstallException("Failed To Unregister from COM Interop");
                }
            }
            finally
            {
                Trace.WriteLine("Completed uninstall custom action");
            }
        }
    }
}