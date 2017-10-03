package md56e1f1294a137a0dc9d0abe14d6f5bdc6;


public class TabActivity1_SampleTabFragment
	extends android.app.Fragment
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onViewCreated:(Landroid/view/View;Landroid/os/Bundle;)V:GetOnViewCreated_Landroid_view_View_Landroid_os_Bundle_Handler\n" +
			"n_onCreateView:(Landroid/view/LayoutInflater;Landroid/view/ViewGroup;Landroid/os/Bundle;)Landroid/view/View;:GetOnCreateView_Landroid_view_LayoutInflater_Landroid_view_ViewGroup_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("WineHangouts.TabActivity1+SampleTabFragment, Wine Outlet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", TabActivity1_SampleTabFragment.class, __md_methods);
	}


	public TabActivity1_SampleTabFragment () throws java.lang.Throwable
	{
		super ();
		if (getClass () == TabActivity1_SampleTabFragment.class)
			mono.android.TypeManager.Activate ("WineHangouts.TabActivity1+SampleTabFragment, Wine Outlet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public TabActivity1_SampleTabFragment (java.lang.String p0, android.app.Activity p1) throws java.lang.Throwable
	{
		super ();
		if (getClass () == TabActivity1_SampleTabFragment.class)
			mono.android.TypeManager.Activate ("WineHangouts.TabActivity1+SampleTabFragment, Wine Outlet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "System.String, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:Android.App.Activity, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0, p1 });
	}


	public void onViewCreated (android.view.View p0, android.os.Bundle p1)
	{
		n_onViewCreated (p0, p1);
	}

	private native void n_onViewCreated (android.view.View p0, android.os.Bundle p1);


	public android.view.View onCreateView (android.view.LayoutInflater p0, android.view.ViewGroup p1, android.os.Bundle p2)
	{
		return n_onCreateView (p0, p1, p2);
	}

	private native android.view.View n_onCreateView (android.view.LayoutInflater p0, android.view.ViewGroup p1, android.os.Bundle p2);

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
