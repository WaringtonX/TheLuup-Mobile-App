package md52077bd0dbdd3694633f187b0095e2229;


public class MyHandlerMenu
	extends android.os.Handler
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_handleMessage:(Landroid/os/Message;)V:GetHandleMessage_Landroid_os_Message_Handler\n" +
			"";
		mono.android.Runtime.register ("TheLuupApp.MyHandlerMenu, TheLuupApp", MyHandlerMenu.class, __md_methods);
	}


	public MyHandlerMenu ()
	{
		super ();
		if (getClass () == MyHandlerMenu.class)
			mono.android.TypeManager.Activate ("TheLuupApp.MyHandlerMenu, TheLuupApp", "", this, new java.lang.Object[] {  });
	}


	public MyHandlerMenu (android.os.Handler.Callback p0)
	{
		super (p0);
		if (getClass () == MyHandlerMenu.class)
			mono.android.TypeManager.Activate ("TheLuupApp.MyHandlerMenu, TheLuupApp", "Android.OS.Handler+ICallback, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public MyHandlerMenu (android.os.Looper p0)
	{
		super (p0);
		if (getClass () == MyHandlerMenu.class)
			mono.android.TypeManager.Activate ("TheLuupApp.MyHandlerMenu, TheLuupApp", "Android.OS.Looper, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public MyHandlerMenu (android.os.Looper p0, android.os.Handler.Callback p1)
	{
		super (p0, p1);
		if (getClass () == MyHandlerMenu.class)
			mono.android.TypeManager.Activate ("TheLuupApp.MyHandlerMenu, TheLuupApp", "Android.OS.Looper, Mono.Android:Android.OS.Handler+ICallback, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}

	public MyHandlerMenu (md52077bd0dbdd3694633f187b0095e2229.Menu p0)
	{
		super ();
		if (getClass () == MyHandlerMenu.class)
			mono.android.TypeManager.Activate ("TheLuupApp.MyHandlerMenu, TheLuupApp", "TheLuupApp.Menu, TheLuupApp", this, new java.lang.Object[] { p0 });
	}


	public void handleMessage (android.os.Message p0)
	{
		n_handleMessage (p0);
	}

	private native void n_handleMessage (android.os.Message p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
