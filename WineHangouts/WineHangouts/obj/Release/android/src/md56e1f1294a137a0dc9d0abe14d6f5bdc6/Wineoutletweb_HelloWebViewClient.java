package md56e1f1294a137a0dc9d0abe14d6f5bdc6;


public class Wineoutletweb_HelloWebViewClient
	extends android.webkit.WebViewClient
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_shouldOverrideUrlLoading:(Landroid/webkit/WebView;Ljava/lang/String;)Z:GetShouldOverrideUrlLoading_Landroid_webkit_WebView_Ljava_lang_String_Handler\n" +
			"";
		mono.android.Runtime.register ("WineHangouts.Wineoutletweb+HelloWebViewClient, Wine Outlet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", Wineoutletweb_HelloWebViewClient.class, __md_methods);
	}


	public Wineoutletweb_HelloWebViewClient () throws java.lang.Throwable
	{
		super ();
		if (getClass () == Wineoutletweb_HelloWebViewClient.class)
			mono.android.TypeManager.Activate ("WineHangouts.Wineoutletweb+HelloWebViewClient, Wine Outlet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public boolean shouldOverrideUrlLoading (android.webkit.WebView p0, java.lang.String p1)
	{
		return n_shouldOverrideUrlLoading (p0, p1);
	}

	private native boolean n_shouldOverrideUrlLoading (android.webkit.WebView p0, java.lang.String p1);

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