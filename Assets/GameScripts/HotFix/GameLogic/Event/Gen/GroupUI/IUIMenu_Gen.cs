//------------------------------------------------------------------------------
//	<auto-generated>
//		This code was generated by autoBindTool.
//		Changes to this file may cause incorrect behavior and will be lost if
//		the code is regenerated.
//	</auto-generated>
//------------------------------------------------------------------------------
using UnityEngine;
using UnityEngine.UI;
using TEngine;

namespace GameLogic
{
	public partial class IUIMenu_Event
	{
		public static readonly int onSettingChange = RuntimeId.ToRuntimeId("IUIMenu_Event.onSettingChange");
	}

	[EventInterfaceImp(EEventGroup.GroupUI)]
	public partial class IUIMenu_Gen : IUIMenu
	{
		private EventDispatcher _dispatcher;
		public IUIMenu_Gen(EventDispatcher dispatcher)
		{
			_dispatcher = dispatcher;
		}

        public void onSettingChange(System.String data)
        {
            _dispatcher.Send(IUIMenu_Event.onSettingChange,data);
        }

	}
}
