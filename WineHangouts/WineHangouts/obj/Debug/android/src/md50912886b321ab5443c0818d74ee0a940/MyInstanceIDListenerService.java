package md50912886b321ab5443c0818d74ee0a940;


public class MyInstanceIDListenerService
	extends com.google.android.gms.iid.InstanceIDListenerService
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onTokenRefresh:()V:GetOnTokenRefreshHandler\n" +
			"";
		mono.android.Runtime.register ("WineHangouts.MyInstanceIDListenerService, WineHangouts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", MyInstanceIDListenerService.class, __md_methods);
	}


	public MyInstanceIDListenerService () throws java.lang.Throwable
	{
		super ();
		if (getClass () == MyInstanceIDListenerService.class)
			mono.android.TypeManager.Activate ("WineHangouts.MyInstanceIDListenerService, WineHangouts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onTokenRefresh ()
	{
		n_onTokenRefresh ();
	}

	private native void n_onTokenRefresh ();

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
