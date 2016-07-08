package md5a4a27fb188421356a03920801799a724;


public class PullDownProgressIndicator
	extends android.widget.ViewSwitcher
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("PullToRefresharp.Android.Views.PullDownProgressIndicator, PullToRefresharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", PullDownProgressIndicator.class, __md_methods);
	}


	public PullDownProgressIndicator (android.content.Context p0) throws java.lang.Throwable
	{
		super (p0);
		if (getClass () == PullDownProgressIndicator.class)
			mono.android.TypeManager.Activate ("PullToRefresharp.Android.Views.PullDownProgressIndicator, PullToRefresharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0 });
	}


	public PullDownProgressIndicator (android.content.Context p0, android.util.AttributeSet p1) throws java.lang.Throwable
	{
		super (p0, p1);
		if (getClass () == PullDownProgressIndicator.class)
			mono.android.TypeManager.Activate ("PullToRefresharp.Android.Views.PullDownProgressIndicator, PullToRefresharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Util.IAttributeSet, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0, p1 });
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
