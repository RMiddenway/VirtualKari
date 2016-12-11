#include <xamarin/xamarin.h>
#import <AppKit/NSAlert.h>
#import <Foundation/NSDate.h>



extern "C" int xammac_setup ()
{
	xamarin_use_il_registrar = false;
	xamarin_marshal_managed_exception_mode = MarshalManagedExceptionModeUnwindNativeCode;
	xamarin_marshal_objectivec_exception_mode = MarshalObjectiveCExceptionModeUnwindManagedCode;

	return 0;
}

