﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code is for run time compilation.
// </auto-generated>
//------------------------------------------------------------------------------

using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace CCNet.Common.Test.Services1
{
	public static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main()
		{
			ServiceBase[] servicesToRun = new ServiceBase[]
			{ 
				new WindowsService1()
			};
			ServiceBase.Run(servicesToRun);
		}
	}

	/// <summary>
	/// Installer of single service in one binary assembly.
	/// </summary>
	[RunInstaller(true)]
	public class WindowsService1Installer : Installer
	{
		public WindowsService1Installer()
		{
			ServiceProcessInstaller processInstaller = new ServiceProcessInstaller();
			Installers.Add(processInstaller);

			processInstaller.Account = ServiceAccount.LocalService;

			ServiceInstaller installer = new ServiceInstaller();
			installer.ServiceName = "WindowsService1";
			installer.DisplayName = "[Test] Windows Service #1";
			Installers.Add(installer);
		}
	}

	/// <summary>
	/// Test service.
	/// </summary>
	public class WindowsService1 : ServiceBase
	{
		private readonly IContainer components;

		public WindowsService1()
		{
			ServiceName = "WindowsService1";
			components = new Container();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}