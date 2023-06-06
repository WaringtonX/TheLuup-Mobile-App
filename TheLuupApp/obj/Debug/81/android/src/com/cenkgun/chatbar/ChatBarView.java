package com.cenkgun.chatbar;


public class ChatBarView
	extends android.widget.FrameLayout
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onFinishInflate:()V:GetOnFinishInflateHandler\n" +
			"";
		mono.android.Runtime.register ("Cenkgun.ChatBar.ChatBarView, ChatBar", ChatBarView.class, __md_methods);
	}


	public ChatBarView (android.content.Context p0)
	{
		super (p0);
		if (getClass () == ChatBarView.class)
			mono.android.TypeManager.Activate ("Cenkgun.ChatBar.ChatBarView, ChatBar", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public ChatBarView (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == ChatBarView.class)
			mono.android.TypeManager.Activate ("Cenkgun.ChatBar.ChatBarView, ChatBar", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public ChatBarView (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == ChatBarView.class)
			mono.android.TypeManager.Activate ("Cenkgun.ChatBar.ChatBarView, ChatBar", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public ChatBarView (android.content.Context p0, android.util.AttributeSet p1, int p2, int p3)
	{
		super (p0, p1, p2, p3);
		if (getClass () == ChatBarView.class)
			mono.android.TypeManager.Activate ("Cenkgun.ChatBar.ChatBarView, ChatBar", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2, p3 });
	}


	public void onFinishInflate ()
	{
		n_onFinishInflate ();
	}

	private native void n_onFinishInflate ();

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
