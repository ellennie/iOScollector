﻿using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

namespace CoreMotion
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// User Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		UIWindow window;
		CoreMotionViewController viewController;

		//
		// This method is invoked when the application has loaded and is ready to run. In this 
		// method, instantiate the window, load the UI into it and then make the window
		// visible.

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			window = new UIWindow (UIScreen.MainScreen.Bounds);

			viewController = new CoreMotionViewController ();
			window.RootViewController = viewController;
			window.MakeKeyAndVisible ();

			return true;
		}
	}
}