package md56e1f1294a137a0dc9d0abe14d6f5bdc6;


public class ReviewPopup_GestureListener
	extends android.view.GestureDetector.SimpleOnGestureListener
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onFling:(Landroid/view/MotionEvent;Landroid/view/MotionEvent;FF)Z:GetOnFling_Landroid_view_MotionEvent_Landroid_view_MotionEvent_FFHandler\n" +
			"";
		mono.android.Runtime.register ("WineHangouts.ReviewPopup+GestureListener, Wine Outlet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", ReviewPopup_GestureListener.class, __md_methods);
	}


	public ReviewPopup_GestureListener () throws java.lang.Throwable
	{
		super ();
		if (getClass () == ReviewPopup_GestureListener.class)
			mono.android.TypeManager.Activate ("WineHangouts.ReviewPopup+GestureListener, Wine Outlet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public boolean onFling (android.view.MotionEvent p0, android.view.MotionEvent p1, float p2, float p3)
	{
		return n_onFling (p0, p1, p2, p3);
	}

	private native boolean n_onFling (android.view.MotionEvent p0, android.view.MotionEvent p1, float p2, float p3);

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
