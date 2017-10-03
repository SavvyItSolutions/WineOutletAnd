package md50912886b321ab5443c0818d74ee0a940;


public class AlertActivity
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("WineHangouts.AlertActivity, WineHangouts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", AlertActivity.class, __md_methods);
	}


	public AlertActivity () throws java.lang.Throwable
	{
		super ();
		if (getClass () == AlertActivity.class)
			mono.android.TypeManager.Activate ("WineHangouts.AlertActivity, WineHangouts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
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
