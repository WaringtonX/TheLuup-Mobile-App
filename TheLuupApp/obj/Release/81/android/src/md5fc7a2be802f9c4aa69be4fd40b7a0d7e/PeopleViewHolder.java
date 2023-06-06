package md5fc7a2be802f9c4aa69be4fd40b7a0d7e;


public class PeopleViewHolder
	extends android.support.v7.widget.RecyclerView.ViewHolder
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("TheLuupApp.adapters.PeopleViewHolder, TheLuupApp", PeopleViewHolder.class, __md_methods);
	}


	public PeopleViewHolder (android.view.View p0)
	{
		super (p0);
		if (getClass () == PeopleViewHolder.class)
			mono.android.TypeManager.Activate ("TheLuupApp.adapters.PeopleViewHolder, TheLuupApp", "Android.Views.View, Mono.Android", this, new java.lang.Object[] { p0 });
	}

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
