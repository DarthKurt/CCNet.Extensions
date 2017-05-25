﻿using System;
using CCNet.Build.Common;

namespace CCNet.Build.SetupPackages
{
	public static class Program
	{
		public static int Main(string[] args)
		{
			if (args == null || args.Length == 0)
			{
				Execute.DisplayUsage("Restores and updates all referenced packages to their most recent versions.", typeof(Args));
				return 0;
			}

			try
			{
				Args.Current = new ArgumentProperties(args);
				Execute.DisplayCurrent(typeof(Args));

				SetupPackages();
			}
			catch (Exception e)
			{
				return Execute.RuntimeError(e);
			}

			return 0;
		}

		private static void SetupPackages()
		{
			var checker = new PackageChecker(
				Config.NuGetDbConnection,
				Args.CustomVersions,
				Args.Dependencies,
				Args.Bundles);

			using (Execute.Step("INIT"))
			{
				Console.Write("Loading local packages... ");
				checker.Load();
				Console.WriteLine("{0} found.", checker.PackageCount);
			}

			var log = new LogPackages();
			var packages = new PackagesHelper(checker, log);
			var references = new ReferencesHelper(checker, log);
			var nuget = new NuGetHelper(checker, log);

			using (Execute.Step("ADJUST"))
			{
				packages.Adjust();
				references.Adjust();
			}

			if (Args.BranchName != null)
			{
				using (Execute.Step("RESTORE (BRANCH)"))
				{
					(new NuGetSubstitute(Config.NuGetDbConnection)).RestoreBranchedPackages();
				}
			}

			using (Execute.Step("RESTORE"))
			{
				nuget.RestoreAll();
			}

			using (Execute.Step("UPDATE"))
			{
				nuget.UpdateAll();
			}

			using (Execute.Step("REPORT & SAVE"))
			{
				log.Report();

				Console.Write("Saving local references... ");
				log.SaveReferences(Args.ReferencesPath);
				Console.WriteLine("OK");

				Console.Write("Saving packages summary... ");
				log.SaveSummary(Args.TempPath);
				Console.WriteLine("OK");
			}
		}
	}
}
