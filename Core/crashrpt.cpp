// Delete this line if you don't use precompiled header in your project
#include "stdafx.h"

#include <CrashRpt.h>

/*
   +-------------------------------------------------+
   | @ {AppName}                                   X |
   +-------------------------------------------------+
   |                                                 |
   | {AppName} has stopped working                   |
   |                                                 |
   | Sending collected information to the {Company}. |
   | This might take several minutes...   +--------+ |
   |                                      | Cancel | |
   |                                      +--------+ |
   +-------------------------------------------------+
*/

// global crash handler
crash_rpt::CrashRpt g_crashRpt(
#error Generate new GUID (http://www.guidgenerator.com/) and replace 00000000-0000-0000-0000-000000000000 with it.
	"00000000-0000-0000-0000-000000000000", // GUID assigned to this application.
	L"ltnet", // Application name that will be used in a message box.
	L"ltnet" // Company name that will be used in a message box.
	);
